using QOBDBusiness;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDManagement.Classes;
using QOBDManagement.Command;
using QOBDManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QOBDManagement.Interfaces;
using QOBDCommon.Classes;

namespace QOBDManagement.ViewModel
{
    public class AgentSideBarViewModel : BindBase
    {
        private Func<object, object> _page;

        //----------------------------[ Models ]------------------

        private AgentModel _selectedAgentModel;
        private IMainWindowViewModel _main;

        //----------------------------[ Commands ]------------------

        public ButtonCommand<string> SetupAgentCommand { get; set; }
        public ButtonCommand<string> UtilitiesCommand { get; set; }


        public AgentSideBarViewModel()
        {
            instancesCommand();
            initEvents();
        }

        public AgentSideBarViewModel(IMainWindowViewModel mainWindowViewModel) :this()
        {
            this._main = mainWindowViewModel;
            _page = mainWindowViewModel.navigation;
        }

        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            PropertyChanged += onSelectedAgentModelChange;
        }

        private void instancesCommand()
        {
            SetupAgentCommand = new ButtonCommand<string>(executeSetupAction, canExcecuteSetupAction);
            UtilitiesCommand = new ButtonCommand<string>(executeUtilityAction, canExecuteUtilityAction);
        }


        //----------------------------[ Properties ]------------------

        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
        }

        public string TxtIconColour
        {
            get { return Utility.getRandomColour(); }
        }

        public AgentModel SelectedAgentModel
        {
            get { return _selectedAgentModel; }
            set { _selectedAgentModel = value; onPropertyChange("SelectedAgentModel"); }
        }

        public Func<object, object> Page
        {
            get { return _page; }
            set { _page = value; onPropertyChange("Page"); }
        }

        //----------------------------[ Actions ]------------------

        private async Task<Agent> loadNewUser(Agent agent)
        {
            Agent newAgent = new Agent();
            if (_main != null)
            {
                await Bl.BlSecurity.DisconnectAuthenticatedUser();
                _main.Context = new Context(_page);
                await Task.Factory.StartNew(() => {
                    _main.ChatRoomViewModel.Dispose();
                });
                newAgent = await Bl.BlSecurity.UseAgentAsync(agent);
                if (Bl.BlSecurity.IsUserAuthenticated())
                {
                    _main.isNewAgentAuthentication = true;
                    _main.SecurityLoginViewModel.AgentModel.Agent = Bl.BlSecurity.GetAuthenticatedUser();
                }
            }
            return newAgent;
        }

        private void updateCommand()
        {
            UtilitiesCommand.raiseCanExecuteActionChanged();
            SetupAgentCommand.raiseCanExecuteActionChanged();
        }

        public override void Dispose()
        {
            PropertyChanged -= onSelectedAgentModelChange;
        }

        //----------------------------[ Event Handler ]------------------
        
        public void onCurrentPageChange_updateCommand(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("CurrentViewModel")
                && ((_main.CurrentViewModel as AgentDetailViewModel) != null)
                || (_main.CurrentViewModel as AgentViewModel) != null)
                updateCommand();
        }

        private void onSelectedAgentModelChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SelectedAgentModel"))
                updateCommand();
        }

        //----------------------------[ Action Commands ]------------------


        private bool canExecuteUtilityAction(string arg)
        {
            bool canUpdate = _main.securityCheck(EAction.Agent, ESecurity._Update);
            bool canWrite = _main.securityCheck(EAction.Agent, ESecurity._Write);
            bool canDelete = _main.securityCheck(EAction.Agent, ESecurity._Delete);
            bool canRead = _main.securityCheck(EAction.Agent, ESecurity._Read);
            bool isUserAdmin = _main.AgentViewModel.IsAuthenticatedAgentAdmin;

            if (!canUpdate || !canWrite)
                return false;

            if (arg.Equals("agent") && Page(null) as AgentViewModel != null)
                return false;

            if (Page(null) as AgentDetailViewModel == null)
                return false;
            
            if (SelectedAgentModel == null || SelectedAgentModel.Agent.ID == 0)
                return false;

            if (!isUserAdmin && 
                (arg.Equals("use")
                || arg.Equals("activate")
                || arg.Equals("deactivate")))
                return false;

            if (SelectedAgentModel.TxtStatus.Equals(EStatus.Active.ToString())
                && arg.Equals("activate"))
                return false;

            if (SelectedAgentModel.TxtStatus.Equals(EStatus.Deactivated.ToString())
                && arg.Equals("deactivate"))
                return false;

            return true;
        }
        
        private async void executeUtilityAction(string obj)
        {
            List<Agent> updatedAgentList = new List<Agent>();
            switch (obj)
            {
                case "activate": // change the agent status to active
                    if (await Dialog.showAsync("Do you confirm " + SelectedAgentModel.TxtLogin + " profile activation?"))
                    {
                        Dialog.showSearch("Activating Status...");
                        SelectedAgentModel.TxtStatus = EStatus.Active.ToString();
                        updatedAgentList = await Bl.BlAgent.UpdateAgentAsync(new List<Agent> { SelectedAgentModel.Agent });
                        if (updatedAgentList.Count > 0)
                            await Dialog.showAsync("The Agent " + updatedAgentList[0].LastName + "has been successfully activated!");
                    }                    
                    break;
                case "deactivate": // change the agent status to deactivated
                    if (await Dialog.showAsync("Do you confirm "+ SelectedAgentModel.TxtLogin + " profile deactivation?"))
                    {
                        Dialog.showSearch("Deactivating Status...");
                        SelectedAgentModel.TxtStatus = EStatus.Deactivated.ToString();
                        updatedAgentList = await Bl.BlAgent.UpdateAgentAsync(new List<Agent> { SelectedAgentModel.Agent });
                        if (updatedAgentList.Count > 0)
                            await Dialog.showAsync("The Agent " + updatedAgentList[0].LastName + " has been successfully deactivated!");
                    }
                    break;
                case "use": // connect a new user
                    if (await Dialog.showAsync("Do you confirm reconnecting as user " + SelectedAgentModel.TxtLogin + "?"))
                    {
                        Dialog.showSearch("Please wait while we are dealing with your request...");
                        var newAgent = await loadNewUser(SelectedAgentModel.Agent);
                        if (newAgent.ID != 0)
                            await Dialog.showAsync("Your are successfully connected as " + newAgent.FirstName + " " + newAgent.LastName);
                    }                    
                   break;
                case "agent": // connect a new user
                    _page(_main.AgentViewModel);
                    break;
            }
            Dialog.IsDialogOpen = false;
            UtilitiesCommand.raiseCanExecuteActionChanged();
        }

        private bool canExcecuteSetupAction(string arg)
        {
            bool canUpdate = _main.securityCheck(EAction.Agent, ESecurity._Update);
            bool canWrite = _main.securityCheck(EAction.Agent, ESecurity._Write);
            if (!canWrite || !canUpdate && Bl.BlSecurity.GetAuthenticatedUser().ID != SelectedAgentModel.Agent.ID)
                return false;
            return true;
        }

        private void executeSetupAction(string obj)
        {
            switch (obj)
            {
                case "new-agent":
                    if (_main != null)
                    {
                        _main.AgentViewModel.AgentDetailViewModel.SelectedAgentModel = SelectedAgentModel = new AgentModel();
                        Page(new AgentDetailViewModel());
                    }
                    break;
            }
        }
    }
}
