using QOBDCommon.Classes;
using QOBDCommon.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        private bool confirmed;
        MainWindowViewModel mainWindowViewModel;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void load()
        {
            // initialize the DataDirectory to the user local folder
            AppDomain.CurrentDomain.SetData("DataDirectory", Utility.BaseDirectory);

            var unWritableAppDataDir = Utility.getDirectory(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
            var writableAppDataDir = (string)AppDomain.CurrentDomain.GetData("DataDirectory");

            try
            {
                // delete database if exists
                if (File.Exists(System.IO.Path.Combine(Utility.getDirectory("App_Data"), "QCBDDatabase.sdf")))
                    File.Delete(System.IO.Path.Combine(Utility.getDirectory("App_Data"), "QCBDDatabase.sdf"));

                // delete icon if exists
                if (File.Exists(System.IO.Path.Combine(Utility.getDirectory("Docs", "images"), "favicon.ico")))
                    File.Delete(System.IO.Path.Combine(Utility.getDirectory("Docs", "images"), "favicon.ico"));

                // copy the database to user local folder
                if (!File.Exists(System.IO.Path.Combine(Utility.getDirectory("App_Data"), "QCBDDatabase.sdf")))
                    File.Copy(System.IO.Path.Combine(unWritableAppDataDir, "QCBDDatabase.sdf"), System.IO.Path.Combine(Utility.getDirectory("App_Data"), "QCBDDatabase.sdf"));
                
                // copy the favicon to user local folder
                if (!File.Exists(System.IO.Path.Combine(Utility.getDirectory("Docs", "images"), "favicon.ico")))
                    File.Copy(System.IO.Path.Combine(Utility.getDirectory("Docs", "images"), "favicon.ico"), System.IO.Path.Combine(Utility.getDirectory("Docs", "images"), "favicon.ico"));

            }
            catch (Exception ex)
            {
                Log.error(ex.Message, EErrorFrom.MAIN);
            }
            mainWindowViewModel = new MainWindowViewModel(new Classes.Startup());
            this.DataContext = mainWindowViewModel;
        }

        private async void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!confirmed && mainWindowViewModel.AuthenticatedUserModel.Agent.ID != 0 && mainWindowViewModel.AuthenticatedUserModel.TxtStatus.Equals(EStatus.Active.ToString()))
            {
                e.Cancel = true;
                if (await mainWindowViewModel.DisposeAsync())
                {     
                    confirmed = true;
                    this.Close();
                }
            }    
        }

        private void DialogHost_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
    }
}
