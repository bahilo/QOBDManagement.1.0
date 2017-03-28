using Entity = QOBDCommon.Entities;
using QOBDManagement.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Models
{
    public class ActionModel : BindBase
    {
        Entity.Action _action;
        PrivilegeModel _privilegeModel;
        private bool _isModified;

        public ActionModel()
        {
            _action = new Entity.Action();
            _privilegeModel = new PrivilegeModel();
        }

        public Entity.Action Action
        {
            get { return _action; }
            set { setProperty(ref _action, value); }
        }

        public string TxtName
        {
            get { return _action.Name; }
            set { _action.Name = value; onPropertyChange(); }
        }

        public string TxtDisplayedName
        {
            get { return _action.DisplayedName; }
            set { _action.DisplayedName = value; onPropertyChange(); }
        }

        public string TxtID
        {
            get { return _action.ID.ToString(); }
            set { _action.ID = Convert.ToInt32(value); onPropertyChange(); }
        }

        public PrivilegeModel PrivilegeModel
        {
            get { return _privilegeModel; }
            set { setProperty(ref _privilegeModel, value); }
        }

        public bool IsModified
        {
            get { return _isModified; }
            set { setProperty(ref _isModified, value); }
        }
    }
}
