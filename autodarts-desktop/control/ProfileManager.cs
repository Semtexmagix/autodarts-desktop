using autodarts_desktop.model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using File = System.IO.File;
using Path = System.IO.Path;


namespace autodarts_desktop.control
{

    /// <summary>
    /// Manages everything around apps-lifecycle.
    /// </summary>
    public class ProfileManager
    {

        // ATTRIBUTES

        public const string appsDownloadableFile = "apps-downloadable.json";
        public const string appsInstallableFile = "apps-installable.json";
        public const string appsLocalFile = "apps-local.json";
        public const string appsOpenFile = "apps-open.json";
        public const string profilesFile = "profiles.json";

        public event EventHandler<AppEventArgs>? AppDownloadStarted;
        public event EventHandler<AppEventArgs>? AppDownloadFinished;
        public event EventHandler<AppEventArgs>? AppDownloadFailed;
        public event EventHandler<DownloadProgressChangedEventArgs>? AppDownloadProgressed;

        public event EventHandler<AppEventArgs>? AppInstallStarted;
        public event EventHandler<AppEventArgs>? AppInstallFinished;
        public event EventHandler<AppEventArgs>? AppInstallFailed;
        public event EventHandler<AppEventArgs>? AppConfigurationRequired;

        private List<AppBase> AppsAll;
        private List<AppDownloadable> AppsDownloadable;
        private List<AppInstallable> AppsInstallable;
        private List<AppLocal> AppsLocal;
        private List<AppOpen> AppsOpen;
        private List<Profile> Profiles;





        // METHODS

        public ProfileManager()
        {
           
        }



