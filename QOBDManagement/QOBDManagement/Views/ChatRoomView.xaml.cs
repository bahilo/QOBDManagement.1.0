using QOBDCommon.Classes;
using QOBDManagement.Classes;
using QOBDManagement.Interfaces;
using QOBDManagement.Models;
using QOBDManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QOBDManagement.Views
{
    /// <summary>
    /// Interaction logic for ChatRoomView.xaml
    /// </summary>
    public partial class ChatRoomView : UserControl
    {

        public ChatRoomView()
        {
            InitializeComponent();
        }

        private void ChatRoomWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if(this.DataContext as DiscussionViewModel != null)
            {
                ((DiscussionViewModel)this.DataContext).load();
            }
        }
    }
}
