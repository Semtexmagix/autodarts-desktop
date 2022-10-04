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
        private AppManager appManager;

        public SetupCustomApp(AppManager appManager)
        {
            InitializeComponent();

            this.appManager = appManager;
            TextBoxobs.Text = Settings.Default.customapp;
            TextBoxCustomAppArgs.Text = Settings.Default.customappargs;
        }

        private void Buttonspeichern_Click(object sender, RoutedEventArgs e)
        {
            appManager.SaveConfigurationCustomApp(TextBoxobs.Text, TextBoxCustomAppArgs.Text);
            Close();
        }

        private void Buttonobs_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select File";
            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.Filter = "All files (*.*)|*.*|Anwendung (*.exe)|*.exe";
            openFileDialog.FilterIndex = 2;
            openFileDialog.ShowDialog();
            TextBoxobs.Text = openFileDialog.FileName;
        }
    }
}