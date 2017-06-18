using QOBDBusiness;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDManagement.Classes;
using QOBDManagement.Command;
using QOBDManagement.Interfaces;
using QOBDManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.IO;
using QOBDCommon.Classes;
using System.Configuration;
using QOBDManagement.Helper;

namespace QOBDManagement.ViewModel
{
    public class AgentDetailViewModel : BindBase
    {
        private string _title;
        private Func<string, object> _page;
        private IMainWindowViewModel _main;

        //----------------------------[ Models ]------------------

        private AgentModel _selectedAgentModel;
        private List<AgentModel> _agentsViewModel;

        public AgentSideBarViewModel AgentSideBarViewModel { get; set; }


        //----------------------------[ Commands ]------------------

        public ButtonCommand<object> UpdateCommand { get; set; }
        public ButtonCommand<AgentModel> SearchCommand { get; set; }
        public ButtonCommand<object> OpenFileExplorerCommand { get; set; }


        public AgentDetailViewModel() : base()
        {
            instances();
            instancesModel();
            instancesCommand();

        }

        public AgentDetailViewModel(IMainWindowViewModel main) : this()
        {
            _main = main;
            _page = _main.navigation;
        }

        //----------------------------[ Initialization ]------------------
        

        private void instances()
        {
            _title = ConfigurationManager.AppSettings["title_agent_detail"];
        }

        private void instancesModel()
        {
            _selectedAgentModel = new AgentModel();
            _agentsViewModel = new List<AgentModel>();
        }

        private void instancesCommand()
        {
            UpdateCommand = new ButtonCommand<object>(updateAgent, canUpdateAgent);
            SearchCommand = new ButtonCommand<AgentModel>(searchAgent, canSearchAgent);
            OpenFileExplorerCommand = new ButtonCommand<object>(getFileFromLocal, canGetFileFromLocal);
        }


        //----------------------------[ Properties ]------------------


        public AgentModel SelectedAgentModel
        {
            get { return _selectedAgentModel; }
            set { setProperty(ref _selectedAgentModel, value); AgentSideBarViewModel.SelectedAgentModel = value; }
        }

        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
        }

        public string Title
        {
            get { return _title; }
            set { setProperty(ref _title, value); }
        }

        public List<AgentModel> AgentModelList
        {
            get { return _agentsViewModel; }
            set { setProperty(ref _agentsViewModel, value); }
        }


        //----------------------------[ Actions ]----------------------

        public void load()
        {            
            bool isUserAdmin = _main.securityCheck(QOBDCommon.Enum.EAction.Security, QOBDCommon.Enum.ESecurity.SendEmail)
                             && _main.securityCheck(QOBDCommon.Enum.EAction.Security, QOBDCommon.Enum.ESecurity._Delete)
                                 && _main.securityCheck(QOBDCommon.Enum.EAction.Security, QOBDCommon.Enum.ESecurity._Read)
                                     && _main.securityCheck(QOBDCommon.Enum.EAction.Security, QOBDCommon.Enum.ESecurity._Update)
                                         && _main.securityCheck(QOBDCommon.Enum.EAction.Security, QOBDCommon.Enum.ESecurity._Write);

            // only admin can see other agents profile
            if (isUserAdmin)
            {
                // closing the image source if image already displayed
                if (SelectedAgentModel.Image != null)
                    SelectedAgentModel.Image.Dispose();

                SelectedAgentModel.Image = null;
            }

             loadUserProfileImage();
        }

        private async void loadUserProfileImage()
        {
            if (SelectedAgentModel.Image == null)
            {
                Dialog.showSearch(ConfigurationManager.AppSettings["load_message"]);
                var credentialInfoList = _startup.Bl.BlReferential.searchInfo(new QOBDCommon.Entities.Info { Name = "ftp_" }, ESearchOption.OR);
                
                if (credentialInfoList.Count > 0)
                {
                    SelectedAgentModel.Image = await Task.Factory.StartNew(()=> { return SelectedAgentModel.Image.downloadPicture(ConfigurationManager.AppSettings["ftp_profile_image_folder"], ConfigurationManager.AppSettings["local_profile_image_folder"], SelectedAgentModel.TxtPicture, SelectedAgentModel.TxtProfileImageFileNameBase + "_" + Bl.BlSecurity.GetAuthenticatedUser().ID, credentialInfoList);}) ;
                    onPropertyChange();
                }
                Dialog.IsDialogOpen = false;
            }
        }

        public override void Dispose()
        {
            if (SelectedAgentModel.Image != null)
                SelectedAgentModel.Image.Dispose();
        }

        //----------------------------[ Event Handler ]------------------

        internal async void onPwdBoxVerificationPasswordChange_updateTxtClearPasswordVerification(object sender, RoutedEventArgs e)
        {
            PasswordBox pwd = ((PasswordBox)sender);
            if (pwd.Password.Count() > 0)
            {
                SelectedAgentModel.TxtClearPasswordVerification = pwd.Password;
                if (!SelectedAgentModel.TxtClearPassword.Equals(SelectedAgentModel.TxtClearPasswordVerification))
                {
                    await Dialog.showAsync("Password are not Identical!");
                }
            }
        }

