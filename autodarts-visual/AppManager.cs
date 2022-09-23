using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace autodarts_visual
{
    public class AppManager
    {   
        // Attribute
        private const string pathToApps = @".\";
        private const string autodartsUrl = "https://autodarts.io";
        private const string downloadUrlAutodartsCaller = "https://github.com/lbormann/autodarts-caller/releases/download/v1.2.1/autodarts-caller.exe";
        private const string downloadUrlAutodartsExtern = "https://github.com/lbormann/autodarts-extern/releases/download/v1.3.0/autodarts-extern.exe";
        //private const string downloadUrlAutodartsBot = "https://github.com/xinixke/autodartsbot/releases/download/0.0.1/autodartsbot-0.0.1.windows.x64.zip";
        //private const string downloadUrlVdz = "https://www.lehmann-bo.de/Downloads/VDZ/Virtual%20Darts%20Zoom.zip";
        //private const string downloadUrlDartboardsClient = "https://dartboards.online/dboclient_0.8.6.exe";

        public event EventHandler<EventArgs> DownloadAppStarted;
        public event EventHandler<EventArgs> DownloadAppProgressed;
        public event EventHandler<EventArgs> DownloadAppStopped;


        // Öffentliche Methoden

        // Install

        public void InstallAutodartsCaller()
        {
            InstallApp(downloadUrlAutodartsCaller);
        }

        public void InstallAutodartsExtern()
        {
            InstallApp(downloadUrlAutodartsExtern);
        }

        public void InstallAutodartsBot()
        {
            throw new NotImplementedException();
        }

        public void InstallVdz()
        {
            throw new NotImplementedException();
        }

        public void InstallDartboardsClient()
        {
            throw new NotImplementedException();
        }


        // Run

        public static void RunAutodartsWeb()
        {
            var ps = new ProcessStartInfo(autodartsUrl)
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(ps);
        }

        public static void RunAutodartsCaller()
        {
            string autodartsUser = Properties.Settings.Default.emailautodarts;
            string autodartsPassword = Properties.Settings.Default.pwautodarts;
            string autodartsBoardId = Properties.Settings.Default.boardid;
            string autodartsSounds = Properties.Settings.Default.media;
            string autodartsCallerVol = Properties.Settings.Default.callervol;
            string autodartsRandomCaller = Properties.Settings.Default.randomcaller;
            string autodartsRandomCallerEachLeg = Properties.Settings.Default.randomcallereachleg;
            string autodartsCallerWtt = Properties.Settings.Default.callerwtt;

            string appFileName = GetAppFileNameByUrl(downloadUrlAutodartsCaller);
            string appPath = Path.Join(pathToApps, RemoveFileExtension(appFileName));
            string argumentDelimitter = " ";

            IDictionary<string, string> arguments = new Dictionary<string, string>
            {
                    { "-U", autodartsUser },
                    { "-P", autodartsPassword },
                    { "-B", autodartsBoardId },
                    { "-M", autodartsSounds },
                    { "-V", autodartsCallerVol },
                    { "-R", autodartsRandomCaller },
                    { "-L", autodartsRandomCallerEachLeg },
                    { "-WTT", autodartsCallerWtt }
            };
            RunApp(appFileName, appPath, arguments, argumentDelimitter);

        }

        public static void RunAutodartsExtern(string portal)
        {
            string port = Properties.Settings.Default.hostport;
            string autodartsUser = Properties.Settings.Default.emailautodarts;
            string autodartsPassword = Properties.Settings.Default.pwautodarts;
            string autodartsBoardId = Properties.Settings.Default.boardid;
            string lidartsUser = Properties.Settings.Default.emaillidarts;
            string lidartsPassword = Properties.Settings.Default.pwlidarts;
            string skipdarts = Properties.Settings.Default.skipdarts;
            string timer = Properties.Settings.Default.timetoend;
            string messagestart = Properties.Settings.Default.messagestart;
            string messageend = Properties.Settings.Default.messageend;
            string browserpath = Properties.Settings.Default.browserpath;
            string dboUser = Properties.Settings.Default.dbouser;
            string dboPassword = Properties.Settings.Default.dbopw;
            
            string appFileName = GetAppFileNameByUrl(downloadUrlAutodartsExtern);
            string appPath = Path.Join(pathToApps, RemoveFileExtension(appFileName));
            string argumentDelimitter = " ";

            IDictionary<string, string> arguments = new Dictionary<string, string>
            {
                { "--browser_path", browserpath },
                { "--host_port", port },
                { "--autodarts_user", autodartsUser },
                { "--autodarts_password", autodartsPassword },
                { "--autodarts_board_id", autodartsBoardId },
                { "--extern_platform", portal },
                { "--time_before_exit", timer },
                { "--lidarts_user", lidartsUser },
                { "--lidarts_password", lidartsPassword },
                { "--lidarts_skip_dart_modals", skipdarts },
                { "--lidarts_chat_message_start", messagestart },
                { "--lidarts_chat_message_end", messageend },
                { "--nakka_skip_dart_modals", skipdarts },
                { "--dartboards_user", dboUser },
                { "--dartboards_password", dboPassword },
                { "--dartboards_skip_dart_modals", skipdarts }
            };

            RunApp(appFileName, appPath, arguments, argumentDelimitter);
        }

        public static void RunAutodartsBot()
        {
            throw new NotImplementedException();
        }

        public static void RunVdz()
        {
            throw new NotImplementedException();
        }

        public static void RunDartboardsClient()
        {
            throw new NotImplementedException();
        }




        // private- / interne Methoden
        private void InstallApp(string url)
        {
            try
            {
                // wir holen uns den Namen der App einfach aus der Url und haben so ein allgemeines Schema für
                // unsere Ordnerstrukturen
                string appFileName = GetAppFileNameByUrl(url);
                string appFolderName = RemoveFileExtension(appFileName);
                string appPath = Path.Join(pathToApps, appFolderName);
                string downloadPath = Path.Join(appPath, appFileName);

                if (Directory.Exists(appPath))
                {
                    Directory.Delete(appPath, true);
                }
                Directory.CreateDirectory(appPath);

                OnDownloadAppStarted(EventArgs.Empty);
                DownloadApp(url, downloadPath);
            }
            catch (Exception)
            {
                OnDownloadAppStopped(EventArgs.Empty);
                throw;
            }
        }

        private static void RunApp(string processName, string workingDirectory, IDictionary<string, string> arguments, string argumentDelimitter)
        {
            // Wir setzen die übergebenen 'arguments' zu einen String zusammen, der dann beim Prozess starten genutzt werden kann
            string composedArguments = "";

            // Wir durchlaufen alle übergebenen Argumente und hängen diese dem String an
            foreach (KeyValuePair<string, string> a in arguments)
            {
                // Das Start Argument hat kein Key-Value, deshalb unterscheiden wir hier und nehmen an,
                // dass es kein Value gibt, wenn der Wert ein leerer String ist.
                if (String.IsNullOrEmpty(a.Value))
                {
                    composedArguments += " " + a.Key;
                }
                // ... sonst hängen wir den Value an
                else
                {
                    composedArguments += " " + a.Key + argumentDelimitter + "\"" + a.Value + "\"";
                }
            }

            Console.WriteLine(composedArguments);

            using (var process = new Process())
            {
                try
                {
                    process.StartInfo.WorkingDirectory = workingDirectory;
                    process.StartInfo.FileName = processName;
                    process.StartInfo.Arguments = composedArguments;
                    process.StartInfo.RedirectStandardOutput = false;
                    process.StartInfo.RedirectStandardError = false;
                    process.StartInfo.UseShellExecute = true;
                    process.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred trying to start \"{processName}\" with \"{composedArguments}\":\n{ex.Message}");
                    throw;
                }
            }
        }

        private void DownloadApp(string url, string path)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
            webClient.DownloadFileAsync(new System.Uri(url), path);
            Console.WriteLine("###########################################################################");
            Console.WriteLine("###############  Download nach: " + path + " wurde gestartet");
            Console.WriteLine("###########################################################################");
        }

        private static string GetAppFileNameByUrl(string url)
        {
            string[] urlSplitted = url.Split("/");
            return urlSplitted[urlSplitted.Length - 1];
        }

        private static string RemoveFileExtension(string filename)
        {
            string extension = Path.GetExtension(filename);
            return filename.Substring(0, filename.Length - extension.Length);
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            OnDownloadAppProgressed(e);
        }

        private void WebClient_DownloadFileCompleted(object? sender, AsyncCompletedEventArgs e)
        {
            OnDownloadAppStopped(EventArgs.Empty);
        }

        protected virtual void OnDownloadAppStarted(EventArgs e)
        {
            if (DownloadAppStarted != null)
                DownloadAppStarted(this, e);
        }

        protected virtual void OnDownloadAppProgressed(EventArgs e)
        {
            if (DownloadAppProgressed != null)
                DownloadAppProgressed(this, e);
        }

        protected virtual void OnDownloadAppStopped(EventArgs e)
        {
            if (DownloadAppStopped != null)
                DownloadAppStopped(this, e);
        }


    }
}
