using BackChannel.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace BackChannel.Classes
{
    /// <summary>
    /// A class for easy access to standard debugging functions
    /// </summary>
    public static class Debug
    {
        /// <summary>
        /// Must be set on application startup. <br></br>
        /// Used to identify and seperate logs.
        /// </summary>
        public static string SessionID { get; set; }

        /// <summary>
        /// Creates the SessionId for this run of the application
        /// </summary>
        public static void CreateSessionID()
        {
            var localDate = DateTime.Now;
            var culture = new CultureInfo("en-US");
            var protoId = localDate.ToString(culture);
            protoId = protoId.Replace("/", "");
            protoId = protoId.Replace(":", "");
            protoId = protoId.Replace(" ", "");
            SessionID = protoId;
        }

        /// <summary>
        /// Writes a log to the session log file
        /// </summary>
        /// <param name="type">The severity of the info to tag.</param>
        /// <param name="infoToWrite">The string to write to the log file</param>
        public static async void WriteLog(string type, string infoToWrite, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            Task.Run(() => WriteLogAsync(type, infoToWrite, filePath, lineNumber));
        }

        /// <summary>
        /// Actuall writes the log on a seperate thread.
        /// </summary>
        /// <param name="type">The severity of the info to tag.</param>
        /// <param name="infoToWrite">The string to write to the log file</param>
        /// <param name="filePath"></param>
        /// <param name="lineNumber"></param>
        private static void WriteLogAsync(string type, string infoToWrite, string filePath, int lineNumber)
        {
            // Get the file and the folder names/path.
            var fileName = "Logs.txt";
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var settingsDirectory = $"{folder}\\BackChannel\\Logs";

            // Create Directory if it doesn't exist. 
            Directory.CreateDirectory(settingsDirectory);

            // Read in the log file if it exists
            if (!File.Exists($"{settingsDirectory}\\{fileName}"))
            {
                while (true)
                {
                    try
                    {
                        FileStream stream = File.Create($"{settingsDirectory}\\{fileName}");
                        stream.Close();
                        break;
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            // Get the current logs from the log file.
            StreamReader sr = null;
            while (true)
            {
                try
                {
                    sr = new StreamReader($"{settingsDirectory}\\{fileName}");
                    break;
                }
                catch (Exception)
                {

                }
            }
            var logs = new List<string>();
            var line = sr.ReadLine(); // Get the first line
            if (line != null)
            {
                logs.Add(line);
            }
            while (line != null) // Get the rest of the line
            {
                line = sr.ReadLine();
                if (line != null)
                {
                    logs.Add(line);
                }
            }
            sr.Close(); // Close the file

            if (logs.Count > 2000) // If the file gets to fat, delete old info
            {
                logs.Clear();
            }

            // Get the date and time
            var localDate = DateTime.Now;
            var culture = new CultureInfo("en-US");

            // Add the latest log
            logs.Add($"({localDate.ToString(culture)}):\n<{filePath}::{lineNumber}>\n[{type}] {infoToWrite}");

            // Write to the log file
            StreamWriter sw = null;
            while (true)
            {
                try
                {
                    sw = new StreamWriter($"{settingsDirectory}\\{fileName}");
                    break;
                }
                catch (Exception)
                {

                }
            }
            foreach (string s in logs)
            {
                sw.WriteLine(s);
            }
            sw.Close();
        }

        /// <summary>
        /// Displays the error popup in the center of the application.
        /// </summary>
        /// <param name="type">The type of error.</param>
        /// <param name="info">The error message/info.</param>
        /// <param name="layout">The layout of the panel, aka the buttons to show at the bottom</param>
        public static void ShowError(string type, string info, DebugPopupType layout)
        {
            MainWindow wnd = null;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                // Get a reference to the main window
                wnd = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            }));
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {          
                wnd.ErrorTextTitle.Text = $"{type} Error";
                wnd.ErrorTextDescription.Text = info;
                wnd.ErrorPopup.Visibility = Visibility.Visible;
            }));
            // Set the info popup properties
            switch (layout)
            {
                case DebugPopupType.Notify:
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        wnd.ClosePopupButton.Visibility = Visibility.Visible;
                        wnd.CloseAppButton.Visibility = Visibility.Collapsed;
                        wnd.CanelButton.Visibility = Visibility.Collapsed;
                        wnd.AllowButton.Visibility = Visibility.Collapsed;
                    }));
                    break;
                case DebugPopupType.PossiblyFatal:
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        wnd.ClosePopupButton.Visibility = Visibility.Visible;
                        wnd.CloseAppButton.Visibility = Visibility.Visible;
                        wnd.CanelButton.Visibility = Visibility.Collapsed;
                        wnd.AllowButton.Visibility = Visibility.Collapsed;
                    }));
                    break;
                case DebugPopupType.Fatal:
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        wnd.ClosePopupButton.Visibility = Visibility.Collapsed;
                        wnd.CloseAppButton.Visibility = Visibility.Visible;
                        wnd.CanelButton.Visibility = Visibility.Collapsed;
                        wnd.AllowButton.Visibility = Visibility.Collapsed;
                    }));
                    break;
                case DebugPopupType.Security:
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        wnd.ClosePopupButton.Visibility = Visibility.Collapsed;
                        wnd.CloseAppButton.Visibility = Visibility.Collapsed;
                        wnd.CanelButton.Visibility = Visibility.Visible;
                        wnd.AllowButton.Visibility = Visibility.Visible;
                    }));
                    break;
            }
        }
    }
}
