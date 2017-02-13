using QOBDCommon.Classes;
using QOBDCommon.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;
using System.Configuration;
using System.Globalization;
using QOBDCommon.Enum;

namespace QOBDManagement.Classes
{
    public class GeneralInfos : BindBase
    {
        private int _SelectedlistSize;
        private List<int> _listSizeList;

        public GeneralInfos()
        {
            _listSizeList = new List<int>();
            generateListSizeList();
        }

        public List<int> ListSizeList
        {
            get { return _listSizeList; }
            set { setProperty(ref _listSizeList, value); }
        }
        
        public int TxtSelectedListSize
        {
            get { return _SelectedlistSize; }
            set { setProperty(ref _SelectedlistSize, value); }
        }

        private void generateListSizeList()
        {
            for (int i = 1; i < 200; i++)
            {
                _listSizeList.Add(i);
            }
        }




        //======================[ Bank Infos ]=====================

        public class Bank : BindBase
        {
            private List<string> _bankfilterList;
            private Dictionary<string, Pair<int, string>> _bankDictionary;
            private List<Info> _infosList;

            public Bank(List<Info> infosList)
            {
                _bankDictionary = new Dictionary<string, Pair<int, string>>();
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
                fillBankDictionaryFromInfos(infosList);
            }

            public List<Info> InfosList
            {
                get { return _infosList; }
                set { setProperty(ref _infosList, value, "InfosList"); }
            }

            public string TxtSortCode
            {
                get { return (_bankDictionary.ContainsKey(_bankfilterList[0])) ? _bankDictionary[_bankfilterList[0]].PairValue : ""; }
                set { if (_bankDictionary.ContainsKey(_bankfilterList[0])) { _bankDictionary[_bankfilterList[0]].PairValue = value; onPropertyChange("TxtSortCode"); } }
            }

            public string TxtAccountNumber
            {
                get { return (_bankDictionary.ContainsKey(_bankfilterList[1])) ? _bankDictionary[_bankfilterList[1]].PairValue : ""; }
                set { if (_bankDictionary.ContainsKey(_bankfilterList[1])) { _bankDictionary[_bankfilterList[1]].PairValue = value; onPropertyChange("TxtAccountNumber"); } }
            }

            public string TxtAccountKeyNumber
            {
                get { return (_bankDictionary.ContainsKey(_bankfilterList[2])) ? _bankDictionary[_bankfilterList[2]].PairValue : ""; }
                set { if (_bankDictionary.ContainsKey(_bankfilterList[2])) { _bankDictionary[_bankfilterList[2]].PairValue = value; onPropertyChange("TxtAccountKeyNumber"); } }
            }

            public string TxtBankName
            {
                get { return (_bankDictionary.ContainsKey(_bankfilterList[3])) ? _bankDictionary[_bankfilterList[3]].PairValue : ""; }
                set { if (_bankDictionary.ContainsKey(_bankfilterList[3])) { _bankDictionary[_bankfilterList[3]].PairValue = value; onPropertyChange("TxtBankName"); } }
            }

            public string TxtAgencyCode
            {
                get { return (_bankDictionary.ContainsKey(_bankfilterList[4])) ? _bankDictionary[_bankfilterList[4]].PairValue : ""; }
                set { if (_bankDictionary.ContainsKey(_bankfilterList[4])) { _bankDictionary[_bankfilterList[4]].PairValue = value; onPropertyChange("TxtAgencyCode"); } }
            }

            public string TxtIBAN
            {
                get { return (_bankDictionary.ContainsKey(_bankfilterList[5])) ? _bankDictionary[_bankfilterList[5]].PairValue : ""; }
                set { if (_bankDictionary.ContainsKey(_bankfilterList[5])) { _bankDictionary[_bankfilterList[5]].PairValue = value; onPropertyChange("TxtIBAN"); } }
            }

            public string TxtBIC
            {
                get { return (_bankDictionary.ContainsKey(_bankfilterList[6])) ? _bankDictionary[_bankfilterList[6]].PairValue : ""; }
                set { if (_bankDictionary.ContainsKey(_bankfilterList[6])) { _bankDictionary[_bankfilterList[6]].PairValue = value; onPropertyChange("TxtBIC"); } }
            }

            public string TxtBankAddress
            {
                get { return (_bankDictionary.ContainsKey(_bankfilterList[7])) ? _bankDictionary[_bankfilterList[7]].PairValue : ""; }
                set { if (_bankDictionary.ContainsKey(_bankfilterList[7])) { _bankDictionary[_bankfilterList[7]].PairValue = value; onPropertyChange("TxtBankAddress"); } }
            }

            private void fillBankDictionaryFromInfos(List<Info> infosList)
            {
                foreach (var filter in _bankfilterList) 
                {
                    var match = infosList.Where(x => x.Name.Equals(filter)).ToList();
                    Pair<int, string> pair = new Pair<int, string>();
                    if (match.Count() > 0)
                    {
                        pair.PairID = match[0].ID;
                        pair.PairValue = match[0].Value;
                        _bankDictionary[filter] = pair;
                    }
                    else
                    {
                        pair.PairValue = "";
                        _bankDictionary[filter] = pair;
                    }
                }
            }

            public List<Info> extractInfosListFromBankDictionary()
            {
                _infosList = new List<Info>();
                foreach (var bank in _bankDictionary)
                {
                    Info bankInfos = new Info();
                    bankInfos.Name = bank.Key;
                    bankInfos.ID = bank.Value.PairID;
                    bankInfos.Value = bank.Value.PairValue;
                    InfosList.Add(bankInfos);
                }
                return InfosList;
            }
        }




        //======================[ Address/Contact Infos ]=====================


