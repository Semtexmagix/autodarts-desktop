using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace autodarts_visual
{
    public enum AutodartsExternPlatforms
    {
        lidarts,
        dartboards,
        nakka
    }

    public class AppManager
    {   
        // Attribute
        // Key = Download-Link
        // Value = Speicherort
        private KeyValuePair<string, string> autodarts = new("https://github.com/autodarts/releases/releases/download/v0.17.0/autodarts0.17.0.windows-amd64.zip", "autodarts");
        private KeyValuePair<string, string> autodartsCaller = new("https://github.com/lbormann/autodarts-caller/releases/download/v1.2.1/autodarts-caller.exe", "autodarts-caller");
        private KeyValuePair<string, string> autodartsExtern = new("https://github.com/lbormann/autodarts-extern/releases/download/v1.3.0/autodarts-extern.exe", "autodarts-extern");
        private KeyValuePair<string, string> autodartsBot = new("https://github.com/xinixke/autodartsbot/releases/download/0.0.1/autodartsbot-0.0.1.windows.x64.zip", "autodarts-bot");
        private KeyValuePair<string, string> virtualDartsZoom = new("https://www.lehmann-bo.de/Downloads/VDZ/Virtual Darts Zoom.zip", "virtual-darts-zoom");
        private KeyValuePair<string, string> dartboardsClient = new("https://dartboards.online/dboclient_0.8.6.exe", "dartboards-client");
        private const string autodartsUrl = "https://autodarts.io";



        private string? pathToApps;
        public event EventHandler<EventArgs> DownloadAppStarted;
        public event EventHandler<EventArgs> DownloadAppProgressed;
        public event EventHandler<EventArgs> DownloadAppStopped;


        public AppManager()
        {
            string strExeFilePath = Assembly.GetExecutingAssembly().Location;
            pathToApps = Path.GetDirectoryName(strExeFilePath);
        }



        // Methoden

        public List<string> GetAppsAvailable()
        {
            List<string> appsAvailable = new List<string>();

            string executable;

            executable = FindExecutable(autodarts.Value);
            if (executable != null)
            {
                appsAvailable.Add(autodarts.Value);
            }
            executable = FindExecutable(autodartsCaller.Value);
            if (executable != null)
            {
                appsAvailable.Add(autodartsCaller.Value);
            }
            executable = FindExecutable(autodartsExtern.Value);
            if (executable != null)
            {
                appsAvailable.Add(autodartsExtern.Value);
            }
            executable = FindExecutable(autodartsBot.Value);
            if (executable != null)
            {
                appsAvailable.Add(autodartsBot.Value);
            }
            executable = FindExecutable(virtualDartsZoom.Value);
            if (executable != null)
            {
                appsAvailable.Add(virtualDartsZoom.Value);
            }
            executable = FindExecutable(dartboardsClient.Value);
            if (executable != null)
            {
                appsAvailable.Add(dartboardsClient.Value);
            }
            return appsAvailable;
        }

        public void DownloadAutodarts()
        {
            DownloadApp(autodarts);
        }

        public void DownloadAutodartsCaller()
        {
            DownloadApp(autodartsCaller);
        }

        public void DownloadAutodartsExtern()
        {
            DownloadApp(autodartsExtern);
        }

        public void DownloadAutodartsBot()
        {
            DownloadApp(autodartsBot);
        }

        public void DownloadVirtualDartsZoom()
        {
            DownloadApp(virtualDartsZoom);
        }

        public void DownloadDartboardsClient()
        {
            DownloadApp(dartboardsClient);
        }


        public void RunAutodartsPortal()
        {
            var ps = new ProcessStartInfo(autodartsUrl)
            {
                UseShellExecute = true,
                Verb = "open"
            };
            Process.Start(ps);
        }

        public void RunAutodarts()
        {
            string appPath = Path.Join(pathToApps, autodarts.Value);
            RunApp(appPath);
        }

        public void RunAutodartsCaller()
        {
            string autodartsUser = Properties.Settings.Default.emailautodarts;
            string autodartsPassword = Properties.Settings.Default.pwautodarts;
            string autodartsBoardId = Properties.Settings.Default.boardid;
            string autodartsSounds = Properties.Settings.Default.media;
            string autodartsCallerVol = Properties.Settings.Default.callervol;
            string autodartsRandomCaller = Properties.Settings.Default.randomcaller;
            string autodartsRandomCallerEachLeg = Properties.Settings.Default.randomcallereachleg;
            string autodartsCallerWtt = Properties.Settings.Default.callerwtt;

            string appPath = Path.Join(pathToApps, autodartsCaller.Value);
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
            string argumentDelimitter = " ";

            RunApp(appPath, arguments, argumentDelimitter);
        }

        public void RunAutodartsExtern(AutodartsExternPlatforms platform)
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
            
            string appPath = Path.Join(pathToApps, autodartsExtern.Value);
            string argumentDelimitter = " ";

            IDictionary<string, string> arguments = new Dictionary<string, string>
            {
                { "--browser_path", browserpath },
                { "--host_port", port },
                { "--autodarts_user", autodartsUser },
                { "--autodarts_password", autodartsPassword },
                { "--autodarts_board_id", autodartsBoardId },
                { "--extern_platform", platform.ToString() },
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

            RunApp(appPath, arguments, argumentDelimitter);
        }

        public void RunAutodartsBot()
        {
            string appPath = Path.Join(pathToApps, autodartsBot.Value);
            RunApp(appPath);
        }

        public void RunVirtualDartsZoom()
        {
            string appPath = Path.Join(pathToApps, virtualDartsZoom.Value);
            RunApp(appPath);
        }

        public void RunDartboardsClient()
        {
            string appPath = Path.Join(pathToApps,dartboardsClient.Value);
            RunApp(appPath);
        }

        public void RunOpenBroadcasterSofware()
        {
            RunExtraApp(Properties.Settings.Default.obs);
        }




        private void DownloadApp(KeyValuePair<string, string> app)
        {
            try
            {
                string appFileName = GetAppFileNameByUrl(app.Key);
                string appPath = Path.Join(pathToApps, app.Value);
                string downloadPath = Path.Join(appPath, appFileName);

                if (Directory.Exists(appPath))
                {
                    Directory.Delete(appPath, true);
                }
                Directory.CreateDirectory(appPath);

                // Informiere die GUI, das ein Download gestartet wird
                OnDownloadAppStarted(EventArgs.Empty);

                // Starte den Download
                DownloadApp(app.Key, downloadPath);
            }
            catch (Exception)
            {
                OnDownloadAppStopped(EventArgs.Empty);
                throw;
            }
        }
        private void DownloadApp(string url, string path)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;
            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
            webClient.QueryString.Add("pathToDownload", path);
            webClient.DownloadFileAsync(new Uri(url), path);
            Console.WriteLine("###########################################################################");
            Console.WriteLine("###############  Download nach: " + path + " wurde gestartet");
            Console.WriteLine("###########################################################################");
        }
        private static void RunApp(string workingDirectory)
        {
            RunApp(workingDirectory, new Dictionary<string, string> { }, " ");
        }
        private static void RunApp(string workingDirectory, IDictionary<string, string> arguments, string argumentDelimitter)
        {
            string executable = FindExecutable(workingDirectory);
            if (executable == null)
            {
                throw new FileNotFoundException(workingDirectory + " not found");
            }

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

            //Console.WriteLine(composedArguments);

            using (var process = new Process())
            {
                try
                {
                    process.StartInfo.WorkingDirectory = workingDirectory;
                    process.StartInfo.FileName = executable;
                    process.StartInfo.Arguments = composedArguments;
                    process.StartInfo.RedirectStandardOutput = false;
                    process.StartInfo.RedirectStandardError = false;
                    process.StartInfo.UseShellExecute = true;
                    process.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred trying to start \"{executable}\" with \"{composedArguments}\":\n{ex.Message}");
                    throw;
                }
            }
        }

        private static void RunExtraApp(string pathToExecutable)
        {
            using (var process = new Process())
            {
                try
                {
                    process.StartInfo.WorkingDirectory = Path.GetFullPath(Path.GetDirectoryName(pathToExecutable));
                    process.StartInfo.FileName = Path.GetFileName(pathToExecutable); ;
                    process.StartInfo.UseShellExecute = true;
                    //process.StartInfo.Verb = "runas";
                    process.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred trying to start \"{pathToExecutable}\":\n{ex.Message}");
                    throw;
                }
            }
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            OnDownloadAppProgressed(e);
        }

        private void WebClient_DownloadFileCompleted(object? sender, AsyncCompletedEventArgs e)
        {
            string pathToFile = (sender as WebClient).QueryString["pathToDownload"];
            if (Path.GetExtension(pathToFile).ToLower() == ".zip")
            {
                ZipFile.ExtractToDirectory(pathToFile, Path.GetDirectoryName(pathToFile));
            }
            OnDownloadAppStopped(EventArgs.Empty);
            Console.WriteLine("###########################################################################");
            Console.WriteLine("###############  Download abgeschlossen");
            Console.WriteLine("###########################################################################");
        }

        private static string? FindExecutable(string appPath)
        {
            if (!Directory.Exists(appPath))
            {
                return null;
            }
            string executable = Directory
                .EnumerateFiles(appPath, "*.*", SearchOption.AllDirectories)
                .FirstOrDefault(s => Path.GetExtension(s).TrimStart('.').ToLowerInvariant() == "exe");
            return executable;
        }

        private static string GetAppFileNameByUrl(string url)
        {
            string[] urlSplitted = url.Split("/");
            return urlSplitted[urlSplitted.Length - 1];
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
