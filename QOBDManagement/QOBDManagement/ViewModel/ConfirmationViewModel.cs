using MaterialDesignThemes.Wpf;
using QOBDManagement.Classes;
using QOBDManagement.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QOBDManagement.ViewModel
{
    public class ConfirmationViewModel : INotifyPropertyChanged
    {
        string _message;
        bool _response;
        bool _isDialogOpen;
        bool _isChatDialogOpen;
        bool _isLeftBarClosed;

        public event PropertyChangedEventHandler PropertyChanged;

        public ConfirmationViewModel()
        {
            _message = "";
        }

        public void onPropertyChange([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public string TxtMessage
        {
            get { return _message; }
            set { _message = value; onPropertyChange(); }
        }

        public bool Response
        {
            get { return _response; }
            set { _response = value; onPropertyChange(); }
        }

        public bool IsDialogOpen
        {
            get { return _isDialogOpen; }
            set { _isDialogOpen = value; onPropertyChange(); }
        }

        public bool IsChatDialogOpen
        {
            get { return _isChatDialogOpen; }
            set { _isChatDialogOpen = value; onPropertyChange(); }
        }

        public bool IsLeftBarClosed
        {
            get { return _isLeftBarClosed; }
            set { _isLeftBarClosed = value; onPropertyChange("IsLeftBarClosed"); }
        }

        public void showSearch(string message, bool isChatDialogBox = false)
        {
            if (Application.Current != null)
                Application.Current.Dispatcher.Invoke(() => {
                    showSearchMessage(message, isChatDialogBox);
                });
        }

        public async Task<bool> showAsync(string message, bool isChatDialogBox = false)
        {
            bool result = false;

            if (Application.Current != null)
                result = await Application.Current.Dispatcher.Invoke(async () => {
                    return await showMessageAsync(message, isChatDialogBox);
                });
            return result;
        }

        public async Task<bool> showAsync(object viewModel, bool isChatDialogBox = false)
        {
            bool result = false;

            if (Application.Current != null)
                result = await Application.Current.Dispatcher.Invoke(async()=> {
                    return await showMessageViewModelAsync(viewModel, isChatDialogBox);
                });
            return result;
        }

        private async Task<bool> showMessageAsync(string message, bool isChatDialogBox = false)
        {
            IsDialogOpen = false;
            TxtMessage = message;
            object result = new object();

            result = await DialogHost.Show(this, getDialogBox(isChatDialogBox));

            if ((result as bool?) != null)
                Response = (bool)result;
            return Response;
        }

        private async Task<bool> showMessageViewModelAsync(object viewModel, bool isChatDialogBox = false)
        {
            IsDialogOpen = false;
            object result = new object();

            result = await DialogHost.Show(viewModel, getDialogBox(isChatDialogBox));

            if ((result as bool?) != null)
                Response = (bool)result;

            return Response;
        }

        public async void showSearchMessage(string message, bool isChatDialogBox = false)
        {
            TxtMessage = message;
            IsDialogOpen = false;
            await DialogHost.Show(new Views.SearchConfirmationView(), getDialogBox(isChatDialogBox));
        }

        private string getDialogBox(bool isChatDialogBox = false)
        {
            string result = "";
            if (isChatDialogBox)
            {
                result = "RootDialogChatRoom";
                _isChatDialogOpen = true;
            }
            else
            {
                result = "RootDialog";
                _isDialogOpen = true;
            }
                
            return result;
        }

    }
}
