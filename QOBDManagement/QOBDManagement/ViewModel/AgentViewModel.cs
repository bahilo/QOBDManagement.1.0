using QOBDCommon.Entities;
using QOBDManagement.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QOBDBusiness;
using QOBDManagement.Classes;
using System.Collections;
using QOBDManagement.Models;
using QOBDCommon.Enum;
using QOBDManagement.Interfaces;
using System.Windows.Threading;

namespace QOBDManagement.ViewModel
{
    public class AgentViewModel : BindBase, IAgentViewModel
    {
        private List<string> _saveSearchParametersList;
        private Func<Object, Object> _page;    
        private List<Agent> _agents;
        private List<string> _chatUserGroupList;
        private List<Agent> _clientAgentToMoveList;
        private string _title;

        //----------------------------[ Models ]------------------

        private AgentModel _agentModel;
        private AgentDetailViewModel _agentDetailViewModel;
        private IMainWindowViewModel _main;
        public IDiscussionViewModel _chatDiscussionViewModel;

        //----------------------------[ Commands ]------------------

        public ButtonCommand<AgentModel> CheckBoxCommand { get; set; }
        public ButtonCommand<string> NavigCommand { get; set; }
        public ButtonCommand<AgentModel> GetCurrentAgentCommand { get; set; }
        public ButtonCommand<AgentModel> ClientMoveCommand { get; set; }


        public AgentViewModel()
        {
            instances();
            instancesCommand();
        }

        public AgentViewModel(IMainWindowViewModel mainWindowViewModel): this()
        {
            _main = mainWindowViewModel;
            _page = _main.navigation;
            instancesModel(mainWindowViewModel);
            if ((_main.getObject("main") as BindBase) != null)
            {
                (_main.getObject("main") as BindBase).PropertyChanged += onStartupChange;
                (_main.getObject("main") as BindBase).PropertyChanged += onDialogChange;
            }
        }

        public AgentViewModel(IMainWindowViewModel mainWindowViewModel, IDiscussionViewModel discussion) : this(mainWindowViewModel)
        {
            _chatDiscussionViewModel = discussion;
        }

        //----------------------------[ Initialization ]------------------

        private void instances()
        {
            _title = "Agent Management";
            _saveSearchParametersList = new List<string>();
            _clientAgentToMoveList = new List<Agent>();
            _agents = new List<Agent>();
        }
        

        private void instancesModel(IMainWindowViewModel mainWindowViewModel)
        {
            _agentModel = new AgentModel();
            _agentDetailViewModel = new AgentDetailViewModel(mainWindowViewModel);
            AgentSideBarViewModel = new AgentSideBarViewModel(mainWindowViewModel);
        }

        private void instancesCommand()
        {
            ClientMoveCommand = new ButtonCommand<AgentModel>(moveAgentCLient, canMoveClientAgent);
            CheckBoxCommand = new ButtonCommand<AgentModel>(saveResultGridChecks, canSaveResultGridChecks);
            NavigCommand = new ButtonCommand<string>(executeNavig, canExecuteNavig);
            GetCurrentAgentCommand = new ButtonCommand<AgentModel>(selectAgent, canSelectAgent);         
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

        public AgentModel AgentModel
        {
            get { return _agentModel; }
            set { _agentModel = value; onPropertyChange(); }
        }

        public List<AgentModel> AgentModelList
        {
            get { return _agentDetailViewModel.AgentModelList; }
            set { _agentDetailViewModel.AgentModelList = value; onPropertyChange(); onPropertyChange("UserModelList"); onPropertyChange("ActiveAgentModelList"); onPropertyChange("DeactivatedAgentModelList"); }
        }

        public List<AgentModel> ActiveAgentModelList
        {
            get { return AgentModelList.Where(x => x.TxtStatus.Equals(EStatus.Active.ToString())).ToList(); }
        }

        public List<AgentModel> DeactivatedAgentModelList
        {
            get { return AgentModelList.Where(x => x.TxtStatus.Equals(EStatus.Deactivated.ToString())).ToList(); }
        }

        public AgentModel SelectedAgentModel
        {
            get { return AgentDetailViewModel.SelectedAgentModel; }
            set { AgentDetailViewModel.SelectedAgentModel = value; onPropertyChange("SelectedAgentModel"); }
        }

        public AgentDetailViewModel AgentDetailViewModel
        {
            get { return _agentDetailViewModel; }
            set { _agentDetailViewModel = value; onPropertyChange("AgentDetailViewModel"); }
        }

        public AgentSideBarViewModel AgentSideBarViewModel
        {
            get { return _agentDetailViewModel.AgentSideBarViewModel; }
            set { _agentDetailViewModel.AgentSideBarViewModel = value; onPropertyChange("AgentSideBarViewModel"); }
        }

        public List<AgentModel> UserModelList
        {
            get { return AgentModelList.Where(x => x.Agent.ID != Bl.BlSecurity.GetAuthenticatedUser().ID).OrderBy(x => x.Agent.IsOnline).ToList(); }
        }

        public List<string> UserGroupList
        {
            get { return _chatUserGroupList; }
            set { setProperty(ref _chatUserGroupList, value); }
        }



        //----------------------------[ Actions ]------------------

        /// <summary>
        /// Convert the list of agent received from the web service into 
        /// a list of agent model used by the agent view
        /// </summary>
        /// <param name="AgentList">data from the web service</param>
        /// <returns></returns>
        public List<AgentModel> agentListToModelViewList(List<Agent> AgentList)
        {
            List<AgentModel> output = new List<AgentModel>();
            foreach (Agent Agent in AgentList)
            {
                AgentModel avm = new AgentModel();
                avm.Agent = Agent;
                _agents.Add(Agent);
                output.Add(avm);
            }
            return output;
        }

        /// <summary>
        /// initialize the agent view (called by the view code behind)
        /// </summary>
        public void loadAgents()
        {
            Dialog.showSearch("loading...");
            AgentModelList = agentListToModelViewList(Bl.BlAgent.GetAgentData(999));
            Dialog.IsDialogOpen = false;
        } 
        
        public async void getAgentOnlineStatus()
        {
            // getting the agents from the web service
            AgentModelList = agentListToModelViewList(await Bl.BlAgent.GetAgentDataAsync(-999));
        }       

        public override void Dispose()
        {
            if ((_main.getObject("main") as BindBase) != null)
            {
                (_main.getObject("main") as BindBase).PropertyChanged -= onStartupChange;
                (_main.getObject("main") as BindBase).PropertyChanged -= onDialogChange;
            }

            Bl.BlAgent.Dispose();
            AgentDetailViewModel.Dispose();
            AgentSideBarViewModel.Dispose();
        }


        //----------------------------[ Event Handler ]------------------
        
        private void onStartupChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Startup"))
            {
                Startup = (_main.getObject("main") as BindBase).Startup;
                AgentDetailViewModel.Startup = Startup;
                AgentSideBarViewModel.Startup = Startup;
            }
        }

