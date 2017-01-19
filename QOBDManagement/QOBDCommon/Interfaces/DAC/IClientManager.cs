using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace QOBDCommon.Interfaces.DAC
{
    public interface IClientManager : REMOTE.IClientManager, IContactManager, IAddressManager, INotifyPropertyChanged, IDisposable
    {
        void initializeCredential(Agent user);

        void progressBarManagement(Func<double, double> progressBarFunc);

        List<Client> GetClientData(int nbLine);

        List<Client> GetClientDataByBillList(List<Bill> billList);

        List<Order> GetQuoteCLient(int id);

        List<Order> GetOrderClient(int id);

        List<Client> searchClient(Client client, ESearchOption filterOperator);

        List<Client> GetClientDataById(int id);

    } /* end interface IClientManager */
}