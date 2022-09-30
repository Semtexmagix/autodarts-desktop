using autodarts_desktop.Properties;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading;
using Path = System.IO.Path;

namespace autodarts_desktop
{
    public enum AutodartsExternPlatforms
    {
        lidarts,
        dartboards,
        nakka
    }

    public class AppConfigurationRequiredEventArgs : EventArgs
    {
        private readonly string _app;
        private readonly string _message;

        public AppConfigurationRequiredEventArgs(string app, string message)
        {
            _app = app;
            _message = message;
        }

        public string App
        {
            get { return _app; }
        }
        public string Message
        {
            get { return _message; }
        }

    }


    /// <summary>
    /// Manages everything around apps-lifecycle.
    /// </summary>
    public class AppManager
    {
        // Attributes
        // Key = Download-Link
        // Value = Storage-path
        public KeyValuePair<string, string> autodarts = new("https://github.com/autodarts/releases/releases/download/v0.17.0/autodarts0.17.0.windows-amd64.zip", "autodarts");
        public KeyValuePair<string, string> autodartsCaller = new("https://github.com/lbormann/autodarts-caller/releases/download/v1.3.0/autodarts-caller.exe", "autodarts-caller");
        public KeyValuePair<string, string> autodartsExtern = new("https://github.com/lbormann/autodarts-extern/releases/download/v1.4.0/autodarts-extern.exe", "autodarts-extern");
        public KeyValuePair<string, string> autodartsBot = new("https://github.com/xinixke/autodartsbot/releases/download/0.0.1/autodartsbot-0.0.1.windows.x64.zip", "autodarts-bot");
        public KeyValuePair<string, string> virtualDartsZoom = new("https://www.lehmann-bo.de/Downloads/VDZ/Virtual Darts Zoom.zip", "virtual-darts-zoom");
        public KeyValuePair<string, string> dartboardsClient = new("https://dartboards.online/dboclient_0.8.6.exe", "dartboards-client");
        public const string autodartsUrl = "https://autodarts.io";

        public event EventHandler<AppConfigurationRequiredEventArgs> AppDownloadRequired;
        public event EventHandler<AppConfigurationRequiredEventArgs> AppConfigurationRequired;
        public event EventHandler<EventArgs> DownloadAppStarted;
        public event EventHandler<EventArgs> DownloadAppProgressed;
        public event EventHandler<EventArgs> DownloadAppStopped;
        public event EventHandler<EventArgs> ConfigurationChanged;
        private const string argumentPrefixKey = "argumentPrefixKey";
        private const string argumentDelimiterKey = "argumentDelimiterKey";
        private const string specificFileKey = "specificFile";
        private const string argumentErrorKey = "ArgumentValidateParse-Error";
        private string? pathToApps;

        

        public AppManager()
        {
            string strExeFilePath = AppContext.BaseDirectory;
            pathToApps = Path.GetDirectoryName(strExeFilePath);
        }



        // Methods

        public void SaveConfigurationCustomApp(string pathToCustomApp, string customAppArguments)
        {
            Settings.Default.obs = pathToCustomApp;
            Settings.Default.customappargs = customAppArguments;
            Settings.Default.Save();
            OnConfigurationChanged(EventArgs.Empty);
        }

        public void CheckDefaultRequirements()
        {
            Dictionary<string, bool> appsInstallState = GetAppsInstallState();
            bool autodartsCallerInstalled = false;
            appsInstallState.TryGetValue(autodartsCaller.Value, out autodartsCallerInstalled);
            if (autodartsCallerInstalled == false)
            {
                DownloadAutodartsCaller();
                OnAppDownloadRequired(new AppConfigurationRequiredEventArgs(autodartsCaller.Value, "Requirements (autodarts-caller) not satisfied"));
            }
        }

        public void CloseRunningApps(){
            CloseRunningApp(autodarts);
            CloseRunningApp(autodartsCaller);
            CloseRunningApp(autodartsExtern);
            CloseRunningApp(autodartsBot);
            CloseRunningApp(virtualDartsZoom);
            CloseRunningApp(dartboardsClient);
        }

