using QOBDManagement.Classes;
using QOBDManagement.Command;
using QOBDManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using QOBDManagement.Interfaces;
using System.Runtime.CompilerServices;
using System.Configuration;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDBusiness;
using QOBDCommon.Classes;
using System.Net;

namespace QOBDManagement.ViewModel
{
    public class OptionGeneralViewModel : BindBase
    {
        private Func<object, object> _page;
        private List<InfoManager.Bank> _bankDetails;
        private List<InfoManager.Contact> _addressDetails;
        private GeneralInfos _generalInfos;
        private string _selectedTaxInteger;
        private string _selectedTaxFloat;
        private InfoManager.FileWriter _legalInformationFileManagement;
        private InfoManager.FileWriter _saleGeneralConditionFileManagement;
        private string _validationEmail;
        private string _reminderEmail;
        private string _invoiceEmail;
        private string _quoteEmail;
        private string _email;
        private List<string> _emailfilterList;
        private string _title;
        private CurrencyModel _currency;

        //----------------------------[ Models ]------------------

        private List<TaxModel> _taxes;
        private TaxModel _taxModel;
        private IMainWindowViewModel _main;

        //----------------------------[ Commands ]------------------

        public ButtonCommand<object> UpdateListSizeCommand { get; set; }
        public ButtonCommand<object> UpdateBankDetailCommand { get; set; }
        public ButtonCommand<object> UpdateAddressCommand { get; set; }
        public ButtonCommand<object> AddTaxCommand { get; set; }
        public ButtonCommand<TaxModel> DeleteTaxCommand { get; set; }
        public ButtonCommand<CurrencyModel> AddCurrencyCommand { get; set; }
        public ButtonCommand<CurrencyModel> DeleteCurrencyCommand { get; set; }
        public ButtonCommand<object> UpdateLegalInformationCommand { get; set; }
        public ButtonCommand<object> UpdateSaleGeneralConditionCommand { get; set; }
        public ButtonCommand<object> NewLegalInformationCommand { get; set; }
        public ButtonCommand<object> NewSaleGeneralConditionCommand { get; set; }
        public ButtonCommand<object> UpdateEmailCommand { get; set; }
        public ButtonCommand<CurrencyModel> UpdateDefaultCurrencyCommand { get; set; }
        public ButtonCommand<TaxModel> UpdateDefaultTaxCommand { get; set; }
        public ButtonCommand<object> ClearNewTaxCommand { get; set; }
        public ButtonCommand<object> ClearNewCurrencyCommand { get; set; }



        public OptionGeneralViewModel()
        {
            instances();
            instancesModel();
            instancesCommand();
        }

        public OptionGeneralViewModel(IMainWindowViewModel main) : this()
        {
            _main = main;
            _page = _main.navigation;
            initEvents();
        }

        //----------------------------[ Initialization ]------------------

        private void instances()
        {
            _currency = new CurrencyModel();
            _legalInformationFileManagement = new InfoManager.FileWriter("legal_information", EOption.texts);
            _saleGeneralConditionFileManagement = new InfoManager.FileWriter("sale_general_condition", EOption.texts);
            _addressDetails = new List<InfoManager.Contact>();
            _bankDetails = new List<InfoManager.Bank>();
            _generalInfos = new GeneralInfos();
            _emailfilterList = new List<string> { "email", "invoice_email", "quote_email", "reminder_email", "validation_email" };
            _title = ConfigurationManager.AppSettings["title_settings"];
        }

        private void instancesModel()
        {
            _taxModel = new TaxModel();
            _taxes = new List<TaxModel>();
        }

        private void initEvents()
        {
            PropertyChanged += onTaxFloatSelectedOrTaxIntegerSelectedChange;
            PropertyChanged += onCurrencyFloatOrCurrencyIntegerSelectedChange;
        }

