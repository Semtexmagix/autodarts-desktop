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
            TextBoxEmailAutodarts.Text = Properties.Settings.Default.emailautodarts;
            TextBoxPWAutodarts.Password = Properties.Settings.Default.pwautodarts;

            // Settings Caller
            TextBoxBoardID.Text = Properties.Settings.Default.boardid;
            TextBoxmedia.Text = Properties.Settings.Default.media;
            TextBoxcallervol.Text = Properties.Settings.Default.callervol;
            Checkboxrandomcaller.IsChecked = Properties.Settings.Default.checkboxrandomcaller;
            Checkboxrandomcallereachleg.IsChecked = Properties.Settings.Default.checkboxrandomcallereachleg;
            slValue.Value = Properties.Settings.Default.sliderpos;
            TextBoxCallerWtt.Text = Properties.Settings.Default.callerwtt;
        }

        private void Buttonspeichern_Click(object sender, RoutedEventArgs e)
        {
            //----------------------------------------------------------------------------------------------
            //------------------->>>>> Speichern der Textboxen als Settings 
            //----------------------------------------------------------------------------------------------

            // Settings Autodarts.io
            Properties.Settings.Default.emailautodarts = TextBoxEmailAutodarts.Text;
            Properties.Settings.Default.pwautodarts = TextBoxPWAutodarts.Password;

            // Settings Caller
            Properties.Settings.Default.boardid = TextBoxBoardID.Text;
            Properties.Settings.Default.media = TextBoxmedia.Text;
            Properties.Settings.Default.callervol = TextBoxcallervol.Text;
            if (Checkboxrandomcaller.IsChecked == true)
            {
                Properties.Settings.Default.randomcaller = "1";
                Properties.Settings.Default.checkboxrandomcaller = true;
            }
            else
            {
                Properties.Settings.Default.randomcaller = "0";
                Properties.Settings.Default.checkboxrandomcaller = false;
            }

            if (Checkboxrandomcallereachleg.IsChecked == true)
            {
                Properties.Settings.Default.randomcallereachleg = "1";
                Properties.Settings.Default.checkboxrandomcallereachleg = true;
            }
            else
            {
                Properties.Settings.Default.randomcallereachleg = "0";
                Properties.Settings.Default.checkboxrandomcallereachleg = false;
            }
            if (Checkboxpossiblecheckoutcall.IsChecked == true)
            {
                Properties.Settings.Default.possiblecheckoutcall = "1";
            }
            else
            {
                Properties.Settings.Default.possiblecheckoutcall = "0";
            }
            Properties.Settings.Default.sliderpos = slValue.Value;
            Properties.Settings.Default.callerwtt = TextBoxCallerWtt.Text;

            // Setup Caller erledigt -> Ja
            Properties.Settings.Default.setupcallerdone = true;

            // Settings Speichern
            Properties.Settings.Default.Save();
            this.Close();
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
