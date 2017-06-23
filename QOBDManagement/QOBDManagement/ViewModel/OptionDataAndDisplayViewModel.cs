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
using QOBDManagement.Helper;
using QOBDManagement.Classes.Themes;
using System.Configuration;

namespace QOBDManagement.ViewModel
{
    public class OptionDataAndDisplayViewModel : BindBase
    {
        private ObservableCollection<InfoManager.Display> _imageList;
        private List<string> _imageWidthSizeList;
        private List<string> _imageHeightSizeList;
        private Func<InfoManager.Display, string, InfoManager.Display> _imageManagement;        
        private List<InfoManager.Data> _dataList;
        private CultureInfo[] _cultureInfoArray;
        private string _title;
        private IMainWindowViewModel _main;
        private IEnumerable<Swatch> _swatches;
        private InfoManager.Display _theme;
        
        //----------------------------[ Commands ]------------------

        public ButtonCommand<InfoManager.Display> OpenFileExplorerCommand { get; set; }
        public ButtonCommand<InfoManager.Display> DeleteImageCommand { get; set; }
        public ButtonCommand<string> UpdateLanguageCommand { get; set; }
        public ButtonCommand<string> AddNewRowLanguageCommand { get; set; }
        public ButtonCommand<bool> ToggleThemeBaseCommand { get; set; }
        public ButtonCommand<Swatch> ApplyThemeAccentStyleCommand { get; set; }
        public ButtonCommand<Swatch> ApplyThemePrimaryStyleCommand { get; set; }

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
            _title = ConfigurationManager.AppSettings["title_setting_display"];
            _imageList = new ObservableCollection<InfoManager.Display>();
            _imageWidthSizeList = new List<string>();
            _imageHeightSizeList = new List<string>();
            _cultureInfoArray = CultureInfo.GetCultures(CultureTypes.AllCultures & CultureTypes.NeutralCultures);

            _theme = new InfoManager.Display();

            // palette initialization
            _swatches = new SwatchesProvider().Swatches;
                
            // populating the image size list
            ImageWidthSizeList = ImageHeightSizeList = InfoManager.getGeneratedImageSizeList();
        }

        private void instancesCommand()
        {
            OpenFileExplorerCommand = new ButtonCommand<InfoManager.Display>(getFileFromLocal, canGetFileFromLocal);
            DeleteImageCommand = new ButtonCommand<InfoManager.Display>(deleteImage, canDeleteImage);
            ToggleThemeBaseCommand = new ButtonCommand<bool>(changeTheme, canChangeTheme);
            ApplyThemeAccentStyleCommand = new ButtonCommand<Swatch>(applyThemeAccentStyle, canApplyThemeAccentStyle);
            ApplyThemePrimaryStyleCommand = new ButtonCommand<Swatch>(applyThemePrimaryStyle, canApplyThemePrimaryStyle);
        }