        private void instancesCommand()
        {
            UpdateAddressCommand = new ButtonCommand<object>(updateAddress, canUpdateAddress);
            UpdateBankDetailCommand = new ButtonCommand<object>(updateBankDetail, canUpdateBankDetail);
            UpdateLegalInformationCommand = new ButtonCommand<object>(updateLegalInformation, canUpdateLegalInformation);
            UpdateSaleGeneralConditionCommand = new ButtonCommand<object>(updateSaleGeneralCondition, canUpdateSaleGeneralCondition);
            NewLegalInformationCommand = new ButtonCommand<object>(eraseLegalInformation, canEraseLegalInformation);
            NewSaleGeneralConditionCommand = new ButtonCommand<object>(eraseSaleGeneralCondition, canEraseSaleGeneralCondition);
            UpdateListSizeCommand = new ButtonCommand<object>(updateListSize, canUpdateListSize);
            AddTaxCommand = new ButtonCommand<object>(addTax, canAddTax);
            DeleteTaxCommand = new ButtonCommand<TaxModel>(deleteTax, canDeleteTax);
            AddCurrencyCommand = new ButtonCommand<CurrencyModel>(addCurrency, canAddCurrency);
            DeleteCurrencyCommand = new ButtonCommand<CurrencyModel>(deleteCurrency, canDeleteCurrency);
            UpdateEmailCommand = new ButtonCommand<object>(updateEmail, canUpdateEmail);
            UpdateDefaultCurrencyCommand = new ButtonCommand<CurrencyModel>(updateDefaultCurrency, canUpdateDefaultCurrency);
            UpdateDefaultTaxCommand = new ButtonCommand<TaxModel>(updateDefaultTax, canUpdateDefaultTax); ;
            ClearNewCurrencyCommand = new ButtonCommand<object>(clearNewCurrency, canClearNewCurrency);
            ClearNewTaxCommand = new ButtonCommand<object>(clearNewTax, canClearNewTax);
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

        public InfoManager.FileWriter LegalInformationFileManagement
        {
            get { return _legalInformationFileManagement; }
            set { setProperty(ref _legalInformationFileManagement, value); }
        }

        public InfoManager.FileWriter SaleGeneralConditionFileManagement
        {
            get { return _saleGeneralConditionFileManagement; }
            set { setProperty(ref _saleGeneralConditionFileManagement, value); }
        }

        public List<int> ListSizeList
        {
            get { return _generalInfos.ListSizeList; }
            set { _generalInfos.ListSizeList = value; onPropertyChange(); }
        }

        public int TxtSelectedListSize
        {
            get { return _generalInfos.TxtSelectedListSize; }
            set { _generalInfos.TxtSelectedListSize = value; onPropertyChange(); }
        }

        public TaxModel TaxModel
        {
            get { return _taxModel; }
            set
            {
                if (value != null)
                {
                    _taxModel = value;
                    _selectedTaxInteger = _taxModel.TxtTaxValue.Split('.')[0];
                    _selectedTaxFloat = (_taxModel.TxtTaxValue.Split('.').Count() > 1) ? _taxModel.TxtTaxValue.Split('.')[1] : "0";
                    onPropertyChange("SelectedTaxInteger");
                    onPropertyChange("SelectedTaxFloat");
                    onPropertyChange();
                }
            }
        }

        public CurrencyModel CurrencyModel
        {
            get { return _currency; }
            set { _currency = value; onPropertyChange(); }
        }

        public string SelectedTaxInteger
        {
            get { return _selectedTaxInteger; }
            set { setProperty(ref _selectedTaxInteger, value); }
        }

        public string SelectedTaxFloat
        {
            get { return _selectedTaxFloat; }
            set { setProperty(ref _selectedTaxFloat, value); }
        }

        public List<string> SymbolsList
        {
            get { return CurrencyModel.getCurrencyCodes(); }
        }

        public string TxtValidationEmail
        {
            get { return _validationEmail; }
            set { setProperty(ref _validationEmail, value); }
        }

        public string TxtReminderEmail
        {
            get { return _reminderEmail; }
            set { setProperty(ref _reminderEmail, value); }
        }

        public string TxtInvoiceEmail
        {
            get { return _invoiceEmail; }
            set { setProperty(ref _invoiceEmail, value); }
        }

        public string TxtQuoteEmail
        {
            get { return _quoteEmail; }
            set { setProperty(ref _quoteEmail, value); }
        }

        public string TxtEmail
        {
            get { return _email; }
            set { setProperty(ref _email, value); }
        }

        public List<TaxModel> TaxList
        {
            get { return _taxes; }
            set { setProperty(ref _taxes, value); }
        }

        public List<CurrencyModel> CurrenciesList
        {
            get { return _main.OrderViewModel.CurrenciesList; }
            set { _main.OrderViewModel.CurrenciesList = value; onPropertyChange(); }
        }

        public List<InfoManager.Bank> BankDetailList
        {
            get { return _bankDetails; }
            set { setProperty(ref _bankDetails, value); }
        }

        public List<InfoManager.Contact> AddressList
        {
            get { return _addressDetails; }
            set { setProperty(ref _addressDetails, value); }
        }

        public string BlockBankDetailVisibility
        {
            get { return disableUIElementByString(); }
        }

        public string BlockAddressDetailVisibility
        {
            get { return disableUIElementByString(); }
        }

        public string BlockLegalInfosDetailVisibility
        {
            get { return disableUIElementByString(); }
        }

        public string BlockTaxDetailVisibility
        {
            get { return disableUIElementByString(); }
        }

        //----------------------------[ Actions ]------------------

        public List<TaxModel> TaxListToTaxModelList(List<Tax> taxList)
        {
            List<TaxModel> output = new List<TaxModel>();
            foreach (var tax in taxList)
            {
                TaxModel taxModel = new TaxModel();
                taxModel.Tax = tax;
                output.Add(taxModel);
            }
            return output;
        }

        public async void load()
        {
            Dialog.showSearch(ConfigurationManager.AppSettings["load_message"]);
            var userListSizeFoundList = _generalInfos.ListSizeList.Where(x => x.Equals(Bl.BlSecurity.GetAuthenticatedUser().ListSize)).ToList();
            TxtSelectedListSize = (userListSizeFoundList.Count > 0) ? userListSizeFoundList[0] : 0;
            TaxList = TaxListToTaxModelList(await Bl.BlOrder.GetTaxDataAsync(999));
            
            var infosFoundList = Bl.BlReferential.GetInfoData(999);
            BankDetailList = new List<InfoManager.Bank> { new InfoManager.Bank(infosFoundList) };
            AddressList = new List<InfoManager.Contact> { new InfoManager.Contact(infosFoundList) };

            LoadEmail();
            loadTexts();

            Dialog.IsDialogOpen = false;
        }

        private async void loadTexts()
        {
            await Task.Factory.StartNew(() =>
            {
                string login = (_startup.Bl.BlReferential.searchInfo(new QOBDCommon.Entities.Info { Name = "ftp_login" }, ESearchOption.OR).FirstOrDefault() ?? new Info()).Value;
                string password = (_startup.Bl.BlReferential.searchInfo(new QOBDCommon.Entities.Info { Name = "ftp_password" }, ESearchOption.OR).FirstOrDefault() ?? new Info()).Value;

                SaleGeneralConditionFileManagement.TxtLogin = LegalInformationFileManagement.TxtLogin = login;
                SaleGeneralConditionFileManagement.TxtPassword = LegalInformationFileManagement.TxtPassword = password;

                LegalInformationFileManagement.read();
                SaleGeneralConditionFileManagement.read();
            });
        }

        private async void LoadEmail()
        {
            List<Info> insertList = new List<Info>();
            foreach (string filter in _emailfilterList)
            {
                var infosFoundList = Bl.BlReferential.searchInfo(new Info { Name = filter }, ESearchOption.AND);
                if (infosFoundList.Count > 0)
                {
                    switch (filter)
                    {
                        case "email":
                            TxtEmail = infosFoundList[0].Value;
                            break;
                        case "invoice_email":
                            TxtInvoiceEmail = infosFoundList[0].Value;
                            break;
                        case "quote_email":
                            TxtQuoteEmail = infosFoundList[0].Value;
                            break;
                        case "reminder_email":
                            TxtReminderEmail = infosFoundList[0].Value;
                            break;
                        case "validation_email":
                            TxtValidationEmail = infosFoundList[0].Value;
                            break;
                    }
                }
                else
                    insertList.Add(new Info { Name = filter });
            }
            var infosInsertedList = await Bl.BlReferential.InsertInfoAsync(insertList);
        }

        private string disableUIElementByString([CallerMemberName] string obj = "")
        {
            bool isWrite = _main.securityCheck(QOBDCommon.Enum.EAction.Option, QOBDCommon.Enum.ESecurity._Write);
            bool isUpdate = _main.securityCheck(QOBDCommon.Enum.EAction.Option, QOBDCommon.Enum.ESecurity._Update);
            if ((!isWrite || !isUpdate)
                && (obj.Equals("BlockBankDetailVisibility")
                || obj.Equals("BlockTaxDetailVisibility")
                || obj.Equals("BlockAddressDetailVisibility")
                || obj.Equals("BlockLegalInfosDetailVisibility")))
                return "Hidden";

            return "Visible";
        }

        private decimal getTaxValue()
        {
            return Utility.decimalTryParse(_selectedTaxInteger + "." + _selectedTaxFloat);
        }

        private decimal getCurrencyValue()
        {
            var defaultCurrency = CurrenciesList.Where(x => x.IsDefault).SingleOrDefault();
            if (defaultCurrency != null)
                return ExchangeRateFromAPI(1, defaultCurrency.CurrencyEnum.ToString(), CurrencyModel.CurrencyEnum.ToString());
            else
                return ExchangeRateFromAPI(1, CurrencyModel.CurrencyEnum.ToString(), CurrencyModel.CurrencyEnum.ToString());
        }

        /// <summary>
        /// update all prices regarding the new default currency
        /// </summary>
        private bool updateDefaultCurrencyPrices()
        {
            //<<<<< TO DO >>>>>

            return true;
        }

        private decimal ExchangeRateFromAPI(decimal amount, string firstCcode, string lastCcode)
        {
            try
            {
                WebClient web = new WebClient();
                const string urlPattern = "http://finance.yahoo.com/d/quotes.csv?s={0}{1}=X&f=l1";
                string url = String.Format(urlPattern, firstCcode, lastCcode);
                // Get response as string
                string response = new WebClient().DownloadString(url);
                // Convert string to number
                decimal exchangeRate = Utility.decimalTryParse(response);
                return exchangeRate;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<bool> refreshCurrenciesRate()
        {
            bool result = false;
            var defaultCurrency = CurrenciesList.Where(x=>x.IsDefault).SingleOrDefault();
            if (defaultCurrency != null)
            {
                foreach (CurrencyModel currencyModel in CurrenciesList)
                    currencyModel.Currency.Rate = ExchangeRateFromAPI(1, defaultCurrency.TxtCurrencyCode, currencyModel.TxtCurrencyCode);

                var savedCurrencies = await Bl.BlOrder.UpdateCurrencyAsync(CurrenciesList.Select(x => x.Currency).ToList());
                if (savedCurrencies.Count > 0)
                {
                    result = true;
                    CurrenciesList = new List<CurrencyModel>(savedCurrencies.Select(x=> new CurrencyModel { Currency = x }).ToList());
                }                    
            }
            return result;
        }

        public override void Dispose()
        {
            PropertyChanged -= onTaxFloatSelectedOrTaxIntegerSelectedChange;
            PropertyChanged -= onCurrencyFloatOrCurrencyIntegerSelectedChange;
        }

        //----------------------------[ Event Handler ]------------------

        private void onTaxFloatSelectedOrTaxIntegerSelectedChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TaxModel") && TaxModel != null)
            {
                TaxModel.TxtTaxValue = getTaxValue().ToString();
            }
        }

        private void onCurrencyFloatOrCurrencyIntegerSelectedChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("CurrencyModel") && CurrencyModel != null)
            {
                CurrencyModel.TxtRate = getCurrencyValue().ToString();
             }
        }

