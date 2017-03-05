﻿using QOBDManagement.Classes;
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
    /// Interaction logic for CLientSideBarView.xaml
    /// </summary>
    public partial class CLientSideBarView : UserControl
    {
        public CLientSideBarView()
        {
            InitializeComponent();
        }

        private void CLientSideBarView_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext dataContext = new DataContext();
            dataContext.setWindowContext(this);
        }
    }
}
