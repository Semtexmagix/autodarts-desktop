using autodarts_desktop.Properties;
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
    /// Interaktionslogik für SetupCaller.xaml
    /// </summary>
    public partial class SetupCaller : Window
    {
        public SetupCaller()
        {
            InitializeComponent();
            //----------------------------------------------------------------------------------------------
            //------------------->>>>> Laden der Settings als Input für die Textboxen
            //----------------------------------------------------------------------------------------------


            // Settings Autodarts.io
            TextBoxEmailAutodarts.Text = Settings.Default.emailautodarts;
            TextBoxPWAutodarts.Password = Settings.Default.pwautodarts;

            // Settings Caller
            TextBoxBoardID.Text = Settings.Default.boardid;
            TextBoxmedia.Text = Settings.Default.media;
            TextBoxcallervol.Text = Settings.Default.callervol;
            Checkboxrandomcaller.IsChecked = Settings.Default.checkboxrandomcaller;
            Checkboxrandomcallereachleg.IsChecked = Settings.Default.checkboxrandomcallereachleg;
            slValue.Value = Settings.Default.sliderpos;
            TextBoxCallerWtt.Text = Settings.Default.callerwtt;
        }

        private void Buttonspeichern_Click(object sender, RoutedEventArgs e)
        {
            //----------------------------------------------------------------------------------------------
            //------------------->>>>> Speichern der Textboxen als Settings 
            //----------------------------------------------------------------------------------------------

            // Settings Autodarts.io
            Settings.Default.emailautodarts = TextBoxEmailAutodarts.Text;
            Settings.Default.pwautodarts = TextBoxPWAutodarts.Password;

            // Settings Caller
            Settings.Default.boardid = TextBoxBoardID.Text;
            Settings.Default.media = TextBoxmedia.Text;
            Settings.Default.callervol = TextBoxcallervol.Text;
            if (Checkboxrandomcaller.IsChecked == true)
            {
                Settings.Default.randomcaller = "1";
                Settings.Default.checkboxrandomcaller = true;
            }
            else
            {
                Settings.Default.randomcaller = "0";
                Settings.Default.checkboxrandomcaller = false;
            }

            if (Checkboxrandomcallereachleg.IsChecked == true)
            {
                Settings.Default.randomcallereachleg = "1";
                Settings.Default.checkboxrandomcallereachleg = true;
            }
            else
            {
                Settings.Default.randomcallereachleg = "0";
                Settings.Default.checkboxrandomcallereachleg = false;
            }
            if (Checkboxpossiblecheckoutcall.IsChecked == true)
            {
                Settings.Default.possiblecheckoutcall = "1";
            }
            else
            {
                Settings.Default.possiblecheckoutcall = "0";
            }
            Settings.Default.sliderpos = slValue.Value;
            Settings.Default.callerwtt = TextBoxCallerWtt.Text;

            // Setup Caller erledigt -> Ja
            Settings.Default.setupcallerdone = true;

            // Settings Speichern
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
            String slidValAsStr = Math.Round(slValue.Value, 2).ToString().Replace(",", ".");
            TextBoxcallervol.Text = slidValAsStr;
        }
    }
}