        public void LoadAppsAndProfiles()
        {
            AppsAll = new();
            AppsDownloadable = new();
            AppsInstallable = new();
            AppsLocal = new();
            AppsOpen = new();

            Profiles = new();

            if (File.Exists(appsDownloadableFile))
            {
                try
                {
                    var appsDownloadable = JsonConvert.DeserializeObject<List<AppDownloadable>>(File.ReadAllText(appsDownloadableFile));
                    AppsDownloadable.AddRange(appsDownloadable);
                    MigrateAppsDownloadable();
                    AppsAll.AddRange(appsDownloadable);
                }
                catch (Exception ex)
                {
                    throw new ConfigurationException(appsDownloadableFile, ex.Message);
                }
            }
            else
            {
                CreateDummyAppsDownloadable();
            }

            if (File.Exists(appsInstallableFile))
            {
                try
                {
                    var appsInstallable = JsonConvert.DeserializeObject<List<AppInstallable>>(File.ReadAllText(appsInstallableFile));
                    AppsInstallable.AddRange(appsInstallable);
                    MigrateAppsInstallable();
                    AppsAll.AddRange(appsInstallable);
                }
                catch (Exception ex)
                {
                    throw new ConfigurationException(appsInstallableFile, ex.Message);
                }
            }
            else
            {
                CreateDummyAppsInstallable();
            }


            if (File.Exists(appsLocalFile))
            {
                try
                {
                    var appsLocal = JsonConvert.DeserializeObject<List<AppLocal>>(File.ReadAllText(appsLocalFile));
                    AppsLocal.AddRange(appsLocal);
                    MigrateAppsLocal();
                    AppsAll.AddRange(appsLocal);
                }
                catch (Exception ex)
                {
                    throw new ConfigurationException(appsLocalFile, ex.Message);
                }
            }
            else
            {
                CreateDummyAppsLocal();
            }

            if (File.Exists(appsOpenFile))
            {
                try
                {
                    var appsOpen = JsonConvert.DeserializeObject<List<AppOpen>>(File.ReadAllText(appsOpenFile));
                    AppsOpen.AddRange(appsOpen);
                    MigrateAppsOpen();
                    AppsAll.AddRange(appsOpen);
                }
                catch (Exception ex)
                {
                    throw new ConfigurationException(appsOpenFile, ex.Message);
                }
            }
            else
            {
                CreateDummyAppsOpen();
            }


            if (File.Exists(profilesFile))
            {
                try
                {
                    Profiles = JsonConvert.DeserializeObject<List<Profile>>(File.ReadAllText(profilesFile));
                    MigrateProfiles();
                }
                catch (Exception ex)
                {
                    throw new ConfigurationException(profilesFile, ex.Message);
                }
            }
            else
            {
                CreateDummyProfiles();
            }


            foreach (var profile in Profiles)
            {
                foreach(KeyValuePair<string, ProfileState> profileLink in profile.Apps)
                {
                    var appFound = false;
                    foreach(var app in AppsAll)
                    {
                        if(app.Name == profileLink.Key)
                        {
                            appFound = true;
                            profileLink.Value.SetApp(app);
                            break;
                        }
                    }
                    if (!appFound) throw new Exception($"Profile-App '{profileLink.Key}' not found");
                }
            }


            foreach (var appDownloadable in AppsDownloadable)
            {
                appDownloadable.DownloadStarted += AppDownloadable_DownloadStarted;
                appDownloadable.DownloadFinished += AppDownloadable_DownloadFinished;
                appDownloadable.DownloadFailed += AppDownloadable_DownloadFailed;
                appDownloadable.DownloadProgressed += AppDownloadable_DownloadProgressed;
                appDownloadable.AppConfigurationRequired += App_AppConfigurationRequired;
            }
            foreach (var appInstallable in AppsInstallable)
            {
                appInstallable.DownloadStarted += AppDownloadable_DownloadStarted;
                appInstallable.DownloadFinished += AppDownloadable_DownloadFinished;
                appInstallable.DownloadFailed += AppDownloadable_DownloadFailed;
                appInstallable.DownloadProgressed += AppDownloadable_DownloadProgressed;
                appInstallable.InstallStarted += AppInstallable_InstallStarted;
                appInstallable.InstallFinished += AppInstallable_InstallFinished;
                appInstallable.InstallFailed += AppInstallable_InstallFailed;
                appInstallable.AppConfigurationRequired += App_AppConfigurationRequired;
            }
            foreach (var appLocal in AppsLocal)
            {
                appLocal.AppConfigurationRequired += App_AppConfigurationRequired;
            }
            foreach (var appOpen in AppsOpen)
            {
                appOpen.AppConfigurationRequired += App_AppConfigurationRequired;
            }
        }

        public void StoreApps()
        {
            SerializeApps(AppsDownloadable, appsDownloadableFile);
            SerializeApps(AppsInstallable, appsInstallableFile);
            SerializeApps(AppsLocal, appsLocalFile);
            SerializeApps(AppsOpen, appsOpenFile);
            SerializeProfiles(Profiles, profilesFile);
        }

        public void DeleteConfigurationFile(string configurationFile)
        {
            File.Delete(configurationFile);
        }

        public static bool RunProfile(Profile? profile)
        {
            if (profile == null) return false;

            var allAppsRunning = true;
            var appsTaggedForStart = profile.Apps.Where(x => x.Value.TaggedForStart);
            foreach (KeyValuePair<string, ProfileState> app in appsTaggedForStart)
            {
                // as here is no catch, apps-run stops when there is an error
                if (!app.Value.App.Run(app.Value.RuntimeArguments)) allAppsRunning = false;
            }
            return allAppsRunning;
        }

        public void CloseApps()
        {
            foreach (var app in AppsAll)
            {
                try
                {
                    app.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Closing failed for app: {app.Name} - {ex.Message}");
                }
            }
        }

        public List<Profile> GetProfiles()
        {
            return Profiles;
        }







