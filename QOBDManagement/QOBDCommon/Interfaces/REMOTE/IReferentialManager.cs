// FILE: D:/Just IT Training/BillManagment/Classes//IReferentialManager.cs

// In this section you can add your own using directives
// section -64--88-0-12--3914362f:15397d27317:-8000:0000000000000DF9 begin
// section -64--88-0-12--3914362f:15397d27317:-8000:0000000000000DF9 end

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
namespace QOBDCommon.Interfaces.REMOTE
{
    public interface IReferentialManager : IInfosManager, INotifyPropertyChanged, IDisposable
    {
        void setServiceCredential(object channel);

    } /* end interface IReferentialManager */
}