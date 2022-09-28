using autodarts_desktop.Properties;
using System;
using System.Collections.Generic;
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
            appManager = new AppManager();
            appManager.ConfigurationChanged += AppManager_ConfigurationChanged;
            appManager.AppConfigurationRequired += AppManager_AppConfigurationRequired;
            appManager.AppDownloadRequired += AppManager_AppDownloadRequired;
            appManager.DownloadAppProgressed += AppManager_DownloadAppProgressed;
            appManager.DownloadAppStopped += AppManager_DownloadAppStopped;
            appManager.CheckDefaultRequirements();
            UpdateAppsInstallState();
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


        private void AppManager_ConfigurationChanged(object? sender, EventArgs e)
        {
            UpdateCustomAppState();
        }

        private void AppManager_DownloadAppProgressed(object? sender, EventArgs e)
        {
            GridMain.IsEnabled = false;
            Waiting.Visibility = Visibility.Visible;
        }

        private void AppManager_DownloadAppStopped(object? sender, EventArgs e)
        {
            UpdateAppsInstallState();
            GridMain.IsEnabled = true;
            Waiting.Visibility = Visibility.Hidden;
        }

        private void AppManager_AppDownloadRequired(object? sender, AppConfigurationRequiredEventArgs e)
        {
            GridMain.IsEnabled = false;
            Waiting.Visibility = Visibility.Visible;
            //MessageBox.Show(e.Message + " - downloading.. please wait.");
        }

        private void AppManager_AppConfigurationRequired(object? sender, AppConfigurationRequiredEventArgs e)
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
            else if(Settings.Default.selectedProfile > -1)
            {
                Comboboxportal.SelectedIndex = 0;
            }
        }

        private void UpdateCustomAppState()
        {
            if (String.IsNullOrEmpty(Settings.Default.obs))
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




    }
}
