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

namespace QOBDManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel mainWindowViewModel;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void load()
        {            
            mainWindowViewModel = new MainWindowViewModel(new Classes.Startup());
            this.DataContext = mainWindowViewModel;
        }

        private void Window_Closing(object sender, EventArgs e)
        {
            mainWindowViewModel.Dispose();
        }

        private void DialogHost_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
    }
}
