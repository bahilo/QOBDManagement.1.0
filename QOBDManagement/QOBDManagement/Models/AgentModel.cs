using QOBDCommon.Entities;
using QOBDManagement.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

namespace QOBDManagement.Models
{
    public class AgentModel : BindBase
    {
        private Random _rd;
        private Agent _agent;
        private List<RoleModel> _roleModelList;
        private string _clearPassword;
        private string _clearPasswordVerification;
        private bool _isModified;
        private List<RoleModel> _roleToAddList;
        private List<RoleModel> _roleToRemoveList;
        private string _profileImageFileNameBase;
        private Dictionary<int, int> _rolePosition;
        private InfoManager.Display _image;
        private List<string> _iconColourList;

        public AgentModel()
        {
            _rd = new Random();
            _agent = new Agent();
            _roleModelList = new List<RoleModel>();
            _roleToAddList = new List<RoleModel>();
            _roleToRemoveList = new List<RoleModel>();
            _rolePosition = new Dictionary<int, int>();
            _profileImageFileNameBase = "profile_image";
            _rolePosition = new Dictionary<int, int>();
            _clearPassword = "";
            _clearPasswordVerification = "";
            PropertyChanged += onRoleModelListChange_updateUIRole;
            _iconColourList = new List<string> { "DarkBlue", "DarkGreen", "DarkMagenta", "DarkOrange", "DarkRed", "DarkOrchid" };
        }

        private void onRoleModelListChange_updateUIRole(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("RoleModelList"))
                updateUIRole();
        }

        public Dictionary<int, int> RolePositionDisplay
        {
            get { return _rolePosition; }
            set { _rolePosition = value; onPropertyChange(); }
        }

        public List<RoleModel> RoleModelList
        {
            get { return _roleModelList; }
            set { _roleModelList = value; onPropertyChange(); }
        }

        public string TxtClearPassword
        {
            get { return _clearPassword; }
            set { setProperty(ref _clearPassword, value); }
        }

        public string TxtProfileImageFileNameBase
        {
            get { return _profileImageFileNameBase; }
            set { setProperty(ref _profileImageFileNameBase, value); }
        }

        public string TxtClearPasswordVerification
        {
            get { return _clearPasswordVerification; }
            set { _clearPasswordVerification = value; onPropertyChange(); }
        }

        public string TxtAdmin
        {
            get { return _agent.Admin; }
            set { _agent.Admin = value; onPropertyChange(); }
        }

        public bool IsModified
        {
            get { return _isModified; }
            set { setProperty(ref _isModified, value); }
        }

        public Agent Agent
        {
            get { return _agent; }
            set { _agent = value; onPropertyChange(); }
        }

        public string TxtID
        {
            get { return _agent.ID.ToString(); }
            set { _agent.ID = Convert.ToInt32(value); onPropertyChange(); }
        }

        public string TxtFirstName
        {
            get { return _agent.FirstName; }
            set { _agent.FirstName = value; onPropertyChange(); }
        }

        public string TxtLastName
        {
            get { return _agent.LastName; }
            set { _agent.LastName = value; onPropertyChange(); }
        }

        public string TxtPhone
        {
            get { return _agent.Phone; }
            set { _agent.Phone = value; onPropertyChange(); }
        }

        public string TxtFax
        {
            get { return _agent.Fax; }
            set { _agent.Fax = value; onPropertyChange(); }
        }

        public string TxtEmail
        {
            get { return _agent.Email; }
            set { _agent.Email = value; onPropertyChange(); }
        }

        public string TxtLogin
        {
            get { return _agent.UserName; }
            set { _agent.UserName = value; onPropertyChange(); onPropertyChange("TxtIcon"); }
        }

        public string TxtHashedPassword
        {
            get { return _agent.HashedPassword; }
            set { _agent.HashedPassword = value; onPropertyChange(); }
        }

        public string TxtPicture
        {
            get { return _agent.Picture; }
            set { _agent.Picture = value; onPropertyChange(); }
        }

        public InfoManager.Display Image
        {
            get { return _image; }
            set { _image = value; onPropertyChange(); }
        }

        public List<Role> RoleList
        {
            get { return _agent.RoleList; }
            set { _agent.RoleList = value; onPropertyChange(); }
        }

