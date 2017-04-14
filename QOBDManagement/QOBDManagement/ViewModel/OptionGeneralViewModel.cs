using QOBDBusiness;
using QOBDCommon.Entities;
using QOBDManagement.Classes;
using QOBDManagement.Command;
using QOBDManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using QOBDManagement.Interfaces;
using QOBDCommon.Enum;
using System.Runtime.CompilerServices;

namespace QOBDManagement.ViewModel
{
    public class OptionGeneralViewModel : BindBase
    {
        private Func<object, object> _page;
        private List<InfoManager.Bank> _bankDetails;
        private List<InfoManager.Contact> _addressDetails;
        private GeneralInfos _generalInfos;
        private InfoManager.FileWriter _legalInformationFileManagement;
        private InfoManager.FileWriter _saleGeneralConditionFileManagement;
        private string _validationEmail;
        private string _reminderEmail;
        private string _invoiceEmail;
        private string _quoteEmail;
        private string _email;
        private List<string> _emailfilterList;
        private string _title;

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
        public ButtonCommand<object> UpdateLegalInformationCommand { get; set; }
        public ButtonCommand<object> UpdateSaleGeneralConditionCommand { get; set; }
        public ButtonCommand<object> NewLegalInformationCommand { get; set; }
        public ButtonCommand<object> NewSaleGeneralConditionCommand { get; set; }
        public ButtonCommand<object> UpdateEmailCommand { get; set; }



        public OptionGeneralViewModel()
        {
            instances();
            instancesModel();
            instancesCommand();
        }

        public OptionGeneralViewModel(IMainWindowViewModel main): this()
        {
            _main = main;
            _page = _main.navigation;
        }

        //----------------------------[ Initialization ]------------------
        
        private void instances()
        {
            _legalInformationFileManagement = new InfoManager.FileWriter("legal_information", EOption.texts);
            _saleGeneralConditionFileManagement = new InfoManager.FileWriter("sale_general_condition", EOption.texts);
            _addressDetails = new List<InfoManager.Contact>();
            _bankDetails = new List<InfoManager.Bank>();
            _generalInfos = new GeneralInfos();
            _emailfilterList = new List<string> { "email", "invoice_email", "quote_email", "reminder_email", "validation_email" };
            _title = "Option Management";
        }

        private void instancesModel()
        {
            _taxModel = new TaxModel();
            _taxes = new List<TaxModel>();
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
            UpdateEmailCommand = new ButtonCommand<object>(updateEmail, canUpdateEmail);
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
            set { setProperty(ref _taxModel, value); }
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

        public async void loadData()
        {
            Dialog.showSearch("Loading...");

            var userListSizeFoundList = _generalInfos.ListSizeList.Where(x => x.Equals(Bl.BlSecurity.GetAuthenticatedUser().ListSize)).ToList();
            TxtSelectedListSize = (userListSizeFoundList.Count > 0) ? userListSizeFoundList[0] : 0;
            TaxList = TaxListToTaxModelList(await Bl.BlOrder.GetTaxDataAsync(999));

            var infosFoundList = Bl.BlReferential.GetInfoData(999);
            BankDetailList = new List<InfoManager.Bank> { new InfoManager.Bank(infosFoundList) };
            AddressList = new List<InfoManager.Contact> { new InfoManager.Contact(infosFoundList) };

            LoadEmail();

            string login = (_startup.Bl.BlReferential.searchInfo(new QOBDCommon.Entities.Info { Name = "ftp_login" }, ESearchOption.OR).FirstOrDefault() ?? new Info()).Value;
            string password = (_startup.Bl.BlReferential.searchInfo(new QOBDCommon.Entities.Info { Name = "ftp_password" }, ESearchOption.OR).FirstOrDefault() ?? new Info()).Value;

            SaleGeneralConditionFileManagement.TxtLogin = LegalInformationFileManagement.TxtLogin = login;
            SaleGeneralConditionFileManagement.TxtPassword = LegalInformationFileManagement.TxtPassword = password;

            LegalInformationFileManagement.read();
            SaleGeneralConditionFileManagement.read();
            
            Dialog.IsDialogOpen = false;
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

        //----------------------------[ Action Commands ]------------------


        private async void deleteTax(TaxModel obj)
        {
            Dialog.showSearch("Tax deleting...");
            var NotDeletedTax = await Bl.BlOrder.DeleteTaxAsync(new List<QOBDCommon.Entities.Tax> { obj.Tax });
            if (NotDeletedTax.Count == 0)
            {
                await Dialog.showAsync("Tax Deleted Successfully!");
                TaxList.Remove(obj);
                TaxList = new List<TaxModel>(TaxList);
            }
            Dialog.IsDialogOpen = false;
        }

        private bool canDeleteTax(TaxModel arg)
        {
            return true;
        }

        private async void addTax(object obj)
        {
            Dialog.showSearch("Tax creation...");
            TaxModel.TxtDate = DateTime.Now.ToString();
            var savedTaxList = await Bl.BlOrder.InsertTaxAsync(new List<QOBDCommon.Entities.Tax> { TaxModel.Tax });
            if (savedTaxList.Count > 0)
            {
                await Dialog.showAsync("Tax added Successfully!");
                TaxList = new List<TaxModel>(TaxList.Concat(TaxListToTaxModelList(savedTaxList)));
            }
            Dialog.IsDialogOpen = false;
        }

        private bool canAddTax(object arg)
        {
            return true;
        }

        private async void updateListSize(object obj)
        {
            Dialog.showSearch("List size updating...");
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
            Dialog.showSearch("Legal information updating...");
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
            Dialog.showSearch("Sale General Condition updating...");
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
            Dialog.showSearch("Bank details updating...");
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
            Dialog.showSearch("Address updating...");
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
            Dialog.showSearch("Updating email...");
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
