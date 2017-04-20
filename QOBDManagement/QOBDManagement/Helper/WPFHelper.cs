using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDManagement.Classes;
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
                Log.warning(ex.Message, QOBDCommon.Enum.EErrorFrom.HELPER);
            }
            return output;
        }

        /// <summary>
        /// downloading picture from the ftp server
        /// </summary>
        /// <param name="image">Image to update</param>
        /// <param name="recordedFileName">The image file name already in the database</param>
        /// <param name="fileName">The new image file name in case of absence of a record</param>
        /// <param name="ftpCredentialInfoList">The credential information list</param>
        /// <returns></returns>
        public static InfoManager.Display downloadPicture(this InfoManager.Display image, string ftpDirectory, string localDirectory, string recordedFileName, string fileName, List<Info> ftpCredentialInfoList)
        {
            Info usernameInfo = ftpCredentialInfoList.Where(x => x.Name == "ftp_login").FirstOrDefault() ?? new Info();
            Info passwordInfo = ftpCredentialInfoList.Where(x => x.Name == "ftp_password").FirstOrDefault() ?? new Info();

            if (ftpCredentialInfoList.Count > 0)
            {
                Info info = new Info { Name = fileName, Value = fileName };
                // closing item picture file before update
                if (image != null)
                    image.closeImageSource();
                else
                    image = new InfoManager.Display(ftpDirectory, localDirectory, usernameInfo.Value, passwordInfo.Value);

                if (!string.IsNullOrEmpty(recordedFileName) && recordedFileName.Split('.').Count() > 1 && !string.IsNullOrEmpty(recordedFileName.Split('.')[0]))
                {
                    info.Name = recordedFileName.Split('.')[0].Replace(' ', '_').Replace(':', '_');
                    info.Value = recordedFileName;
                }                    

                image.TxtFileNameWithoutExtension = info.Name;
                image.FilterList = new List<string> { info.Name };
                image.updateFields(new List<Info> { info });
                image.downloadFile();
            }
            return image;
        }


    }
}