        public List<RoleModel> RoleToAddList
        {
            get { return _roleToAddList; }
            set { _roleToAddList = value; onPropertyChange(); }
        }

        public List<RoleModel> RoleToRemoveList
        {
            get { return _roleToRemoveList; }
            set { _roleToRemoveList = value; onPropertyChange(); }
        }

        public string TxtStatus
        {
            get { return _agent.Status; }
            set { _agent.Status = value; onPropertyChange(); }
        }

        public string TxtComment
        {
            get { return _agent.Comment; }
            set { _agent.Comment = value; onPropertyChange(); }
        }

        public string TxtIcon
        {
            get { return TxtLogin.Substring(0,2).ToUpper() ; }
        }

        public string TxtIconColour
        {
            get { return _iconColourList[_rd.Next(0, _iconColourList.Count - 1)]; }
        }

        public bool IsOnline
        {
            get { return _agent.IsOnline; }
            set { _agent.IsOnline = value; onPropertyChange(); }
        }

        public string TxtIPAddress
        {
            get { return _agent.IPAddress; }
            set { _agent.IPAddress = value; onPropertyChange(); }
        }

        public string TxtListSize
        {
            get { return _agent.ListSize.ToString(); }
            set { int converted; if (int.TryParse(value, out converted)) { _agent.ListSize = converted; } else _agent.ListSize = 0;  onPropertyChange("TxtListSize");}
        }

        public bool IsRole1
        {
            get { return (RolePositionDisplay.Count > 0) ? getRoleBooleanByID(RolePositionDisplay[0]) : false; }
            set { if (RolePositionDisplay.Count > 0) { addRoleBy(RolePositionDisplay[0]); onPropertyChange(); onPropertyChange("IsIsRole1AnonymousEnable"); } }
        }

        public bool IsIsRole1AnonymousEnable
        {
            get { return (RolePositionDisplay.Count > 0 && checkIfRoleAnonymous(RolePositionDisplay[0])) ? false : true; }            
        }

        public bool IsRole2
        {
            get { return (RolePositionDisplay.Count > 1) ? getRoleBooleanByID(RolePositionDisplay[1]) : false; }
            set { if (RolePositionDisplay.Count > 1) { addRoleBy(RolePositionDisplay[1]); onPropertyChange(); onPropertyChange("IsIsRole2AnonymousEnable"); } }
        }

        public bool IsIsRole2AnonymousEnable
        {
            get { return (RolePositionDisplay.Count > 1 && checkIfRoleAnonymous(RolePositionDisplay[1])) ? false : true; }
        }

        public bool IsRole3
        {
            get { return (RolePositionDisplay.Count > 2) ? getRoleBooleanByID(RolePositionDisplay[2]) : false; }
            set { if (RolePositionDisplay.Count > 2) { addRoleBy(RolePositionDisplay[2]); onPropertyChange(); onPropertyChange("IsIsRole3AnonymousEnable"); } }
        }

        public bool IsIsRole3AnonymousEnable
        {
            get { return (RolePositionDisplay.Count > 2 && checkIfRoleAnonymous(RolePositionDisplay[2])) ? false : true; }
        }

        public bool IsRole4
        {
            get { return (RolePositionDisplay.Count > 3) ? getRoleBooleanByID(RolePositionDisplay[3]) : false; }
            set { if (RolePositionDisplay.Count > 3) { addRoleBy(RolePositionDisplay[3]); onPropertyChange(); onPropertyChange("IsIsRole4AnonymousEnable"); } }
        }

        public bool IsIsRole4AnonymousEnable
        {
            get { return (RolePositionDisplay.Count > 3 && checkIfRoleAnonymous(RolePositionDisplay[3])) ? false : true; }
        }

        public bool IsRole5
        {
            get { return (RolePositionDisplay.Count > 4) ? getRoleBooleanByID(RolePositionDisplay[4]) : false; }
            set { if (RolePositionDisplay.Count > 4) { addRoleBy(RolePositionDisplay[4]); onPropertyChange(); onPropertyChange("IsIsRole5AnonymousEnable"); } }
        }

        public bool IsIsRole5AnonymousEnable
        {
            get { return (RolePositionDisplay.Count > 4 && checkIfRoleAnonymous(RolePositionDisplay[4])) ? false : true; }
        }