        //----------------------------[ Action Commands ]------------------

        private async void deleteTax(TaxModel obj)
        {
            if (await Dialog.showAsync("[Tax = "+obj.TxtTaxValue+"] Do you confirm the deletion?"))
            {
                Dialog.showSearch(ConfigurationManager.AppSettings["delete_message"]);
                var NotDeletedTax = await Bl.BlOrder.DeleteTaxAsync(new List<QOBDCommon.Entities.Tax> { obj.Tax });
                if (NotDeletedTax.Count == 0)
                {
                    await Dialog.showAsync("Tax Deleted Successfully!");
                    TaxList.Remove(obj);
                    TaxList = new List<TaxModel>(TaxList);
                }
                clearNewTax(null);
                Dialog.IsDialogOpen = false;
            }            
        }

        private bool canDeleteTax(TaxModel arg)
        {
            return true;
        }

        private async void addTax(object obj)
        {
            Dialog.showSearch(ConfigurationManager.AppSettings["update_message"]);

            List<Tax> savedTaxList = new List<Tax>();            
            TaxModel.TxtTaxValue = getTaxValue().ToString();
            TaxModel.TxtDate = DateTime.Now.ToString();

            if (TaxModel.Tax.ID == 0)
            {
                savedTaxList = await Bl.BlOrder.InsertTaxAsync(new List<QOBDCommon.Entities.Tax> { TaxModel.Tax });
                TaxList = new List<TaxModel>(TaxList.Concat(TaxListToTaxModelList(savedTaxList)));
            }
            else
                savedTaxList = await Bl.BlOrder.UpdateTaxAsync(new List<QOBDCommon.Entities.Tax> { TaxModel.Tax });

            if (savedTaxList.Count > 0)
            {
                await Dialog.showAsync("Tax added Successfully!");
                onPropertyChange("TaxList");
            }
            clearNewTax(null);
            Dialog.IsDialogOpen = false;
        }

