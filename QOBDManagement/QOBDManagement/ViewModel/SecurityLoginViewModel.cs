using QOBDBusiness;
using QOBDManagement.Classes;
using QOBDManagement.Command;
using QOBDManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using MaterialDesignThemes.Wpf;
using System.Windows;
using QOBDCommon.Classes;
using System.Threading;
using System.Windows.Controls;
using QOBDCommon.Enum;
using QOBDManagement.Interfaces;

namespace QOBDManagement.ViewModel
{
    public class SecurityLoginViewModel : BindBase, ISecurityLoginViewModel
    {
        private Func<Object, Object> _page;
        private string _errorMessage;
        private NotifyTaskCompletion<bool> _DialogtaskCompletion;
        private string _clearPassword;
        private string _login;

        //----------------------------[ Models ]------------------

        private AgentModel _agentModel;
        private IMainWindowViewModel _main;

        //----------------------------[ Commands ]------------------

        public ButtonCommand<object> LogoutCommand { get; set; }


        public SecurityLoginViewModel()
        {
            instances();
            instancesModel();
            instancesCommand();  
        }

        public SecurityLoginViewModel(IMainWindowViewModel mainWindowViewModel): this()
        {
            _main = mainWindowViewModel;
            _page = _main.navigation;
            initEvents();
        }

        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            AgentModel.PropertyChanged += onAgentChange;
            AgentModel.PropertyChanged += onAgentChange_goToHomePage;
            _DialogtaskCompletion.PropertyChanged += onDialogDisplayTaskComplete_authenticateUser;

            if ((_main.getObject("main") as BindBase) != null)
            {
                (_main.getObject("main") as BindBase).PropertyChanged += onStartupChange;
                (_main.getObject("main") as BindBase).PropertyChanged += onDialogChange;
            }
        }

        private void instances()
        {
            _errorMessage = "";
            _DialogtaskCompletion = new NotifyTaskCompletion<bool>();
            LogoutCommand = new ButtonCommand<object>(logOut, canLogOut);
        }

        private void instancesModel()
        {
            _agentModel = new AgentModel();
        }

        private void instancesCommand()
        {

        }
        //----------------------------[ Properties ]------------------

        public AgentModel AgentModel
        {
            get { return _agentModel; }
            set { setProperty(ref _agentModel, value, "AgentModel"); }
        }

        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
            set { _startup.Bl = value; onPropertyChange("Bl"); }
        }

        public string TxtErrorMessage
        {
            get { return _errorMessage; }
            set { setProperty(ref _errorMessage, value, "TxtErrorMessage"); }
        }

        public string TxtClearPassword
        {
            get { return _clearPassword; }
            set { _clearPassword = value; onPropertyChange("TxtClearPassword"); }
        }

        public string TxtLogin
        {
            get { return _login; }
            set { _login = value; onPropertyChange("TxtLogin"); }
        }

        //----------------------------[ Actions ]------------------


        public void showLoginView()
        {
            _DialogtaskCompletion.initializeNewTask(Dialog.show(this));
        }
        
        private async Task<object> authenticateAgent()
        {
            try
            {
                var agentFound = await Bl.BlSecurity.AuthenticateUserAsync(TxtLogin, TxtClearPassword);
                if (agentFound != null && agentFound.ID != 0 && agentFound.Status.Equals(EStatus.Active.ToString()))
                {
                    AgentModel.Agent = agentFound;
                    TxtLogin = "";
                    TxtClearPassword = "";
                    TxtErrorMessage = "";
                }
                else
                {
                    TxtErrorMessage = "Your User Name or password is incorrect or Deactivated!";
                }
            }
            catch (Exception)
            {
                await Dialog.show("This application requires internet connection!"+Environment.NewLine 
                    + "Please check your internet connection." );
            }             
            
            return null;
        }

        public async void startAuthentication()
        {
            TxtLogin = "demo";// "<< Login here for dev mode >>";
            TxtClearPassword = "demo"; //"<< Password here for dev mode >>";
            await authenticateAgent();
        }

        public override void Dispose()
        {
            if ((_main.getObject("main") as BindBase) != null)
            {
                (_main.getObject("main") as BindBase).PropertyChanged -= onStartupChange;
                (_main.getObject("main") as BindBase).PropertyChanged -= onDialogChange;
            }
            AgentModel.PropertyChanged -= onAgentChange;
            AgentModel.PropertyChanged -= onAgentChange_goToHomePage;
            _DialogtaskCompletion.PropertyChanged -= onDialogDisplayTaskComplete_authenticateUser;
        }

        //----------------------------[ Event Handler ]------------------

        private void onAgentChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Agent"))
            {
                _page(new HomeViewModel());
            }
        }

        internal void onPwdBoxPasswordChange_updateTxtClearPassword(object sender, RoutedEventArgs e)
        {
            TxtClearPassword = ((PasswordBox)sender).Password;
        }

        private void onAgentChange_goToHomePage(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Agent"))
                _page(new HomeViewModel());
        }

        private async void onDialogDisplayTaskComplete_authenticateUser(object sender, PropertyChangedEventArgs e)
        {

            if (e.PropertyName.Equals("IsSuccessfullyCompleted"))
            {
                bool result = _DialogtaskCompletion.Result;
                if (!string.IsNullOrEmpty(TxtLogin) && !string.IsNullOrEmpty(TxtClearPassword) && result)
                {
                    await authenticateAgent();
                    if (AgentModel.Agent.ID == 0)
                    {
                        showLoginView();
                    }
                }                    
                else
                    showLoginView();
            }
        }
        

        private void onStartupChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Startup"))
            {
                Startup = (_main.getObject("main") as BindBase).Startup;
            }
        }

        private void onDialogChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Dialog"))
            {
                Dialog = (_main.getObject("main") as BindBase).Dialog;
                //showLoginView();
                startAuthentication();
            }
        }

        //----------------------------[ Action Commands ]------------------

        private void logOut(object obj)
        {
            AgentModel.Agent = new QOBDCommon.Entities.Agent();
            showLoginView();
        }

        private bool canLogOut(object arg)
        {
            return true;
        }




    }
}
