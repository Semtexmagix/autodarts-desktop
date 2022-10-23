using autodarts_desktop.control;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace autodarts_desktop.model
{
    /// <summary>
    /// Main functions for using an app
    /// </summary>
    public abstract class AppBase : IApp
    {

        // ATTRIBUTES

        public string Name { get; private set; }
        public string? HelpUrl { get; private set; }
        public string? DescriptionShort { get; private set; }
        public string? DescriptionLong { get; private set; }
        public bool RunAsAdmin { get; private set; }
        public ProcessWindowStyle StartWindowState { get; private set; }
        public Configuration? Configuration { get; protected set; }

        public event EventHandler<AppEventArgs>? AppConfigurationRequired;

        [JsonIgnore]
        public Argument? ArgumentRequired { get; private set; }

        private int processId;
        private string? executable;
        private const int defaultProcessId = 0;
        private readonly ProcessWindowStyle DefaultStartWindowState = ProcessWindowStyle.Minimized;
        protected Dictionary<string, string>? runtimeArguments;





        // METHODS


        public AppBase(string name,
                        string? helpUrl,
                        string? descriptionShort,
                        string? descriptionLong,
                        bool runAsAdmin,
                        ProcessWindowStyle? startWindowState,
                        Configuration? configuration = null
            )
        {
            Name = name;
            HelpUrl = helpUrl;
            DescriptionShort = descriptionShort;
            DescriptionLong = descriptionLong;
            RunAsAdmin = runAsAdmin;
            StartWindowState = (ProcessWindowStyle)(startWindowState == null ? DefaultStartWindowState : startWindowState);
            Configuration = configuration;

            processId = defaultProcessId;
        }



        public bool Run(Dictionary<string, string>? runtimeArguments = null)
        {
            this.runtimeArguments = runtimeArguments;
            executable = SetRunExecutable();
            if (IsRunning()) return true;
            if (Install(false)) return false;
            return RunProcess(runtimeArguments);
        }

        public bool IsInstalled()
        {
            executable = SetRunExecutable();
            return executable != null;
        }

        public bool IsRunning()
        {
            if (processId != defaultProcessId && Helper.IsProcessRunning(processId)) return true;
            return Helper.IsProcessRunning(executable);
        }

        public bool Install(bool removeExistingInstallation)
        {
            if (removeExistingInstallation)
            {
                Close();
            }
            else
            {
                if (IsInstalled()) return false; 
            }
            DoInstallProcess(removeExistingInstallation);
            return true;
        }

        public void Close()
        {
            if(!IsRunning()) return;
            if (IsRunnable())
            {
                if (processId != defaultProcessId)
                {
                    try
                    {
                        Helper.KillProcess(processId);
                    }
                    catch
                    {
                    }
                }
                if (executable != null)
                {
                    Helper.KillProcess(executable);
                }
            }
        }

        public abstract bool IsConfigurable();

        public abstract bool IsInstallable();



        private bool RunProcess(Dictionary<string, string>? runtimeArguments = null)
        {
            if (!IsRunnable()) return true;

            if (String.IsNullOrEmpty(executable)) return false;
            var arguments = ComposeArguments(this, runtimeArguments);
            if (String.IsNullOrEmpty(arguments)) return false;

            // Console.WriteLine($"Start App '{executable}' with Arguments '{arguments}'");

            using var process = new Process();
            try
            {
                bool isUri = Uri.TryCreate(executable, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                if(!isUri) process.StartInfo.WorkingDirectory = Path.GetDirectoryName(executable);
                process.StartInfo.FileName = executable;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.RedirectStandardOutput = false;
                process.StartInfo.RedirectStandardError = false;
                process.StartInfo.UseShellExecute = true;
                if (RunAsAdmin) process.StartInfo.Verb = "runas";
                process.StartInfo.WindowStyle = StartWindowState;
                process.Start();
                processId = process.Id;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred trying to start \"{executable}\" with \"{arguments}\":\n{ex.Message}");
                throw;
            }
        }

        protected virtual bool IsRunnable()
        {
            return true;
        }

        protected abstract string? SetRunExecutable();

        protected abstract void DoInstallProcess(bool isReinstall);

        protected string? ComposeArguments(AppBase app, Dictionary<string, string>? runtimeArguments = null)
        {
            try
            {
                var ret = IsConfigurable() ? Configuration.GenerateArgumentString(app, runtimeArguments) : "";
                ArgumentRequired = null;
                return ret;
            }
            catch (Exception ex)
            { 
                if (ex.Message.StartsWith(Configuration.ArgumentErrorKey))
                {
                    string invalidArgumentErrorMessage = ex.Message.Substring(Configuration.ArgumentErrorKey.Length, ex.Message.Length - Configuration.ArgumentErrorKey.Length);
                    ArgumentRequired = (Argument)ex.Data["argument"];
                    invalidArgumentErrorMessage += " is required. Please fill it out.";
                    OnAppConfigurationRequired(new AppEventArgs(this, invalidArgumentErrorMessage));
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }
   
        protected virtual void OnAppConfigurationRequired(AppEventArgs e)
        {
            AppConfigurationRequired?.Invoke(this, e);
        }

    }
}
