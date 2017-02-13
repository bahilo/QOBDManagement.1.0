using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Interfaces
{
    public interface ISecurityLoginViewModel
    {
        void showLoginView();
        void startAuthentication();
        void Dispose();
    }
}
