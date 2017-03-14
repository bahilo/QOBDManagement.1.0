using Microsoft.Win32;
using QOBDCommon.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using QOBDCommon.Entities;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using System.Configuration;
using System.Windows.Threading;

namespace QOBDManagement.Classes
{
    public class DisplayAndData : BindBase
    {

        public DisplayAndData()
        {

        }

        public static string ExecuteOpenFileDialog(string title, List<string> fileFilterList)
        {
            string outputFile = null;
            string fileFilter = "Image Files (";
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (fileFilterList.Count > 0)
            {
                foreach (string fileType in fileFilterList)
                    fileFilter += "*."+fileType+";";

                fileFilter += ")|";

                foreach (string fileType in fileFilterList)
                    fileFilter += "*."+fileType+";";

                openFileDialog.Filter = fileFilter;
            }            

            openFileDialog.Title = title;
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if (openFileDialog.ShowDialog() == true)
                outputFile = openFileDialog.FileName;

            return outputFile;
        }

        //======================[ Display ]=====================


        public class Display : BindBase
        {

            public Display()
            {

            }

            //======================[ Display - Image ]=====================

            public class Image : BindBase
            {
                private string _name;
                private int _width;
                private int _height;
                private string _ftpUrl;
                private string _ftpHost;
                private string _remotePath;
                private string _localPath;
                private string _fileFullPath;
                private string _fileName;
                private string _chosenFile;
                private Info _imageInfos;
                private Info _imageWidth;
                private Info _imageHeight;
                private List<Info> _imageData;
                private string _fileNameWithoutExtension;
                private List<string> _filter;
                private BitmapImage _imageSource;
                private string _password;
                private string _login;

                public Image(string login = "", string password = "")
                {
                    _login = login;
                    _password = password;
                    _filter = new List<string> {
                        "_width",
                        "_height"
                    };
                    _imageData = new List<Info>();
                    _height = 100;
                    _width = 150;
                    _imageSource = new BitmapImage();
                    _imageHeight = new Info();
                    _imageWidth = new Info();
                    PropertyChanged += onTxtChosenFileChange_setup;
                    PropertyChanged += onTxtFileFullPathDelete_deleteTxtChosenFileChange;
                    PropertyChanged += onImageDataListChange_splitImageData;
                }

                private void onTxtFileFullPathDelete_deleteTxtChosenFileChange(object sender, PropertyChangedEventArgs e)
                {
                    if (e.PropertyName.Equals("TxtFileFullPath") && string.IsNullOrEmpty(TxtFileFullPath))
                    {
                        TxtChosenFile = "";
                    }
                }

                private void onImageDataListChange_splitImageData(object sender, PropertyChangedEventArgs e)
                {
                    if (e.PropertyName.Equals("ImageDataList") && ImageDataList.Count > 0)
                    {
                        ImageWidth = ImageDataList.Where(x => x != null && x.Name == TxtFileNameWithoutExtension + _filter[0]).FirstOrDefault();
                        ImageHeight = ImageDataList.Where(x => x != null && x.Name == TxtFileNameWithoutExtension + _filter[1]).FirstOrDefault();
                        ImageInfos = ImageDataList.Where(x => x != null && x.Name == TxtFileNameWithoutExtension).FirstOrDefault();
                        downloadImage();
                    }
                }

                private void onTxtChosenFileChange_setup(object sender, PropertyChangedEventArgs e)
                {
                    if (e.PropertyName.Equals("TxtChosenFile") && !string.IsNullOrEmpty(TxtChosenFile))
                    {
                        setup();
                        copyImage();
                    }
                }

                public BitmapImage ImageSource
                {
                    get { return _imageSource; }
                    set {
                            Dispatcher.CurrentDispatcher.Invoke(() =>
                            {                                
                                setProperty(ref _imageSource, value, "ImageSource");
                            });
                         }
                }

                public Info ImageInfos
                {
                    get { return _imageInfos; }
                    set { setProperty(ref _imageInfos, value, "ImageInfos"); }
                }

                public Info ImageHeight
                {
                    get { return _imageHeight; }
                    set { setProperty(ref _imageHeight, value, "ImageHeight"); }
                }

