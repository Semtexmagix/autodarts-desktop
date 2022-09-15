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
                //Properties.Settings.Default.boxcaller = false;
            }
        }
        private void Checkboxexterninstall_Click(object sender, RoutedEventArgs e)
        {


            if (Checkboxexterninstall.IsChecked == true)
            {
                Checkboxinstallvdz.Visibility = Visibility.Visible;
                Labelvdz.Visibility = Visibility.Visible;
            }
            else
            {
                Checkboxinstallvdz.IsChecked = false;
                Checkboxinstallvdz.Visibility = Visibility.Collapsed;
                Labelvdz.Visibility = Visibility.Collapsed;

            }

        }

        private void Buttonabbrechen_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonInstall_Click(object sender, RoutedEventArgs e)
        {



            /////////////////////////////// Virtual Darts Zoom
            ///
   
            // Download vdz.zip
            string vdz = @"./vdz.zip";
            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;

            p.StartInfo = info;
            p.Start();

            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    string vdzordner = @"./vdz/";
                    if (!Directory.Exists(vdzordner))
                    {
                        sw.WriteLine("mkdir vdz");
                    }

                    if (!System.IO.File.Exists(vdz))
                    {
                        sw.WriteLine("curl https://www.lehmann-bo.de/Downloads/VDZ/Virtual%20Darts%20Zoom.zip --output vdz.zip");
                    }

                    Console.WriteLine("Sleep for 20 seconds.");
                    Thread.Sleep(20000);

                    if (!Directory.Exists(vdzordner))
                    {
                        // Entpacken VDZ
                        string zipPath = @".\vdz.zip";
                        string extractPath = @".\vdz";

                        ZipFile.ExtractToDirectory(zipPath, extractPath);  
                    }
                }
            }

            








            // Download bot
            string bot = @"./bot.zip";
            Process p1 = new Process();
            ProcessStartInfo info1 = new ProcessStartInfo();
            info1.FileName = "cmd.exe";
            info1.RedirectStandardInput = true;
            info1.RedirectStandardOutput = true;
            info1.UseShellExecute = false;

            p1.StartInfo = info1;
            p1.Start();

            using (StreamWriter sw = p1.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    string botordner = @"./bot/";
                    if (!Directory.Exists(botordner))
                    {
                        sw.WriteLine("mkdir bot");
                    }
                    if (!System.IO.File.Exists(bot))
                    {
                        sw.WriteLine("curl https://github.com/xinixke/autodartsbot/releases/download/0.0.1/autodartsbot-0.0.1.windows.x64.zip --output bot.zip");
                    }

                    Console.WriteLine("Sleep for 20 seconds.");
                    Thread.Sleep(20000);


                    if (!Directory.Exists(botordner))
                    {
                        // Entpacken bot
                        string zipPath = @".\bot.zip";
                        string extractPath = @".\bot";

                        ZipFile.ExtractToDirectory(zipPath, extractPath);
                    }
                }
            }


        }
    }
}
