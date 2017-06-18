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
using System.Globalization;
using QOBDCommon.Enum;
using QOBDManagement.Helper;
using System.Windows;

namespace QOBDManagement.Classes
{
    public class InfoManager : BindBase
    {
        GeneralInfos _generalInfo;
        public InfoManager()
        {
            _generalInfo = new GeneralInfos();
        }

        public GeneralInfos GeneralInfo
        {
            get { return _generalInfo; }
            set { setProperty(ref _generalInfo, value); }
        }

        public static string ExecuteOpenFileDialog(string title, List<string> fileFilterList)
        {
            string outputFile = null;
            string fileFilter = "Image Files (";
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (fileFilterList.Count > 0)
            {
                foreach (string fileType in fileFilterList)
                    fileFilter += "*." + fileType + ";";

                fileFilter += ")|";

                foreach (string fileType in fileFilterList)
                    fileFilter += "*." + fileType + ";";

                openFileDialog.Filter = fileFilter;
            }

            openFileDialog.Title = title;
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if (openFileDialog.ShowDialog() == true)
                outputFile = openFileDialog.FileName;

            return outputFile;
        }

        public static List<string> getGeneratedImageSizeList()
        {
            return new InfoManager().GeneralInfo.getGeneratedImageSizeList();
        }

        public class InfoBase : BindBase
        {
            protected string _password;
            protected string _login;
            protected List<string> _filter;
            //protected List<Info> _infoDataList;
            protected Dictionary<string, Info> _dictionary;

            public InfoBase()
            {
                _password = "";
                _login = "";
                _filter = new List<string>();
                _dictionary = new Dictionary<string, Info>();
            }

            /// <summary>
            /// initialize the image information fieds
            /// </summary>
            public virtual void initializeFields()
            {
                //_infoDataList = new InfoManager().GeneralInfo.retrieveInfoDataListFromDictionary(_dictionary);
            }

            /// <summary>
            /// initialize the image information fieds
            /// </summary>
            public virtual void updateFields(List<Info> infoList)
            {
                _dictionary = new InfoManager().GeneralInfo.retrieveInfoDataDictionaryFromList(infoList, _filter);
            }
        }

        //======================[ Display ]=====================

        public class Display : InfoBase
        {

            private string _name;
            private string _ftpUrl;
            private string _ftpHost;
            private string _remotePath;
            private string _localPath;
            private string _fileFullPath;
            private string _chosenFile;
            private Info _imageInfo;
            private Info _imageInfoWidth;
            private Info _imageInfoHeight;
            private string _fileNameWithoutExtension;
            private BitmapImage _imageSource;

            public Display()
            {
                initEvents();
                initialize();
            }

            public Display(string login = "", string password = "") : this()
            {
                _login = login;
                _password = password;
            }

            public Display(string ftpPath, string localPath, string login = "", string password = "")
                : this(login, password)
            {
                _remotePath = ftpPath;
                _localPath = Utility.getDirectory(localPath);
            }

            public Display(string fileNameWithoutExtension, List<string> filter, string ftpPath, string localPath, string login = "", string password = "")
                : this(ftpPath, localPath, login, password)
            {
                _fileNameWithoutExtension = fileNameWithoutExtension;
                foreach (var fileNamePart in _fileNameWithoutExtension.Split('_'))
                    _name += fileNamePart + " ";
                _filter = filter;
            }

            public Display(List<Info> infoList, string fileNameWithoutExtension, List<string> filter, string ftpPath, string localPath, string login = "", string password = "")
                : this(fileNameWithoutExtension, filter, ftpPath, localPath, login, password)
            {
                _dictionary = new InfoManager().GeneralInfo.retrieveInfoDataDictionaryFromList(infoList, _filter);
            }


            //----------------------------[ Initialization ]------------------

            private void initialize()
            {
                _name = "";

                _imageSource = new BitmapImage();
                _imageInfoHeight = new Info();
                _imageInfoWidth = new Info();
                _imageInfo = new Info();
            }

