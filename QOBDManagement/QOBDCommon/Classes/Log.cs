using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Classes
{
    public static class Log
    {
        static string directory;
        static string fileName;
        static string fileFullPath;
        static object _lock = new object();

        public static void initialize()
        {
            fileName = "log_" + DateTime.Now.ToString("yyyy_MM") + ".txt";
            directory = Utility.getDirectory("Logs");
            fileFullPath = Utility.getDirectory(directory, fileName);
        }

        public static void error(string message, Enum.EErrorFrom errorFromPage, [CallerMemberName] string callerName = null, string localCallerName = null)
        {
            initialize();
            lock (_lock) 
                try
                {
                    if (string.IsNullOrEmpty(localCallerName))
                        File.AppendAllLines(fileFullPath, new List<string> { string.Format(@"[{0}]-[{1}] - [{2}][{3}] {4}", DateTime.Now.ToString("yy/MM/dd HH:mm:ss"), "ERR", errorFromPage.ToString(), callerName, message) });
                    else
                        File.AppendAllLines(fileFullPath, new List<string> { string.Format(@"[{0}]-[{1}] - [{2}][{3}] {4}", DateTime.Now.ToString("yy/MM/dd HH:mm:ss"), "ERR", errorFromPage.ToString(), localCallerName, message) });
                }
                catch (Exception) { }
        }

        public static void warning(string message, Enum.EErrorFrom errorFromPage, [CallerMemberName] string callerName = null, string localCallerName = null)
        {
            initialize();
            lock (_lock) try
                {

                    if (string.IsNullOrEmpty(localCallerName))
                        File.AppendAllLines(fileFullPath, new List<string> { string.Format(@"[{0}]-[{1}] - [{2}][{3}] {4}", DateTime.Now.ToString("yy/MM/dd HH:mm:ss"), "WAR", errorFromPage.ToString(), callerName, message) });
                    else
                        File.AppendAllLines(fileFullPath, new List<string> { string.Format(@"[{0}]-[{1}] - [{2}][{3}] {4}", DateTime.Now.ToString("yy/MM/dd HH:mm:ss"), "WAR", errorFromPage.ToString(), localCallerName, message) });
                }
                catch (Exception) { }
        }

        public static void debug(string message, Enum.EErrorFrom errorFromPage, [CallerMemberName] string callerName = null, string localCallerName = null)
        {
            initialize();
            lock (_lock) try
                {

                    if (string.IsNullOrEmpty(localCallerName))
                        File.AppendAllLines(fileFullPath, new List<string> { string.Format(@"[{0}]-[{1}] - [{2}][{3}] {4}", DateTime.Now.ToString("yy/MM/dd HH:mm:ss"), "TES", errorFromPage.ToString(), callerName, message) });
                    else
                        File.AppendAllLines(fileFullPath, new List<string> { string.Format(@"[{0}]-[{1}] - [{2}][{3}] {4}", DateTime.Now.ToString("yy/MM/dd HH:mm:ss"), "TES", errorFromPage.ToString(), localCallerName, message) });
                }
                catch (Exception) { }

        }

        public static void write(string message, string messageType, Enum.EErrorFrom errorFromPage, [CallerMemberName] string callerName = null)
        {
            switch (messageType)
            {
                case "ERR":
                    error(message, errorFromPage, localCallerName: callerName);
                    break;
                case "WAR":
                    warning(message, errorFromPage, localCallerName: callerName);
                    break;
                default:
                    debug(message, errorFromPage, localCallerName: callerName);
                    break;
            }
        }
    }
}
