using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QOBDCommon.Classes
{
    public static class Utility
    {
        public static DateTime DateTimeMinValueInSQL2005 = new DateTime(1753, 1, 1);
        public static string BaseDirectory = getDirectory(Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), ConfigurationManager.AppSettings["info_company_name"]); //AppDomain.CurrentDomain.BaseDirectory;

        public static DateTime convertToDateTime(string dateString, bool? isFromDatePicker = false)
        {          
            try
            {
                var listDateElement = dateString.Split('/');
                if (isFromDatePicker == true && listDateElement.Count() > 1)
                {
                    int day = Convert.ToInt32(listDateElement[1]);
                    int month = Convert.ToInt32(listDateElement[0]);
                    int year = Convert.ToInt32(listDateElement[2].Split(' ')[0]);
                    dateString = day + "/" + month + "/" + year;// +" "+ DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                }
            }
            catch (Exception)
            {
                Log.warning("Error parsing date: '" + dateString + "'", Enum.EErrorFrom.UTILITY);
            }

            DateTime outDate = new DateTime();
            if (DateTime.TryParse(dateString, out outDate) && outDate > DateTimeMinValueInSQL2005)
                return outDate;

            return DateTimeMinValueInSQL2005;
        }

        public static bool convertToBoolean(string boolString)
        {
            bool outBool = new bool();
            if (bool.TryParse(boolString, out outBool))
                return outBool;

            return false;
        }

        public static int intTryParse(string input)
        {
            int result = 0;
            if (int.TryParse(input, out result))
                return result;
            return result;
        }

        public static decimal decimalTryParse(string input)
        {
            decimal result = 0m;
            if (decimal.TryParse(input, out result))
                return result;
            return result;
        }

        public static double doubleTryParse(string input)
        {
            double result = 0;
            if (double.TryParse(input, out result))
                return result;
            return result;
        }

        public static string encodeStringToBase64(string stringToEncode)
        {
            object _lock = new object();
            lock (_lock)
            {
                if (!string.IsNullOrEmpty(stringToEncode))
                {
                    byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(stringToEncode);
                    string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);

                    return returnValue;
                }

                return stringToEncode;
            }            
        }

        public static string decodeBase64ToString(string encodedString)
        {
            object _lock = new object();
            lock (_lock)
            {
                string returnValue = "";
                try
                {
                    if (!string.IsNullOrEmpty(encodedString))
                    {
                        byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedString);
                        returnValue = System.Text.Encoding.UTF8.GetString(encodedDataAsBytes);
                    }
                }
                catch (Exception)
                {
                    return encodedString;
                }

                if (encodeStringToBase64(returnValue) != encodedString)
                    return encodedString;

                return returnValue;
            }            
        }       


        public static bool uploadFIle(string ftpUrl, string fileFullPath, string username, string password)
        {
            bool isComplete = false;
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpUrl);
            req.UseBinary = true;
            req.KeepAlive = true;
            req.Method = WebRequestMethods.Ftp.UploadFile;
            req.Credentials = new NetworkCredential(username, password);
            Stream requestStream = null;
            FtpWebResponse response = default(FtpWebResponse);

            byte[] buffer;
            int totalByte;
            int bytes = 0;

            FileStream fs = File.Open(fileFullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            totalByte = (int)fs.Length;
            try
            {
                buffer = new byte[4096];
                req.ContentLength = fs.Length;
                requestStream = req.GetRequestStream();

                while (totalByte > 0)
                {
                    bytes = fs.Read(buffer, 0, buffer.Length);
                    requestStream.Write(buffer, 0, bytes);
                    totalByte = totalByte - bytes;
                }                
            }
            catch (WebException ex)
            {
                String status = ((FtpWebResponse)ex.Response).StatusDescription;
                Log.error(status, Enum.EErrorFrom.UTILITY);
            }
            catch (Exception ex)
            {
                Log.error(ex.Message, Enum.EErrorFrom.UTILITY);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
                if(requestStream != null)
                    requestStream.Close();
            }

            try
            {
                downloadFIle(ftpUrl, fileFullPath, username, password);

                response = (FtpWebResponse)req.GetResponse();
                if (response != null && response.StatusCode.Equals(FtpStatusCode.ClosingData))
                    isComplete = true;
            }
            catch (Exception ex)
            {
                Log.error(ex.Message, Enum.EErrorFrom.UTILITY);
            }

            return isComplete;
        }

        public static bool downloadFIle(string ftpUrl, string fileFullPath, string username, string password)
        {
            bool isComplete = false;
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpUrl);
            req.UseBinary = true;
            req.KeepAlive = false;
            req.Method = WebRequestMethods.Ftp.DownloadFile;
            req.Credentials = new NetworkCredential(username, password);
            req.Timeout = 600000;
            FtpWebResponse response = null;
            Stream ftpStream = null;

            FileStream fs = null;
            int bytes = 0;
            int bufferSize = 4096;
            byte[] buffer = new byte[bufferSize];

            try
            {
                response = (FtpWebResponse)req.GetResponse();
                ftpStream = response.GetResponseStream();

                //if (File.Exists(fileFullPath))
                //    fileFullPath.Clone();

                fs = new FileStream(fileFullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Write);
                
                while (true)
                {
                    bytes = ftpStream.Read(buffer, 0, bufferSize);

                    if (bytes == 0)
                        break;

                    fs.Write(buffer, 0, bytes);
                }
            }
            catch (WebException ex)
            {
                String status = ((FtpWebResponse)ex.Response).StatusDescription;
                Log.warning(status + " => " + ex.Message, Enum.EErrorFrom.UTILITY);
            }
            catch(Exception ex)
            {
                Log.error(ex.Message, Enum.EErrorFrom.UTILITY);
            }
            finally
            {
                if(fs != null)
                    fs.Close();
                if(response != null)
                    response.Close();
                if(ftpStream != null)
                    ftpStream.Close();
            }
                        
            if (response != null && response.StatusCode.Equals(FtpStatusCode.ClosingData))
                isComplete = true;

            return isComplete;
        }

        public static string getDirectory(string directory, params string[] pathElements)
        {
            string path = "";
            var dirElements = directory.Split('/');
            var allPathElements = dirElements.Concat(pathElements).ToArray();

            if (!string.IsNullOrEmpty(BaseDirectory))
                path = BaseDirectory;

            foreach (string pathElement in allPathElements)
            {
                if (!string.IsNullOrEmpty(pathElement))
                    path = Path.Combine(path, pathElement);
            }

            // check if it is a full path file or only directory 
            var pathChecking = Path.GetFileName(path).Split('.');

            if (!File.Exists(path) && !Directory.Exists(path))
            {

                if (pathChecking.Count() > 1)
                {
                    var dir = Path.GetDirectoryName(path);
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    File.Create(path);
                }
                else
                    Directory.CreateDirectory(path);
            }

            return Path.GetFullPath(path);
        }

        public static Dictionary<T, P> concat<T, P>(Dictionary<T, P> dictTarget, Dictionary<T, P> dictSource)
        {
            foreach (var dict in dictSource)
            {
                if(!dictTarget.Keys.Contains(dict.Key))
                    dictTarget.Add(dict.Key, dict.Value);
            }

            return dictTarget;
        }

        public static List<T> concat<T>(List<T> Target, List<T> Source)
        {
            foreach (var value in Source)
            {
                if(!Target.Contains(value))
                Target.Add(value);
            }

            return Target;
        }


    }
}
