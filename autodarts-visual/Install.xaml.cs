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
                // Install Autodarts.io Caller
                try
                {
                    appManager.InstallAutodartsCaller();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Installieren von 'Caller': " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Bitte Software zur Installation auswählen");
            }



            /////////////////////////////// Autodarts.io Extern ///////////////////////////////

            if (Checkboxexterninstall.IsChecked == true)
            {
                // Install Autodarts.io Extern
                try
                {
                    appManager.InstallAutodartsExtern();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Installieren von 'Extern': " + ex.Message);
                }
            }


            /////////////////////////////// Autodarts.io Bot ///////////////////////////////

            if (Checkboxinstallbot.IsChecked == true)
            {
                // Install bot

                // TODO: Aufruf wie bei caller/ extern, interne Hilfsmethoden im AppManager ggf. anpassen, um zip-Downloads zu unterstützen

                string botzip = @".\bot\autodartsbot-0.0.1.windows.x64.zip";
                if (!System.IO.File.Exists(botzip))
                {
                    //downloadFile("https://github.com/xinixke/autodartsbot/releases/download/0.0.1/autodartsbot-0.0.1.windows.x64.zip");
                }

                // Ordner erstellen
                string folderPathbot = @".\bot";
                if (!Directory.Exists(folderPathbot))
                {
                    Directory.CreateDirectory(folderPathbot);
                    Console.WriteLine(folderPathbot);
                }

                // Entpacken bot
                string bot = @".\bot\autodartsbot-0.0.1.windows.x64.zip";
                if (!System.IO.File.Exists(bot))
                {
                    string zipPathbot = @".\autodartsbot-0.0.1.windows.x64.zip";
                    string extractPathbot = @".\bot";
                    ZipFile.ExtractToDirectory(zipPathbot, extractPathbot);
                }

                // Zip in Ordner verschieben
                string pathbot = @".\autodartsbot-0.0.1.windows.x64.zip";
                string path2bot = @".\bot\autodartsbot-0.0.1.windows.x64.zip";
                try
                {
                    if (!System.IO.File.Exists(pathbot))
                    {
                        // This statement ensures that the file is created,
                        // but the handle is not kept.
                        using (FileStream fs = System.IO.File.Create(pathbot)) { }
                    }

                    // Ensure that the target does not exist.
                    if (System.IO.File.Exists(path2bot))
                        System.IO.File.Delete(path2bot);

                    // Move the file.
                    System.IO.File.Move(pathbot, path2bot);
                    Console.WriteLine("{0} wurde verschoben nach {1}.", pathbot, path2bot);

                    // See if the original exists now.
                    if (System.IO.File.Exists(pathbot))
                    {
                        Console.WriteLine("Es gab Probleme bei der Installation");
                    }
                    else
                    {
                        Console.WriteLine("Bot wurde installiert (Beim ersten Start muss der Bot in seinem Fenster konfiguriert werden");
                    }
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Installertion fehlgeschlagen!!!: {0}", e.ToString());
                }

            }


            /////////////////////////////// Virtual Darts Zoom ///////////////////////////////

            if (Checkboxinstallvdz.IsChecked == true)
            {


                // Download VDZ
                string vdzzip = @"./vdz/Virtual%20Darts%20Zoom.zip";
                if (!System.IO.File.Exists(vdzzip))
                {
                    //downloadFile("https://www.lehmann-bo.de/Downloads/VDZ/Virtual%20Darts%20Zoom.zip");
                }

                // Ordner erstellen
                string folderPathvdz = @".\vdz";
                if (!Directory.Exists(folderPathvdz))
                {
                    Directory.CreateDirectory(folderPathvdz);
                    Console.WriteLine(folderPathvdz);
                }

                // Entpacken vdz
                string vdz = @"./vdz/Virtual%20Darts%20Zoom.zip";
                if (!System.IO.File.Exists(vdz))
                {
                    string zipPathvdz = @".\Virtual%20Darts%20Zoom.zip";
                    string extractPathvdz = @".\vdz";
                    ZipFile.ExtractToDirectory(zipPathvdz, extractPathvdz);
                }

                // Zip in Ordner verschieben
                string pathvdz = @".\Virtual%20Darts%20Zoom.zip";
                string path2vdz = @".\vdz\Virtual%20Darts%20Zoom.zip";
                try
                {
                    if (!System.IO.File.Exists(pathvdz))
                    {
                        // This statement ensures that the file is created,
                        // but the handle is not kept.
                        using (FileStream fs = System.IO.File.Create(pathvdz)) { }
                    }

                    // Ensure that the target does not exist.
                    if (System.IO.File.Exists(path2vdz))
                        System.IO.File.Delete(path2vdz);

                    // Move the file.
                    System.IO.File.Move(pathvdz, path2vdz);
                    Console.WriteLine("{0} wurde verschoben nach {1}.", pathvdz, path2vdz);

                    // See if the original exists now.
                    if (System.IO.File.Exists(pathvdz))
                    {
                        Console.WriteLine("Es gab Probleme bei der Installation");
                    }
                    else
                    {
                        Console.WriteLine("Visual Darts Zoom wurde installiert");
                    }
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Installertion fehlgeschlagen!!!: {0}", e.ToString());
                }

            }

            /////////////////////////////// DartsBoards.Online Client (benötigt für Webcams) ///////////////////////////////

            if (Checkboxinstalldbo.IsChecked == true)
            {

                // Download DBO
                string dboexe = @"./dbo/dboclient_0.8.6.exe";
                if (!System.IO.File.Exists(dboexe))
                {
                    //downloadFile("https://dartboards.online/dboclient_0.8.6.exe");
                }

                // Ordner erstellen
                string folderPathdbo = @".\dbo";
                if (!Directory.Exists(folderPathdbo))
                {
                    Directory.CreateDirectory(folderPathdbo);
                    Console.WriteLine(folderPathdbo);
                }


                // Exe in Ordner verschieben
                string pathdbo = @".\dboclient_0.8.6.exe";
                string path2dbo = @".\dbo\dboclient_0.8.6.exe";
                try
                {
                    if (!System.IO.File.Exists(pathdbo))
                    {
                        // This statement ensures that the file is created,
                        // but the handle is not kept.
                        using (FileStream fs = System.IO.File.Create(pathdbo)) { }
                    }

                    // Ensure that the target does not exist.
                    if (System.IO.File.Exists(path2dbo))
                        System.IO.File.Delete(path2dbo);

                    // Move the file.
                    System.IO.File.Move(pathdbo, path2dbo);
                    Console.WriteLine("{0} wurde verschoben nach {1}.", pathdbo, path2dbo);

                    // See if the original exists now.
                    if (System.IO.File.Exists(pathdbo))
                    {
                        Console.WriteLine("Es gab Probleme bei der Installation");
                    }
                    else
                    {
                        Console.WriteLine("DartsBoardOnline Client (Webcam support) wurde installiert");
                    }
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Installertion fehlgeschlagen!!!: {0}", e.ToString());
                }
            }
        }

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