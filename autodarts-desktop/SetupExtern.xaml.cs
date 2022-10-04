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

            TextBoxEmailLidarts.Text = Settings.Default.emaillidarts;
            TextBoxPWLidarts.Password = Settings.Default.pwlidarts;
            TextBoxEmaildbo.Text = Settings.Default.dbouser;
            TextBoxPWdbo.Password = Settings.Default.dbopw;
            TextBoxmessagestart.Text = Settings.Default.messagestart;
            TextBoxmessageend.Text = Settings.Default.messageend;
            Checkboxskipdart.IsChecked = WindowHelper.GetBoolByString(Settings.Default.skipdarts);
            TextBoxtime.Text = Settings.Default.timetoend;
            TextBoxbrowser.Text = Settings.Default.browserpath;
            TextBoxExternHostPort.Text = Settings.Default.hostport;
        }

        private void Buttonspeichern_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.emaillidarts = TextBoxEmailLidarts.Text;
            Settings.Default.pwlidarts = TextBoxPWLidarts.Password;
            Settings.Default.dbouser = TextBoxEmaildbo.Text;
            Settings.Default.dbopw = TextBoxPWdbo.Password;
            Settings.Default.messagestart = TextBoxmessagestart.Text;
            Settings.Default.messageend = TextBoxmessageend.Text;
            Settings.Default.skipdarts = WindowHelper.GetStringByBool((bool)Checkboxskipdart.IsChecked);
            Settings.Default.timetoend = TextBoxtime.Text;
            Settings.Default.browserpath = TextBoxbrowser.Text;
            Settings.Default.hostport = TextBoxExternHostPort.Text;
            Settings.Default.Save();
            this.Close();
        }

        private void Buttonbrowser_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select File";
            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.Filter = "All files (*.*)|*.*|Anwendung (*.exe)|*.exe";
            openFileDialog.FilterIndex = 2;
            openFileDialog.ShowDialog();
            TextBoxbrowser.Text = openFileDialog.FileName;
        }
    }
}