        public bool IsRole6
        {
            get { return (RolePositionDisplay.Count > 5) ? getRoleBooleanByID(RolePositionDisplay[5]) : false; }
            set { if (RolePositionDisplay.Count > 5) { addRoleBy(RolePositionDisplay[5]); onPropertyChange(); onPropertyChange("IsIsRole6AnonymousEnable"); } }
        }

        public bool IsIsRole6AnonymousEnable
        {
            get { return (RolePositionDisplay.Count > 5 && checkIfRoleAnonymous(RolePositionDisplay[5])) ? false : true; }
        }

        public bool IsRole7
        {
            get { return (RolePositionDisplay.Count > 6) ? getRoleBooleanByID(RolePositionDisplay[6]) : false; }
            set { if (RolePositionDisplay.Count > 6) { addRoleBy(RolePositionDisplay[6]); onPropertyChange(); onPropertyChange("IsIsRole7AnonymousEnable"); } }
        }

        public bool IsIsRole7AnonymousEnable
        {
            get { return (RolePositionDisplay.Count > 6 && checkIfRoleAnonymous(RolePositionDisplay[6])) ? false : true; }
        }

        public bool IsRole8
        {
            get { return (RolePositionDisplay.Count > 7) ? getRoleBooleanByID(RolePositionDisplay[7]) : false; }
            set { if (RolePositionDisplay.Count > 7) { addRoleBy(RolePositionDisplay[7]); onPropertyChange(); onPropertyChange("IsIsRole8AnonymousEnable"); } }
        }

        public bool IsIsRole8AnonymousEnable
        {
            get { return (RolePositionDisplay.Count > 7 && checkIfRoleAnonymous(RolePositionDisplay[7])) ? false : true; }
        }

        public bool IsRole9
        {
            get { return (RolePositionDisplay.Count > 8) ? getRoleBooleanByID(RolePositionDisplay[8]) : false; }
            set { if (RolePositionDisplay.Count > 8) { addRoleBy(RolePositionDisplay[8]); onPropertyChange(); onPropertyChange("IsIsRole9AnonymousEnable"); } }
        }

        public bool IsIsRole9AnonymousEnable
        {
            get { return (RolePositionDisplay.Count > 8 && checkIfRoleAnonymous(RolePositionDisplay[8])) ? false : true; }
        }

        private bool getRoleBooleanByID(int id)
        {
            object _lock = new object();
            if(RoleList != null)
            {
                Role roleFound = RoleList.Where(x => x.ID == id).FirstOrDefault();
                lock (_lock)
                    if (roleFound != null)
                        return true;
            }            
            return false;
        }

        private bool checkIfRoleAnonymous(int id)
        {
            object _lock = new object();
            if (RoleList != null)
            {
                Role roleFound = RoleList.Where(x => x.ID == id && x.Name == "Anonymous").FirstOrDefault();
                lock (_lock)
                    if (roleFound != null)
                        return true;
            }
            return false;
        }

        private void addRoleBy(int id)
        {
            object _lock = new object();
            if (RoleModelList != null)
            {
                Role roleFound = RoleModelList.Where(x => x.Role.ID == id).Select(x=>x.Role).FirstOrDefault();
                lock (_lock)
                    if (roleFound != null)
                    {
                        if (RoleList == null)
                            RoleList = new List<Role>();

                        if (RoleList.Where(x=>x.ID == roleFound.ID).Count() == 0)
                        {
                            RoleList.Add(roleFound);
                            RoleToAddList.Add(new RoleModel { Role = roleFound });
                        }
                        else
                        {
                            RoleList = RoleList.Where(x => x.ID != roleFound.ID).ToList();
                            RoleToRemoveList.Add(new RoleModel { Role = roleFound });
                        }                            
                    }                        
            }  
        }

        private void updateUIRole()
        {
            onPropertyChange("IsRole1");
            onPropertyChange("IsRole2");
            onPropertyChange("IsRole3");
            onPropertyChange("IsRole4");
            onPropertyChange("IsRole5");
            onPropertyChange("IsRole6");
            onPropertyChange("IsRole7");
            onPropertyChange("IsRole8");
            onPropertyChange("IsRole9");
        }




    }
}
