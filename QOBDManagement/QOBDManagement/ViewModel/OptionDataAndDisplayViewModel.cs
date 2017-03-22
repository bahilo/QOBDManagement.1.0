using Microsoft.Win32;
using QOBDBusiness;
using QOBDCommon.Classes;
using QOBDManagement.Classes;
using QOBDManagement.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using QOBDCommon.Entities;
using System.Collections.ObjectModel;
using System.Globalization;
using QOBDManagement.Models;
using System.Threading;
using System.IO;
using System.Xml.Serialization;
using QOBDManagement.Interfaces;
using QOBDCommon.Enum;

namespace QOBDManagement.ViewModel
{
    public class OptionDataAndDisplayViewModel : BindBase
    {
        private ObservableCollection<DisplayAndData.Display.Image> _imageList;
        private List<int> _imageWidthSizeList;
        private List<int> _imageHeightSizeList;
        private Func<DisplayAndData.Display.Image, string, DisplayAndData.Display.Image> _imageManagement;        
        private List<DisplayAndData.Data> _dataList;
        private CultureInfo[] _cultureInfoArray;
        private string _title;
        private IMainWindowViewModel _main;
        
        //----------------------------[ Commands ]------------------

        public ButtonCommand<DisplayAndData.Display.Image> OpenFileExplorerCommand { get; set; }
        public ButtonCommand<DisplayAndData.Display.Image> DeleteImageCommand { get; set; }
        public ButtonCommand<string> UpdateLanguageCommand { get; set; }
        public ButtonCommand<string> AddNewRowLanguageCommand { get; set; }

        public OptionDataAndDisplayViewModel() : base()
        {
            instances();
            instancesCommand();
        }

        public OptionDataAndDisplayViewModel(IMainWindowViewModel main):this()
        {
            _main = main;
            _imageManagement = _main.ImageManagement;
        }

        //----------------------------[ Initialization ]------------------

        private void instances()
        {
            _title = "Data/Display Management";
            _imageList = new ObservableCollection<DisplayAndData.Display.Image>();
            _imageWidthSizeList = new List<int>();
            _imageHeightSizeList = new List<int>();
            _cultureInfoArray = CultureInfo.GetCultures(CultureTypes.AllCultures & CultureTypes.NeutralCultures);

            // populating the image size list
            ImageWidthSizeList = ImageHeightSizeList = DisplayAndData.Display.getGeneratedImageSizeList();
        }

        private void instancesCommand()
        {
            OpenFileExplorerCommand = new ButtonCommand<DisplayAndData.Display.Image>(getFileFromLocal, canGetFileFromLocal);
            DeleteImageCommand = new ButtonCommand<DisplayAndData.Display.Image>(deleteImage, canDeleteImage);
        }

        //----------------------------[ Properties ]------------------


        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
        }

        public string Title
        {
            get { return _title; }
            set { setProperty(ref _title, value); }
        }

        public List<int> ImageWidthSizeList
        {
            get { return _imageWidthSizeList; }
            set { setProperty(ref _imageWidthSizeList, value); }
        }

        public List<int> ImageHeightSizeList
        {
            get { return _imageHeightSizeList; }
            set { setProperty(ref _imageHeightSizeList, value); }
        }

        public ObservableCollection<DisplayAndData.Display.Image> ImageList
        {
            get { return _imageList; }
            set { setProperty(ref _imageList, value); }
        }

        public CultureInfo[] CultureInfoArray
        {
            get { return _cultureInfoArray; }
            set { setProperty(ref _cultureInfoArray, value); }
        }

        //----------------------------[ Display by LanguageModel ]------------------

        public List<DisplayAndData.Data> DataList
        {
            get { return _dataList; }
            set { setProperty(ref _dataList, value); }
        }
        
        //----------------------------[ Actions ]------------------

        public List<LanguageModel> LanguageListToLanguageModelList(List<Language> languageList)
        {
            List<LanguageModel> output = new List<LanguageModel>();
            
            foreach (Language language in languageList)
            {
                LanguageModel languageModel = new LanguageModel();
                languageModel.Language = language;

                output.Add(languageModel);
            }

            return output;
        }
        public void loadData()
        {
            loadImages();
        }
              
       
        private void loadImages()
        {
            Dialog.showSearch("Loading...");
            Dispose();
            ImageList.Clear();

            //----[ Bill Image ]
            // get Logo image created by MainWindowViewModel for updating
            DisplayAndData.Display.Image displayBillImage = _imageManagement(null, "bill"); 
            displayBillImage.PropertyChanged += onFilePathChange_updateUIImage;
            displayBillImage.PropertyChanged += onImageInfoChange;
            ImageList.Add(displayBillImage);

            //----[ Logo Image ]
            // get Logo image created by MainWindowViewModel for displaying in the UI Header
            DisplayAndData.Display.Image displayLogoImage = _imageManagement(null, "logo"); 
            displayLogoImage.PropertyChanged += onFilePathChange_updateUIImage;
            displayLogoImage.PropertyChanged += onImageInfoChange;
            ImageList.Add(displayLogoImage);

            //----[ Header Image ] 
            // get Header image created by MainWindowViewModel for displaying in the UI Header
            DisplayAndData.Display.Image displayHeaderImage = _imageManagement(null, "header"); 
            displayHeaderImage.PropertyChanged += onFilePathChange_updateUIImage;
            displayHeaderImage.PropertyChanged += onImageInfoChange;
            ImageList.Add(displayHeaderImage);
                                    
            Dialog.IsDialogOpen = false;
        }

