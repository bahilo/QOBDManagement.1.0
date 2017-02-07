using QOBDCommon.Entities;
using QOBDManagement.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Models
{
    public class RoleModel : BindBase
    {
        private Role _role;
        private ActionModel _actionModel;
        private bool _isModified;

        public RoleModel()
        {
            _role = new Role();
            _actionModel = new ActionModel();
        }

        public bool IsModified
        {
            get { return _isModified; }
            set {setProperty(ref _isModified, value); }
        }

        public Role Role
        {
            get { return _role; }
            set { setProperty(ref _role, value); }
        }

        public string TxtID
        {
            get { return _role.ID.ToString(); }
            set { _role.ID = Convert.ToInt32(value); onPropertyChange(); }
        }

        public string TxtName
        {
            get { return _role.Name; }
            set { _role.Name = value; onPropertyChange(); }
        }

        public List<QOBDCommon.Entities.Action> ActionList
        {
            get { return _role.ActionList; }
            set { _role.ActionList = value; onPropertyChange(); }
        }

        public ActionModel ActionModel
        {
            get { return _actionModel; }
            set { _actionModel = value; onPropertyChange(); }
        }
    }
}