        public Dictionary<string, bool> GetAppsInstallState()
        {
            Dictionary<string, bool> appsInstallState = new();
            appsInstallState.Add(autodarts.Value, FindExecutable(autodarts.Value) != null ? true : false);
            appsInstallState.Add(autodartsCaller.Value, FindExecutable(autodartsCaller.Value) != null ? true : false);
            appsInstallState.Add(autodartsExtern.Value, FindExecutable(autodartsExtern.Value) != null ? true : false);
            appsInstallState.Add(autodartsBot.Value, FindExecutable(autodartsBot.Value) != null ? true : false);
            appsInstallState.Add(virtualDartsZoom.Value, FindExecutable(virtualDartsZoom.Value) != null ? true : false);
            appsInstallState.Add(dartboardsClient.Value, FindExecutable(dartboardsClient.Value) != null ? true : false);
            return appsInstallState;
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

        public bool RunAutodarts()
        {
            return RunApp(autodarts);
        }

        public bool RunAutodartsCaller()
        {
            return RunApp(autodartsCaller);
        }

        public bool RunAutodartsExtern(AutodartsExternPlatforms platform)
        {
            return RunApp(autodartsExtern, new Dictionary<string, string> { { "extern_platform", platform.ToString() } });
        }

        public bool RunAutodartsBot()
        {
            return RunApp(autodartsBot);
        }

        public bool RunVirtualDartsZoom()
        {
            return RunApp(virtualDartsZoom);
        }

        public bool RunDartboardsClient()
        {
            return RunApp(dartboardsClient);
        }

        public static void RunCustomApp()
        {
            string pathToExecutable = Settings.Default.obs;
            string arguments = Settings.Default.customappargs;

            using (var process = new Process())
            {
                try
                {
                    process.StartInfo.WorkingDirectory = Path.GetFullPath(Path.GetDirectoryName(pathToExecutable));
                    process.StartInfo.FileName = Path.GetFileName(pathToExecutable);
                    if (!String.IsNullOrEmpty(arguments))
                    {
                        process.StartInfo.Arguments = arguments;
                    }
                    process.StartInfo.UseShellExecute = true;
                    process.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred trying to start \"{pathToExecutable}\":\n{ex.Message}");
                    throw;
                }
            }
        }




