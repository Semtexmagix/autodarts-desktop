using autodarts_desktop.Properties;
using Microsoft.Win32;
using System.Windows;

namespace autodarts_desktop
{
    /// <summary>
    /// Interaktionslogik für SetupExtern.xaml
    /// </summary>
    public partial class SetupExtern : Window
    {
        public SetupExtern()
        {
            InitializeComponent();
            //----------------------------------------------------------------------------------------------
            //------------------->>>>> Laden der Settings als Input für die Textboxen
            //----------------------------------------------------------------------------------------------


            // Extern / Portale

            // Settings Lidarts.org
            TextBoxEmailLidarts.Text = Settings.Default.emaillidarts;
            TextBoxPWLidarts.Password = Settings.Default.pwlidarts;

            // Settings Dartboards.online
            TextBoxEmaildbo.Text = Settings.Default.dbouser;
            TextBoxPWdbo.Password = Settings.Default.dbopw;

            // Settings Extern
            TextBoxmessagestart.Text = Settings.Default.messagestart;
            TextBoxmessageend.Text = Settings.Default.messageend;
            Checkboxskipdart.IsChecked = Settings.Default.checkboxskipdart;
            TextBoxtime.Text = Settings.Default.timetoend;
            TextBoxbrowser.Text = Settings.Default.browserpath;
            TextBoxExternHostPort.Text = Settings.Default.hostport;
        }

        private void Buttonspeichern_Click(object sender, RoutedEventArgs e)
        {
            //----------------------------------------------------------------------------------------------
            //------------------->>>>> Speichern der Textboxen als Settings 
            //----------------------------------------------------------------------------------------------

            // Settings Lidarts.org
            Settings.Default.emaillidarts = TextBoxEmailLidarts.Text;
            Settings.Default.pwlidarts = TextBoxPWLidarts.Password;

            // Settings Dartboards.online
            Settings.Default.dbouser = TextBoxEmaildbo.Text;
            Settings.Default.dbopw = TextBoxPWdbo.Password;

            // Settings Extern
            Settings.Default.messagestart = TextBoxmessagestart.Text;
            Settings.Default.messageend = TextBoxmessageend.Text;
            if (Checkboxskipdart.IsChecked == true)
            {
                Settings.Default.skipdarts = "true";
                Settings.Default.checkboxskipdart = true;
            }
            else
            {
                Settings.Default.skipdarts = "false";
                Settings.Default.checkboxskipdart = false;
            }
            Settings.Default.timetoend = TextBoxtime.Text;
            Settings.Default.browserpath = TextBoxbrowser.Text;
            Settings.Default.hostport = TextBoxExternHostPort.Text;


            // Settings Speichern
            Settings.Default.Save();
            this.Close();
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
    }
}
