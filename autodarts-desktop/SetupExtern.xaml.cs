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
            TextBoxEmailLidarts.Text = Properties.Settings.Default.emaillidarts;
            TextBoxPWLidarts.Password = Properties.Settings.Default.pwlidarts;

            // Settings Dartboards.online
            TextBoxEmaildbo.Text = Properties.Settings.Default.dbouser;
            TextBoxPWdbo.Password = Properties.Settings.Default.dbopw;

            // Settings Extern
            TextBoxmessagestart.Text = Properties.Settings.Default.messagestart;
            TextBoxmessageend.Text = Properties.Settings.Default.messageend;
            Checkboxskipdart.IsChecked = Properties.Settings.Default.checkboxskipdart;
            TextBoxtime.Text = Properties.Settings.Default.timetoend;
            TextBoxbrowser.Text = Properties.Settings.Default.browserpath;
            TextBoxExternHostPort.Text = Properties.Settings.Default.hostport;
        }

        private void Buttonspeichern_Click(object sender, RoutedEventArgs e)
        {
            //----------------------------------------------------------------------------------------------
            //------------------->>>>> Speichern der Textboxen als Settings 
            //----------------------------------------------------------------------------------------------

            // Settings Lidarts.org
            Properties.Settings.Default.emaillidarts = TextBoxEmailLidarts.Text;
            Properties.Settings.Default.pwlidarts = TextBoxPWLidarts.Password;

            // Settings Dartboards.online
            Properties.Settings.Default.dbouser = TextBoxEmaildbo.Text;
            Properties.Settings.Default.dbopw = TextBoxPWdbo.Password;

            // Settings Extern
            Properties.Settings.Default.messagestart = TextBoxmessagestart.Text;
            Properties.Settings.Default.messageend = TextBoxmessageend.Text;
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

            // Setup Extern erledigt -> Ja
            Properties.Settings.Default.setupexterndone = true;

            // Settings Speichern
            Properties.Settings.Default.Save();
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