        private void onDialogChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Dialog"))
            {
                Dialog = (_main.getObject("main") as BindBase).Dialog;
                AgentDetailViewModel.Dialog = Dialog;
                AgentSideBarViewModel.Dialog= Dialog;
            }
        }
        

        //----------------------------[ Action Commands ]------------------

        public async void moveAgentCLient(AgentModel obj)
        {
            Dialog.showSearch("Moving CLients to "+obj.TxtLastName+" in progress...");
            List<Client> clientMovedList = new List<Client>();
            if(obj != null)
                foreach (var fromAgent in _clientAgentToMoveList)
                {
                    clientMovedList = clientMovedList.Concat( await Bl.BlAgent.MoveAgentClient(fromAgent, obj.Agent)).ToList();
                }
            if (clientMovedList.Count > 0)
                await Dialog.showAsync(string.Format("CLients moved successfully to {0}.", obj.TxtLastName));
            _clientAgentToMoveList.Clear();
            Dialog.IsDialogOpen = false;
            _page(this);
        }

        private bool canMoveClientAgent(AgentModel arg)
        {
            bool _isUserAdmin = _main.securityCheck(QOBDCommon.Enum.EAction.Security, QOBDCommon.Enum.ESecurity.SendEmail)
                             && _main.securityCheck(QOBDCommon.Enum.EAction.Security, QOBDCommon.Enum.ESecurity._Delete)
                                 && _main.securityCheck(QOBDCommon.Enum.EAction.Security, QOBDCommon.Enum.ESecurity._Read)
                                     && _main.securityCheck(QOBDCommon.Enum.EAction.Security, QOBDCommon.Enum.ESecurity._Update)
                                         && _main.securityCheck(QOBDCommon.Enum.EAction.Security, QOBDCommon.Enum.ESecurity._Write);

            if (_isUserAdmin)
                return true;

            return false;
        }

        public void saveResultGridChecks(AgentModel param)
        {
            if (!_clientAgentToMoveList.Contains(param.Agent))
                _clientAgentToMoveList.Add(param.Agent);
            else
                _clientAgentToMoveList.Remove(param.Agent);
        }

        private bool canSaveResultGridChecks(AgentModel arg)
        {
            return true;
        }
        
        public void selectAgent(AgentModel obj)
        {
            SelectedAgentModel = obj;
            executeNavig("agent-detail");
        }

        private bool canSelectAgent(AgentModel arg)
        {
            return true;
        }

        public void executeNavig(string obj)
        {
            switch (obj)
            {
                case "agent":
                    _page(this);
                    break;
                case "agent-detail":
                    _page(AgentDetailViewModel);
                    break;
                default:
                    goto case "agent";
            }
        }

        private bool canExecuteNavig(string arg)
        {
            return true;
        }

        


    }
}
