using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QOBDCommon.Classes
{
    public static class Utility
    {
        public static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
        public static DateTime DateTimeMinValueInSQL2005 = new DateTime(1753, 1, 1);
        public static string BaseDirectory = getOrCreateDirectory(Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), ConfigurationManager.AppSettings["info_company_name"]); //AppDomain.CurrentDomain.BaseDirectory;
        public static List<string> MaterialDesignColourList = new List<string> { "Accent", "Dark", "Inverted", "PrimaryDark", "PrimaryLight", "PrimaryMid" };
        public static List<string> ColourList = new List<string> { "DarkBlue", "DarkGreen", "DarkMagenta", "DarkOrange", "DarkRed", "DarkOrchid", "DarkCyan", "DarkGoldenrod", "DarkSalmon", "DarkSeaGreen", "DarkSlateGray", "DarkTurquoise", "DarkViolet", "DeepPink" };

        public static DateTime convertToDateTime(string dateString, bool? isFromDatePicker = false)
        {
            object _lock = new object();
            lock (_lock)
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
                if (DateTime.TryParse(dateString, out outDate))
                    return outDate;

                return new DateTime();
            }
        }

        public static bool convertToBoolean(string boolString)
        {
            object _lock = new object();
            lock (_lock)
            {
                bool outBool = false;
                if (bool.TryParse(boolString, out outBool))
                    return outBool;
                return outBool;
            }
        }

        public static int intTryParse(string input)
        {
            object _lock = new object();
            lock (_lock)
            {
                try
                {
                    return int.Parse(input, System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public static decimal decimalTryParse(string input)
        {
            object _lock = new object();
            lock (_lock)
            {
                try
                {
                    return decimal.Round(decimal.Parse(input, System.Globalization.CultureInfo.InvariantCulture), 7);
                }
                catch (Exception)
                {
                    return 0m;
                }
            }
        }

        public static long longTryParse(string input)
        {
            object _lock = new object();
            lock (_lock)
            {
                try
                {
                    return long.Parse(input, System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public static double doubleTryParse(string input)
        {
            object _lock = new object();
            lock (_lock)
            {
                try
                {
                    return double.Parse(input, System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public static string encodeStringToBase64(string stringToEncode)
        {
            object _lock = new object();
            lock (_lock)
            {
                if (!string.IsNullOrEmpty(stringToEncode))
                {
                    byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.UTF8.GetBytes(stringToEncode);
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

                var str = encodeStringToBase64(returnValue);
                if (str != encodedString)
                    return encodedString;

                return returnValue;
            }
        }

        public static bool isBase64Encoded(string inputBase64Encoded)
        {
            var decodedString = decodeBase64ToString(inputBase64Encoded);
            var encodedString = encodeStringToBase64(decodedString);

            return encodedString == inputBase64Encoded;
        }

        public static bool isMD5Encoded(string encodedInput)
        {
            return Regex.IsMatch(encodedInput, "[0-9a-z]{32}");
        }

        public static string getDirectory(string directory, params string[] pathElements)
        {
            object _lock = new object();
            lock (_lock)
            {
                string path = "";
                string[] dirElements = new string[]{ };
                string[] allPathElements = new string[]{ };

                if (!string.IsNullOrEmpty(directory))
                {
                    dirElements = directory.Split('/');
                    allPathElements = dirElements.Concat(pathElements).ToArray();
                }                    

                if (!string.IsNullOrEmpty(BaseDirectory))
                    path = BaseDirectory;

                foreach (string pathElement in allPathElements)
                {
                    if (!string.IsNullOrEmpty(pathElement))
                        path = Path.Combine(path, pathElement);
                }

                return Path.GetFullPath(path);
            }
        }

        public static string getOrCreateDirectory(string directory, params string[] pathElements)
        {
            string path = getDirectory(directory, pathElements);

            // check if it is a full path file or only directory 
            var pathChecking = Path.GetFileName(path).Split('.');

            if (!string.IsNullOrEmpty(path) && !File.Exists(path) && !Directory.Exists(path))
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
            object _lock = new object();
            lock (_lock)
            {
                foreach (var dict in dictSource)
                {
                    if (!dictTarget.Keys.Contains(dict.Key))
                        dictTarget.Add(dict.Key, dict.Value);
                }

                return dictTarget;
            }
        }

        public static List<T> concat<T>(List<T> Target, List<T> Source)
        {
            object _lock = new object();
            lock (_lock)
            {
                foreach (var value in Source)
                {
                    if (!Target.Contains(value))
                        Target.Add(value);
                }

                return Target;
            }
        }

        public static string stringSpliter(string stringToSplit, int nbCharToDisplay = 40)
        {
            object _lock = new object();
            lock (_lock)
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

        public static string getRandomMaterialDesignColour()
        {
            return MaterialDesignColourList[randomProvider(0, MaterialDesignColourList.Count - 1)];
        }

        public static string getRandomColour()
        {
            return ColourList[randomProvider(0, ColourList.Count - 1)];
        }

        public static int randomProvider(int min, int max)
        {
            object _lock = new object();
            lock (_lock)
            {
                if (min > max || min < 0)
                    throw new ArgumentException("Range min [" + min + "] and max [" + max + "] for random numbers are not correct");

                byte[] randomNumber = new byte[1];
                bool IsminRandom;
                bool IsMaxRandom;
                do
                {
                    rngCsp.GetBytes(randomNumber);
                    byte Val = (byte)((randomNumber[0] % (byte)max) + 1);
                    IsminRandom = (byte)min <= Val;
                    IsMaxRandom = Val <= (byte)max;
                } while (!(IsminRandom && IsMaxRandom));

                return (byte)((randomNumber[0] % max) + 1);
            }
        }
    }
}
