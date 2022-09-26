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
            TextBoxobs.Text = Properties.Settings.Default.obs;
            TextBoxCustomAppArgs.Text = Properties.Settings.Default.customappargs;
        }

        private void Buttonspeichern_Click(object sender, RoutedEventArgs e)
        {
            //----------------------------------------------------------------------------------------------
            //------------------->>>>> Speichern der Textboxen als Settings 
            //----------------------------------------------------------------------------------------------

            // Settings Zusatz Programme Lidarts (OBS)
            Properties.Settings.Default.obs = TextBoxobs.Text;
            Properties.Settings.Default.customappargs = TextBoxCustomAppArgs.Text;

            // Setup Custom app erledigt -> Ja
            Properties.Settings.Default.setupcustomappdone = true;

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
    }
}