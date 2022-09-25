using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.WebRequestMethods;
using System.Xml.Linq;
using System.IO.Compression;
using System.Reflection;
using System.Threading;
using System.Security.Policy;
using System.Net.Http;
using Windows.Media.Protection.PlayReady;
using System.Net.Mime;
using ABI.System;
using System.ComponentModel;
using Exception = System.Exception;
using File = System.IO.File;
using Path = System.IO.Path;
using CheckBox = System.Windows.Controls.CheckBox;
using Control = System.Windows.Controls.Control;

namespace autodarts_visual
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
            if (list.Any())
            {
                ButtonInstall.IsEnabled = true;
            }
            else
            {
                ButtonInstall.IsEnabled = false;
            }
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

        private void Buttonweiter_Click(object sender, RoutedEventArgs e)
        {
            Setup S1 = new Setup();
            S1.ShowDialog();
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


        private void VisitHelpPage(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo(url)
                {
                    UseShellExecute = true
                });
            }catch(Exception ex)
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

                switch (appInstallState.Key)
                {
                    case "autodarts":
                        autodartsInstallState.Content = appStateText;
                        autodartsInstallState.IsEnabled = appInstallState.Value;
                        autodartsInstallState.Foreground = appStateColor;
                        break;
                    case "autodarts-caller":
                        autodartsCallerInstallState.IsEnabled = appInstallState.Value;
                        autodartsCallerInstallState.Content = appStateText;
                        autodartsCallerInstallState.Foreground = appStateColor;
                        break;
                    case "autodarts-extern":
                        autodartsExternInstallState.IsEnabled = appInstallState.Value;
                        autodartsExternInstallState.Content = appStateText;
                        autodartsExternInstallState.Foreground = appStateColor;
                        break;
                    case "autodarts-bot":
                        autodartsBotInstallState.IsEnabled = appInstallState.Value;
                        autodartsBotInstallState.Content = appStateText;
                        autodartsBotInstallState.Foreground = appStateColor;
                        break;
                    case "virtual-darts-zoom":
                        virtualDartsZoomInstallState.IsEnabled = appInstallState.Value;
                        virtualDartsZoomInstallState.Content = appStateText;
                        virtualDartsZoomInstallState.Foreground = appStateColor;
                        break;
                    case "dartboards-client":
                        dartboardsClientInstallState.IsEnabled = appInstallState.Value;
                        dartboardsClientInstallState.Content = appStateText;
                        dartboardsClientInstallState.Foreground = appStateColor;
                        break;
                    default:
                        break;
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