using Microsoft.Win32;
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

namespace autodarts_visual
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

            // Settings Webcamdarts.com
            //textBoxemailWebcamdarts.Text = Properties.Settings.Default.emailwebcamdarts;
            //textBoxPWWebcamdarts.Text = Properties.Settings.Default.pwwebcamdarts;

            // Settings Caller
            TextBoxBoardID.Text = Properties.Settings.Default.boardid;
            TextBoxmedia.Text = Properties.Settings.Default.media;
            //TextBoxcallervol.Text = Properties.Settings.Default.callervol;
            TextBoxrandomcaller.Text = Properties.Settings.Default.randomcaller;
            TextBoxrcel.Text = Properties.Settings.Default.randomcallereachleg;
            TextBoxwebhook.Text = Properties.Settings.Default.webhook;

            // Settings Extern
            TextBoxmessagestart.Text = Properties.Settings.Default.messagestart;
            TextBoxmessageend.Text = Properties.Settings.Default.messageend;
            TextBoxskipdarts.Text = Properties.Settings.Default.skipdarts;
            TextBoxtime.Text = Properties.Settings.Default.timetoend;
            TextBoxbrowser.Text = Properties.Settings.Default.browserpath;

            // Settings Zusatz Programme Lidarts (OBS,VDZ)
            TextBoxvdz.Text = Properties.Settings.Default.vdz;
            TextBoxobs.Text = Properties.Settings.Default.obs;

            // Settings Zusatz Programme Autodarts (Bot)
            TextBoxbot.Text = Properties.Settings.Default.bot;

            // Settings Zusatz Programme DartsBoard.Online (Client, wird für Webcam benötigt)
            TextBoxdbo.Text = Properties.Settings.Default.dbo;
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

            // Settings Webcamdarts.com
            //Properties.Settings.Default.emailwebcamdarts = textBoxemailWebcamdarts.Text;
            //Properties.Settings.Default.pwwebcamdarts = textBoxPWWebcamdarts.Text;

            // Settings Caller
            Properties.Settings.Default.boardid = TextBoxBoardID.Text;
            Properties.Settings.Default.media = TextBoxmedia.Text;
            Properties.Settings.Default.callervol = TextBoxcallervol.Text;  
            Properties.Settings.Default.randomcaller = TextBoxrandomcaller.Text;
            Properties.Settings.Default.randomcallereachleg = TextBoxrcel.Text;
            Properties.Settings.Default.webhook = TextBoxwebhook.Text;

            // Settings Extern
            Properties.Settings.Default.messagestart = TextBoxmessagestart.Text;
            Properties.Settings.Default.messageend = TextBoxmessageend.Text;
            Properties.Settings.Default.skipdarts = TextBoxskipdarts.Text;
            Properties.Settings.Default.timetoend = TextBoxtime.Text;
            Properties.Settings.Default.browserpath = TextBoxbrowser.Text;


            // Settings Zusatz Programme Lidarts (OBS,VDZ)
            Properties.Settings.Default.obs = TextBoxobs.Text;
            Properties.Settings.Default.vdz = TextBoxvdz.Text;

            // Settings Zusatz Programme Autodarts (Bot)
            Properties.Settings.Default.bot = TextBoxbot.Text;

            // Settings Zusatz Programme DartsBoard.Online (Client, wird für Webcam benötigt)
            Properties.Settings.Default.dbo = TextBoxdbo.Text;

            // Setup erledigt -> Ja
            Properties.Settings.Default.setupdone = true;

            // Settings Speichern
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void Buttonabbrechen_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Buttonvdz_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select File";
            openFileDialog.InitialDirectory = @"C:\";//--"C:\\";
            openFileDialog.Filter = "All files (*.*)|*.*|Anwendung (*.exe)|*.exe";
            openFileDialog.FilterIndex = 2;
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")
            { TextBoxvdz.Text = openFileDialog.FileName; }
            else
            { TextBoxvdz.Text = "Bitte Datei auswählen"; }
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

        private void Buttonbot_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select File";
            openFileDialog.InitialDirectory = @"C:\";//--"C:\\";
            openFileDialog.Filter = "All files (*.*)|*.*|Anwendung (*.exe)|*.exe";
            openFileDialog.FilterIndex = 2;
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")
            { TextBoxbot.Text = openFileDialog.FileName; }
            else
            { TextBoxbot.Text = "Bitte Datei auswählen"; }
        }

        private void Buttondbo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select File";
            openFileDialog.InitialDirectory = @"C:\";//--"C:\\";
            openFileDialog.Filter = "All files (*.*)|*.*|Anwendung (*.exe)|*.exe";
            openFileDialog.FilterIndex = 2;
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")
            { TextBoxdbo.Text = openFileDialog.FileName; }
            else
            { TextBoxdbo.Text = "Bitte Datei auswählen"; }
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
            MessageBox.Show("TBD folderbrowser nicht auffindbar");
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
           
        }
    }
}
