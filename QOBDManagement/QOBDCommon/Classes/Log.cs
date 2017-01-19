﻿using System;
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

            if (!File.Exists(fileFullPath))
                File.Create(fileFullPath);
        }

        public static void error(string message, [CallerMemberName] string callerName = null, string localCallerName = null)
        {
            initialize();
            lock (_lock) try
                {

                    if (string.IsNullOrEmpty(localCallerName))
                        File.AppendAllLines(fileFullPath, new List<string> { string.Format(@"[{0}]-[{1}] - [{2}] {3}", DateTime.Now.ToString("dd/MM/yy HH:mm:ss"), "ERR", callerName, message) });
                    else
                        File.AppendAllLines(fileFullPath, new List<string> { string.Format(@"[{0}]-[{1}] - [{2}] {3}", DateTime.Now.ToString("dd/MM/yy HH:mm:ss"), "ERR", localCallerName, message) });
                }
                catch (Exception) { }
        }

        public static void warning(string message, [CallerMemberName] string callerName = null, string localCallerName = null)
        {
            initialize();
            lock (_lock) try
                {

                    if (string.IsNullOrEmpty(localCallerName))
                        File.AppendAllLines(fileFullPath, new List<string> { string.Format(@"[{0}]-[{1}] - [{2}] {3}", DateTime.Now.ToString("dd/MM/yy HH:mm:ss"), "WAR", callerName, message) });
                    else
                        File.AppendAllLines(fileFullPath, new List<string> { string.Format(@"[{0}]-[{1}] - [{2}] {3}", DateTime.Now.ToString("dd/MM/yy HH:mm:ss"), "WAR", localCallerName, message) });
                }
                catch (Exception) { }
        }

        public static void debug(string message, [CallerMemberName] string callerName = null, string localCallerName = null)
        {
            initialize();
            lock (_lock) try
                {

                    if (string.IsNullOrEmpty(localCallerName))
                        File.AppendAllLines(fileFullPath, new List<string> { string.Format(@"[{0}]-[{1}] - [{2}] {3}", DateTime.Now.ToString("dd/MM/yy HH:mm:ss"), "TES", callerName, message) });
                    else
                        File.AppendAllLines(fileFullPath, new List<string> { string.Format(@"[{0}]-[{1}] - [{2}] {3}", DateTime.Now.ToString("dd/MM/yy HH:mm:ss"), "TES", localCallerName, message) });
                }
                catch (Exception) { }

        }

        public static void write(string message, string messageType, [CallerMemberName] string callerName = null)
        {
            switch (messageType)
            {
                case "ERR":
                    error(message, localCallerName: callerName);
                    break;
                case "WAR":
                    warning(message, localCallerName: callerName);
                    break;
                default:
                    debug(message, localCallerName: callerName);
                    break;
            }
        }
    }
}
