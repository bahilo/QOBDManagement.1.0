using QOBDBusiness;
using QOBDCommon.Entities;
using QOBDManagement.Models;
using QOBDManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Interfaces
{
    public interface IClientViewModel
    {
        // properties

        BusinessLogic Bl { get; }
        string Title { get; set; }
        List<ClientModel> ClientModelList { get; set; }
        List<Agent> AgentList { get; set; }
        ClientDetailViewModel ClientDetailViewModel { get; set; }
        ClientModel SelectedCLientModel { get; set; }
        ClientModel ClientModel { get; set; }


        // actions

        void loadClients();
        List<ClientModel> clientListToModelViewList(List<Client> clientList);
        void Dispose();
    }
}