        internal void onPwdBoxPasswordChange_updateTxtClearPassword(object sender, RoutedEventArgs e)
        {
            PasswordBox pwd = ((PasswordBox)sender);
            if (pwd.Password.Count() > 0)
            {
                SelectedAgentModel.TxtClearPassword = pwd.Password;
            }
        }
        
        //----------------------------[ Action Commands ]------------------

        private async void updateAgent(object obj)
        {
            bool isPasswordIdentical = false;
            if (!string.IsNullOrEmpty(SelectedAgentModel.TxtClearPasswordVerification))
            {
                if (SelectedAgentModel.TxtClearPassword.Equals(SelectedAgentModel.TxtClearPasswordVerification))
                {
                    SelectedAgentModel.TxtHashedPassword = QOBDBusiness.Core.BlSecurity.CalculateHash(SelectedAgentModel.TxtClearPassword);
                    isPasswordIdentical = true;
                }
            }

            if (SelectedAgentModel.Agent.ID == 0)
            {
                if (isPasswordIdentical)
                {
                    Dialog.showSearch(ConfigurationManager.AppSettings["create_message"]);
                    SelectedAgentModel.Agent.Status = EStatus.Deactivated.ToString();
                    var insertedAgentList = await Bl.BlAgent.InsertAgentAsync(new List<Agent> { SelectedAgentModel.Agent });
                    if (insertedAgentList.Count > 0)
                        await Dialog.showAsync("Agent " + SelectedAgentModel.Agent.LastName + " Successfully Created!");
                    Dialog.IsDialogOpen = false;
                }
                else
                    await Dialog.showAsync("Passwords are not Identicals!");
            }
            else
            {
                if (isPasswordIdentical || string.IsNullOrEmpty(SelectedAgentModel.TxtClearPasswordVerification))
                {
                    Dialog.showSearch(ConfigurationManager.AppSettings["update_message"]);
                    var updatedAgentList = await Bl.BlAgent.UpdateAgentAsync(new List<Agent> { SelectedAgentModel.Agent });
                    if (updatedAgentList.Count > 0)
                        await Dialog.showAsync("Agent " + SelectedAgentModel.Agent.LastName + " Successfully Updated!");
                    Dialog.IsDialogOpen = false;
                }
                else
                    await Dialog.showAsync("Passwords are not Identicals!");
            }
            isPasswordIdentical = false;
        }

        private bool canUpdateAgent(object arg)
        {
            bool isWrite = _main.securityCheck(EAction.Agent, ESecurity._Write);
            bool isUpdate = _main.securityCheck(EAction.Agent, ESecurity._Update);
            if (isUpdate && isWrite)
                return true;

            return false;
        }

        private void searchAgent(AgentModel obj)
        {
            SelectedAgentModel = obj;
        }

        private bool canSearchAgent(AgentModel arg)
        {
            return true;
        }

        private async void getFileFromLocal(object obj)
        {
            string newFileFullPath = InfoManager.ExecuteOpenFileDialog("Select an image file", new List<string> { "png", "jpeg", "jpg" });
            if (!string.IsNullOrEmpty(newFileFullPath) && File.Exists(newFileFullPath))
            {
                var ftpCredentials = Bl.BlReferential.searchInfo(new Info { Name = "ftp_" }, ESearchOption.AND);

                if (!string.IsNullOrEmpty(SelectedAgentModel.TxtPicture))
                    WPFHelper.deleteFileFromFtpServer(ConfigurationManager.AppSettings["ftp_profile_image_folder"], SelectedAgentModel.TxtPicture, ftpCredentials);

                if (SelectedAgentModel.Image != null)
                {
                    SelectedAgentModel.Image.closeImageSource();
                    WPFHelper.deleteFileFromFtpServer(ConfigurationManager.AppSettings["ftp_profile_image_folder"], SelectedAgentModel.TxtPicture, ftpCredentials);
                }
                else
                    SelectedAgentModel.Image = await Task.Factory.StartNew(() => { return SelectedAgentModel.Image.downloadPicture(ConfigurationManager.AppSettings["ftp_profile_image_folder"], ConfigurationManager.AppSettings["local_profile_image_folder"], SelectedAgentModel.TxtPicture, SelectedAgentModel.TxtProfileImageFileNameBase + "_" + Bl.BlSecurity.GetAuthenticatedUser().ID, ftpCredentials); });

                // opening the file explorer to choose and resize an image file
                SelectedAgentModel.Image.TxtChosenFile = newFileFullPath.resizeImage();
                SelectedAgentModel.TxtPicture = SelectedAgentModel.Image.TxtFileName;

                Dialog.showSearch(ConfigurationManager.AppSettings["update_message"]);

                // upload the image file to the FTP server
                SelectedAgentModel.Image.uploadImage();

                // update item image
                var savedAgentList = await Bl.BlAgent.UpdateAgentAsync(new List<Agent> { SelectedAgentModel.Agent });

                if (savedAgentList.Count > 0)
                    await Dialog.showAsync("The picture has been saved successfully!");
                else
                {
                    string errorMessage = "Error occured while updating the agent [" + SelectedAgentModel.TxtLastName + "] picture";
                    Log.error(errorMessage, EErrorFrom.ITEM);
                    await Dialog.showAsync(errorMessage);
                }

                Dialog.IsDialogOpen = false;
            }
        }

        private bool canGetFileFromLocal(object arg)
        {
            return true;
        }

    }
}
