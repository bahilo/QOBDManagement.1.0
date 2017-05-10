using QOBDCommon;
using QOBDCommon.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
/// <summary>
///  An interface defining operations expected of ...
/// 
///  @see OtherClasses
///  @author Dago
/// </summary>
namespace QOBDCommon.Interfaces.DAC
{
    public interface ISecurityManager : REMOTE.ISecurityManager, INotifyPropertyChanged, IDisposable
    {
        void initializeCredential(Agent user);
        

    } /* end interface Isecurity */
}