        private void DownloadApp(KeyValuePair<string, string> app, string specificFile = "")
        {
            try
            {
                string appFileName = GetAppFileNameByUrl(app.Key);
                string appPath = Path.Join(pathToApps, app.Value);
                string downloadPath = Path.Join(appPath, appFileName);

                // Find the executable and kill running process
                string executable = String.IsNullOrEmpty(specificFile) ? FindExecutable(appPath) : FindExecutable(appPath, specificFile);
                if (executable != null)
                {
                    KillApp(Path.GetFileNameWithoutExtension(executable));
                    KillApp(Path.GetFileNameWithoutExtension(executable));
                }

                // Removes existing app and creates a new directory
                if (Directory.Exists(appPath))
                {
                    Directory.Delete(appPath, true);
                }
                Directory.CreateDirectory(appPath);

                // Inform subscribers about a pending download
                OnDownloadAppStarted(EventArgs.Empty);

                // Start the download
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
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            OnDownloadAppProgressed(e);
        }

        private void WebClient_DownloadFileCompleted(object? sender, AsyncCompletedEventArgs e)
        {
            try
            {
                // Extract download if zip-file
                string pathToFile = (sender as WebClient).QueryString["pathToDownload"];
                if (Path.GetExtension(pathToFile).ToLower() == ".zip")
                {
                    ZipFile.ExtractToDirectory(pathToFile, Path.GetDirectoryName(pathToFile));
                }
                OnDownloadAppStopped(e);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured: {ex.Message}");
                OnDownloadAppStopped(e);
            }
        }

        private bool RunApp(KeyValuePair<string, string> app, Dictionary<string, string> dynamicArguments = null)
        {
            // Get Configuration setup by user
            Dictionary<string, string> configuration = GetConfiguration(app, dynamicArguments);
            if (configuration == null) return false;
            string argumentPrefix;
            configuration.TryGetValue(argumentPrefixKey, out argumentPrefix);
            string argumentDelimitter;
            configuration.TryGetValue(argumentDelimiterKey, out argumentDelimitter);
            string specificFile;
            configuration.TryGetValue(specificFileKey, out specificFile);
            configuration.Remove(argumentPrefixKey);
            configuration.Remove(argumentDelimiterKey);
            configuration.Remove(specificFileKey);

            // Check if app exists
            string workingDirectory = Path.Join(pathToApps, app.Value);
            string executable = String.IsNullOrEmpty(specificFile) ? FindExecutable(workingDirectory) : FindExecutable(workingDirectory, specificFile);
            if (executable == null)
            {
                throw new FileNotFoundException(workingDirectory + " not found");
            }

            // Check if app is already running
            if (CheckAppRunningState(Path.GetFileNameWithoutExtension(executable)))
            {
                Console.WriteLine($"\"{executable}\" is already running");
                return true;
            }

            // Wir setzen die übergebenen 'arguments' zu einen String zusammen, der dann beim Prozess starten genutzt werden kann
            string composedArguments = "";

            // Wir durchlaufen alle übergebenen Argumente und hängen diese dem String an
            foreach (KeyValuePair<string, string> a in configuration)
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
                    composedArguments += " " + argumentPrefix + a.Key + argumentDelimitter + "\"" + a.Value + "\"";
                }
            }

            // Console.WriteLine(composedArguments);

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
            return true;
        }