        private void CreateDummyAppsLocal()
        {
            List<AppLocal> apps = new();

            AppLocal custom =
               new(
                   name: "custom",
                   descriptionShort: "Starts a program on your file-system"
                   );

            apps.Add(custom);

            AppsLocal.AddRange(apps);
            AppsAll.AddRange(apps);
            SerializeApps(apps, appsLocalFile);
        }

        private void MigrateAppsLocal()
        {

            // Add more migs..
        }

        private void CreateDummyAppsOpen()
        {
            List<AppOpen> apps = new();

            AppOpen autodartsWeb =
                new(
                    name: "autodarts.io",
                    descriptionShort: "Opens autodart`s web-platform",
                    defaultValue: "https://autodarts.io"
                    );

            apps.Add(autodartsWeb);

            AppsOpen.AddRange(apps);
            AppsAll.AddRange(apps);
            SerializeApps(apps, appsOpenFile);
        }

        private void MigrateAppsOpen()
        {

            // Add more migs..
        }

        private void CreateDummyAppsInstallable()
        {
            List<AppInstallable> apps = new();

            AppInstallable dartboardsClient =
                new(
                    downloadUrl: "https://dartboards.online/dboclient_0.8.6.exe",
                    name: "dartboards-client",
                    helpUrl: "https://dartboards.online/client",
                    descriptionShort: "webcam connection client for dartboards.online",
                    executable: "dartboardsonlineclient.exe",
                    defaultPathExecutable: Path.Join(Helper.GetUserDirectoryPath(), @"AppData\Local\Programs\dartboardsonlineclient"),
                    startsAfterInstallation: true
                    );

            AppInstallable droidCam =
                new(
                    downloadUrl: "https://github.com/dev47apps/windows-releases/releases/download/win-6.5.2/DroidCam.Setup.6.5.2.exe",
                    name: "droid-cam",
                    helpUrl: "https://www.dev47apps.com",
                    descriptionShort: "uses your android phone/tablet as local camera",
                    defaultPathExecutable: @"C:\Program Files (x86)\DroidCam",
                    executable: "DroidCamApp.exe",
                    runAsAdminInstall: true,
                    startsAfterInstallation: false
                    );

            AppInstallable epocCam =
                new(
                    downloadUrl: "https://edge.elgato.com/egc/windows/epoccam/EpocCam_Installer64_3_4_0.exe",
                    name: "epoc-cam",
                    helpUrl: "https://www.elgato.com/de/epoccam",
                    descriptionShort: "uses your iOS phone/tablet as local camera",
                    defaultPathExecutable: @"C:\Program Files (x86)\Elgato\EpocCam",
                    // epoccamtray.exe
                    executable: "EpocCamService.exe",
                    runAsAdminInstall: false,
                    startsAfterInstallation: false,
                    isService: true
                    );

            apps.Add(dartboardsClient);
            apps.Add(droidCam);
            apps.Add(epocCam);

            AppsInstallable.AddRange(apps);
            AppsAll.AddRange(apps);
            SerializeApps(apps, appsInstallableFile);
        }

        private void MigrateAppsInstallable()
        {

            // Add more migs..
        }

