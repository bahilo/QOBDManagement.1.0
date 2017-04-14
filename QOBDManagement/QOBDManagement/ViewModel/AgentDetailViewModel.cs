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

namespace QOBDManagement.ViewModel
{
    public class AgentDetailViewModel : BindBase
    {
        //private string _listSize;
        private string _title;
        private Func<string, object> _page;
        private IMainWindowViewModel _main;
        private InfoManager.Display _profileImageDisplay;
        private string _profileImageFileNameBase;

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
            initEvents();

        }

        public AgentDetailViewModel(IMainWindowViewModel main) : this()
        {
            _main = main;
            _page = _main.navigation;
        }

        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            PropertyChanged += onSelectedAgentModelChange;
        }

        private void instances()
        {
            _title = "Agent Description";
            _profileImageFileNameBase = "profile_image";

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
            set { setProperty(ref _selectedAgentModel, value); }
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

        public InfoManager.Display ProfileImageDisplay
        {
            get { return _profileImageDisplay; }
            set { setProperty(ref _profileImageDisplay, value); }
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
                if (ProfileImageDisplay != null)
                {
                    ProfileImageDisplay.closeImageSource();
                    //ProfileImageDisplay.PropertyChanged -= onProfileImageChange_updateUIImage;
                    ProfileImageDisplay.PropertyChanged -= onProfileImageSizeChange;
                    ProfileImageDisplay.Dispose();
                }
                ProfileImageDisplay = null;
            }

            loadUserProfileImage();
        }

        private void loadUserProfileImage()
        {
            if (ProfileImageDisplay == null)
            {
                var username = (_startup.Bl.BlReferential.searchInfo(new QOBDCommon.Entities.Info { Name = "ftp_login" }, ESearchOption.OR).FirstOrDefault() ?? new Info()).Value;
                var password = (_startup.Bl.BlReferential.searchInfo(new QOBDCommon.Entities.Info { Name = "ftp_password" }, ESearchOption.OR).FirstOrDefault() ?? new Info()).Value;

                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    // get profile image for updating
                    ProfileImageDisplay = new InfoManager.Display(Bl.BlReferential.searchInfo(new Info { Name =_profileImageFileNameBase }, ESearchOption.AND),_profileImageFileNameBase, new List<string> { _profileImageFileNameBase }, ConfigurationManager.AppSettings["ftp_profile_image_folder"], ConfigurationManager.AppSettings["local_profile_image_folder"], username, password);// _main.ImageManagement(null, "profile");

                    // get the picture info from the database
                    ProfileImageDisplay.TxtFileNameWithoutExtension = _profileImageFileNameBase + "_" + SelectedAgentModel.Agent.ID;
                    var loadedImage = _main.loadImage(ProfileImageDisplay);

                    // display the picture
                    if (Application.Current != null)
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            ProfileImageDisplay = loadedImage;
                            //ProfileImageDisplay.PropertyChanged += onProfileImageChange_updateUIImage;
                            ProfileImageDisplay.PropertyChanged += onProfileImageSizeChange;
                        });
                }                
            }
        }

        public override void Dispose()
        {
            PropertyChanged -= onSelectedAgentModelChange;
            if (ProfileImageDisplay != null)
            {
                //ProfileImageDisplay.PropertyChanged -= onProfileImageChange_updateUIImage;
                ProfileImageDisplay.PropertyChanged -= onProfileImageSizeChange;
                ProfileImageDisplay.Dispose();
            }
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

        private void onSelectedAgentModelChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SelectedAgentModel"))
            {
                AgentSideBarViewModel.SelectedAgentModel = SelectedAgentModel;
                load();
            }
        }

        /*private void onProfileImageChange_updateUIImage(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TxtFileFullPath") && !string.IsNullOrEmpty(((InfoManager.Display)sender).TxtFileFullPath))
            {
                ProfileImageDisplay = _main.ImageManagement((InfoManager.Display)sender, "profile");
            }
        }*/

        private async void onProfileImageSizeChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ImageInfoUpdated"))
            {
                var infoUpdatedList = await _main.ReferentialViewModel.OptionDataAndDisplayViewModel.saveInfo(((InfoManager.Display)sender).InfoDataList);
                //((InfoManager.Display)sender).updateFields(infoUpdatedList);
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
                    SelectedAgentModel.TxtHashedPassword = Bl.BlSecurity.CalculateHash(SelectedAgentModel.TxtClearPassword);
                    isPasswordIdentical = true;
                }
            }

            if (SelectedAgentModel.Agent.ID == 0)
            {
                if (isPasswordIdentical)
                {
                    Dialog.showSearch("Creating Agent " + SelectedAgentModel.Agent.LastName + "...");
                    SelectedAgentModel.Agent.Status = EStatus.Deactivated.ToString();
                    var insertedAgentList = await Bl.BlAgent.InsertAgentAsync(new List<Agent> { SelectedAgentModel.Agent });
                    if (insertedAgentList.Count > 0)
                        await Dialog.showAsync("Agent " + SelectedAgentModel.Agent.LastName + " Successfully Created!");
                    Dialog.IsDialogOpen = false;
                }
                else
                    await Dialog.showAsync("Password are not Identical!");
            }
            else
            {
                if (isPasswordIdentical || string.IsNullOrEmpty(SelectedAgentModel.TxtClearPasswordVerification))
                {
                    Dialog.showSearch("Updating Agent " + SelectedAgentModel.Agent.LastName + "...");
                    var updatedAgentList = await Bl.BlAgent.UpdateAgentAsync(new List<Agent> { SelectedAgentModel.Agent });
                    if (updatedAgentList.Count > 0)
                        await Dialog.showAsync("Agent " + SelectedAgentModel.Agent.LastName + " Successfully Updated!");
                    Dialog.IsDialogOpen = false;
                }
                else
                    await Dialog.showAsync("Password are not Identical!");
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

        private void getFileFromLocal(object obj)
        {
            if (ProfileImageDisplay != null)
                ProfileImageDisplay.closeImageSource();

            _main.ReferentialViewModel.OptionDataAndDisplayViewModel.getFileFromLocal(ProfileImageDisplay);
        }

        private bool canGetFileFromLocal(object arg)
        {
            return true;
        }

    }
}
