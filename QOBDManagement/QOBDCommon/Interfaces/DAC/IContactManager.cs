using QOBDCommon.Entities;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Interfaces.DAC
{
    public interface IContactManager: REMOTE.IContactManager
    {
        List<Contact> GetContactData(int nbLine);

        List<Contact> GetContactDataByClientList(List<Client> clientList);

        List<Contact> searchContact(Contact Contact, ESearchOption filterOperator);

        List<Contact> GetContactDataById(int id);
    }
}
