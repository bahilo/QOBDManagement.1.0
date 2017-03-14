using QOBDCommon.Enum;
using QOBDManagement.Classes;
using QOBDManagement.Command;
using QOBDManagement.Models;
using QOBDManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Interfaces
{
    public interface IMainWindowViewModel
    {
        //----------------[ Properties ]--------------
        ClientViewModel ClientViewModel { get; set; }
        ItemViewModel ItemViewModel { get; set; }
        OrderViewModel OrderViewModel { get; set; }
        AgentViewModel AgentViewModel { get; set; }
        NotificationViewModel NotificationViewModel { get; set; }
        HomeViewModel HomeViewModel { get; set; }
        ReferentialViewModel ReferentialViewModel { get; set; }
        StatisticViewModel StatisticViewModel { get; set; }
        QuoteViewModel QuoteViewModel { get; set; }
        ChatRoomViewModel ChatRoomViewModel { get; set; }
        SecurityLoginViewModel SecurityLoginViewModel { get; set; }
        Object CurrentViewModel { get; set; }
        Object ChatRoomCurrentView { get; set; }
        IStartup Startup { get; set; }
        bool isNewAgentAuthentication { get; set; }
        AgentModel AuthenticatedUserModel { get; }
        Cart Cart { get; }
        ButtonCommand<string> CommandNavig { get; set; }

        //---------------[ Actions ]---------------------
        object getObject(string objectName);
        double progressBarManagement(double status = 0);
        Object navigation(Object centralPageContent = null);
        DisplayAndData.Display.Image ImageManagement(DisplayAndData.Display.Image newImage = null, string fileType = null);
        bool securityCheck(EAction action, ESecurity right);
    }
}
