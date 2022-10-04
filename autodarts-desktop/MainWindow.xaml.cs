using autodarts_desktop.Properties;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;


namespace autodarts_desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppManager appManager;
        private Dictionary<string, bool> appsInstallState;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                appManager = new AppManager();
                appManager.DownloadAppStarted += AppManager_DownloadAppStarted;
                appManager.DownloadAppFinished += AppManager_DownloadAppFinished;
                appManager.DownloadAppFailed += AppManager_DownloadAppFailed;
                appManager.DownloadAppProgressed += AppManager_DownloadAppProgressed;
                appManager.ConfigurationChanged += AppManager_ConfigurationChanged;
                appManager.AppConfigurationRequired += AppManager_AppConfigurationRequired;
                appManager.AppDownloadRequired += AppManager_AppDownloadRequired;
                appManager.NewReleaseFound += AppManager_NewReleaseFound;
                appManager.NewReleaseReady += AppManager_NewReleaseReady;
                appManager.CheckNewVersion();
                appManager.CheckDefaultRequirements();
                UpdateAppsInstallState();

                string[] args = Environment.GetCommandLineArgs();
                if (args.Length > 1 && args[1] == "-U") appManager.UpdateInstalledApps();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong: " + ex.Message);
                Close();
            }
        }




        private void Buttonstart_Click(object sender, RoutedEventArgs e)
        {
            var selectedTag = ((ComboBoxItem)Comboboxportal.SelectedItem).Tag.ToString();
            Settings.Default.selectedProfile = Comboboxportal.SelectedIndex;
            Settings.Default.Save();

            try
            {
                if (!appManager.RunAutodartsCaller()) return;

                switch (selectedTag)
                {
                    case "caller":
                        appManager.RunAutodartsPortal();
                        break;
                    case "lidarts":
                        if (!appManager.RunAutodartsExtern(AutodartsExternPlatforms.lidarts)) return;
                        break;
                    case "nakka":
                        if (!appManager.RunAutodartsExtern(AutodartsExternPlatforms.nakka)) return;
                        break;
                    case "dartboards":
                        if (!appManager.RunAutodartsExtern(AutodartsExternPlatforms.dartboards)) return;
                        break;
                }

                if (Checkboxbot.IsChecked == true)
                {
                    if (!appManager.RunAutodartsBot()) return;
                }
                if (Checkboxvdz.IsChecked == true)
                {
                    if (!appManager.RunVirtualDartsZoom()) return;
                }
                if (Checkboxdboc.IsChecked == true)
                {
                    if (!appManager.RunDartboardsClient()) return;
                }
                if (Checkboxcustom.IsChecked == true)
                {
                    AppManager.RunCustomApp();
                }

                WindowState = WindowState.Minimized;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
    
        }

        private void Buttonabout_Click(object sender, RoutedEventArgs e)
        {
            About S1 = new About();
            S1.ShowDialog();
        }

        private void Buttoninstall_Click(object sender, RoutedEventArgs e)
        {
            Install I1 = new Install(appManager);
            I1.ShowDialog();
        }

        private void Comboboxportal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Comboboxportal.SelectedIndex == -1) return;

            UpdateCustomAppState();

            bool appState;
            appsInstallState.TryGetValue(appManager.autodarts.Value, out appState);
            Checkboxad.IsEnabled = appState;
            Checkboxad.IsChecked = appState ? Settings.Default.ad_start_default : false;


            var selectedTag = ((ComboBoxItem)Comboboxportal.SelectedItem).Tag.ToString();
            switch (selectedTag)
            {
                case "caller":
                    appsInstallState.TryGetValue(appManager.autodartsBot.Value, out appState);
                    Checkboxbot.IsEnabled = appState;
                    Checkboxbot.IsChecked = appState ? Settings.Default.bot_start_default : false;

                    Checkboxvdz.IsEnabled = false;
                    Checkboxvdz.IsChecked = false;

                    Checkboxdboc.IsEnabled = false;
                    Checkboxdboc.IsChecked = false;
                    break;
                case "lidarts":
                    appsInstallState.TryGetValue(appManager.virtualDartsZoom.Value, out appState);
                    Checkboxvdz.IsEnabled = appState;
                    Checkboxvdz.IsChecked = appState ? Settings.Default.vdz_start_default : false;

                    Checkboxbot.IsEnabled = false;
                    Checkboxbot.IsChecked = false;

                    Checkboxdboc.IsEnabled = false;
                    Checkboxdboc.IsChecked = false;
                    break;
                case "nakka":
                    appsInstallState.TryGetValue(appManager.virtualDartsZoom.Value, out appState);
                    Checkboxvdz.IsEnabled = appState;
                    Checkboxvdz.IsChecked = appState ? Settings.Default.vdz_start_default : false;

                    Checkboxbot.IsEnabled = false;
                    Checkboxbot.IsChecked = false;

                    Checkboxdboc.IsEnabled = false;
                    Checkboxdboc.IsChecked = false;

                    break;
                case "dartboards":
                    appsInstallState.TryGetValue(appManager.dartboardsClient.Value, out appState);
                    Checkboxdboc.IsEnabled = appState;
                    Checkboxdboc.IsChecked = appState ? Settings.Default.dboc_start_default : false;

                    appsInstallState.TryGetValue(appManager.virtualDartsZoom.Value, out appState);
                    Checkboxvdz.IsEnabled = appState;
                    Checkboxvdz.IsChecked = appState ? Settings.Default.vdz_start_default : false;

                    Checkboxbot.IsEnabled = false;
                    Checkboxbot.IsChecked = false;
                    break;
            }
        }

        private void CheckboxStartApp_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkboxStartApp = sender as CheckBox;

            switch (checkboxStartApp.Name)
            {
                case "Checkboxad":
                    Settings.Default.ad_start_default = (bool)Checkboxad.IsChecked;
                    break;
                case "Checkboxbot":
                    Settings.Default.bot_start_default = (bool)Checkboxbot.IsChecked;
                    break;
                case "Checkboxvdz":
                    Settings.Default.vdz_start_default = (bool)Checkboxvdz.IsChecked;
                    break;
                case "Checkboxdboc":
                    Settings.Default.dboc_start_default = (bool)Checkboxdboc.IsChecked;
                    break;
                case "Checkboxcustom":
                    Settings.Default.custom_start_default = (bool)Checkboxcustom.IsChecked;
                    break;
            }
            Settings.Default.Save();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                appManager.CloseRunningApps();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured: " + ex.Message);
            }
        }



        private void AppManager_DownloadAppStarted(object? sender, AppEventArgs e)
        {
            SetGUIForDownload(true, "Downloading " + e.App + "..");
        }

        private void AppManager_DownloadAppFinished(object? sender, AppEventArgs e)
        {
            UpdateAppsInstallState();
            SetGUIForDownload(false);
        }

        private void AppManager_DownloadAppFailed(object? sender, AppEventArgs e)
        {
            if(e.App == AppManager.releaseAppKey)
            {
                Hide();
                MessageBox.Show("Checking for new release failed! Please check your internet-connection and try again. " + e.Message);
                Close();
                return;
            }
            SetGUIForDownload(false, "Download failed! Please check your internet-connection and try again. " + e.Message);
        }

        private void AppManager_DownloadAppProgressed(object? sender, DownloadProgressChangedEventArgs e)
        {
            SetGUIForDownload(true);
        }

        private void AppManager_AppDownloadRequired(object? sender, AppEventArgs e)
        {
            SetGUIForDownload(true, "Downloading " + e.App + "..");
        }

        private void AppManager_AppConfigurationRequired(object? sender, AppEventArgs e)
        {
            MessageBox.Show(e.Message);
            if (e.App == appManager.autodarts.Value)
            {
                
            }
            else if(e.App == appManager.autodartsBot.Value)
            {
            }
            else if (e.App == appManager.autodartsCaller.Value)
            {
                SetupCaller SC1 = new SetupCaller();
                SC1.ShowDialog();
            }
            else if (e.App == appManager.autodartsExtern.Value)
            {
                SetupExtern SE1 = new SetupExtern();
                SE1.ShowDialog();
            }
            else if (e.App == appManager.virtualDartsZoom.Value)
            {
            }
            else if (e.App == appManager.dartboardsClient.Value)
            {
            }
        }

        private void AppManager_NewReleaseFound(object? sender, AppEventArgs e)
        {
            if (MessageBox.Show($"New Version '{e.Message}' available! Do you want to update?", "New Version", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    appManager.UpdateToNewVersion();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Update to new version failed: " + ex.Message);
                }

            }
        }

        private void AppManager_NewReleaseReady(object? sender, AppEventArgs e)
        {
            Close();
        }

        private void AppManager_ConfigurationChanged(object? sender, EventArgs e)
        {
            UpdateCustomAppState();
        }

        private void UpdateAppsInstallState()
        {
            appsInstallState = appManager.GetAppsInstallState();

            Comboboxportal.Items.Clear();
            foreach (var appInstallState in appsInstallState)
            {
                if(appInstallState.Value == true)
                {
                    if (appInstallState.Key == appManager.autodartsCaller.Value)
                    {
                        AddComboBoxItem("autodarts.io + autodarts-caller", "caller");
                    }
                    else if (appInstallState.Key == appManager.autodartsExtern.Value)
                    {
                        AddComboBoxItem("autodarts-extern: lidarts.org", "lidarts");
                        AddComboBoxItem("autodarts-extern: nakka.com/n01/online", "nakka");
                        AddComboBoxItem("autodarts-extern: dartboards.online", "dartboards");
                    }
                }
            }
            if(Settings.Default.selectedProfile > -1 && Settings.Default.selectedProfile < Comboboxportal.Items.Count)
            {
                Comboboxportal.SelectedIndex = Settings.Default.selectedProfile;
            }
            else
            {
                Comboboxportal.SelectedIndex = 0;
            }
        }

        private void UpdateCustomAppState()
        {
            if (String.IsNullOrEmpty(Settings.Default.customapp))
            {
                Checkboxcustom.IsEnabled = false;
                Checkboxcustom.IsChecked = false;
            }
            else
            {
                Checkboxcustom.IsEnabled = true;
                Checkboxcustom.IsChecked = Settings.Default.custom_start_default;
            }
        }

        private void AddComboBoxItem(string content, String value)
        {
            var appItem = new ComboBoxItem();
            appItem.Content = content;
            appItem.Tag = value;
            Comboboxportal.Items.Add(appItem);
        }

        private void SetGUIForDownload(bool downloading, string waitingText = "")
        {
            string downloadMessage = String.IsNullOrEmpty(waitingText) ? WaitingText.Content.ToString() : waitingText;

            if (downloading)
            {
                GridMain.IsEnabled = false;
                Waiting.Visibility = Visibility.Visible;
                WaitingText.Content = downloadMessage;
                WaitingText.Visibility = Visibility.Visible;
            }
            else
            {
                GridMain.IsEnabled = true;
                Waiting.Visibility = Visibility.Hidden;
                WaitingText.Content = downloadMessage;
                WaitingText.Visibility = String.IsNullOrEmpty(waitingText) ? Visibility.Hidden : Visibility.Visible;
            }
        }

    }
}