        private void CreateDummyAppsDownloadable()
        {
            List<AppDownloadable> apps = new();

            AppDownloadable autodarts =
                new(
                    downloadUrl: "https://github.com/autodarts/releases/releases/download/v0.17.0/autodarts0.17.0.windows-amd64.zip",
                    name: "autodarts-client",
                    helpUrl: "https://github.com/autodarts/docs",
                    descriptionShort: "Client for dart recognition with cameras"
                    );

            AppDownloadable autodartsCaller =
                new(
                    downloadUrl: "https://github.com/lbormann/autodarts-caller/releases/download/v1.3.3/autodarts-caller.exe",
                    name: "autodarts-caller",
                    helpUrl: "https://github.com/lbormann/autodarts-caller",
                    descriptionShort: "calls out thrown points",
                    configuration: new(
                        prefix: "-",
                        delimitter: " ",
                        arguments: new List<Argument> {
                            new(name: "U", type: "string", required: true, nameHuman: "autodarts-username", section: "Autodarts"),
                            new(name: "P", type: "password", required: true, nameHuman: "autodarts-password", section: "Autodarts"),
                            new(name: "B", type: "string", required: true, nameHuman: "autodarts-board-id", section: "Autodarts"),
                            new(name: "M", type: "path", required: true, nameHuman: "path-to-sound-files", section: "Media"),
                            new(name: "V", type: "float[0.0..1.0]", required: false, nameHuman: "caller-volume", section: "Media"),
                            new(name: "R", type: "bool", required: false, nameHuman: "random-caller", section: "Random", valueMapping: new Dictionary<string, string>{["True"] = "1",["False"] = "0"}),
                            new(name: "L", type: "bool", required: false, nameHuman: "random-caller-each-leg", section: "Random", valueMapping: new Dictionary<string, string>{["True"] = "1",["False"] = "0"}),
                            new(name: "E", type: "bool", required: false, nameHuman: "call-every-dart", section: "Calls", valueMapping: new Dictionary<string, string>{["True"] = "1",["False"] = "0"}),
                            new(name: "PCC", type: "bool", required: false, nameHuman: "call-possible-checkout", section: "Calls", valueMapping: new Dictionary<string, string>{["True"] = "1",["False"] = "0"}),
                            new(name: "WTT", type: "string", required: false, nameHuman: "webhook", section: "", value: "http://localhost:8080/throw"),
                        })
                    );

            AppDownloadable autodartsExtern =
                new(
                    downloadUrl: "https://github.com/lbormann/autodarts-extern/releases/download/v1.4.4/autodarts-extern.exe",
                    name: "autodarts-extern",
                    helpUrl: "https://github.com/lbormann/autodarts-extern",
                    descriptionShort: "automates dart web platforms with autodarts",
                    configuration: new(
                        prefix: "--",
                        delimitter: " ",
                        arguments: new List<Argument> {
                            new(name: "browser_path", type: "file", required: true, nameHuman: "Path to browser", section: "", description: "Path to browser. fav. Chrome"),
                            new(name: "host_port", type: "string", required: true, nameHuman: "Host-Port", section: "", value: "8080"),
                            new(name: "autodarts_user", type: "string", required: true, nameHuman: "Autodarts-Email", section: "Autodarts"),
                            new(name: "autodarts_password", type: "password", required: true, nameHuman: "Autodarts-Password", section: "Autodarts"),
                            new(name: "autodarts_board_id", type: "string", required: true, nameHuman: "Autodarts-Board-ID", section: "Autodarts"),
                            new(name: "extern_platform", type: "selection[lidarts,nakka,dartboards]", required: true, nameHuman: "", isRuntimeArgument: true),
                            new(name: "time_before_exit", type: "int[0..150000]", required: false, nameHuman: "Dwel after match end (in milliseconds)", section: "Match"),
                            new(name: "lidarts_user", type: "string", required: false, nameHuman: "Lidarts-Email", section: "Lidarts", requiredOnArgument: "extern_platform=lidarts"),
                            new(name: "lidarts_password", type: "password", required: false, nameHuman: "Lidarts-Password", section: "Lidarts", requiredOnArgument: "extern_platform=lidarts"),
                            new(name: "lidarts_skip_dart_modals", type: "bool", required: false, nameHuman: "Skip dart-modals", section: "Lidarts"),
                            new(name: "lidarts_chat_message_start", type: "string", required: false, nameHuman: "Chat-message on match-start", section: "Lidarts", value: "Hi, GD! Automated darts-scoring - powered by autodarts.io - Enter the community: https://discord.gg/bY5JYKbmvM"),
                            new(name: "lidarts_chat_message_end", type: "string", required: false, nameHuman: "Chat-message on match-end", section: "Lidarts", value: "Thanks GG, WP!"),
                            new(name: "nakka_skip_dart_modals", type: "bool", required: false, nameHuman: "Skip dart-modals", section: "Nakka"),
                            new(name: "dartboards_user", type: "string", required: false, nameHuman: "Dartboards-Email", section: "Dartboards", requiredOnArgument: "extern_platform=dartboards"),
                            new(name: "dartboards_password", type: "password", required: false, nameHuman: "Dartboards-Password", section: "Dartboards", requiredOnArgument: "extern_platform=dartboards"),
                            new(name: "dartboards_skip_dart_modals", type: "bool", required: false, nameHuman: "Skip dart-modals", section: "Dartboards"),
                        })
                );

            AppDownloadable autodartsBot =
                new(
                    downloadUrl: "https://github.com/xinixke/autodartsbot/releases/download/0.0.1/autodartsbot-0.0.1.windows.x64.zip",
                    name: "autodarts-bot",
                    helpUrl: "https://github.com/xinixke/autodartsbot",
                    descriptionShort: "Computer opponent"
                    );

            AppDownloadable virtualDartsZoom =
                new(
                    downloadUrl: "https://www.lehmann-bo.de/Downloads/VDZ/Virtual Darts Zoom.zip",
                    name: "virtual-darts-zoom",
                    helpUrl: "https://lehmann-bo.de/?p=28",
                    descriptionShort: "zooms webcam image onto the thrown darts",
                    runAsAdmin: true
                    );

            apps.Add(autodarts);
            apps.Add(autodartsCaller);
            apps.Add(autodartsExtern);
            apps.Add(autodartsBot);
            apps.Add(virtualDartsZoom);

            AppsDownloadable.AddRange(apps);
            AppsAll.AddRange(apps);
            
            SerializeApps(apps, appsDownloadableFile);
        }