        private Dictionary<string, string> GetConfiguration(KeyValuePair<string, string> app, Dictionary<string, string> dynamicArguments = null)
        {
            try
            {
                Dictionary<string, string> configuration = new();

                if (app.Value == autodarts.Value)
                {
                    configuration.Add(argumentPrefixKey, "-");
                    configuration.Add(argumentDelimiterKey, " ");
                    configuration.Add(specificFileKey, "");
                }
                else if (app.Value == autodartsCaller.Value)
                {
                    configuration.Add(argumentPrefixKey, "-");
                    configuration.Add(argumentDelimiterKey, " ");
                    configuration.Add(specificFileKey, "");

                    string autodartsUser = ValidateParseArgument(nameof(Settings.Default.emailautodarts), Settings.Default.emailautodarts, true);
                    string autodartsPassword = ValidateParseArgument(nameof(Settings.Default.pwautodarts), Settings.Default.pwautodarts, true);
                    string autodartsBoardId = ValidateParseArgument(nameof(Settings.Default.boardid), Settings.Default.boardid, true);
                    string autodartsSounds = ValidateParseArgument(nameof(Settings.Default.media), Settings.Default.media, true);
                    string autodartsCallerVol = ValidateParseArgument(nameof(Settings.Default.callervol), Settings.Default.callervol, false);
                    string autodartsRandomCaller = ValidateParseArgument(nameof(Settings.Default.randomcaller), Settings.Default.randomcaller, false);
                    string autodartsRandomCallerEachLeg = ValidateParseArgument(nameof(Settings.Default.randomcallereachleg), Settings.Default.randomcallereachleg, false);
                    string autodartsCallerPCC = ValidateParseArgument(nameof(Settings.Default.possiblecheckoutcall), Settings.Default.possiblecheckoutcall, false);
                    string autodartsCallerWtt = ValidateParseArgument(nameof(Settings.Default.callerwtt), Settings.Default.callerwtt, false);

                    configuration.Add("U", autodartsUser);
                    configuration.Add("P", autodartsPassword);
                    configuration.Add("B", autodartsBoardId);
                    configuration.Add("M", autodartsSounds);
                    configuration.Add("V", autodartsCallerVol);
                    configuration.Add("R", autodartsRandomCaller);
                    configuration.Add("L", autodartsRandomCallerEachLeg);
                    configuration.Add("PCC", autodartsCallerPCC);
                    configuration.Add("WTT", autodartsCallerWtt);
                }
                else if (app.Value == autodartsExtern.Value)
                {
                    configuration.Add(argumentPrefixKey, "--");
                    configuration.Add(argumentDelimiterKey, " ");
                    configuration.Add(specificFileKey, "");

                    // dynamic-arguments, like extern_platform
                    Dictionary<string, string> parsedValidatedDynamicArguments = new();
                    foreach (var dynamicArgument in dynamicArguments)
                    {
                        string dynamicArgumentParsed = ValidateParseArgument(nameof(dynamicArgument.Key), dynamicArgument.Value, true);
                        parsedValidatedDynamicArguments.Add(dynamicArgument.Key, dynamicArgumentParsed);
                    }

                    string externPlatform;
                    parsedValidatedDynamicArguments.TryGetValue("extern_platform", out externPlatform);

                    // normal arguments
                    string browserpath = ValidateParseArgument(nameof(Settings.Default.browserpath), Settings.Default.browserpath, true);
                    string port = ValidateParseArgument(nameof(Settings.Default.hostport), Settings.Default.hostport, true);
                    string autodartsUser = ValidateParseArgument(nameof(Settings.Default.emailautodarts), Settings.Default.emailautodarts, true);
                    string autodartsPassword = ValidateParseArgument(nameof(Settings.Default.pwautodarts), Settings.Default.pwautodarts, true);
                    string autodartsBoardId = ValidateParseArgument(nameof(Settings.Default.boardid), Settings.Default.boardid, true);

                    bool lidartsRequired = externPlatform == AutodartsExternPlatforms.lidarts.ToString();
                    string lidartsUser = ValidateParseArgument(nameof(Settings.Default.emaillidarts), Settings.Default.emaillidarts, lidartsRequired);
                    string lidartsPassword = ValidateParseArgument(nameof(Settings.Default.pwlidarts), Settings.Default.pwlidarts, lidartsRequired);
                    string skipdarts = ValidateParseArgument(nameof(Settings.Default.skipdarts), Settings.Default.skipdarts);
                    string timer = ValidateParseArgument(nameof(Settings.Default.timetoend), Settings.Default.timetoend);
                    string messagestart = ValidateParseArgument(nameof(Settings.Default.messagestart), Settings.Default.messagestart);
                    string messageend = ValidateParseArgument(nameof(Settings.Default.messageend), Settings.Default.messageend);

                    bool dartboardsRequired = externPlatform == AutodartsExternPlatforms.dartboards.ToString();
                    string dboUser = ValidateParseArgument(nameof(Settings.Default.dbouser), Settings.Default.dbouser, dartboardsRequired);
                    string dboPassword = ValidateParseArgument(nameof(Settings.Default.dbopw), Settings.Default.dbopw, dartboardsRequired);

                    configuration.Add("browser_path", browserpath);
                    configuration.Add("host_port", port);
                    configuration.Add("autodarts_user", autodartsUser);
                    configuration.Add("autodarts_password", autodartsPassword);
                    configuration.Add("autodarts_board_id", autodartsBoardId);
                    configuration.Add("extern_platform", externPlatform);
                    configuration.Add("time_before_exit", timer);
                    configuration.Add("lidarts_user", lidartsUser);
                    configuration.Add("lidarts_password", lidartsPassword);
                    configuration.Add("lidarts_skip_dart_modals", skipdarts);
                    configuration.Add("lidarts_chat_message_start", messagestart);
                    configuration.Add("lidarts_chat_message_end", messageend);
                    configuration.Add("nakka_skip_dart_modals", skipdarts);
                    configuration.Add("dartboards_user", dboUser);
                    configuration.Add("dartboards_password", dboPassword);
                    configuration.Add("dartboards_skip_dart_modals", skipdarts);
                }
                else if (app.Value == autodartsBot.Value)
                {
                    configuration.Add(argumentPrefixKey, "-");
                    configuration.Add(argumentDelimiterKey, " ");
                    configuration.Add(specificFileKey, "");
                }
                else if (app.Value == virtualDartsZoom.Value)
                {
                    configuration.Add(argumentPrefixKey, "-");
                    configuration.Add(argumentDelimiterKey, " ");
                    configuration.Add(specificFileKey, "");
                }
                else if (app.Value == dartboardsClient.Value)
                {
                    configuration.Add(argumentPrefixKey, "-");
                    configuration.Add(argumentDelimiterKey, " ");
                    configuration.Add(specificFileKey, "");
                }
                else
                {
                    throw new ArgumentException($"{app.Value} is not a valid app.");
                }
                return configuration;
            }
            catch (ArgumentException ae)
            {
                if (ae.Message.StartsWith(argumentErrorKey))
                {
                    string invalidArgumentErrorMessage = ae.Message.Substring(argumentErrorKey.Length, ae.Message.Length - argumentErrorKey.Length);
                    invalidArgumentErrorMessage += " is required. Please go to the app-settings and fill it.";
                    AppConfigurationRequiredEventArgs eventArgs = new AppConfigurationRequiredEventArgs(app.Value, invalidArgumentErrorMessage);
                    OnAppConfigurationRequired(eventArgs);
                    return null;
                }
                else
                {
                    throw;
                }
            }
            
        }

