using QOBDCommon.Entities;
using QOBDManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Interfaces
{
    public interface IClientViewModel
    {
        void loadClients();
        List<ClientModel> clientListToModelViewList(List<Client> clientList);
        ClientModel loadContactsAndAddresses(ClientModel cLientViewModel);
        void Dispose();
    }
}