        public class Contact : BindBase
        {
            private List<string> _contactfilterList;
            private Dictionary<string, Pair<int, string>> _contactDictionary;
            private List<Info> _infosList;

            public Contact(List<Info> infosList)
            {
                _infosList = new List<Info>();
                _contactDictionary = new Dictionary<string, Pair<int, string>>();
                _contactfilterList = new List<string> {
                    "company_name",         //=> nom_societe
                    "address",              //=> adresse
                    "phone",                //=> tel
                    "fax",                  //=> fax
                    "email",                //=> email
                    "tax_code",             //=> num_tva    
                };
                fillContactDictionaryFromInfos(infosList);
            }

            public string TxtCompanyName
            {
                get { return (_contactDictionary.ContainsKey(_contactfilterList[0])) ? _contactDictionary[_contactfilterList[0]].PairValue : ""; }
                set { if (_contactDictionary.Count > 0) { _contactDictionary[_contactfilterList[0]].PairValue = value; onPropertyChange("TxtCompanyName"); } }
            }

            public string TxtAddress
            {
                get { return (_contactDictionary.ContainsKey(_contactfilterList[1])) ? _contactDictionary[_contactfilterList[1]].PairValue : ""; }
                set { if (_contactDictionary.Count > 0) { _contactDictionary[_contactfilterList[1]].PairValue = value; onPropertyChange("TxtAddress"); } }
            }

            public string TxtPhone
            {
                get { return (_contactDictionary.ContainsKey(_contactfilterList[2])) ? _contactDictionary[_contactfilterList[2]].PairValue : ""; }
                set { if (_contactDictionary.Count > 0) { _contactDictionary[_contactfilterList[2]].PairValue = value; onPropertyChange("TxtPhone"); } }
            }

            public string TxtFax
            {
                get { return (_contactDictionary.ContainsKey(_contactfilterList[3])) ? _contactDictionary[_contactfilterList[3]].PairValue : ""; }
                set { if (_contactDictionary.Count > 0) { _contactDictionary[_contactfilterList[3]].PairValue = value; onPropertyChange("TxtFax"); } }
            }

            public string TxtEmail
            {
                get { return (_contactDictionary.ContainsKey(_contactfilterList[4])) ? _contactDictionary[_contactfilterList[4]].PairValue : ""; }
                set { if (_contactDictionary.Count > 0) { _contactDictionary[_contactfilterList[4]].PairValue = value; onPropertyChange("TxtEmail"); } }
            }

            public string TxtTaxCode
            {
                get { return (_contactDictionary.ContainsKey(_contactfilterList[5])) ? _contactDictionary[_contactfilterList[5]].PairValue : ""; }
                set { if (_contactDictionary.Count > 0) { _contactDictionary[_contactfilterList[5]].PairValue = value; onPropertyChange("TxtTaxCode"); } }
            }

            public List<Info> InfosList
            {
                get { return _infosList; }
                set { setProperty(ref _infosList, value, "InfosList"); }
            }

            private void fillContactDictionaryFromInfos(List<Info> infosList)
            {
                foreach (var filter in _contactfilterList)
                {
                    var match = infosList.Where(x => x.Name.Equals(filter)).ToList();
                    Pair<int, string> pair = new Pair<int, string>();
                    if (match.Count() > 0)
                    {
                        pair.PairID = match[0].ID;
                        pair.PairValue = match[0].Value;
                        _contactDictionary[filter] = pair;
                    }
                    else
                    {
                        pair.PairValue = "";
                        _contactDictionary[filter] = pair;
                    }
                }
            }

            public List<Info> extractInfosListFromContactDictionary()
            {
                _infosList = new List<Info>();
                foreach (var contact in _contactDictionary)
                {
                    Info contactInfos = new Info();
                    contactInfos.Name = contact.Key;
                    contactInfos.ID = contact.Value.PairID;
                    contactInfos.Value = contact.Value.PairValue;
                    InfosList.Add(contactInfos);
                }

                return InfosList;
            }
        }




        //======================[ File writer ]=====================


        public class FileWriter : BindBase
        {
            private string _content;
            private string _subject;
            private string _path;
            private string _fullPath;
            private string _fileName;
            private string _ftpHost;
            private string _remotePath;
            private string _localPath;
            private string _fileNameWithoutExtension;
            private string _login;
            private string _password;
            private string _typeOfFile; 

            public FileWriter(string fileName, EOption typeOfFile, string ftpLogin = "", string ftpPassword = "" )
            {
                _typeOfFile = typeOfFile.ToString();
                _login = ftpLogin;
                _password = ftpPassword;
                _fileNameWithoutExtension = fileName;
                _content = "Message here";
                _path = Utility.getDirectory("Docs", "Files");
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
                set {setProperty(ref _content , value); }
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
                _remotePath = ConfigurationManager.AppSettings["ftp_doc_base_folder"] + lang + "/" + _typeOfFile + "/";
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
                isSavedSuccessfully = Utility.uploadFIle(TxtFtpUrl, TxtFileFullPath, _login, _password);
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
                   isFileFound = Utility.downloadFIle(TxtFtpUrl, TxtFileFullPath, TxtLogin, TxtPassword);
                   

                if (isFileFound && File.Exists(TxtFileFullPath))
                    TxtContent = File.ReadAllText(TxtFileFullPath);
                else
                    TxtContent = "";

            }
        }


        //======================[ Pair ]=====================


        public class Pair<T1, T2>
        {
            public T1 PairID { get; set; }
            public T2 PairValue { get; set; }
        }


        //======================[ Email ]=====================


        public class Email : BindBase
        {
            string _textContent;

            public Email()
            {
                _textContent = "";
            }

            public string TextContent
            {
                get { return _textContent; }
                set { setProperty(ref _textContent, value, "TextContent"); }
            }
        }


    }
}