        private string ValidateParseArgument(string argumentName, string argumentValue, bool required = false)
        {
            string parsedArgumentValue = argumentValue;
            if (required && String.IsNullOrEmpty(parsedArgumentValue))
            {
                throw new ArgumentException(argumentErrorKey + argumentName);
            }
            return parsedArgumentValue;
        }

        private static string? FindExecutable(string appPath, string filename)
        {
            string pathToFile = Path.Join(appPath, filename);
            if (File.Exists(pathToFile))
            {
                return pathToFile;
            }
            else
            {
                return null;
            }
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

        private static bool CheckAppRunningState(string processName)
        {
            return Process.GetProcessesByName(processName).FirstOrDefault(p => p.ProcessName.ToLower().Contains(processName.ToLower())) != null;
        }

        private void CloseRunningApp(KeyValuePair<string, string> app, string specificFile = "")
        {
            string appPath = Path.Join(pathToApps, app.Value);

            // Find the executable and kill running process
            string executable = String.IsNullOrEmpty(specificFile) ? FindExecutable(appPath) : FindExecutable(appPath, specificFile);
            if (executable != null)
            {
                KillApp(Path.GetFileNameWithoutExtension(executable));
                KillApp(Path.GetFileNameWithoutExtension(executable));
            }
        }

        private static void KillApp(string processName)
        {
            try
            {
                var process = Process.GetProcessesByName(processName).FirstOrDefault(p => p.ProcessName.Contains(processName));
                if (process != null)
                {
                    process.Kill();
                    Thread.Sleep(175);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred trying to kill process:\n{ex.Message}");
                throw;
            }
        }

        private static string GetAppFileNameByUrl(string url)
        {
            string[] urlSplitted = url.Split("/");
            return urlSplitted[urlSplitted.Length - 1];
        }


        protected virtual void OnConfigurationChanged(EventArgs e)
        {
            if (ConfigurationChanged != null)
                ConfigurationChanged(this, e);
        }

        protected virtual void OnAppDownloadRequired(AppConfigurationRequiredEventArgs e)
        {
            if (AppDownloadRequired != null)
                AppDownloadRequired(this, e);
        }

        protected virtual void OnAppConfigurationRequired(AppConfigurationRequiredEventArgs e)
        {
            if (AppConfigurationRequired != null)
                AppConfigurationRequired(this, e);
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
