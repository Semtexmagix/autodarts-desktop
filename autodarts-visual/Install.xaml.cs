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
        }


        private void SetGUIForDownload(bool downloading)
        {
            if (downloading)
            {
                GridMain.IsEnabled = false;
                Progressbarcaller.Visibility = Visibility.Visible;
            }
            else
            {
                GridMain.IsEnabled = true;
                Progressbarcaller.Visibility = Visibility.Hidden;
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
            Progressbarcaller.Value = dpce.ProgressPercentage;
        }

        private void AppManager_DownloadAppStopped(object? sender, EventArgs e)
        {
            SetGUIForDownload(false);
        }



        private void ButtonInstall_Click(object sender, RoutedEventArgs e)
        {

            /////////////////////////////// Autodarts.io Caller ///////////////////////////////

            if (Checkboxcallerinstall.IsChecked == true)
            {
                try
                {
                    appManager.DownloadAutodartsCaller();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Installieren von 'Caller': " + ex.Message);
                }
            }


            /////////////////////////////// Autodarts.io Main ///////////////////////////////

            if (Checkboxautodartsinstall.IsChecked == true)
            {
                try
                {
                    appManager.DownloadAutodarts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Installieren von 'Autodarts': " + ex.Message);
                }
            }

            /////////////////////////////// Autodarts.io Extern ///////////////////////////////

            if (Checkboxexterninstall.IsChecked == true)
            {
                try
                {
                    appManager.DownloadAutodartsExtern();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Installieren von 'Extern': " + ex.Message);
                }
            }

            /////////////////////////////// Autodarts.io Bot ///////////////////////////////

            if (Checkboxinstallbot.IsChecked == true)
            {
                try
                {
                    appManager.DownloadAutodartsBot();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Installieren von 'Bot': " + ex.Message);
                } 
            }

            /////////////////////////////// Virtual Darts Zoom ///////////////////////////////

            if (Checkboxinstallvdz.IsChecked == true)
            {
                try
                {
                    appManager.DownloadVirtualDartsZoom();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Installieren von 'Virtual Darts Zoom': " + ex.Message);
                }
            }

            /////////////////////////////// DartsBoards.Online Client (benötigt für Webcams) ///////////////////////////////

            if (Checkboxinstalldbo.IsChecked == true)
            {
                try
                {
                    appManager.DownloadDartboardsClient();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Installieren von 'Dartboards-client': " + ex.Message);
                }

            }
        }

        // Click="Checkboxautodartsinstall_Click" Checked="Checkboxautodartsinstall_Checked" Unchecked="Checkboxautodartsinstall_Unchecked"


        private void Checkboxcallerinstall_Click(object sender, RoutedEventArgs e)
        {
            if (Checkboxcallerinstall.IsChecked == true)
            {
                Checkboxexterninstall.Visibility = Visibility.Visible;
                Labelextern.Visibility = Visibility.Visible;
                Checkboxinstallbot.Visibility = Visibility.Visible;
                Labelbot.Visibility = Visibility.Visible;
            }
            else
            {
                Checkboxexterninstall.IsChecked = false;
                Checkboxexterninstall.Visibility = Visibility.Collapsed;
                Labelextern.Visibility = Visibility.Collapsed;
                Checkboxinstallbot.IsChecked = false;
                Checkboxinstallbot.Visibility = Visibility.Collapsed;
                Labelbot.Visibility = Visibility.Collapsed;
                Checkboxinstallvdz.IsChecked = false;
                Checkboxinstallvdz.Visibility = Visibility.Collapsed;
                Labelvdz.Visibility = Visibility.Collapsed;
                Checkboxinstalldbo.IsChecked = false;
                Checkboxinstalldbo.Visibility = Visibility.Collapsed;
                Labeldbo.Visibility = Visibility.Collapsed;
                //Properties.Settings.Default.boxcaller = false;
            }
        }

        private void Checkboxexterninstall_Click(object sender, RoutedEventArgs e)
        {


            if (Checkboxexterninstall.IsChecked == true)
            {
                Checkboxinstallvdz.Visibility = Visibility.Visible;
                Labelvdz.Visibility = Visibility.Visible;
                Checkboxinstalldbo.Visibility = Visibility.Visible;
                Labeldbo.Visibility = Visibility.Visible;
            }
            else
            {
                Checkboxinstallvdz.IsChecked = false;
                Checkboxinstalldbo.IsChecked = false;
                Checkboxinstallvdz.Visibility = Visibility.Collapsed;
                Checkboxinstalldbo.Visibility = Visibility.Collapsed;
                Labelvdz.Visibility = Visibility.Collapsed;
                Labeldbo.Visibility = Visibility.Collapsed;
            }

        }

        private void Buttonabbrechen_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Buttonweiter_Click(object sender, RoutedEventArgs e)
        {
            Setup S1 = new Setup();
            S1.ShowDialog();
        }

        private void Checkboxcallerinstall_Checked(object sender, RoutedEventArgs e)
        {
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////// Install Button anzeigen!
            ButtonInstall.Visibility = Visibility.Visible;
        }
        
        private void Checkboxcallerinstall_Unchecked(object sender, RoutedEventArgs e)
        {
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////// Install Button verstecken!
            ButtonInstall.Visibility = Visibility.Collapsed;
        }


    }
}