        private void MigrateAppsDownloadable()
        {
            var autodartsCaller = AppsDownloadable.Single(a => a.Name == "autodarts-caller");
            if (autodartsCaller != null)
            {
                // 2. Mig (Add ValueMapping for bool)
                foreach (var arg in autodartsCaller.Configuration.Arguments)
                {
                    switch (arg.Name)
                    {
                        case "R":
                        case "L":
                        case "E":
                        case "PCC":
                            arg.ValueMapping = new Dictionary<string, string> { ["True"] = "1", ["False"] = "0" };
                            break;
                    }
                }

                // 3. Mig (Set default values)
                var wtt = autodartsCaller.Configuration.Arguments.Find(a => a.Name == "WTT");
                if (wtt != null && String.IsNullOrEmpty(wtt.Value)) wtt.Value = "http://localhost:8080/throw";

                // 5. Mig (Update download version)
                autodartsCaller.DownloadUrl = "https://github.com/lbormann/autodarts-caller/releases/download/v1.3.3/autodarts-caller.exe";

                // 6. Mig (Update download version)
                autodartsCaller.DownloadUrl = "https://github.com/lbormann/autodarts-caller/releases/download/v1.3.5/autodarts-caller.exe";

                // 7. Mig (Update download version)
                autodartsCaller.DownloadUrl = "https://github.com/lbormann/autodarts-caller/releases/download/v1.3.6/autodarts-caller.exe";

                // 8. Mig (Update download version)
                autodartsCaller.DownloadUrl = "https://github.com/lbormann/autodarts-caller/releases/download/v1.3.7/autodarts-caller.exe";

                // 10. Mig (Update download version)
                autodartsCaller.DownloadUrl = "https://github.com/lbormann/autodarts-caller/releases/download/v1.3.8/autodarts-caller.exe";

                // 11. Mig (Update download version)
                autodartsCaller.DownloadUrl = "https://github.com/lbormann/autodarts-caller/releases/download/v1.4.0/autodarts-caller.exe";

                // 12. Mig (Update download version)
                autodartsCaller.DownloadUrl = "https://github.com/lbormann/autodarts-caller/releases/download/v1.5.0/autodarts-caller.exe";

                // 13. Mig (Update download version)
                autodartsCaller.DownloadUrl = "https://github.com/lbormann/autodarts-caller/releases/download/v1.5.1/autodarts-caller.exe";
            }

            var autodartsExtern = AppsDownloadable.Single(a => a.Name == "autodarts-extern");
            if (autodartsExtern != null)
            {
                // 1. Mig (Update download version)
                autodartsExtern.DownloadUrl = "https://github.com/lbormann/autodarts-extern/releases/download/v1.4.4/autodarts-extern.exe";

                // 4. Mig (Set default values)
                var hostPort = autodartsExtern.Configuration.Arguments.Find(a => a.Name == "host_port");
                if (hostPort != null && String.IsNullOrEmpty(hostPort.Value))
                {
                    hostPort.Value = "8080";
                }

                var lidartsChatMessageStart = autodartsExtern.Configuration.Arguments.Find(a => a.Name == "lidarts_chat_message_start");
                if (lidartsChatMessageStart != null && String.IsNullOrEmpty(lidartsChatMessageStart.Value))
                {
                    lidartsChatMessageStart.Value = "Hi, GD! Automated darts-scoring - powered by autodarts.io - Enter the community: https://discord.gg/bY5JYKbmvM";
                }

                var lidartsChatMessageEnd = autodartsExtern.Configuration.Arguments.Find(a => a.Name == "lidarts_chat_message_end");
                if (lidartsChatMessageEnd != null && String.IsNullOrEmpty(lidartsChatMessageEnd.Value))
                {
                    lidartsChatMessageEnd.Value = "Thanks GG, WP!";
                }

                // 14. Mig (Update download version)
                autodartsCaller.DownloadUrl = "https://github.com/lbormann/autodarts-extern/releases/download/v1.4.5/autodarts-extern.exe";
            }


            var autodartsBotIndex = AppsDownloadable.FindIndex(a => a.Name == "autodarts-bot");
            // 9. Mig (Remove app)
            if (autodartsBotIndex != -1)
            {
                AppsDownloadable.RemoveAt(autodartsBotIndex);
            }




            // Add more migs..
        }

