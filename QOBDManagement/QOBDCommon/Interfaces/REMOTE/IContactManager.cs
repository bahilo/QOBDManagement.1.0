using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.REMOTE
{
    public interface IContactManager
    {
        Task<List<Contact>> InsertContactAsync(List<Contact> listContact);

        Task<List<Contact>> UpdateContactAsync(List<Contact> listContact);

        Task<List<Contact>> DeleteContactAsync(List<Contact> listContact);

        Task<List<Contact>> GetContactDataByClientListAsync(List<Client> clientList);

        Task<List<Contact>> GetContactDataAsync(int nbLine);

        Task<List<Contact>> searchContactAsync(Contact Contact, ESearchOption filterOperator);
    }
}
