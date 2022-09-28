using autodarts_desktop.Properties;
using Microsoft.VisualBasic.Devices;
using Microsoft.Win32;
using System.Windows;

namespace autodarts_desktop
{
    /// <summary>
    /// Interaktionslogik für SetupCustomApp.xaml
    /// </summary>
    public partial class SetupCustomApp : Window
    {
        private AppManager appManager;

        public SetupCustomApp(AppManager appManager)
        {
            InitializeComponent();
            this.appManager = appManager;

            //----------------------------------------------------------------------------------------------
            //------------------->>>>> Laden der Settings als Input für die Textboxen
            //----------------------------------------------------------------------------------------------

            TextBoxobs.Text = Settings.Default.obs;
            TextBoxCustomAppArgs.Text = Settings.Default.customappargs;
        }

        private void Buttonspeichern_Click(object sender, RoutedEventArgs e)
        {
            //----------------------------------------------------------------------------------------------
            //------------------->>>>> Speichern der Textboxen als Settings 
            //----------------------------------------------------------------------------------------------

            appManager.SaveConfigurationCustomApp(TextBoxobs.Text, TextBoxCustomAppArgs.Text);

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