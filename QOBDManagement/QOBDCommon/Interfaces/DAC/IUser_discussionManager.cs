using QOBDCommon.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.DAC
{
    public interface IUser_discussionManager: REMOTE.IUser_discussionManager, INotifyPropertyChanged, IDisposable
    {
        void initializeCredential(Agent user);
    }
}