                public Info ImageWidth
                {
                    get { return _imageWidth; }
                    set { setProperty(ref _imageWidth, value, "ImageWidth"); }
                }

                public List<Info> ImageDataList
                {
                    get { return _imageData; }
                    set { _imageData = value; onPropertyChange("ImageDataList"); }
                }

                public string TxtLogin
                {
                    get { return _login; }
                    set { setProperty(ref _login, value); }
                }

                public string TxtPassword
                {
                    get { return _password; }
                    set { setProperty(ref _password, value); }
                }

                public string TxtName
                {
                    get { return _name; }
                    set { setProperty(ref _name, value); }
                }

                public int TxtWidth
                {
                    get { return _width; }
                    set { if (ImageWidth != null) _imageWidth.Value = value.ToString(); setProperty(ref _width, value); }
                }

                public int TxtHeight
                {
                    get { return _height; }
                    set { if (ImageHeight != null) _imageHeight.Value = value.ToString(); setProperty(ref _height, value); }
                }

                public string TxtFtpUrl
                {
                    get { return _ftpUrl; }
                    set { setProperty(ref _ftpUrl, value); }
                }

                public string TxtFileName
                {
                    get { return _fileName; }
                    set { setProperty(ref _fileName, value); }
                }

                public string TxtFileNameWithoutExtension
                {
                    get { return _fileNameWithoutExtension; }
                    set { setProperty(ref _fileNameWithoutExtension, value); }
                }

                public string TxtFileFullPath
                {
                    get { return _fileFullPath; }
                    set { setProperty(ref _fileFullPath, value); }
                }

                public string TxtBaseDir
                {
                    get { return Utility.getDirectory("Docs", "Images"); }
                }

                public string TxtChosenFile
                {
                    get { return _chosenFile; }
                    set { setProperty(ref _chosenFile, value); }
                }

                public void setup()
                {
                    if (!string.IsNullOrEmpty(TxtChosenFile))
                    {
                        _ftpHost = ConfigurationManager.AppSettings["ftp"];
                        _remotePath = ConfigurationManager.AppSettings["ftp_image_folder"];
                        _localPath = TxtBaseDir;
                        
                        var chosenFileName = Path.GetFileName(TxtChosenFile);
                        var filseExtension = chosenFileName.Split('.').LastOrDefault() ?? "";
                        TxtFileName = TxtFileNameWithoutExtension + "." + filseExtension;
                        TxtFtpUrl = _ftpHost + _remotePath + TxtFileName;
                        TxtFileFullPath = Path.Combine(_localPath, TxtFileName);

                        if (!Directory.Exists(_localPath))
                            Directory.CreateDirectory(_localPath);
                    }
                }

                private void copyImage()
                {
                    closeImageSource();

                    if (File.Exists(Path.Combine(_localPath, TxtChosenFile)))
                        _chosenFile = Path.Combine(_localPath, TxtChosenFile);

                    if (File.Exists(TxtChosenFile)
                       && !Path.GetFileName(TxtFileFullPath).Equals(Path.GetFileName(TxtChosenFile)))
                    {
                        if (File.Exists(TxtFileFullPath))
                            File.Delete(TxtFileFullPath);

                        File.Copy(TxtChosenFile, TxtFileFullPath);
                    }

                    if (ImageWidth != null)
                        int.TryParse(ImageWidth.Value, out _width);
                    if (ImageHeight != null)
                        int.TryParse(ImageHeight.Value, out _height);

                    updateImageSource();
                }

                private void downloadImage()
                {
                    bool isFileFound = false;

                    if (ImageInfos != null && ImageInfos.ID != 0 && !string.IsNullOrEmpty(ImageInfos.Value))
                    {
                        _chosenFile = ImageInfos.Value;
                        setup();

                        if (TxtFtpUrl != null && TxtFileFullPath != null)
                            isFileFound = Utility.downloadFIle(TxtFtpUrl, TxtFileFullPath, _login, _password);
                            

                        if (isFileFound && File.Exists(TxtFileFullPath))
                            copyImage();                       

                    }
                }

