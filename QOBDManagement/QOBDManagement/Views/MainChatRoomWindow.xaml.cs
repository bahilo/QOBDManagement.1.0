using QOBDManagement.Classes;
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
    /// Interaction logic for MainChatRoomWindow.xaml
    /// </summary>
    public partial class MainChatRoomWindow : UserControl
    {
        public MainChatRoomWindow()
        {
            InitializeComponent();
        }

        private void MainChatRoom_Unloaded(object sender, RoutedEventArgs e)
        {
            if(this.DataContext as ChatRoomViewModel != null)
                ((ChatRoomViewModel)this.DataContext).DiscussionViewModel.DiscussionModel = new Models.DiscussionModel();
        }
    }
}