        private bool canAddTax(object arg)
        {
            return true;
        }

        private async void updateDefaultTax(TaxModel obj)
        {
            if (await Dialog.showAsync("Do you confirm the update of the default tax?"))
            {
                Dialog.showSearch(ConfigurationManager.AppSettings["update_message"]);
                var savedTaxes = await Bl.BlOrder.UpdateTaxAsync(new List<Tax> { obj.Tax });
                if (savedTaxes.Count > 0)
                    await Dialog.showAsync("The default Tax has been successfully updated!");
            }
            else
            {
                Tax taxFound = Bl.BlOrder.searchTax(new Tax { Tax_current = 1 }, ESearchOption.AND).SingleOrDefault();
                if (taxFound != null)
                {
                    var taxModel = TaxList.Where(x => x.Tax.ID == taxFound.ID).SingleOrDefault();
                    if (taxModel != null)
                        taxModel.IsDefault = true;
                }
            }
            clearNewTax(null);
            Dialog.IsDialogOpen = false;
        }

        private bool canUpdateDefaultTax(TaxModel arg)
        {
            return true;
        }

        private void clearNewTax(object obj)
        {
            TaxModel = new TaxModel();
        }

        private bool canClearNewTax(object arg)
        {
            return true;
        }

        private async void deleteCurrency(CurrencyModel obj)
        {
            if (await Dialog.showAsync("[Currency = " + obj.TxtName + "] Do you confirm the deletion?"))
            {
                Dialog.showSearch(ConfigurationManager.AppSettings["delete_message"]);
                var NotDeletedCurrencies = await Bl.BlOrder.DeleteCurrencyAsync(new List<QOBDCommon.Entities.Currency> { obj.Currency });
                if (NotDeletedCurrencies.Count == 0)
                {
                    await Dialog.showAsync("Currency Deleted Successfully!");
                    CurrenciesList.Remove(obj);
                    CurrenciesList = new List<CurrencyModel>(CurrenciesList);
                    clearNewCurrency(null);
                }
                clearNewCurrency(null);
                Dialog.IsDialogOpen = false;
            }            
        }

