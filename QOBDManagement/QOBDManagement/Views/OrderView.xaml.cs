using QOBDManagement.Classes;
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
    /// Interaction logic for OrderView.xaml
    /// </summary>
    public partial class OrderView : UserControl
    {
        public OrderView()
        {
            InitializeComponent();
        }

        private void OrderView_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext dataContext = new DataContext();
            if (dataContext.setContext(this) != null)
            {
                if (!((MainWindowViewModel)this.DataContext).IsThroughContext)
                    ((MainWindowViewModel)this.DataContext).OrderViewModel.loadOrders();
                
            }
        }
    }
}
