using QOBDBusiness;
using QOBDCommon.Entities;
using QOBDManagement.Classes;
using QOBDManagement.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QOBDManagement.Interfaces;
using QOBDCommon.Enum;

namespace QOBDManagement.ViewModel
{
    public class OptionEmailViewModel : BindBase
    {
        Dictionary<string, GeneralInfos.FileWriter> _emails;
        private string _title;
        private IMainWindowViewModel _main;
        
        //----------------------------[ Commands ]------------------

        public ButtonCommand<string> DeleteCommand { get; set; }
        public ButtonCommand<string> UpdateCommand { get; set; }


        public OptionEmailViewModel()
        {
            instances();
            instancesCommand();
        }

        public OptionEmailViewModel(IMainWindowViewModel main): this()
        {
            _main = main;            
        }


        //----------------------------[ Initialization ]------------------

        private void instances()
        {
            _title = "Email Management";
            _emails = new Dictionary<string, GeneralInfos.FileWriter>();
            _emails["quote"] = new GeneralInfos.FileWriter("quote", EOption.mails);
            _emails["reminder_1"] = new GeneralInfos.FileWriter("reminder_1", EOption.mails);
            _emails["reminder_2"] = new GeneralInfos.FileWriter("reminder_2", EOption.mails);
            _emails["bill"] = new GeneralInfos.FileWriter("bill", EOption.mails);
            _emails["order_confirmation"] = new GeneralInfos.FileWriter("order_confirmation", EOption.mails);
        }

        private void instancesCommand()
        {
            UpdateCommand = new ButtonCommand<string>(updateEmailFiles, canUpdateEmailFiles);
            DeleteCommand = new ButtonCommand<string>(eraseContent, canEraseContent);
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

        public GeneralInfos.FileWriter OrderConfirmationEmailFile
        {
            get { return _emails["order_confirmation"]; }
            set { _emails["order_confirmation"] = value; onPropertyChange("OrderConfirmationEmailFile"); }
        }

        public GeneralInfos.FileWriter BillEmailFile
        {
            get { return _emails["bill"]; }
            set { _emails["bill"] = value; onPropertyChange("BillEmailFile"); }
        }

        public GeneralInfos.FileWriter ReminderTwoEmailFile
        {
            get { return _emails["reminder_2"]; }
            set { _emails["reminder_2"] = value; onPropertyChange("ReminderTwoEmailFile"); }
        }

        public GeneralInfos.FileWriter ReminderOneEmailFile
        {
            get { return _emails["reminder_1"]; }
            set { _emails["reminder_1"] = value; onPropertyChange("ReminderEmailFile"); }
        }

        public GeneralInfos.FileWriter QuoteEmailFile
        {
            get { return _emails["quote"]; }
            set { _emails["quote"] = value; onPropertyChange("QuoteEmailFile"); }
        }

        //----------------------------[ Actions ]------------------

        public void loadData()
        {
            Dialog.showSearch("Loading...");
            
            string login = (_startup.Bl.BlReferential.searchInfo(new QOBDCommon.Entities.Info { Name = "ftp_login" }, ESearchOption.OR).FirstOrDefault() ?? new Info()).Value;
            string password = ( _startup.Bl.BlReferential.searchInfo(new QOBDCommon.Entities.Info { Name = "ftp_password" }, ESearchOption.OR).FirstOrDefault() ?? new Info()).Value;

            foreach (var email in _emails)
            {
                email.Value.TxtLogin = login;
                email.Value.TxtPassword = password;
                email.Value.read();
            }

            Dialog.IsDialogOpen = false;
        }

        //----------------------------[ Action Commands ]------------------
        
        private void eraseContent(string obj)
        {
            switch (obj)
            {
                case "bill":
                    _emails["bill"].TxtContent = "";
                    break;
                case "reminder-2":
                    _emails["reminder_2"].TxtContent = "";
                    break;
                case "reminder-1":
                    _emails["reminder_1"].TxtContent = "";
                    break;
                case "order-confirmation":
                    _emails["order_confirmation"].TxtContent = "";
                    break;
                case "quote":
                    _emails["quote"].TxtContent = "";
                    break;
            }
        }

        private bool canEraseContent(string arg)
        {
            bool isWrite = _main.securityCheck(QOBDCommon.Enum.EAction.Option, QOBDCommon.Enum.ESecurity._Write);
            bool isUpdate = _main.securityCheck(QOBDCommon.Enum.EAction.Option, QOBDCommon.Enum.ESecurity._Update);
            if (isUpdate && isWrite)
                return true;
            return false;
        }

        private async void updateEmailFiles(string obj)
        { 
            switch (obj)
            {
                case "bill":
                    if (_emails["bill"].save())
                        await Dialog.show("Email Bill saved Successfully!");
                    break;
                case "reminder-2":
                    if (_emails["reminder_2"].save())
                        await Dialog.show("Email first Bill reminder saved Successfully!");
                    break;
                case "reminder-1":
                    if (_emails["reminder_1"].save())
                        await Dialog.show("Email second Bill reminder saved Successfully!");
                    break;
                case "order-confirmation":
                    if (_emails["order_confirmation"].save())
                        await Dialog.show("Email validation Order confirmation saved Successfully!");
                    break;
                case "quote":
                    if (_emails["quote"].save())
                        await Dialog.show("Email Quote saved Successfully!");
                    break;
            }
        }

        private bool canUpdateEmailFiles(string arg)
        {
            bool isWrite = _main.securityCheck(QOBDCommon.Enum.EAction.Option, QOBDCommon.Enum.ESecurity._Write);
            bool isUpdate = _main.securityCheck(QOBDCommon.Enum.EAction.Option, QOBDCommon.Enum.ESecurity._Update);
            if (isUpdate && isWrite)
                return true;
            return false;
        }
    }
}
