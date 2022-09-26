using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace autodarts_desktop
{
    /// <summary>
    /// Interaktionslogik für Setup.xaml
    /// </summary>
    public partial class Setup : Window
    {
        public Setup()
        {
            InitializeComponent();


            //----------------------------------------------------------------------------------------------
            //------------------->>>>> Laden der Settings als Input für die Textboxen
            //----------------------------------------------------------------------------------------------


            // Settings Autodarts.io
            TextBoxEmailAutodarts.Text = Properties.Settings.Default.emailautodarts;
            TextBoxPWAutodarts.Password = Properties.Settings.Default.pwautodarts;
            
            // Settings Lidarts.org
            TextBoxEmailLidarts.Text = Properties.Settings.Default.emaillidarts;
            TextBoxPWLidarts.Password = Properties.Settings.Default.pwlidarts;

            // Settings Dartboards.online
            TextBoxEmaildbo.Text = Properties.Settings.Default.dbouser;
            TextBoxPWdbo.Password = Properties.Settings.Default.dbopw;

            // Settings Caller
            TextBoxBoardID.Text = Properties.Settings.Default.boardid;
            TextBoxmedia.Text = Properties.Settings.Default.media;
            TextBoxcallervol.Text = Properties.Settings.Default.callervol;
            Checkboxrandomcaller.IsChecked = Properties.Settings.Default.checkboxrandomcaller;
            Checkboxrandomcallereachleg.IsChecked = Properties.Settings.Default.checkboxrandomcallereachleg;
            slValue.Value = Properties.Settings.Default.sliderpos;
            TextBoxCallerWtt.Text = Properties.Settings.Default.callerwtt;

            // Settings Extern
            TextBoxmessagestart.Text = Properties.Settings.Default.messagestart;
            TextBoxmessageend.Text = Properties.Settings.Default.messageend;
            Checkboxskipdart.IsChecked = Properties.Settings.Default.checkboxskipdart;
            TextBoxtime.Text = Properties.Settings.Default.timetoend;
            TextBoxbrowser.Text = Properties.Settings.Default.browserpath;
            TextBoxExternHostPort.Text = Properties.Settings.Default.hostport;

            // Settings Zusatz Programme Lidarts (OBS)
            TextBoxobs.Text = Properties.Settings.Default.obs;
            TextBoxCustomAppArgs.Text = Properties.Settings.Default.customappargs;
        }

        private void Buttonspeichern_Click(object sender, RoutedEventArgs e)
        {
            //----------------------------------------------------------------------------------------------
            //------------------->>>>> Speichern der Textboxen als Settings 
            //----------------------------------------------------------------------------------------------

            // Settings Autodarts.io
            Properties.Settings.Default.emailautodarts = TextBoxEmailAutodarts.Text;
            Properties.Settings.Default.pwautodarts = TextBoxPWAutodarts.Password;

            // Settings Lidarts.org
            Properties.Settings.Default.emaillidarts = TextBoxEmailLidarts.Text;
            Properties.Settings.Default.pwlidarts = TextBoxPWLidarts.Password;

            // Settings Dartboards.online
            Properties.Settings.Default.dbouser = TextBoxEmaildbo.Text;
            Properties.Settings.Default.dbopw = TextBoxPWdbo.Password;

            // Settings Caller
            Properties.Settings.Default.boardid = TextBoxBoardID.Text;
            Properties.Settings.Default.media = TextBoxmedia.Text;
            Properties.Settings.Default.callervol = TextBoxcallervol.Text;
            if (Checkboxrandomcaller.IsChecked == true)
            {
                Properties.Settings.Default.randomcaller = "1";
                Properties.Settings.Default.checkboxrandomcaller = true;           
            }
            else
            {
                Properties.Settings.Default.randomcaller = "0";
                Properties.Settings.Default.checkboxrandomcaller = false;
            }

            if (Checkboxrandomcallereachleg.IsChecked == true)
            {
                Properties.Settings.Default.randomcallereachleg = "1";
                Properties.Settings.Default.checkboxrandomcallereachleg = true;
            }
            else
            {
                Properties.Settings.Default.randomcallereachleg = "0";
                Properties.Settings.Default.checkboxrandomcallereachleg = false;
            }
            if(Checkboxpossiblecheckoutcall.IsChecked == true)
            {
                Properties.Settings.Default.possiblecheckoutcall = "1";
            }
            else
            {
                Properties.Settings.Default.possiblecheckoutcall = "0";
            }
            Properties.Settings.Default.sliderpos = slValue.Value;
            Properties.Settings.Default.callerwtt = TextBoxCallerWtt.Text;



            // Settings Extern
            Properties.Settings.Default.messagestart = TextBoxmessagestart.Text;
            Properties.Settings.Default.messageend = TextBoxmessageend.Text;
            //Properties.Settings.Default.skipdarts = TextBoxskipdarts.Text;
            if (Checkboxskipdart.IsChecked == true)
            {
                Properties.Settings.Default.skipdarts = "true";
                Properties.Settings.Default.checkboxskipdart = true;
            }
            else
            {
                Properties.Settings.Default.skipdarts = "false";
                Properties.Settings.Default.checkboxskipdart = false;
            }
            Properties.Settings.Default.timetoend = TextBoxtime.Text;
            Properties.Settings.Default.browserpath = TextBoxbrowser.Text;
            Properties.Settings.Default.hostport = TextBoxExternHostPort.Text;


            // Settings Zusatz Programme Lidarts (OBS)
            Properties.Settings.Default.obs = TextBoxobs.Text;
            Properties.Settings.Default.customappargs = TextBoxCustomAppArgs.Text;

            // Setup erledigt -> Ja
            Properties.Settings.Default.setupdone = true;

            // Settings Speichern
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void Buttonobs_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select File";
            openFileDialog.InitialDirectory = @"C:\";//--"C:\\";
            openFileDialog.Filter = "All files (*.*)|*.*|Anwendung (*.exe)|*.exe";
            openFileDialog.FilterIndex = 2;
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")
            { TextBoxobs.Text = openFileDialog.FileName; }
            else
            { TextBoxobs.Text = "Bitte Datei auswählen"; }
        }

        private void Buttonbrowser_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select File";
            openFileDialog.InitialDirectory = @"C:\";//--"C:\\";
            openFileDialog.Filter = "All files (*.*)|*.*|Anwendung (*.exe)|*.exe";
            openFileDialog.FilterIndex = 2;
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")
            { TextBoxbrowser.Text = openFileDialog.FileName; }
            else
            { TextBoxbrowser.Text = "Bitte Datei auswählen"; }
        }

        private void Buttonmedia_Click(object sender, RoutedEventArgs e)
        {
            var ookiiDialog = new VistaFolderBrowserDialog();
            if (ookiiDialog.ShowDialog() == true)
            {
                TextBoxmedia.Text = ookiiDialog.SelectedPath;
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            String slidValAsStr = Math.Round(slValue.Value, 2).ToString().Replace(",", ".");
            TextBoxcallervol.Text = slidValAsStr;
        }
    }
}
