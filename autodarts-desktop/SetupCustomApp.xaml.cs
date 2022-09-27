using autodarts_desktop.Properties;
using Microsoft.Win32;
using System.Windows;

namespace autodarts_desktop
{
    /// <summary>
    /// Interaktionslogik für SetupCustomApp.xaml
    /// </summary>
    public partial class SetupCustomApp : Window
    {
        public SetupCustomApp()
        {
            InitializeComponent();
            //----------------------------------------------------------------------------------------------
            //------------------->>>>> Laden der Settings als Input für die Textboxen
            //----------------------------------------------------------------------------------------------

            // Settings Zusatz Programme (OBS)
            TextBoxobs.Text = Settings.Default.obs;
            TextBoxCustomAppArgs.Text = Settings.Default.customappargs;
        }

        private void Buttonspeichern_Click(object sender, RoutedEventArgs e)
        {
            //----------------------------------------------------------------------------------------------
            //------------------->>>>> Speichern der Textboxen als Settings 
            //----------------------------------------------------------------------------------------------

            // Settings Zusatz Programme Lidarts (OBS)
            Settings.Default.obs = TextBoxobs.Text;
            Settings.Default.customappargs = TextBoxCustomAppArgs.Text;

            // Setup Custom app erledigt -> Ja
            Settings.Default.setupcustomappdone = true;

            // Settings Speichern
            Settings.Default.Save();
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
    }
}