        private void CreateDummyProfiles()
        {
            var p1Name = "autodarts-caller";
            var p1Apps = new Dictionary<string, ProfileState> {
                { "autodarts-client", new() },
                { "autodarts.io", new() },
                { "autodarts-bot", new() },
                { "autodarts-caller", new (isRequired: true) },
                { "custom", new() },
            };

            var p2Name = "autodarts-extern: lidarts.org";
            var p2Args = new Dictionary<string, string> { { "extern_platform", "lidarts" } };
            var p2Apps = new Dictionary<string, ProfileState> {
                { "autodarts-client", new() },
                { "autodarts.io", new() },
                { "autodarts-caller", new(isRequired: true) },
                { "autodarts-extern", new(isRequired: true, runtimeArguments: p2Args) },
                { "virtual-darts-zoom", new() },
                { "droid-cam", new() },
                { "epoc-cam", new () },
                { "custom", new() },
            };

            var p3Name = "autodarts-extern: nakka.com/n01/online";
            var p3Args = new Dictionary<string, string> { { "extern_platform", "nakka" } };
            var p3Apps = new Dictionary<string, ProfileState> {
                { "autodarts-client", new() },
                { "autodarts.io", new () },
                { "autodarts-caller", new (isRequired: true) },
                { "autodarts-extern", new (isRequired: true, runtimeArguments: p3Args) },
                { "virtual-darts-zoom", new() },
                { "droid-cam", new() },
                { "epoc-cam", new () },
                { "custom", new() },
            };

            var p4Name = "autodarts-extern: dartboards.online";
            var p4Args = new Dictionary<string, string> { { "extern_platform", "dartboards" } };
            var p4Apps = new Dictionary<string, ProfileState> {
                { "autodarts-client", new() },
                { "autodarts.io", new () },
                { "autodarts-caller", new (isRequired: true) },
                { "autodarts-extern", new (isRequired: true, runtimeArguments: p4Args) },
                { "virtual-darts-zoom", new() },
                { "dartboards-client", new() },
                { "droid-cam", new() },
                { "epoc-cam", new () },
                { "custom", new() },
            };

            Profiles.Add(new Profile(p1Name, p1Apps));
            Profiles.Add(new Profile(p2Name, p2Apps));
            Profiles.Add(new Profile(p3Name, p3Apps));
            Profiles.Add(new Profile(p4Name, p4Apps));

            SerializeProfiles(Profiles, profilesFile);
        }

