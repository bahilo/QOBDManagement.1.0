using QOBDCommon;
using QOBDCommon.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        void progressBarManagement(Func<double, double> progressBarFunc);

    } /* end interface Isecurity */
}