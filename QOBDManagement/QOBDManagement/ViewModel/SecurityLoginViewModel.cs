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
            AgentModel.PropertyChanged += onAgentChange_goToHomePage;

            if ((_main.getObject("main") as BindBase) != null)
            {
                (_main.getObject("main") as BindBase).PropertyChanged += onStartupChange;
                (_main.getObject("main") as BindBase).PropertyChanged += onDialogChange;
            }
        }

        private void instances()
        {
            _errorMessage = "";
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

        public async Task showLoginView()
        {
            bool result = await Dialog.showAsync(this);
            if (!string.IsNullOrEmpty(TxtLogin) && !string.IsNullOrEmpty(TxtClearPassword) && result)
            {
                await authenticateAgent();
                if (!Bl.BlSecurity.IsUserAuthenticated())
                    await showLoginView();
            }
            else
                await showLoginView();
        }

        private async Task<object> authenticateAgent()
        {
            try
            {
                var agentFound = await Bl.BlSecurity.AuthenticateUserAsync(TxtLogin, TxtClearPassword);
                if (Bl.BlSecurity.IsUserAuthenticated())
                {
                    if(agentFound.Status.Equals(EStatus.Active.ToString()))
                    {
                        AgentModel.Agent = agentFound;
                        TxtLogin = "";
                        TxtClearPassword = "";
                        TxtErrorMessage = "";
                    }
                    else
                        TxtErrorMessage = "Your profile has been Deactivated!";
                }
                else
                    TxtErrorMessage = "Your User Name or password is incorrect!";
            }
            catch (ApplicationException)
            {
                await Dialog.showAsync("This application requires an internet connection!"+Environment.NewLine 
                    + "Please check your internet connection." );
                await showLoginView();
            }             
            
            return null;
        }

        public async Task startAuthentication()
        {
            TxtLogin = "";// "<< Login here for dev mode >>";
            TxtClearPassword = ""; //"<< Password here for dev mode >>";
            await authenticateAgent();
        }

        public override void Dispose()
        {
            if ((_main.getObject("main") as BindBase) != null)
            {
                (_main.getObject("main") as BindBase).PropertyChanged -= onStartupChange;
                (_main.getObject("main") as BindBase).PropertyChanged -= onDialogChange;
            }

            AgentModel.PropertyChanged -= onAgentChange_goToHomePage;
        }

        //----------------------------[ Event Handler ]------------------
        
        /// <summary>
        /// get the password from the authentication dialog box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void onPwdBoxPasswordChange_updateTxtClearPassword(object sender, RoutedEventArgs e)
        {
            TxtClearPassword = ((PasswordBox)sender).Password;
        }

        /// <summary>
        /// Display the home page if the user is authenticated 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onAgentChange_goToHomePage(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Agent") && Bl.BlSecurity.IsUserAuthenticated())
                _page(new HomeViewModel());
        }        

        /// <summary>
        /// Initialize the business logic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onStartupChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Startup"))
            {
                Startup = (_main.getObject("main") as BindBase).Startup;
            }
        }

        /// <summary>
        /// Show the login page when the Dialog box is ready
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void onDialogChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Dialog"))
            {
                Dialog = (_main.getObject("main") as BindBase).Dialog;
                if (Application.Current != null)
                    await Application.Current.Dispatcher.Invoke(async ()=> {
                        await showLoginView();
                        //await startAuthentication(); //"<< for dev mode >>";
                    });  
            }
        }

        //----------------------------[ Action Commands ]------------------

        private async void logOut(object obj)
        {
            Bl.BlSecurity.DisconnectAuthenticatedUser();
            await Task.Factory.StartNew(()=> {
                _main.ChatRoomViewModel.Dispose();
            });
            AgentModel.Agent = new QOBDCommon.Entities.Agent();
            await showLoginView();
        }

        private bool canLogOut(object arg)
        {
            return true;
        }




    }
}
