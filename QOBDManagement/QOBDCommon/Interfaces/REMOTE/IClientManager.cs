using QOBDCommon;
using QOBDCommon.Entities;
using QOBDCommon.Enum;
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
namespace QOBDCommon.Interfaces.REMOTE
{
    public interface IClientManager : IContactManager, IAddressManager, INotifyPropertyChanged, IDisposable
    {
        // Operations

        void setServiceCredential(object channel);

        Task<List<Client>> InsertClientAsync(List<Client> clientList);

        Task<List<Client>> UpdateClientAsync(List<Client> clientList);

        Task<List<Client>> DeleteClientAsync(List<Client> clientList);

        Task<List<Client>> GetClientDataAsync(int nbLine);

        Task<List<Client>> GetClientDataByBillListAsync(List<Bill> billList);

        Task<List<Client>> GetClientMaxCreditOverDataByAgentAsync(int agentId);

        Task<List<Client>> searchClientAsync(Client client, ESearchOption filterOperator);

    } /* end interface IClientManager */
}