using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QOBDCommon.Classes
{
    public static class Utility
    {
        public static DateTime DateTimeMinValueInSQL2005 = new DateTime(1753, 1, 1);
        static string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        public static DateTime convertToDateTime(string dateString, bool? isFromDatePicker = false)
        {
            var listDateElement = dateString.Split('/');

            try
            {
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
                Log.write("Error parsing date " + dateString, "WAR");
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

        public static string encodeStringToBase64(string stringToEncode)
        {
            if (!string.IsNullOrEmpty(stringToEncode))
            {
                byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(stringToEncode);
                string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);

                return returnValue;
            }

            return stringToEncode;
        }

        public static string decodeBase64ToString(string encodedString)
        {
            string returnValue = "";
            try
            {
                byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedString);
                returnValue = System.Text.Encoding.UTF8.GetString(encodedDataAsBytes);
            }
            catch (Exception)
            {
                Debug.WriteLine(string.Format("[Warning] - decode base64 of not encoded variable ({0})", encodedString));
                return encodedString;
            }

            return returnValue;
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
                Log.error(status);
            }
            catch (Exception ex)
            {
                Log.error(ex.Message);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
                if(requestStream != null)
                    requestStream.Close();
            }

            downloadFile(ftpUrl, fileFullPath, username, password);

            FtpWebResponse response = (FtpWebResponse)req.GetResponse();
            if (response.StatusCode.Equals(FtpStatusCode.ClosingData)) // && count > 0)
                isComplete = true;

            return isComplete;
        }

        public static bool downloadFile(string ftpUrl, string fileFullPath, string username, string password)
        {
            object _lock = new object();
            lock (_lock)
            {
                bool isComplete = false;
                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpUrl);
                req.UseBinary = true;
                req.KeepAlive = true;
                req.Method = WebRequestMethods.Ftp.DownloadFile;
                req.Credentials = new NetworkCredential(username, password);
                req.Timeout = 600000;
                FtpWebResponse response = null;
                Stream ftpStream = null;

                FileStream fs = null;
                int totalByte;
                int bytes = 0;
                int bufferSize = 4096;
                byte[] buffer = new byte[bufferSize];

                try
                {
                    response = (FtpWebResponse)req.GetResponse();
                    ftpStream = response.GetResponseStream();

                    fs = new FileStream(fileFullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Write);
                    totalByte = (int)fs.Length;

                    while (totalByte > 0)
                    {
                        bytes = ftpStream.Read(buffer, 0, bufferSize);
                        fs.Write(buffer, 0, bytes);
                        totalByte = totalByte - bytes;
                    }
                }
                catch (WebException ex)
                {
                    String status = ((FtpWebResponse)ex.Response).StatusDescription;
                    Log.warning(ex.Message + " - " + ftpUrl);
                }
                catch (Exception ex)
                {
                    Log.error(ex.Message);
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
                    if (response != null)
                        response.Close();
                    if (ftpStream != null)
                        ftpStream.Close();
                }

                if (response != null && response.StatusCode.Equals(FtpStatusCode.ClosingData))
                    isComplete = true;

                return isComplete;
            }
            
        }

        public static string getDirectory(string directory, params string[] pathElements)
        {
            var dirElements = directory.Split('/');
            var allPathElements = dirElements.Concat(pathElements).ToArray();
            string path = _baseDirectory;
            foreach (string pathElement in allPathElements)
            {
                if (!string.IsNullOrEmpty(pathElement))
                    path = Path.Combine(path, pathElement);
            }

            // check if it is a full path file or only directory 
            var pathChecking = path.Split('.'); 

            if (!Directory.Exists(path) && pathChecking.Count() == 1)
                Directory.CreateDirectory(path);
            else if (!Directory.Exists(path) && pathChecking.Count() > 1)
                Directory.CreateDirectory(pathChecking[0]);


            return Path.GetFullPath(path);
        }


    }
}