            private void initEvents()
            {
                PropertyChanged += onTxtChosenFileChange_setup;
                PropertyChanged += onTxtFileFullPathDelete_deleteTxtChosenFileChange;
                PropertyChanged += onTxtWdthOrHeightChange;
            }

            //----------------------------[ Properties ]------------------

            public BitmapImage ImageSource
            {
                get { return _imageSource; }
                set
                {
                    if (!Application.Current.Dispatcher.CheckAccess())
                    {
                        Dispatcher.CurrentDispatcher.Invoke(() =>
                        {
                            _imageSource = value;
                            onPropertyChange("ImageSource");
                        });
                    }
                    else
                    {
                        _imageSource = value;
                        onPropertyChange();
                    }
                }
            }

            public List<string> FilterList
            {
                get { return _filter; }
                set { setProperty(ref _filter, value); }
            }

            public List<Info> InfoDataList
            {
                get { return new InfoManager().GeneralInfo.retrieveInfoDataListFromDictionary(_dictionary); }
                set { updateFields(value); }
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

            public string TxtInfoItem
            {
                get { if (_filter.Count > 0 && _dictionary.ContainsKey(_filter[0])) { return _dictionary[_filter[0]].Value; } return ""; }
                set { if (_filter.Count > 0 && _dictionary.ContainsKey(_filter[0])) { _dictionary[_filter[0]].Value = value; onPropertyChange("TxtInfoItem"); } }
            }

            public string TxtInfoItem1
            {
                get { if (_filter.Count > 1 && _dictionary.ContainsKey(_filter[1])) { return _dictionary[_filter[1]].Value; } return ""; }
                set { if (_filter.Count > 1 && _dictionary.ContainsKey(_filter[1])) { _dictionary[_filter[1]].Value = value; onPropertyChange("TxtInfoItem1"); } }
            }

            public string TxtInfoItem2
            {
                get { if (_filter.Count() > 2 && _dictionary.ContainsKey(_filter[2])) { return _dictionary[_filter[2]].Value; } return ""; }
                set { if (_filter.Count() > 2 && _dictionary.ContainsKey(_filter[2])) { _dictionary[_filter[2]].Value = value; onPropertyChange("TxtInfoItem2"); } }
            }

            public string TxtFtpUrl
            {
                get { return _ftpUrl; }
                set { setProperty(ref _ftpUrl, value); }
            }

            public string TxtFileName
            {
                get { return TxtInfoItem; }
                set { TxtInfoItem = value; onPropertyChange(); }
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
                get { return Utility.getDirectory(ConfigurationManager.AppSettings["local_image_folder"]); }
            }

            public string TxtRemotePath
            {
                get { return _remotePath; }
                set { setProperty(ref _remotePath, value); }
            }

            public string TxtLocalPath
            {
                get { return _localPath; }
                set { setProperty(ref _localPath, value); }
            }

            public string TxtChosenFile
            {
                get { return _chosenFile; }
                set { setProperty(ref _chosenFile, value); }
            }

            //----------------------------[ Actions ]------------------

            public void setup()
            {
                if (!string.IsNullOrEmpty(TxtChosenFile))
                {
                    _ftpHost = ConfigurationManager.AppSettings["ftp"];
                    _remotePath = !string.IsNullOrEmpty(_remotePath) ? _remotePath : ConfigurationManager.AppSettings["ftp_image_folder"];
                    _localPath = !string.IsNullOrEmpty(_localPath) ? _localPath : TxtBaseDir;

                    var chosenFileName = Path.GetFileName(TxtChosenFile);
                    var filseExtension = chosenFileName.Split('.').Count() > 1 ? chosenFileName.Split('.').Last() : "";
                    TxtFileName = TxtFileNameWithoutExtension + "." + filseExtension;
                    TxtFtpUrl = _ftpHost + Path.Combine(_remotePath, TxtFileName).Replace("//", "/");
                    TxtFileFullPath = Path.Combine(_localPath, TxtFileName);
                }
            }

            private void copyFile()
            {
                closeImageSource();

                if (File.Exists(Path.Combine(_localPath, TxtChosenFile)))
                    _chosenFile = Path.Combine(_localPath, TxtChosenFile);

                if (File.Exists(TxtChosenFile)
                   && !Path.GetFileName(TxtFileFullPath).Equals(Path.GetFileName(TxtChosenFile)))
                {
                    try
                    {
                        if (File.Exists(TxtFileFullPath))
                            File.Delete(TxtFileFullPath);
                        File.Copy(TxtChosenFile, TxtFileFullPath, true);
                    }
                    catch (Exception ex)
                    {
                        Log.error(ex.Message, EErrorFrom.INFOMANAGER);
                    }
                }

                updateImageSource();
            }

            public void downloadFile()
            {
                bool isFileFound = false;

                _chosenFile = TxtFileName;
                setup();

                if (TxtFtpUrl != null && TxtFileFullPath != null)
                    isFileFound = WPFHelper.ftpGetFile(TxtFtpUrl, TxtFileFullPath, _login, _password);

                if (isFileFound && File.Exists(TxtFileFullPath))
                    copyFile();
            }

            /// <summary>
            /// upload the image to the ftp server
            /// </summary>
            /// <returns></returns>
            public bool uploadImage()
            {
                bool isSavedSuccessfully = false;
                if (File.Exists(TxtFileFullPath))
                {
                    // closing the images stream before updating
                    closeImageSource();

                    isSavedSuccessfully = WPFHelper.ftpSendFile(TxtFtpUrl, TxtFileFullPath, _login, _password);

                    // open the images stream
                    if (isSavedSuccessfully)
                        updateImageSource();
                }
                return isSavedSuccessfully;
            }

            /// <summary>
            /// initialize the image information fieds
            /// </summary>
            public override void updateFields(List<Info> infoList)
            {
                base.updateFields(infoList);
                onPropertyChange("ImageInfoUpdated");
            }

            /// <summary>
            /// update the image source and open the image
            /// </summary>
            public void updateImageSource()
            {
                try
                {
                    if (closeImageSource())
                    {
                        using (FileStream imageStream = new FileStream(TxtFileFullPath, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete))
                        {
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
                }
                catch (Exception ex)
                {
                    Log.error(ex.Message, QOBDCommon.Enum.EErrorFrom.INFOMANAGER);
                }
            }

            /// <summary>
            /// close the image
            /// </summary>
            /// <returns></returns>
            public bool closeImageSource()
            {
                Stream stream = default(Stream); 
                bool isClosed = false;
                try
                {
                    if (Application.Current != null && !Application.Current.Dispatcher.CheckAccess())
                    {
                        Application.Current.Dispatcher.Invoke(() => {
                            stream = ImageSource.StreamSource;
                        });
                    }
                    else
                        stream = ImageSource.StreamSource;

                    if (stream != null)
                        stream.Close();
                    isClosed = true;
                }
                catch (Exception ex)
                {
                    Log.error(ex.Message, QOBDCommon.Enum.EErrorFrom.INFOMANAGER);
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
                        Log.error(ex.Message, QOBDCommon.Enum.EErrorFrom.INFOMANAGER);
                    }
                    return true;
                }
                return false;
            }

            public override void Dispose()
            {
                PropertyChanged -= onTxtChosenFileChange_setup;
                PropertyChanged -= onTxtFileFullPathDelete_deleteTxtChosenFileChange;
                PropertyChanged -= onTxtWdthOrHeightChange;
                closeImageSource();
            }

            //----------------------------[ Event Handler ]------------------

            /// <summary>
            /// event listener for image size updating
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void onTxtWdthOrHeightChange(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName.Equals("TxtInfoItem1") || e.PropertyName.Equals("TxtInfoItem2") || e.PropertyName.Equals("TxtInfoItem"))
                {
                    onPropertyChange("ImageInfoUpdated");
                }
            }

            private void onTxtFileFullPathDelete_deleteTxtChosenFileChange(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName.Equals("TxtFileFullPath") && string.IsNullOrEmpty(TxtFileFullPath))
                {
                    TxtChosenFile = "";
                }
            }

