using QOBDCommon.Classes;
using QOBDCommon.Entities;
using QOBDManagement.Classes;
using QOBDManagement.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace QOBDManagement.Helper
{

    public static class WPFHelper
    {
        public static string addPrefix(this string valueString, EPrefix type)
        {
            object _lock = new object();
            lock (_lock)
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
        }

        public static string addPrefix(this int valueInteger, EPrefix type)
        {
            object _lock = new object();
            lock (_lock)
            {
                if (valueInteger == 0)
                    return valueInteger.ToString();
                else
                    return addPrefix(valueInteger.ToString(), type);
            }
        }

        public static string deletePrefix(this string valueString)
        {
            object _lock = new object();
            lock (_lock)
            {
                int length = 0;
                string output = "";
                try
                {
                    length = Convert.ToInt32(ConfigurationManager.AppSettings["length_prefix"]);
                    if (valueString.Length > length)
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
        }

        /// <summary>
        /// downloading picture from the ftp server
        /// </summary>
        /// <param name="image">Image to update</param>
        /// <param name="recordedFileName">The image file name already in the database</param>
        /// <param name="fileName">The new image file name in case of absence of a record</param>
        /// <param name="ftpCredentialInfoList">The ftp credential information list</param>
        /// <returns>Image</returns>
        public static InfoManager.Display getPicture(this InfoManager.Display image, string ftpDirectory, string localDirectory, string recordedFileName, string fileName, List<Info> ftpCredentialInfoList)
        {
            object _lock = new object();
            lock (_lock)
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
                    {
                        // image must be created on the IU Thread
                        if (Application.Current != null)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                image = new InfoManager.Display(ftpDirectory, localDirectory, usernameInfo.Value, passwordInfo.Value);
                            });
                        }
                    }

                    if (!string.IsNullOrEmpty(recordedFileName) && recordedFileName.Split('.').Count() > 1 && !string.IsNullOrEmpty(recordedFileName.Split('.')[0]))
                    {
                        info.Name = recordedFileName.Split('.')[0].Replace(' ', '_').Replace(':', '_');
                        info.Value = recordedFileName;
                    }

                    if (image != null)
                    {
                        image.TxtFileNameWithoutExtension = info.Name;
                        image.FilterList = new List<string> { info.Name };
                        image.updateFields(new List<Info> { info });
                    }
                }
                return image;
            }
        }


        public static InfoManager.Display downloadPicture(this InfoManager.Display image, string ftpDirectory, string localDirectory, string recordedFileName, string fileName, List<Info> ftpCredentialInfoList)
        {
            if (ftpCredentialInfoList.Count > 0)
            {
                image = image.getPicture(ftpDirectory, localDirectory, recordedFileName, fileName, ftpCredentialInfoList);
                if(image != null)
                    image.downloadFile();
            }                
            return image;
        }

        public static string resizeImage(this string imageFullPath)
        {
            object _lock = new object();
            lock (_lock)
            {
                int newImageSizeWidth = Utility.intTryParse(ConfigurationManager.AppSettings["image_size_width"]);
                int newImageSizeHeight = Utility.intTryParse(ConfigurationManager.AppSettings["image_size_height"]);

                // return image if new image sizes are not set
                if (newImageSizeWidth == 0 || newImageSizeHeight == 0 || !File.Exists(imageFullPath))
                    return imageFullPath;

                // getting the original image information
                Bitmap image = new Bitmap(imageFullPath);
                double originalWidth = image.Width;
                double originalHeight = image.Height;

                // tempory image file
                string tmpFolder = Utility.getOrCreateDirectory(ConfigurationManager.AppSettings["local_tmp_folder"]);
                //string fileName = Path.GetFileName(imageFullPath).Split('.')[0] +".jpeg";
                string outImageFullPath = Path.Combine(tmpFolder, Path.GetFileName(imageFullPath));

                // getting the image ratio to preserve quality
                double ratioX = newImageSizeWidth / originalWidth;
                double ratioY = newImageSizeHeight / originalHeight;
                double ratio = Math.Min(ratioX, ratioY);

                // new width and height based on the ratio
                double newOriginalWidht = originalWidth * ratio;
                double newOriginalHeight = originalHeight * ratio;

                // convert image into format RGB
                Bitmap newImage = new Bitmap((int)newOriginalWidht, (int)newOriginalHeight, PixelFormat.Format24bppRgb);

                using (Graphics graphics = Graphics.FromImage(newImage))
                {
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphics.DrawImage(image, 0, 0, (int)newOriginalWidht, (int)newOriginalHeight);
                }

                newImage.Save(outImageFullPath, ImageFormat.Png);

                return outImageFullPath;
            }
        }

        /// <summary>
        /// Method to get encoder info for given image format.
        /// </summary>
        /// <param name="format">Image format</param>
        /// <returns>image codec info.</returns>
        private static ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            return ImageCodecInfo.GetImageDecoders().SingleOrDefault(c => c.FormatID == format.Guid);
        }

        /// <summary>
        /// upload a file to the ftp server
        /// </summary>
        /// <param name="ftpUrl">ftp url</param>
        /// <param name="fileFullPath">file to send</param>
        /// <param name="username">ftp credential username</param>
        /// <param name="password">ftp credential password</param>
        /// <returns>upload status</returns>
        public static bool ftpSendFile(string ftpUrl, string fileFullPath, string username, string password)
        {
            object _lock = new object();
            lock (_lock)
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

                try
                {
                    using (requestStream = req.GetRequestStream())
                    using (FileStream fs = new FileStream(fileFullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
                    {
                        totalByte = (int)fs.Length;
                        buffer = new byte[4096];
                        req.ContentLength = fs.Length;

                        while (totalByte > 0)
                        {
                            bytes = fs.Read(buffer, 0, buffer.Length);
                            requestStream.Write(buffer, 0, bytes);
                            totalByte = totalByte - bytes;
                        }
                    }
                }
                catch (WebException ex)
                {
                    String status = ((FtpWebResponse)ex.Response).StatusDescription;
                    Log.error(status, QOBDCommon.Enum.EErrorFrom.HELPER);
                }
                catch (Exception ex)
                {
                    Log.error(ex.Message, QOBDCommon.Enum.EErrorFrom.HELPER);
                }
                finally
                {
                    if (response != null)
                        response.Close();
                }

                try
                {
                    ftpGetFile(ftpUrl, fileFullPath, username, password);

                    response = (FtpWebResponse)req.GetResponse();
                    if (response != null && response.StatusCode.Equals(FtpStatusCode.ClosingData))
                        isComplete = true;
                }
                catch (Exception ex)
                {
                    Log.error(ex.Message, QOBDCommon.Enum.EErrorFrom.HELPER);
                }

                return isComplete;
            }
        }

        public static bool sendFileToFtpServer(string remotePath, string fileName, string fileToSendFullPath, List<Info> credentials)
        {
            object _lock = new object();
            lock (_lock)
            {
                string ftpHost = ConfigurationManager.AppSettings["ftp"];
                string ftpUrl = ftpHost + Path.Combine(remotePath, fileName);

                Info usernameInfo = credentials.Where(x => x.Name == "ftp_login").FirstOrDefault() ?? new Info();
                Info passwordInfo = credentials.Where(x => x.Name == "ftp_password").FirstOrDefault() ?? new Info();

                return ftpSendFile(ftpUrl, fileToSendFullPath, usernameInfo.Value, passwordInfo.Value);
            }
        }

        /// <summary>
        /// download a file from the ftp server
        /// </summary>
        /// <param name="ftpUrl">ftp url</param>
        /// <param name="fileFullPath">downloaded file name</param>
        /// <param name="username">ftp credential username</param>
        /// <param name="password">ftp credential password</param>
        /// <returns>download status</returns>
        public static bool ftpGetFile(string ftpUrl, string fileFullPath, string username, string password)
        {
            object _lock = new object();
            lock (_lock)
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

                int bytes = 0;
                int bufferSize = 4096;
                byte[] buffer = new byte[bufferSize];
                try
                {
                    using (response = (FtpWebResponse)req.GetResponse())
                    using (ftpStream = response.GetResponseStream())
                    using (FileStream fs = new FileStream(fileFullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
                    {

                        while (true)
                        {
                            bytes = ftpStream.Read(buffer, 0, bufferSize);

                            if (bytes == 0)
                                break;

                            fs.Write(buffer, 0, bytes);
                        }
                    }
                }
                catch (WebException ex)
                {
                    if (!ex.Message.Contains("not found"))
                    {
                        String status = ((FtpWebResponse)ex.Response).StatusDescription;
                        Log.warning(status + " => " + ex.Message, QOBDCommon.Enum.EErrorFrom.HELPER);
                    }
                    else
                        isComplete = true;
                }
                catch (Exception ex)
                {
                    Log.error(ex.Message, QOBDCommon.Enum.EErrorFrom.HELPER);
                }
                finally
                {
                    if (response != null)
                        response.Close();
                    if (ftpStream != null)
                        ftpStream.Close();
                }

                if (!isComplete && response != null && response.StatusCode.Equals(FtpStatusCode.ClosingData))
                    isComplete = true;

                return isComplete;
            }
        }

        public static bool getFileFromFtpServer(string remotePath, string fileName, string outputFileName, List<Info> credentials)
        {
            object _lock = new object();
            lock (_lock)
            {
                string ftpHost = ConfigurationManager.AppSettings["ftp"];
                string ftpUrl = ftpHost + Path.Combine(remotePath, fileName);

                Info usernameInfo = credentials.Where(x => x.Name == "ftp_login").FirstOrDefault() ?? new Info();
                Info passwordInfo = credentials.Where(x => x.Name == "ftp_password").FirstOrDefault() ?? new Info();

                return ftpGetFile(ftpUrl, outputFileName, usernameInfo.Value, passwordInfo.Value);
            }
        }

        /// <summary>
        /// delete a file from the ftp server
        /// </summary>
        /// <param name="ftpUrl">ftp url</param>
        /// <param name="fileFullPath">downloaded file name</param>
        /// <param name="username">ftp credential username</param>
        /// <param name="password">ftp credential password</param>
        /// <returns>download status</returns>
        public static bool ftpDeleteFile(string ftpUrl, string username, string password)
        {
            object _lock = new object();
            lock (_lock)
            {
                bool isComplete = false;
                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpUrl);
                req.UseBinary = true;
                req.KeepAlive = false;
                req.Method = WebRequestMethods.Ftp.DeleteFile;
                req.Credentials = new NetworkCredential(username, password);
                req.Timeout = 600000;
                FtpWebResponse response = null;

                try
                {
                    response = (FtpWebResponse)req.GetResponse();
                }
                catch (WebException ex)
                {
                    if (!ex.Message.Contains("not found"))
                    {
                        String status = ((FtpWebResponse)ex.Response).StatusDescription;
                        Log.warning(status + " => " + ex.Message, QOBDCommon.Enum.EErrorFrom.HELPER);
                    }
                    else
                        isComplete = true;
                }
                catch (Exception ex)
                {
                    Log.error(ex.Message, QOBDCommon.Enum.EErrorFrom.HELPER);
                }
                finally
                {
                    if (response != null)
                        response.Close();
                }

                if (!isComplete && response != null && response.StatusCode.Equals(FtpStatusCode.FileActionOK))
                    isComplete = true;

                return isComplete;
            }
        }

        public static bool deleteFileFromFtpServer(string remotePath, string fileName, List<Info> credentials)
        {
            object _lock = new object();
            lock (_lock)
            {
                string ftpHost = ConfigurationManager.AppSettings["ftp"];
                string ftpUrl = ftpHost + Path.Combine(remotePath, fileName);

                Info usernameInfo = credentials.Where(x => x.Name == "ftp_login").FirstOrDefault() ?? new Info();
                Info passwordInfo = credentials.Where(x => x.Name == "ftp_password").FirstOrDefault() ?? new Info();

                return ftpDeleteFile(ftpUrl, usernameInfo.Value, passwordInfo.Value);
            }
        }


    }
}
