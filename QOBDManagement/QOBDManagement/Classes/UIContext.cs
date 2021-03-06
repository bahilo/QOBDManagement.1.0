﻿using QOBDManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace QOBDManagement.Classes
{
    class UIContext
    {
        public object setWindowContext(UserControl view)
        {
            view.DataContext = null;
            var parent = FindParent.FindChildParent<Window>(view);
            if (parent != null)
            {
                view.DataContext = (MainWindowViewModel)parent.DataContext;
            }
            return view.DataContext;
        }

        public object setChatWindowContext(UserControl view)
        {
            object result = setWindowContext(view);
            if(result != null)
                view.DataContext = ((MainWindowViewModel)result).ChatRoomViewModel;           

            return view.DataContext;
        }
    }
}