            private void onTxtChosenFileChange_setup(object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName.Equals("TxtChosenFile") && !string.IsNullOrEmpty(TxtChosenFile))
                {
                    setup();
                    copyFile();
                    updateFields(InfoDataList);
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


        //======================[ Address/Contact Infos ]=====================

        public class Contact : InfoBase
        {
            private Dictionary<string, Info> _contactDictionary;
            private List<Info> _infosList;

            public Contact(List<Info> infosList)
            {
                _infosList = new List<Info>();
                _contactDictionary = new Dictionary<string, Info>();
                _filter = new List<string> {
                    "company_name",         //=> nom_societe
                    "address",              //=> adresse
                    "phone",                //=> tel
                    "fax",                  //=> fax
                    "email",                //=> email
                    "tax_code",             //=> num_tva    
                };
                _contactDictionary = new InfoManager().GeneralInfo.retrieveInfoDataDictionaryFromList(infosList, _filter);
            }

            public Dictionary<string, Info> ContactDictionary
            {
                get { return _contactDictionary; }
                set { setProperty(ref _contactDictionary, value); }
            }

            public string TxtCompanyName
            {
                get { return (_contactDictionary.ContainsKey(_filter[0])) ? _contactDictionary[_filter[0]].Value : ""; }
                set { if (_contactDictionary.Count > 0) { _contactDictionary[_filter[0]].Value = value; onPropertyChange("TxtCompanyName"); } }
            }

            public string TxtAddress
            {
                get { return (_contactDictionary.ContainsKey(_filter[1])) ? _contactDictionary[_filter[1]].Value : ""; }
                set { if (_contactDictionary.Count > 0) { _contactDictionary[_filter[1]].Value = value; onPropertyChange("TxtAddress"); } }
            }

            public string TxtPhone
            {
                get { return (_contactDictionary.ContainsKey(_filter[2])) ? _contactDictionary[_filter[2]].Value : ""; }
                set { if (_contactDictionary.Count > 0) { _contactDictionary[_filter[2]].Value = value; onPropertyChange("TxtPhone"); } }
            }

            public string TxtFax
            {
                get { return (_contactDictionary.ContainsKey(_filter[3])) ? _contactDictionary[_filter[3]].Value : ""; }
                set { if (_contactDictionary.Count > 0) { _contactDictionary[_filter[3]].Value = value; onPropertyChange("TxtFax"); } }
            }

            public string TxtEmail
            {
                get { return (_contactDictionary.ContainsKey(_filter[4])) ? _contactDictionary[_filter[4]].Value : ""; }
                set { if (_contactDictionary.Count > 0) { _contactDictionary[_filter[4]].Value = value; onPropertyChange("TxtEmail"); } }
            }

            public string TxtTaxCode
            {
                get { return (_contactDictionary.ContainsKey(_filter[5])) ? _contactDictionary[_filter[5]].Value : ""; }
                set { if (_contactDictionary.Count > 0) { _contactDictionary[_filter[5]].Value = value; onPropertyChange("TxtTaxCode"); } }
            }

            public List<Info> InfosList
            {
                get { return _infosList; }
                set { setProperty(ref _infosList, value, "InfosList"); }
            }
        }




        //======================[ File writer, Emails ]=====================


        public class FileWriter : InfoBase
        {
            private string _content;
            private string _subject;
            private string _fullPath;
            private string _fileName;
            private string _ftpHost;
            private string _remotePath;
            private string _localPath;
            private string _fileNameWithoutExtension;
            private string _typeOfFile;

            public FileWriter(string fileName, EOption typeOfFile, string ftpLogin = "", string ftpPassword = "")
            {
                _typeOfFile = typeOfFile.ToString();
                _login = ftpLogin;
                _password = ftpPassword;
                _fileNameWithoutExtension = fileName;
                _content = "Message here";
            }

            public FileWriter(string fileName, EOption typeOfFile, string ftpPath, string ftpLogin = "", string ftpPassword = "")
                : this(fileName, typeOfFile, ftpLogin, ftpPassword)
            {
                _remotePath = ftpPath;
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

            public string TxtContent
            {
                get { return _content; }
                set { setProperty(ref _content, value); }
            }

            public string TxtSubject
            {
                get { return _subject; }
                set { setProperty(ref _subject, value); }
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
                get { return _fullPath; }
                set { setProperty(ref _fullPath, value); }
            }

            public string TxtFtpUrl { get; private set; }

            private void setup()
            {
                string lang = CultureInfo.CurrentCulture.Name.Split('-').FirstOrDefault() ?? "en";
                _ftpHost = ConfigurationManager.AppSettings["ftp"];
                _remotePath = (!string.IsNullOrEmpty(_remotePath) ? _remotePath : ConfigurationManager.AppSettings["ftp_doc_base_folder"]) + lang + "/" + _typeOfFile + "/";
                _localPath = Utility.getDirectory("Docs", _typeOfFile);

                TxtFileName = TxtFileNameWithoutExtension + ".txt";
                TxtFtpUrl = _ftpHost + _remotePath + TxtFileName;
                TxtFileFullPath = Path.Combine(_localPath, TxtFileName);

                if (!Directory.Exists(_localPath))
                    Directory.CreateDirectory(_localPath);
            }

            public bool save()
            {
                bool isSavedSuccessfully = false;

                File.WriteAllText(TxtFileFullPath, TxtContent);
                isSavedSuccessfully = WPFHelper.ftpSendFile(TxtFtpUrl, TxtFileFullPath, _login, _password);
                TxtContent = File.ReadAllText(TxtFileFullPath);

                return isSavedSuccessfully;
            }

            public void read()
            {
                bool isFileFound = false;

                setup();

                if (!string.IsNullOrEmpty(TxtFtpUrl)
                    && !string.IsNullOrEmpty(TxtFileFullPath)
                        && !string.IsNullOrEmpty(TxtLogin)
                            && !string.IsNullOrEmpty(TxtPassword))
                    isFileFound = WPFHelper.ftpGetFile(TxtFtpUrl, TxtFileFullPath, TxtLogin, TxtPassword);


                if (isFileFound && File.Exists(TxtFileFullPath))
                    TxtContent = File.ReadAllText(TxtFileFullPath);
                else
                    TxtContent = "";

            }
        }

        //======================[ Bank Infos ]=====================

        public class Bank : InfoBase
        {
            private List<string> _bankfilterList;
            private Dictionary<string, Info> _bankDictionary;
            private List<Info> _infosList;

            public Bank(List<Info> infosList)
            {
                _infosList = new List<Info>();
                _bankfilterList = new List<string> {
                    "sort_code",                //=> code_banque
                    "account_number",           //=> num_compte
                    "acount_key_number",        //=> cle_rib
                    "bank_name",                //=> nom_banque
                    "branch_code",              //=> guichet
                    "iban",                     //=> IBAN
                    "bic",                      //=> BIC  
                    "bank_address",             //=> adresse_banque     
                };
                _bankDictionary = new GeneralInfos().retrieveInfoDataDictionaryFromList(infosList, _bankfilterList);
                
            }

            public List<Info> InfosList
            {
                get { return _infosList; }
                set { setProperty(ref _infosList, value, "InfosList"); }
            }

            public Dictionary<string, Info> BankDictionary
            {
                get { return _bankDictionary; }
                set { setProperty(ref _bankDictionary, value); }
            }

            public string TxtSortCode
            {
                get { return (_bankDictionary.ContainsKey(_bankfilterList[0])) ? _bankDictionary[_bankfilterList[0]].Value : ""; }
                set { if (_bankDictionary.ContainsKey(_bankfilterList[0])) { _bankDictionary[_bankfilterList[0]].Value = value; onPropertyChange("TxtSortCode"); } }
            }

            public string TxtAccountNumber
            {
                get { return (_bankDictionary.ContainsKey(_bankfilterList[1])) ? _bankDictionary[_bankfilterList[1]].Value : ""; }
                set { if (_bankDictionary.ContainsKey(_bankfilterList[1])) { _bankDictionary[_bankfilterList[1]].Value = value; onPropertyChange("TxtAccountNumber"); } }
            }

            public string TxtAccountKeyNumber
            {
                get { return (_bankDictionary.ContainsKey(_bankfilterList[2])) ? _bankDictionary[_bankfilterList[2]].Value : ""; }
                set { if (_bankDictionary.ContainsKey(_bankfilterList[2])) { _bankDictionary[_bankfilterList[2]].Value = value; onPropertyChange("TxtAccountKeyNumber"); } }
            }

            public string TxtBankName
            {
                get { return (_bankDictionary.ContainsKey(_bankfilterList[3])) ? _bankDictionary[_bankfilterList[3]].Value : ""; }
                set { if (_bankDictionary.ContainsKey(_bankfilterList[3])) { _bankDictionary[_bankfilterList[3]].Value = value; onPropertyChange("TxtBankName"); } }
            }

            public string TxtAgencyCode
            {
                get { return (_bankDictionary.ContainsKey(_bankfilterList[4])) ? _bankDictionary[_bankfilterList[4]].Value : ""; }
                set { if (_bankDictionary.ContainsKey(_bankfilterList[4])) { _bankDictionary[_bankfilterList[4]].Value = value; onPropertyChange("TxtAgencyCode"); } }
            }

            public string TxtIBAN
            {
                get { return (_bankDictionary.ContainsKey(_bankfilterList[5])) ? _bankDictionary[_bankfilterList[5]].Value : ""; }
                set { if (_bankDictionary.ContainsKey(_bankfilterList[5])) { _bankDictionary[_bankfilterList[5]].Value = value; onPropertyChange("TxtIBAN"); } }
            }

            public string TxtBIC
            {
                get { return (_bankDictionary.ContainsKey(_bankfilterList[6])) ? _bankDictionary[_bankfilterList[6]].Value : ""; }
                set { if (_bankDictionary.ContainsKey(_bankfilterList[6])) { _bankDictionary[_bankfilterList[6]].Value = value; onPropertyChange("TxtBIC"); } }
            }

            public string TxtBankAddress
            {
                get { return (_bankDictionary.ContainsKey(_bankfilterList[7])) ? _bankDictionary[_bankfilterList[7]].Value : ""; }
                set { if (_bankDictionary.ContainsKey(_bankfilterList[7])) { _bankDictionary[_bankfilterList[7]].Value = value; onPropertyChange("TxtBankAddress"); } }
            }
        }

        //===================[ Theme manager ]

        public class Theme : InfoBase
        {
            public Theme()
            {
                _filter = new List<string>
                {
                    "theme",
                    "theme_style_primary",
                    "theme_style_accent",
                };
            }

            public Theme(List<Info> infoList) : this()
            {
                _dictionary = new InfoManager().GeneralInfo.retrieveInfoDataDictionaryFromList(infoList, _filter);
            }

            public string TxtThemeName
            {
                get { return (_dictionary.ContainsKey(_filter[0])) ? _dictionary[_filter[0]].Value : ""; }
                set { if (_dictionary.ContainsKey(_filter[0])) { _dictionary[_filter[0]].Value = value; onPropertyChange(); } }
            }

            public string TxtThemeStylePrimary
            {
                get { return (_dictionary.ContainsKey(_filter[1])) ? _dictionary[_filter[1]].Value : ""; }
                set { if (_dictionary.ContainsKey(_filter[1])) { _dictionary[_filter[1]].Value = value; onPropertyChange(); } }
            }

            public string TxtThemeStyleAccent
            {
                get { return (_dictionary.ContainsKey(_filter[2])) ? _dictionary[_filter[2]].Value : ""; }
                set { if (_dictionary.ContainsKey(_filter[2])) { _dictionary[_filter[2]].Value = value; onPropertyChange(); } }
            }

            public List<Info> getInfoDataList()
            {
                return new InfoManager().GeneralInfo.retrieveInfoDataListFromDictionary(_dictionary);
            }

            public override void updateFields(List<Info> infoList)
            {
                base.updateFields(infoList);
            }
        }


    }
}
