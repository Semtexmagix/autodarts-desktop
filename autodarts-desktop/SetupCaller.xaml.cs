using autodarts_desktop.Properties;
using Ookii.Dialogs.Wpf;
using System.Windows;

namespace autodarts_desktop
{
    /// <summary>
    /// Interaktionslogik für SetupCaller.xaml
    /// </summary>
    public partial class SetupCaller : Window
    {
        public SetupCaller()
        {
            InitializeComponent();

            TextBoxEmailAutodarts.Text = Settings.Default.emailautodarts;
            TextBoxPWAutodarts.Password = Settings.Default.pwautodarts;
            TextBoxBoardID.Text = Settings.Default.boardid;
            TextBoxmedia.Text = Settings.Default.media;
            slValue.Value = WindowHelper.GetDoubleByString(Settings.Default.callervol);
            Checkboxrandomcaller.IsChecked = WindowHelper.GetBoolByString(Settings.Default.randomcaller);
            Checkboxrandomcallereachleg.IsChecked = WindowHelper.GetBoolByString(Settings.Default.randomcallereachleg);
            Checkboxpossiblecheckoutcall.IsChecked = WindowHelper.GetBoolByString(Settings.Default.possiblecheckoutcall);
            Checkboxcalleverydart.IsChecked = WindowHelper.GetBoolByString(Settings.Default.calleverydart);
            TextBoxCallerWtt.Text = Settings.Default.callerwtt;
        }

        private void Buttonspeichern_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.emailautodarts = TextBoxEmailAutodarts.Text;
            Settings.Default.pwautodarts = TextBoxPWAutodarts.Password;
            Settings.Default.boardid = TextBoxBoardID.Text;
            Settings.Default.media = TextBoxmedia.Text;
            Settings.Default.callervol = TextBoxcallervol.Text;
            Settings.Default.randomcaller = WindowHelper.GetStringByBool((bool)Checkboxrandomcaller.IsChecked);
            Settings.Default.randomcallereachleg = WindowHelper.GetStringByBool((bool)Checkboxrandomcallereachleg.IsChecked);
            Settings.Default.possiblecheckoutcall = WindowHelper.GetStringByBool((bool)Checkboxpossiblecheckoutcall.IsChecked);
            Settings.Default.calleverydart = WindowHelper.GetStringByBool((bool)Checkboxcalleverydart.IsChecked);
            Settings.Default.callerwtt = TextBoxCallerWtt.Text;
            Settings.Default.Save();
            Close();
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
            TextBoxcallervol.Text = WindowHelper.GetStringByDouble(slValue.Value);
        }




    }
}
