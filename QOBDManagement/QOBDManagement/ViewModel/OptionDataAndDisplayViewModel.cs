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
            set { _startup.Bl = value; onPropertyChange( "Bl"); }
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
            displayBillImage.PropertyChanged += onWidthChange_saveImageWidth;
            displayBillImage.PropertyChanged += onHeightChange_saveImageHeight;
            ImageList.Add(displayBillImage);

            //----[ Logo Image ]
            // get Logo image created by MainWindowViewModel for displaying in the UI Header
            DisplayAndData.Display.Image displayLogoImage = _imageManagement(null, "logo"); 
            displayLogoImage.PropertyChanged += onFilePathChange_updateUIImage;
            displayLogoImage.PropertyChanged += onWidthChange_saveImageWidth;
            displayLogoImage.PropertyChanged += onHeightChange_saveImageHeight;
            ImageList.Add(displayLogoImage);

            //----[ Header Image ] 
            // get Header image created by MainWindowViewModel for displaying in the UI Header
            DisplayAndData.Display.Image displayHeaderImage = _imageManagement(null, "header"); 
            displayHeaderImage.PropertyChanged += onFilePathChange_updateUIImage;
            displayHeaderImage.PropertyChanged += onWidthChange_saveImageWidth;
            displayHeaderImage.PropertyChanged += onHeightChange_saveImageHeight;
            ImageList.Add(displayHeaderImage);

            var widthSizeList = new List<int>();
            ImageWidthSizeList.Clear();
            for (int i = 25; i <= 800; i = i + 25)
            {
                widthSizeList.Add(i);
            }
            ImageWidthSizeList = widthSizeList;

            var heightSizeList = new List<int>();
            ImageHeightSizeList.Clear();
            for (int i = 5; i <= 300; i = i + 5)
            {
                heightSizeList.Add(i);
            }
            ImageHeightSizeList = heightSizeList;
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

        public override void Dispose()
        {
            foreach (var image in ImageList)
            {
                image.PropertyChanged -= onFilePathChange_updateUIImage;
                image.PropertyChanged -= onWidthChange_saveImageWidth;
                image.PropertyChanged -= onHeightChange_saveImageHeight;
            }
        }

        //----------------------------[ Event Handler ]------------------
        
        private async void onHeightChange_saveImageHeight(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TxtHeight"))
                await Bl.BlReferential.UpdateInfoAsync(new List<Info> { ((DisplayAndData.Display.Image)sender).ImageHeight });
        }

        private async void onWidthChange_saveImageWidth(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TxtWidth"))
                await Bl.BlReferential.UpdateInfoAsync(new List<Info> { ((DisplayAndData.Display.Image)sender).ImageWidth });
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
        
        private async void getFileFromLocal(DisplayAndData.Display.Image obj)
        {
            obj.TxtChosenFile = ExecuteOpenFileDialog();
            Dialog.showSearch("File saving...");

            obj.save();

            var infosToUpdateList = obj.ImageDataList.Where(x => x.ID != 0).ToList();
            var infosToCreateList = obj.ImageDataList.Where(x => x.ID == 0).ToList();
            var infosUpdatedList = await Bl.BlReferential.UpdateInfoAsync(infosToUpdateList);
            var infosCreatedList = await Bl.BlReferential.InsertInfoAsync(infosToCreateList);

            if(infosUpdatedList.Count > 0)
                await Dialog.show("Image updated Successfully!");

            if (infosCreatedList.Count > 0)
                await Dialog.show("Image created Successfully!");
            
            Dialog.IsDialogOpen = false;         

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
                await Dialog.show(obj.TxtFileName + " has been successfully deteleted!");
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
