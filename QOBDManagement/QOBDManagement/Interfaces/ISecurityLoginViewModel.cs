﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Interfaces
{
    public interface ISecurityLoginViewModel
    {
        Task showLoginView();
        void Dispose();
    }
}