        private void MigrateProfiles()
        {
            // 9. Mig (Remove autodarts-bot)
            foreach (var p in Profiles)
            {
                p.Apps.Remove("autodarts-bot");
            }

            // Add more migs..
        }



        private void SerializeApps<AppBase>(List<AppBase> apps, string filename)
        {
            var settings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore
            };
            var appsJsonStr = JsonConvert.SerializeObject(apps, Formatting.Indented, settings);
            File.WriteAllText(filename, appsJsonStr);
        }
        
        private void SerializeProfiles(List<Profile> profiles, string filename)
        {
            var settings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore
            };
            var profilesJsonStr = JsonConvert.SerializeObject(profiles, Formatting.Indented, settings);
            File.WriteAllText(filename, profilesJsonStr);
        }




 
        private void AppDownloadable_DownloadStarted(object? sender, AppEventArgs e)
        {
            OnAppDownloadStarted(e);
        }

        private void AppDownloadable_DownloadFinished(object? sender, AppEventArgs e)
        {
            OnAppDownloadFinished(e);
        }

        private void AppDownloadable_DownloadFailed(object? sender, AppEventArgs e)
        {
            OnAppDownloadFailed(e);
        }

        private void AppDownloadable_DownloadProgressed(object? sender, DownloadProgressChangedEventArgs e)
        {
            OnAppDownloadProgressed(e);
        }



        private void AppInstallable_InstallStarted(object? sender, AppEventArgs e)
        {
            OnAppInstallStarted(e);
        }

        private void AppInstallable_InstallFinished(object? sender, AppEventArgs e)
        {
            OnAppInstallFinished(e);
        }

        private void AppInstallable_InstallFailed(object? sender, AppEventArgs e)
        {
            OnAppInstallFailed(e);
        }

        private void App_AppConfigurationRequired(object? sender, AppEventArgs e)
        {
            OnAppConfigurationRequired(e);
        }



        protected virtual void OnAppDownloadStarted(AppEventArgs e)
        {
            AppDownloadStarted?.Invoke(this, e);
        }

        protected virtual void OnAppDownloadFinished(AppEventArgs e)
        {
            AppDownloadFinished?.Invoke(this, e);
        }

        protected virtual void OnAppDownloadFailed(AppEventArgs e)
        {
            AppDownloadFailed?.Invoke(this, e);
        }

        protected virtual void OnAppDownloadProgressed(DownloadProgressChangedEventArgs e)
        {
            AppDownloadProgressed?.Invoke(this, e);
        }



        protected virtual void OnAppInstallStarted(AppEventArgs e)
        {
            AppInstallStarted?.Invoke(this, e);
        }

        protected virtual void OnAppInstallFinished(AppEventArgs e)
        {
            AppInstallFinished?.Invoke(this, e);
        }

        protected virtual void OnAppInstallFailed(AppEventArgs e)
        {
            AppInstallFailed?.Invoke(this, e);
        }

        protected virtual void OnAppConfigurationRequired(AppEventArgs e)
        {
            AppConfigurationRequired?.Invoke(this, e);
        }
        

    }
}