        public string ExecuteOpenFileDialog()
        {
            string outputFile = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Title = "Choose image file";
            openFileDialog.Filter = "Image Files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if (openFileDialog.ShowDialog() == true)
                outputFile = openFileDialog.FileName;

            return outputFile;
        }

        public async void saveImageInfo(DisplayAndData.Display.Image imageInfo )
        {
            Dialog.showSearch("File saving...");
            var infosToUpdateList = imageInfo.ImageDataList.Where(x => x.ID != 0).ToList();
            var infosToCreateList = imageInfo.ImageDataList.Where(x => x.ID == 0).ToList();
            var infosUpdatedList = await Bl.BlReferential.UpdateInfoAsync(infosToUpdateList);
            var infosCreatedList = await Bl.BlReferential.InsertInfoAsync(infosToCreateList);

            if (infosUpdatedList.Count == 0 && infosCreatedList.Count == 0)
            {
                string errorMessage = "Error occurred while saving the file [" + imageInfo.TxtChosenFile + "]";
                Log.error(errorMessage, EErrorFrom.REFERENTIAL);
                await Dialog.showAsync(errorMessage);
            }
            
            Dialog.IsDialogOpen = false;
        }

        public override void Dispose()
        {
            foreach (var image in ImageList)
            {
                image.PropertyChanged -= onFilePathChange_updateUIImage;
                image.PropertyChanged -= onImageInfoChange;
                image.Dispose();
            }
        }

        //----------------------------[ Event Handler ]------------------
        
        private void onImageInfoChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ImageInfoUpdated"))
                saveImageInfo((DisplayAndData.Display.Image)sender );
        }

        private void onFilePathChange_updateUIImage(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TxtFileFullPath") && !string.IsNullOrEmpty(((DisplayAndData.Display.Image)sender).TxtFileFullPath) )
            {
                if (((DisplayAndData.Display.Image)sender).TxtFileNameWithoutExtension.Equals("header_image"))
                    _imageManagement((DisplayAndData.Display.Image)sender, "header");
                else if(((DisplayAndData.Display.Image)sender).TxtFileNameWithoutExtension.Equals("logo_image"))
                    _imageManagement((DisplayAndData.Display.Image)sender, "logo");
                else if (((DisplayAndData.Display.Image)sender).TxtFileNameWithoutExtension.Equals("bill_image"))
                    _imageManagement((DisplayAndData.Display.Image)sender, "bill");
            }            
        }

        //----------------------------[ Action Commands ]------------------
        
        public void getFileFromLocal(DisplayAndData.Display.Image obj)
        {
            // opening the file explorer for image file choosing
            obj.TxtChosenFile = DisplayAndData.ExecuteOpenFileDialog("Select an image file", new List<string> { "png", "jpeg", "jpg" });// ExecuteOpenFileDialog();
            
            // upload the image file to the server FTP
            obj.uploadImage();
            
            // saving the image info into the database
            saveImageInfo(obj); 
        }

        private bool canGetFileFromLocal(DisplayAndData.Display.Image arg)
        {
            return true;
        }

        private async void deleteImage(DisplayAndData.Display.Image obj)
        {
            Dialog.showSearch("Image deleting...");
            var notDeletedInfosList = await Bl.BlReferential.DeleteInfoAsync(obj.ImageDataList);
            var whereImageInfosIDIsZeroList = obj.ImageDataList.Where(x=>x.ID == 0 && x.Name.Equals(obj.TxtFileNameWithoutExtension)).ToList();
            if ((notDeletedInfosList.Count == 0 || whereImageInfosIDIsZeroList.Count > 0) && obj.deleteFiles())
            {
                await Dialog.showAsync(obj.TxtFileName + " has been successfully deteleted!");
                obj.TxtFileFullPath = "";
            }
            Dialog.IsDialogOpen = false;
        }

        private bool canDeleteImage(DisplayAndData.Display.Image arg)
        {
            return true;
        }    


    }
}