        //----------------------------[ Properties ]------------------


        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
        }

        public InfoManager.Display Theme
        {
            get { return _theme; }
            set { setProperty(ref _theme, value); }
        }

        public string Title
        {
            get { return _title; }
            set { setProperty(ref _title, value); }
        }

        public IEnumerable<Swatch> Swatches
        {
            get { return _swatches; }
        }

        public List<string> ImageWidthSizeList
        {
            get { return _imageWidthSizeList; }
            set { setProperty(ref _imageWidthSizeList, value); }
        }

        public List<string> ImageHeightSizeList
        {
            get { return _imageHeightSizeList; }
            set { setProperty(ref _imageHeightSizeList, value); }
        }

        public ObservableCollection<InfoManager.Display> ImageList
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

        public List<InfoManager.Data> DataList
        {
            get { return _dataList; }
            set { setProperty(ref _dataList, value); }
        }

        //----------------------------[ Actions ]------------------

        public void load()
        {
            loadImages();
        }


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

        private void loadImages()
        {
            Dialog.showSearch(ConfigurationManager.AppSettings["load_message"]);
            Dispose();
            ImageList.Clear();

            //----[ Bill Image ]
            // get Logo image created by MainWindowViewModel for updating
            InfoManager.Display displayBillImage = _imageManagement(null, "bill"); 
            displayBillImage.PropertyChanged += onFilePathChange_updateUIImage;
            displayBillImage.PropertyChanged += onImageInfoChange;
            ImageList.Add(displayBillImage);

            //----[ Logo Image ]
            // get Logo image created by MainWindowViewModel for displaying in the UI Header
            InfoManager.Display displayLogoImage = _imageManagement(null, "logo"); 
            displayLogoImage.PropertyChanged += onFilePathChange_updateUIImage;
            displayLogoImage.PropertyChanged += onImageInfoChange;
            ImageList.Add(displayLogoImage);

            //----[ Header Image ] 
            // get Header image created by MainWindowViewModel for displaying in the UI Header
            InfoManager.Display displayHeaderImage = _imageManagement(null, "header"); 
            displayHeaderImage.PropertyChanged += onFilePathChange_updateUIImage;
            displayHeaderImage.PropertyChanged += onImageInfoChange;
            ImageList.Add(displayHeaderImage);

            // set the theme
            loadTheme();

            Dialog.IsDialogOpen = false;
        }

        public void loadTheme()
        {
            //-----[ Theme ]
            Theme = new InfoManager.Display(Bl.BlReferential.GetInfoData(999), "theme_image", new List<string> { "theme", "theme_style_primary", "theme_style_accent" }, "", "", "", ""); //new InfoManager.Display(Bl.BlReferential.GetInfoData(999));
            Theme.PropertyChanged += onImageInfoChange;
            var swatchTargetedPrimary = Swatches.Where(x => string.Compare(x.Name, Theme.TxtInfoItem1, StringComparison.InvariantCultureIgnoreCase) == 0).FirstOrDefault();
            if (swatchTargetedPrimary != null)
                applyThemePrimaryStyle(swatchTargetedPrimary);

            var swatchTargetedAccent = Swatches.Where(x => x.AccentHues.Where(y => y.Color.ToString() == Theme.TxtInfoItem2).Count() > 0).FirstOrDefault();
            if (swatchTargetedAccent != null)
                applyThemeAccentStyle(swatchTargetedAccent);

            bool isDark = Theme.TxtInfoItem == "Dark" ? true : false;
            changeTheme(isDark);
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

        public async Task<List<Info>> saveInfo(List<Info> infoDataList)
        {
            Dialog.showSearch(ConfigurationManager.AppSettings["update_message"]);
            var infoToUpdateList = infoDataList.Where(x => x.ID != 0).ToList();
            var infoToCreateList = infoDataList.Where(x => x.ID == 0).ToList();
            var infoUpdatedList = await Bl.BlReferential.UpdateInfoAsync(infoToUpdateList);
            var infoCreatedList = await Bl.BlReferential.InsertInfoAsync(infoToCreateList);

            if (infoUpdatedList.Count == 0 && infoCreatedList.Count == 0)
            {
                string errorMessage = "Error occurred while saving the picture information!";
                Log.error(errorMessage, EErrorFrom.REFERENTIAL);
                await Dialog.showAsync(errorMessage);
            }
            else if(infoCreatedList.Count > 0)
            {
                foreach(Info info in infoDataList)
                {
                    var createdInfo = infoCreatedList.Where(x=>x.Name == info.Name).SingleOrDefault();
                    if(createdInfo != null)
                        info.ID = createdInfo.ID;
                }
            }
                        
            Dialog.IsDialogOpen = false;
            return infoUpdatedList.Concat(infoCreatedList).ToList();
        }

        public override void Dispose()
        {
            foreach (var image in ImageList)
            {
                image.PropertyChanged -= onFilePathChange_updateUIImage;
                image.PropertyChanged -= onImageInfoChange;
                Theme.PropertyChanged -= onImageInfoChange;
                image.Dispose();
            }
        }

        //----------------------------[ Event Handler ]------------------
        
        private async void onImageInfoChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ImageInfoUpdated"))
            {
                var infoUpdatedList = await saveInfo(((InfoManager.Display)sender).InfoDataList);
            }                
        }

        private void onFilePathChange_updateUIImage(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TxtFileFullPath") && !string.IsNullOrEmpty(((InfoManager.Display)sender).TxtFileFullPath) )
            {
                if (((InfoManager.Display)sender).TxtFileNameWithoutExtension.Equals("header_image"))
                    _imageManagement((InfoManager.Display)sender, "header");
                else if(((InfoManager.Display)sender).TxtFileNameWithoutExtension.Equals("logo_image"))
                    _imageManagement((InfoManager.Display)sender, "logo");
                else if (((InfoManager.Display)sender).TxtFileNameWithoutExtension.Equals("bill_image"))
                    _imageManagement((InfoManager.Display)sender, "bill");
            }            
        }

        //----------------------------[ Action Commands ]------------------
        
        /// <summary>
        /// load and save file information into database
        /// </summary>
        /// <param name="obj">object which the file is associated with</param>
        public void getFileFromLocal(InfoManager.Display obj)
        {
            // opening the file explorer for image file choosing
            string imageFile = InfoManager.ExecuteOpenFileDialog("Select an image file", new List<string> { "png", "jpeg", "jpg" });
            if (!string.IsNullOrEmpty(imageFile))
            {
                Dialog.showSearch(ConfigurationManager.AppSettings["wait_message"]);
                obj.TxtChosenFile = imageFile;

                // upload the image file to the server FTP
                obj.uploadImage();

                Dialog.IsDialogOpen = false;
            }            
        }

        private bool canGetFileFromLocal(InfoManager.Display arg)
        {
            return true;
        }

        private async void deleteImage(InfoManager.Display obj)
        {
            if (await Dialog.showAsync("Do you really want to delete this image ["+obj.TxtFileName+"] ?"))
            {
                Dialog.showSearch(ConfigurationManager.AppSettings["delete_message"]);
                var notDeletedInfosList = await Bl.BlReferential.DeleteInfoAsync(obj.InfoDataList);
                var whereImageInfosIDIsZeroList = obj.InfoDataList.Where(x => x.ID == 0 && x.Name.Equals(obj.TxtFileNameWithoutExtension)).ToList();
                if ((notDeletedInfosList.Count == 0 || whereImageInfosIDIsZeroList.Count > 0) && obj.deleteFiles())
                {
                    var credentials = Bl.BlReferential.searchInfo(new Info { Name = "ftp_" }, ESearchOption.AND);
                    if (WPFHelper.deleteFileFromFtpServer(ConfigurationManager.AppSettings["ftp_image_folder"], obj.TxtFileName, credentials))
                    {
                        await Dialog.showAsync(obj.TxtFileName + " has been successfully deteleted!");
                        obj.TxtFileFullPath = "";
                    }
                    else
                    {
                        string errorMessage = "Error occurred while deleting the image ["+obj.TxtFileName+"]";
                        Log.error(errorMessage, EErrorFrom.REFERENTIAL);
                        Dialog.showSearch(errorMessage);
                    }
                    
                }

                // reset the picture information
                foreach (Info info in obj.InfoDataList)
                    info.ID = 0;

                Dialog.IsDialogOpen = false;
            }            
        }

        private bool canDeleteImage(InfoManager.Display arg)
        {
            return true;
        }

        private void changeTheme(bool obj)
        {
            new PaletteHelper().SetLightDark(obj);
            Theme.TxtInfoItem = obj ? "Dark" : "Light";
        }

        private bool canChangeTheme(bool arg)
        {
            return true;
        }

        private void applyThemePrimaryStyle(Swatch obj)
        {
            new PaletteHelper().ReplacePrimaryColor(obj);
            Theme.TxtInfoItem1 = obj.Name;
        }

        private bool canApplyThemePrimaryStyle(Swatch arg)
        {
            return true;
        }

        private void applyThemeAccentStyle(Swatch obj)
        {
            new PaletteHelper().ReplaceAccentColor(obj);
            Theme.TxtInfoItem2 = obj.Name;
        }

        private bool canApplyThemeAccentStyle(Swatch arg)
        {
            return true;
        }


    }
}
