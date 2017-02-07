using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDManagement.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Models
{
    public class NotificationModel: BindBase
    {
        private Notification _notification;

        public NotificationModel()
        {
            _notification = new Notification();
        }

        public Notification Notification
        {
            get { return _notification; }
            set { setProperty(ref _notification, value); }
        }

        public string TxtID
        {
            get { return _notification.ID.ToString(); }
            set { _notification.ID = Convert.ToInt32(value); onPropertyChange(); }
        }

        public string TxtReminder1
        {
            get { return _notification.Reminder1.ToString(); }
            set { _notification.Reminder1 = Utility.convertToDateTime(value); onPropertyChange(); }
        }

        public string TxtReminder2
        {
            get { return _notification.Reminder2.ToString(); }
            set { _notification.Reminder2 = Utility.convertToDateTime(value); onPropertyChange(); }
        }

        public string TxtDate
        {
            get { return _notification.Date.ToString(); }
            set { _notification.Date = Utility.convertToDateTime(value); onPropertyChange(); }
        }

    }
}
