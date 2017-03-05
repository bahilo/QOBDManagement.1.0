using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.DAC
{
    public interface IClientManager : REMOTE.IClientManager, IContactManager, IAddressManager, INotifyPropertyChanged, IDisposable
    {
        void initializeCredential(Agent user);

        void cacheWebServiceData();

        void progressBarManagement(Func<double, double> progressBarFunc);

        Task UpdateClientDependenciesAsync(List<Client> clientList, bool isActiveProgress = false);

        List<Client> GetClientData(int nbLine);

        List<Client> GetClientDataByBillList(List<Bill> billList);

        List<Order> GetQuoteCLient(int id);

        List<Order> GetOrderClient(int id);

        List<Client> searchClient(Client client, ESearchOption filterOperator);

        List<Client> GetClientDataById(int id);

    } /* end interface IClientManager */
}