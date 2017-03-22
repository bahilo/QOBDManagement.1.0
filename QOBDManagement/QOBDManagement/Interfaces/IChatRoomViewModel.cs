using QOBDManagement.Models;
using QOBDManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Interfaces
{
    public interface IChatRoomViewModel
    {
        //----------------[ Properties ]--------------
        IMainWindowViewModel MainWindowViewModel { get; }
        DiscussionViewModel DiscussionViewModel { get; set; }
        MessageViewModel MessageViewModel { get; set; }

        //---------------[ Actions ]---------------------
        void cleanUp();
        void connectToServer();
        Task signOutFromServer(List<DiscussionModel> discussionList);
        object navigation(object centralPageContent = null);        
    }
}
