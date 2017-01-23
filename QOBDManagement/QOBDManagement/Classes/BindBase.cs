using QOBDCommon.Entities;
using QOBDCommon.Enum;
using QOBDManagement.Interfaces;
using QOBDManagement.Models;
using QOBDManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace QOBDManagement.Classes
{
    public class BindBase : INotifyPropertyChanged, IState, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected ConfirmationViewModel _dialog;
        protected Startup _startup;
        
        protected virtual void setProperty<P>(
            ref P member,
            P val,
            [CallerMemberName]
            string propertyName = null)
        {
            /*if (object.Equals(member,val))
                return;*/

            member = val;
            
            onPropertyChange(propertyName);
        }

        public Startup Startup
        {
            get { return _startup; }
            set { setProperty(ref _startup, value); }
        }

        public ConfirmationViewModel Dialog
        {
            get { return _dialog; }
            set { setProperty(ref _dialog, value); }
        }

        protected void onPropertyChange([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }                
        }


        public void Handle(Context context, Func<object, object> page)
        {
            var prev = context.PreviousState;
            context.PreviousState = context.NextState;
            context.NextState = prev;
            page(context.NextState);
        }

        public virtual void Dispose()
        {
            
        }


    }
}