        private bool canDeleteCurrency(CurrencyModel arg)
        {
            return true;
        }

        private async void addCurrency(CurrencyModel obj)
        {
            CurrencyModel.TxtRate = getCurrencyValue().ToString();

            if (CurrencyModel.Currency.Rate != 0)
            {
                List<Currency> savedCurrenciesList = new List<Currency>();
                Dialog.showSearch(ConfigurationManager.AppSettings["update_message"]);
                CurrencyModel.Currency.Date = DateTime.Now;

                if (CurrencyModel.Currency.ID == 0)
                {
                    savedCurrenciesList = await Bl.BlOrder.InsertCurrencyAsync(new List<QOBDCommon.Entities.Currency> { CurrencyModel.Currency });
                    CurrenciesList = new List<CurrencyModel>(CurrenciesList.Concat(savedCurrenciesList.Select(x => new CurrencyModel { Currency = x }).ToList()));
                }
                else
                {
                    savedCurrenciesList = await Bl.BlOrder.UpdateCurrencyAsync(new List<Currency> { CurrencyModel.Currency });
                }

                if (savedCurrenciesList.Count > 0)
                {
                    await Dialog.showAsync("Currency updated Successfully!");
                }
            }
            else
            {
                CurrencyModel.IsDefault = false;
                Currency currencyFound = Bl.BlOrder.searchCurrency(new Currency { IsDefault = true }, ESearchOption.AND).SingleOrDefault();
                if (currencyFound != null)
                {
                    var currency = CurrenciesList.Where(x => x.Currency.ID == currencyFound.ID).SingleOrDefault();
                    if (currency != null)
                        currency.IsDefault = true;
                }
                await Dialog.showAsync("Cannot set a currency with a rate of [0] !");
            }
                

            clearNewCurrency(null);
            Dialog.IsDialogOpen = false;
        }

