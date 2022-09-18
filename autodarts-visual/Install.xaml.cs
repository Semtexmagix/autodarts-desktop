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

namespace autodarts_visual
{
    /// <summary>
    /// Interaktionslogik für Install.xaml
    /// </summary>
    public partial class Install : Window
    {
        public Install()
        {
            InitializeComponent();
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
                Labeldbo.Visibility= Visibility.Collapsed;
            }

        }

        private void Buttonabbrechen_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /////////////////////////////// Downloader ///////////////////////////////

        private void downloadFile(string url)
        {
            string file = System.IO.Path.GetFileName(url);
            WebClient cln = new WebClient();
            cln.DownloadFile(url, file);
        }

        private void ButtonInstall_Click(object sender, RoutedEventArgs e)
        {






            /////////////////////////////// Autodarts.io Caller ///////////////////////////////

            if (Checkboxcallerinstall.IsChecked == true)
            {

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////// Install erledgit -> Ja
                Properties.Settings.Default.installdone = true;
                Properties.Settings.Default.Save();


                // Download Autodarts.io Caller

                string callerexe = @".\caller\autodarts-caller.exe";
                if (!System.IO.File.Exists(callerexe))
                {
                    downloadFile("https://github.com/lbormann/autodarts-caller/releases/download/v1.1.4/autodarts-caller.exe");
                }

                // Ordner erstellen
                string folderPathcaller = @".\caller";
                if (!Directory.Exists(folderPathcaller))
                {
                    Directory.CreateDirectory(folderPathcaller);
                    Console.WriteLine(folderPathcaller);
                }

                // Exe in Ordner verschieben
                string pathcaller = @".\autodarts-caller.exe";
                string path2caller = @".\caller\autodarts-caller.exe";
                try
                {
                    if (!System.IO.File.Exists(pathcaller))
                    {
                        // This statement ensures that the file is created,
                        // but the handle is not kept.
                        using (FileStream fs = System.IO.File.Create(pathcaller)) { }
                    }

                    // Ensure that the target does not exist.
                    if (System.IO.File.Exists(path2caller))
                        System.IO.File.Delete(path2caller);

                    // Move the file.
                    System.IO.File.Move(pathcaller, path2caller);
                    Console.WriteLine("{0} was moved to {1}.", pathcaller, path2caller);

                    // See if the original exists now.
                    if (System.IO.File.Exists(pathcaller))
                    {
                        Console.WriteLine("Es gab Probleme bei der Installation");
                    }
                    else
                    {
                        Console.WriteLine("Caller wurde runtergeladen");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("The process failed: {0}", e.ToString());
                }









                //MessageBox.Show("Software wurde installiert, nun müssen einige Daten eingegeben werden, dafür wirst Du in das Setup Menü weitergeleitet");
                //Setup S1 = new Setup();
                //S1.ShowDialog();
            }
            else
            {
                MessageBox.Show("Bitte Software zur Installation auswählen");
            }



            /////////////////////////////// Autodarts.io Extern ///////////////////////////////

            if (Checkboxexterninstall.IsChecked == true)
            {
                // Download Autodarts.io Extern

                string externexe = @".\extern\autodarts-extern.exe";
                if (!System.IO.File.Exists(externexe))
                {
                    downloadFile("https://github.com/lbormann/autodarts-extern/releases/download/v1.3.0/autodarts-extern.exe");
                }

                // Ordner erstellen
                string folderPathextern = @".\extern";
                if (!Directory.Exists(folderPathextern))
                {
                    Directory.CreateDirectory(folderPathextern);
                    Console.WriteLine(folderPathextern);
                }

                // Exe in Ordner verschieben
                string pathextern = @".\autodarts-extern.exe";
                string path2extern = @".\extern\autodarts-extern.exe";
                try
                {
                    if (!System.IO.File.Exists(pathextern))
                    {
                        // This statement ensures that the file is created,
                        // but the handle is not kept.
                        using (FileStream fs = System.IO.File.Create(pathextern)) { }
                    }

                    // Ensure that the target does not exist.
                    if (System.IO.File.Exists(path2extern))
                        System.IO.File.Delete(path2extern);

                    // Move the file.
                    System.IO.File.Move(pathextern, path2extern);
                    Console.WriteLine("{0} was moved to {1}.", pathextern, path2extern);

                    // See if the original exists now.
                    if (System.IO.File.Exists(pathextern))
                    {
                        Console.WriteLine("Es gab Probleme bei der Installation");
                    }
                    else
                    {
                        Console.WriteLine("Extern wurde runtergeladen");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("The process failed: {0}", e.ToString());
                }



            }







            /////////////////////////////// Autodarts.io Bot ///////////////////////////////

            if (Checkboxinstallbot.IsChecked == true)
            {
                // Download bot

                string botzip = @".\bot\autodartsbot-0.0.1.windows.x64.zip";
                if (!System.IO.File.Exists(botzip))
                {
                    downloadFile("https://github.com/xinixke/autodartsbot/releases/download/0.0.1/autodartsbot-0.0.1.windows.x64.zip");
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
                    Console.WriteLine("{0} was moved to {1}.", pathbot, path2bot);

                    // See if the original exists now.
                    if (System.IO.File.Exists(pathbot))
                    {
                        Console.WriteLine("Es gab Probleme bei der Installation");
                    }
                    else
                    {
                        Console.WriteLine("Bot wurde runtergeladen (Beim ersten Start muss der Bot in seinem Fenster konfiguriert werden");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("The process failed: {0}", e.ToString());
                }

            }


            /////////////////////////////// Virtual Darts Zoom ///////////////////////////////

            if (Checkboxinstallvdz.IsChecked == true)
            {


                // Download VDZ
                string vdzzip = @"./vdz/Virtual%20Darts%20Zoom.zip";
                if (!System.IO.File.Exists(vdzzip))
                {
                    downloadFile("https://www.lehmann-bo.de/Downloads/VDZ/Virtual%20Darts%20Zoom.zip");
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
                    Console.WriteLine("{0} was moved to {1}.", pathvdz, path2vdz);

                    // See if the original exists now.
                    if (System.IO.File.Exists(pathvdz))
                    {
                        Console.WriteLine("Es gab Probleme bei der Installation");
                    }
                    else
                    {
                        Console.WriteLine("Visual Darts Zoom wurde runtergeladen");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("The process failed: {0}", e.ToString());
                }

            }

            /////////////////////////////// DartsBoards.Online Client (benötigt für Webcams) ///////////////////////////////

            if (Checkboxinstalldbo.IsChecked == true)
            {

                // Download DBO
                string dboexe = @"./dbo/dboclient_0.8.6.exe";
                if (!System.IO.File.Exists(dboexe))
                {
                    downloadFile("https://dartboards.online/dboclient_0.8.6.exe");
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
                    Console.WriteLine("{0} was moved to {1}.", pathdbo, path2dbo);

                    // See if the original exists now.
                    if (System.IO.File.Exists(pathdbo))
                    {
                        Console.WriteLine("Es gab Probleme bei der Installation");
                    }
                    else
                    {
                        Console.WriteLine("DartsBoardOnline wurde runtergeladen");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("The process failed: {0}", e.ToString());
                }



                // Keep console window open in debug mode.
                //Console.WriteLine("Press any key to exit.");
                //Console.ReadKey();
            }
        }

        private void Buttonweiter_Click(object sender, RoutedEventArgs e)
        {
            Setup S1 = new Setup();
            S1.ShowDialog();
        }
    }
}