﻿using MaterialDesignThemes.Wpf;
using QOBDBusiness;
using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDManagement.Classes;
using QOBDManagement.Command;
using QOBDManagement.Interfaces;
using QOBDManagement.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace QOBDManagement.ViewModel
{
    public class ClientViewModel : BindBase, IClientViewModel
    {
        private List<string> _idList;
        private List<string> _saveSearchParametersList;
        private List<string> _companyList;
        private Func<Object, Object> _page;
        //private NotifyTaskCompletion<ClientModel> _selectedCLientTask;
        private string _title;
        private List<Agent> _agentList;
        private List<Address> _addressList;
        private List<Client> _clientList;
        private List<Client> _saveResultParametersList;

        //----------------------------[ Models ]------------------

        private ClientModel _clientModel;
        private List<ClientModel> _clientsModel;
        private ClientDetailViewModel _clientDetailViewModel;
        private IMainWindowViewModel _main;
        public CLientSideBarViewModel ClientSideBarViewModel { get; set; }

        //----------------------------[ Commands ]------------------

        public ButtonCommand<ClientModel> checkBoxResultGridCommand { get; set; }
        public ButtonCommand<string> checkBoxSearchCommand { get; set; }
        public ButtonCommand<string> rBoxSearchCommand { get; set; }
        public ButtonCommand<Agent> btnComboBxCommand { get; set; }
        public ButtonCommand<string> btnSearchCommand { get; set; }
        public ButtonCommand<string> NavigCommand { get; set; }
        public ButtonCommand<ClientModel> GetCurrentItemCommand { get; set; }
                

        public ClientViewModel()
        {
            instances();
            instancesCommand();
        }

        public ClientViewModel(MainWindowViewModel mainWindowViewModel): this()
        {
            this._main = mainWindowViewModel;
            _page = _main.navigation;
            instancesModel(mainWindowViewModel);
            _clientDetailViewModel.Cart = _main.Cart;
            initEvents();
        }

        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            //_selectedCLientTask.PropertyChanged += onSelectedCLientTaskCompletion_saveSelectedClient;
            _clientDetailViewModel.PropertyChanged += onSelectedClientChange;

            if ((_main.getObject("main") as BindBase) != null)
            {
                (_main.getObject("main") as BindBase).PropertyChanged += onStartupChange;
                (_main.getObject("main") as BindBase).PropertyChanged += onDialogChange;
            }
        }

        private void instances()
        {
            _title = "Client Management";
            _idList = new List<string>();
            _companyList = new List<string>();
            _saveSearchParametersList = new List<string>();
            _saveResultParametersList = new List<Client>();
            //_selectedCLientTask = new NotifyTaskCompletion<ClientModel>();
            _agentList = new List<Agent>();
            _clientList = new List<Client>();
            _addressList = new List<Address>();
        }

        private void instancesModel(IMainWindowViewModel main)
        {
            _clientDetailViewModel = new ClientDetailViewModel(main);
            _clientsModel = new List<ClientModel>();
            _clientModel = new ClientModel();
            ClientSideBarViewModel = new CLientSideBarViewModel(main);
        }

        private void instancesCommand()
        {
            checkBoxResultGridCommand = new ButtonCommand<ClientModel>(saveResultGridChecks, canSaveResultGridChecks);
            checkBoxSearchCommand = new ButtonCommand<string>(saveSearchChecks, canSaveSearchChecks);
            rBoxSearchCommand = new ButtonCommand<string>(saveSearchRadioButtonSelection, canSaveSearchRadioButtonSelection);
            btnComboBxCommand = new ButtonCommand<Agent>(moveCLientAgent, canMoveClientAgent);
            btnSearchCommand = new ButtonCommand<string>(filterClient, canFilterClient);
            NavigCommand = new ButtonCommand<string>(executeNavig, canExecuteNavig);
            GetCurrentItemCommand = new ButtonCommand<ClientModel>(selectCurrentClient, canSelectedCurrentClient);

        }
        
        //----------------------------[ Properties ]------------------

        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
            set { _startup.Bl = value;  }
        }

        public string Title
        {
            get { return _title; }
            set { setProperty(ref _title, value); }
        }

        public List<ClientModel> ClientModelList
        {
            get { return _clientsModel; }
            set { setProperty(ref _clientsModel, value); }
        }

        public List<Agent> AgentList
        {
            get { return _agentList; }
            set { setProperty(ref _agentList, value); }
        }

        public ClientDetailViewModel ClientDetailViewModel
        {
            get { return _clientDetailViewModel; }
            set { _clientDetailViewModel = value; onPropertyChange("ClientDetailViewModel"); }
        }

        public ClientModel SelectedCLientModel
        {
            get { return _clientDetailViewModel.SelectedCLientModel; }
            set { _clientDetailViewModel.SelectedCLientModel = value; onPropertyChange("SelectedCLientModel"); }
        }

        public ClientModel ClientModel
        {
            get { return _clientModel; }
            set { _clientModel = value; onPropertyChange("ClientModel"); }
        }


        public List<string> IdList
        {
            get { return _idList; }
            set { _idList = value; onPropertyChange("IdList"); }
        }

        public List<string> CompanyList
        {
            get { return _companyList; }
            set { _companyList = value; onPropertyChange("CompanyList"); }
        }

        public List<Address> AddressList
        {
            get { return _addressList; }
            set { _addressList = value; onPropertyChange("AddressList"); }
        }


        //----------------------------[ Actions ]------------------

        public async void loadClients()
        {
            Dialog.showSearch("Loading...");
            AgentList = await Bl.BlAgent.GetAgentDataAsync(-999); // -999 =>  get the agent list without their roles
            ClientModelList = clientListToModelViewList(Bl.BlClient.GetClientData(999));
            Dialog.IsDialogOpen = false;
        }

        public List<ClientModel> clientListToModelViewList(List<Client> clientList)
        {
            List<ClientModel> output = new List<ClientModel>();
            Parallel.ForEach(clientList, (client) =>
            {
                ClientModel cvm = new ClientModel();
                if (AgentList.Count() > 0)
                {
                    var result = AgentList.Where(x => x.ID.Equals(client.AgentId)).ToList();
                    cvm.Agent.Agent = (result.Count > 0) ? result[0] : new Agent();
                }
                cvm.Client = client;
                output.Add(cvm);

            });
            return output;
        }

        public ClientModel loadContactsAndAddresses(ClientModel cLientViewModel)
        {
            cLientViewModel.AddressList = Bl.BlClient.searchAddress(new Address { ClientId = cLientViewModel.Client.ID }, ESearchOption.AND);
            cLientViewModel.Address = (cLientViewModel.AddressList.Count() > 0) ? cLientViewModel.AddressList.OrderBy(x => x.ID).Last() : new Address();
            cLientViewModel.ContactList = Bl.BlClient.GetContactDataByClientList(new List<Client> { new Client { ID = cLientViewModel.Client.ID } });
            cLientViewModel.Contact = (cLientViewModel.ContactList.Count() > 0) ? cLientViewModel.ContactList.OrderBy(x => x.ID).Last() : new Contact();

            return cLientViewModel;
        }

        public override void Dispose()
        {
            _clientDetailViewModel.PropertyChanged -= onSelectedClientChange;
            //_selectedCLientTask.PropertyChanged -= onSelectedCLientTaskCompletion_saveSelectedClient;
            ClientDetailViewModel.Dispose();
            ClientSideBarViewModel.Dispose();
            if ((_main.getObject("main") as BindBase) != null)
            {
                (_main.getObject("main") as BindBase).PropertyChanged += onStartupChange;
                (_main.getObject("main") as BindBase).PropertyChanged += onDialogChange;
            }
        }

        //----------------------------[ Event Handler ]------------------
        
        private void onSelectedClientChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "SelectedCLientModel"))
            {
                executeNavig("client-detail");
            }
        }

        //private void onSelectedCLientTaskCompletion_saveSelectedClient(object sender, PropertyChangedEventArgs e)
        //{
        //    if (string.Equals(e.PropertyName, "IsSuccessfullyCompleted"))
        //    {
        //        SelectedCLientModel = _selectedCLientTask.Result;
        //    }
        //}

        private void onStartupChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Startup"))
            {
                Startup = (_main.getObject("main") as BindBase).Startup;
                _clientDetailViewModel.Startup = Startup; ClientSideBarViewModel.Startup = Startup;
            }
        }

        private void onDialogChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Dialog"))
            {
                Dialog = (_main.getObject("main") as BindBase).Dialog;
                _clientDetailViewModel.Dialog = Dialog; ClientSideBarViewModel.Dialog = Dialog;
            }
        }

        //----------------------------[ Action Commands ]------------------

        private void selectCurrentClient(ClientModel obj)
        {
            SelectedCLientModel = loadContactsAndAddresses(obj);
            //_selectedCLientTask.initializeNewTask();            
        }

        private bool canSelectedCurrentClient(ClientModel arg)
        {
            return true;
        }

        public void executeNavig(string obj)
        {
            switch (obj)
            {
                case "client":
                    _page(this);
                    break;
                case "client-detail":
                    _page(ClientDetailViewModel);
                    break;
                case "client-new":
                    SelectedCLientModel = new ClientModel();
                    _page(ClientDetailViewModel);
                    break;
                default:
                    goto case "client";
            }
        }

        private bool canExecuteNavig(string arg)
        {
            return true;
        }

        private async void moveCLientAgent(Agent obj)
        {
            Dialog.showSearch("Moving clients...");
            var movedClientList = await Bl.BlClient.MoveClientAgentBySelection(_saveResultParametersList, obj);
            if (movedClientList.Count > 0)
                await Dialog.show(movedClientList.Count +" have been moved to "+obj.LastName+" successfully!");

            _saveResultParametersList.Clear();
            Dialog.IsDialogOpen = false;
            _page(this);
        }

        private bool canMoveClientAgent(Agent arg) 
        {
            return true;
        }

        private async void filterClient(string obj)
        {
            Client client = new Client();
            string restrict = "";
            bool isDeep = false;
            Dialog.showSearch("Searching...");
            //new Thread(delegate ()
            //{
            //    Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
            //     {
            foreach (string checkedValue in _saveSearchParametersList)
            {
                switch (checkedValue)
                {
                    case "cbContact":
                        client.FirstName = obj;
                        client.LastName = obj;
                        break;
                    case "cbCompany":
                        client.Company = obj;
                        //client.CompanyName = obj;
                        break;
                    case "Client":
                        restrict = EStatus.Client.ToString();
                        client.Status = EStatus.Client.ToString();
                        break;
                    case "Prospect":
                        restrict = EStatus.Prospect.ToString();
                        client.Status = EStatus.Prospect.ToString();
                        break;
                    case "cbDeep":
                        isDeep = true;
                        break;
                }
            }

            List<Client> resultAfterFilter = new List<Client>();
            List<Client> resultBeforeFilter = new List<Client>();

            if(isDeep)
                resultBeforeFilter = await Bl.BlClient.searchClientAsync(client, ESearchOption.AND);
            else
                resultBeforeFilter = Bl.BlClient.searchClient(client, ESearchOption.AND);

            if (string.Equals(restrict, EStatus.Client.ToString()))
            {
                resultAfterFilter = resultBeforeFilter.Where(x => x.Status == EStatus.Client.ToString()).ToList();
            }
            else if (string.Equals(restrict, EStatus.Prospect.ToString()))
            {
                resultAfterFilter = resultBeforeFilter.Where(x => x.Status == EStatus.Prospect.ToString()).ToList();
            }
            else
            {
                resultAfterFilter = resultBeforeFilter;
            }

            ClientModelList = clientListToModelViewList(resultAfterFilter);
            Dialog.IsDialogOpen = false;
        }

        private bool canFilterClient(string arg)
        {
            return true;
        }

        public void saveResultGridChecks(ClientModel param)
        {
            //Properties.Settings.Default.cbResultArrayValue.Add(param._client);
            if (!_saveResultParametersList.Contains(param.Client))
                _saveResultParametersList.Add(param.Client);
            else
                _saveResultParametersList.Remove(param.Client);
        }

        public bool canSaveResultGridChecks(ClientModel param)
        {
            return true;
        }

        private void saveSearchChecks(string obj)
        {
            if (!_saveSearchParametersList.Contains(obj))
                _saveSearchParametersList.Add(obj);
            else
                _saveSearchParametersList.Remove(obj);
        }

        private bool canSaveSearchChecks(string arg)
        {
            return true;
        }


        private void saveSearchRadioButtonSelection(string obj)
        {
            if (!_saveSearchParametersList.Contains(obj) && string.Equals(obj, "Client"))
            {
                _saveSearchParametersList.Add(obj);
                _saveSearchParametersList.Remove("Prospect");
            }
            else
            {
                _saveSearchParametersList.Add(obj);
                _saveSearchParametersList.Remove("Client");
            }

        }

        private bool canSaveSearchRadioButtonSelection(string arg)
        {
            return true;
        }


    }
}
