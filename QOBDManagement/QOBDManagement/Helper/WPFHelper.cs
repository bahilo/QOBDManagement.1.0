using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDManagement.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace QOBDManagement.Helper
{
    
    public static class WPFHelper
    {
        public static string addPrefix(this string valueString, EPrefix type)
        {
            string prefix = "";

            switch (type)
            {
                case EPrefix.CLIENT:
                    prefix = ConfigurationManager.AppSettings["client_prefix"];
                    break;
                case EPrefix.ORDER:
                    prefix = ConfigurationManager.AppSettings["order_prefix"];
                    break;
                case EPrefix.INVOICE:
                    prefix = ConfigurationManager.AppSettings["invoice_prefix"];
                    break;
                case EPrefix.DELIVERY:
                    prefix = ConfigurationManager.AppSettings["delivery_prefix"];
                    break;
                case EPrefix.ITEM:
                    prefix = ConfigurationManager.AppSettings["item_prefix"];
                    break;
            }

            return prefix + valueString;
        }

        public static string addPrefix(this int valueInteger, EPrefix type)
        {
            if(valueInteger == 0)
                return valueInteger.ToString();
            else
                return addPrefix(valueInteger.ToString(), type);
        }

        public static string deletePrefix(this string valueString)
        {
            int length = 0;
            string output = "";
            try
            {
                length = Convert.ToInt32(ConfigurationManager.AppSettings["length_prefix"]);
                if(valueString.Length > length)
                    output = valueString.Substring(length);
                else
                    output = valueString;
            }
            catch (Exception ex)
            {
                Log.warning(ex.Message);
            }

            return output;
        }


    }
}