        private bool canAddCurrency(object arg)
        {
            return true;
        }

        private async void updateDefaultCurrency(CurrencyModel obj)
        {
            if (obj.Currency.Rate != 0 && await Dialog.showAsync("Do you confirm the update of the default currency?"+ Environment.NewLine + "(Please note that all prices will be updated.)"))
            {
                bool isErrorDetected = false;
                Dialog.showSearch(ConfigurationManager.AppSettings["update_message"]);
                var savedCurrencies = await Bl.BlOrder.UpdateCurrencyAsync(new List<Currency> { obj.Currency });
                if (savedCurrencies.Count > 0)
                {
                    if (await refreshCurrenciesRate())
                    {
                        if (updateDefaultCurrencyPrices())
                            await Dialog.showAsync("The default currency has been successfully updated!");
                        else
                            isErrorDetected = true;
                    }
                    else
                        isErrorDetected = true;

                    if (isErrorDetected)
                    {
                        string errorMessage = "Error occurred while updating the default currency to [" + obj.TxtName + " (ID=" + obj.TxtID + ")]";
                        Log.error(errorMessage, EErrorFrom.REFERENTIAL);
                        await Dialog.showAsync(errorMessage);
                    }
                }
                
            }
            else
            {
                obj.IsDefault = false;
                if (obj.Currency.Rate == 0)
                    await Dialog.showAsync("Cannot set a currency with a rate of [0] as a default currency!");
                else
                {
                    Currency currencyFound = Bl.BlOrder.searchCurrency(new Currency { IsDefault = true }, ESearchOption.AND).SingleOrDefault();
                    if (currencyFound != null)
                    {
                        var currency = CurrenciesList.Where(x => x.Currency.ID == currencyFound.ID).SingleOrDefault();
                        if (currency != null)
                            currency.IsDefault = true;
                    }
                }
            }
            

            clearNewCurrency(null);
            Dialog.IsDialogOpen = false;
        }

        private bool canUpdateDefaultCurrency(CurrencyModel arg)
        {
            if (_main.AgentViewModel.IsAuthenticatedAgentAdmin)
                return true;
            return false;
        }

        private void clearNewCurrency(object obj)
        {
            CurrencyModel = new CurrencyModel();
        }

        private bool canClearNewCurrency(object arg)
        {
            return true;
        }

        private async void updateListSize(object obj)
        {
            Dialog.showSearch(ConfigurationManager.AppSettings["update_message"]);
            var authenticatedUser = Bl.BlSecurity.GetAuthenticatedUser();
            authenticatedUser.ListSize = Convert.ToInt32(_generalInfos.TxtSelectedListSize);
            var savedAgentList = await Bl.BlAgent.UpdateAgentAsync(new List<Agent> { authenticatedUser });
            if (savedAgentList.Count > 0)
                await Dialog.showAsync("List Size saved Successfully!");
            Dialog.IsDialogOpen = false;
        }

        private bool canUpdateListSize(object arg)
        {
            return true;
        }

        private async void eraseLegalInformation(object obj)
        {
            LegalInformationFileManagement.TxtContent = "";
            LegalInformationFileManagement.save();
            await Dialog.showAsync("Legal Information content has been erased Successfully!");
        }

        private bool canEraseLegalInformation(object arg)
        {
            return true;
        }

        private async void eraseSaleGeneralCondition(object obj)
        {
            SaleGeneralConditionFileManagement.TxtContent = "";
            SaleGeneralConditionFileManagement.save();
            await Dialog.showAsync("Sale General Condition content has been erased Successfully!");
        }

        private bool canEraseSaleGeneralCondition(object arg)
        {
            return true;
        }

        private async void updateLegalInformation(object obj)
        {
            Dialog.showSearch(ConfigurationManager.AppSettings["update_message"]);
            bool isSuccessful = LegalInformationFileManagement.save();
            if (isSuccessful)
                await Dialog.showAsync("Legal Information content has been saved Successfully!");
            Dialog.IsDialogOpen = false;
        }

        private bool canUpdateLegalInformation(object arg)
        {
            return true;
        }

