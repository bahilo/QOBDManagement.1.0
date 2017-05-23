using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Interfaces
{
    public interface IConfirmationViewModel: INotifyPropertyChanged
    {
        string TxtMessage { get; set; }
        bool Response { get; set; }
        bool IsDialogOpen { get; set; }
        bool IsChatDialogOpen { get; set; }
        bool IsLeftBarClosed { get; set; }
        bool IsChatLeftBarOpen { get; set; }

        void showSearch(string message, bool isChatDialogBox = false);
        Task<bool> showAsync(string message, bool isChatDialogBox = false);
        Task<bool> showAsync(object viewModel, bool isChatDialogBox = false);
        void showSearchingMessage(string message, bool isChatDialogBox = false);
    }
}