                public bool uploadImage()
                {
                    bool isSavedSuccessfully = false;
                    
                    ImageDataList.Clear();

                    if (File.Exists(TxtFileFullPath))
                    {
                        // closing the images stream before updating
                        closeImageSource();

                        initializeFields();

                        isSavedSuccessfully = Utility.uploadFIle(TxtFtpUrl, TxtFileFullPath, _login, _password);

                        // open the images stream
                        updateImageSource();
                    }

                    return isSavedSuccessfully;
                }

                public void initializeFields()
                {
                    if (ImageInfos == null)
                    {
                        _imageInfos = new Info { Name = TxtFileNameWithoutExtension, Value = TxtFileName };
                        _imageWidth = new Info { Name = TxtFileNameWithoutExtension + _filter[0], Value = TxtWidth.ToString() };
                        _imageHeight = new Info { Name = TxtFileNameWithoutExtension + _filter[1], Value = TxtHeight.ToString() };
                    }
                    else
                    {
                        _imageInfos.Name = TxtFileNameWithoutExtension;
                        _imageInfos.Value = TxtFileName;

                        if (_imageWidth == null)
                            _imageWidth = new Info { Name = TxtFileNameWithoutExtension + _filter[0], Value = TxtWidth.ToString() };
                        else
                        {
                            _imageWidth.Name = TxtFileNameWithoutExtension + _filter[0];
                            _imageWidth.Value = TxtWidth.ToString();
                        }

                        if (_imageHeight == null)
                            _imageHeight = new Info { Name = TxtFileNameWithoutExtension + _filter[1], Value = TxtHeight.ToString() };
                        else
                        {
                            _imageHeight.Name = TxtFileNameWithoutExtension + _filter[1];
                            _imageHeight.Value = TxtHeight.ToString();
                        }                       
                    }

                    ImageDataList.Add(ImageInfos);
                    ImageDataList.Add(ImageWidth);
                    ImageDataList.Add(ImageHeight);
                }

                public void updateImageSource()
                {
                    try
                    {
                        if (closeImageSource())
                        {
                            FileStream imageStream = new FileStream(TxtFileFullPath, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete);
                            BitmapImage imageSource = new BitmapImage();
                            imageSource.BeginInit();
                            imageSource.UriSource = null;
                            imageSource.StreamSource = imageStream;
                            imageSource.CacheOption = BitmapCacheOption.OnLoad;
                            imageSource.EndInit();
                            imageSource.Freeze();

                            ImageSource = imageSource;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.error(ex.Message);
                    }
                }

                public bool closeImageSource()
                {
                    Stream stream = ImageSource.StreamSource;
                    bool isClosed = true;
                    try
                    {
                        if (stream != null)
                            stream.Close();
                    }
                    catch (Exception ex)
                    {
                        Log.error(ex.Message);
                        isClosed = false;
                    }
                    return isClosed;
                }

                public bool deleteFiles()
                {
                    if (!string.IsNullOrEmpty(TxtFileFullPath) && File.Exists(TxtFileFullPath))
                    {
                        try
                        {
                            if (closeImageSource())
                            {
                                var imageSource = new BitmapImage();
                                imageSource.Freeze();
                                ImageSource = imageSource;
                                File.Delete(TxtFileFullPath);
                            }                            
                        }
                        catch (Exception ex)
                        {
                            Log.error(ex.Message);
                        }
                        return true;
                    }
                    return false;
                }

                private float aspectRatio(float value, bool isWidth)
                {
                    float ratio = _width / _height;

                    if (isWidth)
                        return _height / ratio;
                    else
                        return _width * ratio;

                }

            }
        }


        //======================[ Data ]=====================


        public class Data : BindBase
        {
            private string _name;
            private string _url;
            private string _fileName;

            public Data()
            {

            }

            public string TxtName
            {
                get { return _name; }
                set { setProperty(ref _name, value, "TxtName"); }
            }

            public string TxtUrl
            {
                get { return _url; }
                set { setProperty(ref _url, value, "TxtUrl"); }
            }

            public string TxtFileName
            {
                get { return _fileName; }
                set { setProperty(ref _fileName, value, "TxtFileName"); }
            }
        }

    }
}
