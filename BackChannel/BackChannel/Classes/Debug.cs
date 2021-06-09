using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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

        public static string CurrentFilePath { get; set; }

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

        private static void WriteLogAsync(string type, string infoToWrite, string filePath, int lineNumber)
        {
            var fileName = "Logs.txt";

            // The folder for the roaming current user 
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Combine the appdata folder with the settings location
            var settingsDirectory = $"{folder}\\BackChannel\\Logs";

            CurrentFilePath = settingsDirectory;

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

            var line = sr.ReadLine();

            if (line != null)
            {
                logs.Add(line);
            }

            while (line != null)
            {
                line = sr.ReadLine();
                if (line != null)
                {
                    logs.Add(line);
                }
            }

            //close the file
            sr.Close();

            if (logs.Count > 2000)
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
        /// <param name="Buttons">A bitmask of the butons to show.<br></br>[0] = Close Popup<br></br>[1] = Close App<br></br>[2] = Open Log File.</param>
        public static void ShowError(string type, string info, byte[] Buttons)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                // Get a reference to the main window
                var wnd = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

                // Set the info popup properties
                wnd.ErrorTextTitle.Text = $"{type} Error";
                wnd.ErrorTextDescription.Text = info;
                wnd.ErrorPopup.Visibility = Visibility.Visible;

                if (Buttons[0] == 1)
                {
                    wnd.ClosePopupButton.Visibility = Visibility.Visible;
                }
                else
                {
                    wnd.ClosePopupButton.Visibility = Visibility.Collapsed;
                }

                if (Buttons[1] == 1)
                {
                    wnd.CloseAppButton.Visibility = Visibility.Visible;
                }
                else
                {
                    wnd.CloseAppButton.Visibility = Visibility.Collapsed;
                }

                if (Buttons[2] == 1)
                {
                    wnd.OpenLogButton.Visibility = Visibility.Visible;
                }
                else
                {
                    wnd.OpenLogButton.Visibility = Visibility.Collapsed;
                }

            }));
        }
    }
}
