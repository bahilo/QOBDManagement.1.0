using QOBDManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOBDManagement.Interfaces
{
    public interface ICart
    {
        void AddItem(Cart_itemModel cart_itemModel);
        void RemoveItem(Cart_itemModel cart_itemModel);

    }
}
