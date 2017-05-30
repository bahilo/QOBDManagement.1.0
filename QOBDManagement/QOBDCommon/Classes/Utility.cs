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

        public static string stringSpliter(string stringToSplit, int nbCharToDisplay = 40)
        {
            string valueToProcess = stringToSplit as string;
            string output = "";
            if (valueToProcess != null)
            {
                if (valueToProcess.Length > nbCharToDisplay)
                {
                    int index = 0;
                    string newContent = "";
                    var stringTable = valueToProcess.Split(' ').ToList();
                    while (index < stringTable.Count)
                    {
                        newContent += stringTable[index] + " ";
                        if (newContent.Length >= nbCharToDisplay)
                        {
                            output += newContent + Environment.NewLine;
                            newContent = "";
                        }
                        index++;
                    }
                    output += newContent;
                }
                else
                    output = valueToProcess;

                return output;
            }
            return stringToSplit;
        }


    }
}