        private async void updateSaleGeneralCondition(object obj)
        {
            Dialog.showSearch(ConfigurationManager.AppSettings["update_message"]);
            bool isSuccessful = SaleGeneralConditionFileManagement.save();
            if (isSuccessful)
                await Dialog.showAsync("Sale General Condition content has been saved Successfully!");
            Dialog.IsDialogOpen = false;
        }

        private bool canUpdateSaleGeneralCondition(object arg)
        {
            return true;
        }

        private async void updateBankDetail(object obj)
        {
            Dialog.showSearch(ConfigurationManager.AppSettings["update_message"]);
            var infosList = new InfoManager().GeneralInfo.retrieveInfoDataListFromDictionary(_bankDetails[0].BankDictionary);// _bankDetails[0].extractInfosListFromBankDictionary();
            var infosToUpdateList = infosList.Where(x => x.ID != 0).ToList();
            var infosToCreateList = infosList.Where(x => x.ID == 0).ToList();
            var infosUpdatedList = await Bl.BlReferential.UpdateInfoAsync(infosToUpdateList);
            var infosCreatedList = await Bl.BlReferential.InsertInfoAsync(infosToCreateList);
            if (infosUpdatedList.Count > 0 || infosCreatedList.Count > 0)
            {
                await Dialog.showAsync("Bank Detail saved Successfully!");
                List<Info> savedInfosList = new List<Info>(infosUpdatedList);
                savedInfosList = new List<Info>(savedInfosList.Concat(infosCreatedList));
                BankDetailList = new List<InfoManager.Bank> { new InfoManager.Bank(savedInfosList) };
            }

            Dialog.IsDialogOpen = false;
        }

        private bool canUpdateBankDetail(object arg)
        {
            return true;
        }

        private async void updateAddress(object obj)
        {
            Dialog.showSearch(ConfigurationManager.AppSettings["update_message"]);
            var infosList = new InfoManager().GeneralInfo.retrieveInfoDataListFromDictionary(_addressDetails[0].ContactDictionary);// _addressDetails[0].extractInfosListFromContactDictionary();
            var infosToUpdateList = infosList.Where(x => x.ID != 0).ToList();
            var infosToCreateList = infosList.Where(x => x.ID == 0).ToList();
            var infosUpdatedList = await Bl.BlReferential.UpdateInfoAsync(infosToUpdateList);
            var infosCreatedList = await Bl.BlReferential.InsertInfoAsync(infosToCreateList);
            if (infosUpdatedList.Count > 0 || infosCreatedList.Count > 0)
            {
                await Dialog.showAsync("Address Detail saved Successfully!");
                List<Info> savedInfosList = new List<Info>(infosUpdatedList);
                savedInfosList = new List<Info>(savedInfosList.Concat(infosCreatedList));
                AddressList = new List<InfoManager.Contact> { new InfoManager.Contact(savedInfosList) };
            }
            Dialog.IsDialogOpen = false;
        }

        private bool canUpdateAddress(object arg)
        {
            return true;
        }

        private async void updateEmail(object obj)
        {
            List<Info> updateList = new List<Info>();
            Dialog.showSearch(ConfigurationManager.AppSettings["update_message"]);
            foreach (string filter in _emailfilterList)
            {
                var infosFoundList = Bl.BlReferential.searchInfo(new Info { Name = filter }, ESearchOption.AND);
                if (infosFoundList.Count > 0)
                {
                    switch (filter)
                    {
                        case "email":
                            infosFoundList[0].Value = TxtEmail;
                            break;
                        case "invoice_email":
                            infosFoundList[0].Value = TxtInvoiceEmail;
                            break;
                        case "quote_email":
                            infosFoundList[0].Value = TxtQuoteEmail;
                            break;
                        case "reminder_email":
                            infosFoundList[0].Value = TxtReminderEmail;
                            break;
                        case "validation_email":
                            infosFoundList[0].Value = TxtValidationEmail;
                            break;
                    }
                    updateList.Add(infosFoundList[0]);
                }
            }
            var infosUpdatedList = await Bl.BlReferential.UpdateInfoAsync(updateList);
            if (infosUpdatedList.Count > 0)
                await Dialog.showAsync("Email updated successfully!");
            Dialog.IsDialogOpen = false;
        }

        private bool canUpdateEmail(object arg)
        {
            return true;
        }
    }
}
