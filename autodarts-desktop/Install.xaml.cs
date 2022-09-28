using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CheckBox = System.Windows.Controls.CheckBox;
using Exception = System.Exception;

namespace autodarts_desktop
{
    /// <summary>
    /// Interaktionslogik für Install.xaml
    /// </summary>
    public partial class Install : Window
    {
        private AppManager appManager;


        public Install(AppManager appManager)
        {
            InitializeComponent();
            this.appManager = appManager;
            this.appManager.DownloadAppStarted += AppManager_DownloadAppStarted;
            this.appManager.DownloadAppProgressed += AppManager_DownloadAppProgressed;
            this.appManager.DownloadAppStopped += AppManager_DownloadAppStopped;
            SetInstallStateApps();
        }


        private void Checkbox_Click(object sender, RoutedEventArgs e)
        {
            var list = GridMain.Children.OfType<CheckBox>().Where(x => x.IsChecked == true);
            ButtonInstall.IsEnabled = list.Any() ? true : false;
        }

        private void Checkboxexterninstall_Checked(object sender, RoutedEventArgs e)
        {
            if (autodartsCallerInstallState.IsEnabled)
            {
                Checkboxcallerinstall.IsChecked = false;
            }
            else
            {
                Checkboxcallerinstall.IsChecked = true;
            }
            if (virtualDartsZoomInstallState.IsEnabled)
            {
                Checkboxinstallvdz.IsChecked = false;
            }
            else
            {
                Checkboxinstallvdz.IsChecked = true;
            }
            if (dartboardsClientInstallState.IsEnabled)
            {
                Checkboxinstalldbo.IsChecked = false;
            }
            else
            {
                Checkboxinstalldbo.IsChecked = true;
            }
        }

        private void Checkboxexterninstall_Unchecked(object sender, RoutedEventArgs e)
        {
            Checkboxcallerinstall.IsChecked = false;
            Checkboxinstallvdz.IsChecked = false;
            Checkboxinstalldbo.IsChecked = false;
        }


        private void ButtonInstall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Checkboxcallerinstall.IsChecked == true)
                {
                    appManager.DownloadAutodartsCaller();
                }
                if (Checkboxautodartsinstall.IsChecked == true)
                {
                    appManager.DownloadAutodarts();
                }
                if (Checkboxexterninstall.IsChecked == true)
                {
                    appManager.DownloadAutodartsExtern();
                }
                if (Checkboxinstallbot.IsChecked == true)
                {
                    appManager.DownloadAutodartsBot();
                }
                if (Checkboxinstallvdz.IsChecked == true)
                {
                    appManager.DownloadVirtualDartsZoom();
                }
                if (Checkboxinstalldbo.IsChecked == true)
                {
                    appManager.DownloadDartboardsClient();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            
        }

        private void ButtonHelp_Click(object sender, RoutedEventArgs e)
        {
            Button helpButton = sender as Button;
            switch (helpButton.Name)
            {
                case "autodartsHelp":
                    VisitHelpPage("https://github.com/autodarts/docs");
                    break;
                case "autodartsBotHelp":
                    VisitHelpPage("https://github.com/xinixke/autodartsbot");
                    break;
                case "autodartsCallerHelp":
                    VisitHelpPage("https://github.com/lbormann/autodarts-caller");
                    break;
                case "autodartsExternHelp":
                    VisitHelpPage("https://github.com/lbormann/autodarts-extern");
                    break;
                case "virtualDartsZoomHelp":
                    VisitHelpPage("https://lehmann-bo.de/?p=28");
                    break;
                case "dartboardsClientHelp":
                    VisitHelpPage("https://dartboards.online/client");
                    break;

            }
        }

        private void Buttonsetupcustom_Click(object sender, RoutedEventArgs e)
        {
            SetupCustomApp SC1 = new SetupCustomApp(appManager);
            SC1.ShowDialog();
        }

        private void Buttonsetupextern_Click(object sender, RoutedEventArgs e)
        {
            SetupExtern SE1 = new SetupExtern();
            SE1.ShowDialog();
        }

        private void Buttonsetupcaller_Click(object sender, RoutedEventArgs e)
        {
            SetupCaller SC1 = new SetupCaller();
            SC1.ShowDialog();
        }


        private void VisitHelpPage(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo(url)
                {
                    UseShellExecute = true
                });
            } catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void SetInstallStateApps()
        {
            foreach (var appInstallState in appManager.GetAppsInstallState())
            {
                string appStateText = appInstallState.Value == true ? "✓" : "x";
                Brush appStateColor = appInstallState.Value == true ? Brushes.Green : Brushes.Red;
                Visibility appStateVisibility = appInstallState.Value == true ? Visibility.Visible : Visibility.Hidden;

                if(appInstallState.Key == appManager.autodarts.Value)
                {
                    autodartsInstallState.IsEnabled = appInstallState.Value;
                    autodartsInstallState.Content = appStateText;
                    autodartsInstallState.Foreground = appStateColor;
                }
                else if(appInstallState.Key == appManager.autodartsCaller.Value)
                {
                    autodartsCallerInstallState.IsEnabled = appInstallState.Value;
                    autodartsCallerInstallState.Content = appStateText;
                    autodartsCallerInstallState.Foreground = appStateColor;
                    Buttonsetupcaller.Visibility = appStateVisibility;
                }
                else if (appInstallState.Key == appManager.autodartsExtern.Value)
                {
                    autodartsExternInstallState.IsEnabled = appInstallState.Value;
                    autodartsExternInstallState.Content = appStateText;
                    autodartsExternInstallState.Foreground = appStateColor;
                    Buttonsetupextern.Visibility = appStateVisibility;
                }
                else if (appInstallState.Key == appManager.autodartsBot.Value)
                {
                    autodartsBotInstallState.IsEnabled = appInstallState.Value;
                    autodartsBotInstallState.Content = appStateText;
                    autodartsBotInstallState.Foreground = appStateColor;
                }
                else if (appInstallState.Key == appManager.virtualDartsZoom.Value)
                {
                    virtualDartsZoomInstallState.IsEnabled = appInstallState.Value;
                    virtualDartsZoomInstallState.Content = appStateText;
                    virtualDartsZoomInstallState.Foreground = appStateColor;
                }
                else if (appInstallState.Key == appManager.dartboardsClient.Value)
                {
                    dartboardsClientInstallState.IsEnabled = appInstallState.Value;
                    dartboardsClientInstallState.Content = appStateText;
                    dartboardsClientInstallState.Foreground = appStateColor;
                }
            }
        }

        private void SetGUIForDownload(bool downloading)
        {
            if (downloading)
            {
                GridMain.IsEnabled = false;
                progressDownloads.Visibility = Visibility.Visible;
            }
            else
            {
                GridMain.IsEnabled = true;
                progressDownloads.Visibility = Visibility.Hidden;
                progressDownloads.Value = 0;
            }
        }

        private void AppManager_DownloadAppStarted(object? sender, EventArgs e)
        {
            SetGUIForDownload(true);
        }

        private void AppManager_DownloadAppProgressed(object? sender, EventArgs e)
        {
            DownloadProgressChangedEventArgs dpce = (DownloadProgressChangedEventArgs)e;
            SetGUIForDownload(true);
            progressDownloads.Value = dpce.ProgressPercentage;
        }

        private void AppManager_DownloadAppStopped(object? sender, EventArgs e)
        {
            SetGUIForDownload(false);
            SetInstallStateApps();
            foreach (CheckBox checkbox in GridMain.Children.OfType<CheckBox>())
            {
                checkbox.IsChecked = false;
            }
            ButtonInstall.IsEnabled = false;
        }


    }
}