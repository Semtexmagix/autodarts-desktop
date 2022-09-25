using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.Foundation.Numerics;

namespace autodarts_visual
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppManager appManager;


        public MainWindow()
        {
            InitializeComponent();

            appManager = new AppManager();


            ////////////////////////////////// Abfrage ob Setup und Install durchgeführt wurden

            if (Properties.Settings.Default.installdone == false)
            {
                MessageBox.Show("Autodarts-Visual hat erkannt das noch keine Programme installiert wurden, daher wirst du zum Install weitergeleitet");
                Install I1 = new Install(appManager);
                I1.ShowDialog();
            }

            if (Properties.Settings.Default.setupdone == false)
            {    
                MessageBox.Show("Autodarts-Visual hat erkannt das noch keine Setup Daten eingetragen wurden, daher wirst du zum Setup weitergeleitet");
                Setup S1 = new Setup();
                S1.ShowDialog();
            }



            if (Comboboxportal.SelectedIndex == 0)
            {
                Checkboxbot.Visibility = Visibility.Collapsed;
                Checkboxvdzobs.Visibility = Visibility.Collapsed;
                Checkboxdbovdzobs.Visibility = Visibility.Collapsed;
                Checkboxbot.IsChecked = false;
                Checkboxvdzobs.IsChecked = false;
                Checkboxdbovdzobs.IsChecked = false;
            }


        }


        private void Buttonstart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (Comboboxportal.SelectedIndex)
                {
                    case 0:
                        MessageBox.Show("Bitte Portal auswählen");
                        return;
                    case 1:
                        appManager.RunAutodartsPortal();
                        break;
                    case 2:
                        appManager.RunAutodartsExtern(AutodartsExternPlatforms.lidarts);
                        break;
                    case 3:
                        appManager.RunAutodartsExtern(AutodartsExternPlatforms.dartboards);
                        break;
                    case 4:
                        appManager.RunAutodartsExtern(AutodartsExternPlatforms.nakka);
                        break;
                }

                appManager.RunAutodartsCaller();

                if (Checkboxbot.IsChecked == true)
                {
                    appManager.RunAutodartsBot();
                }

                if (Checkboxvdzobs.IsChecked == true)
                {
                    appManager.RunOpenBroadcasterSofware();
                    appManager.RunVirtualDartsZoom();
                }

                if (Checkboxdbovdzobs.IsChecked == true)
                {
                    appManager.RunOpenBroadcasterSofware();
                    appManager.RunVirtualDartsZoom();
                    appManager.RunDartboardsClient();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
    
        }

        private void Buttonsetup_Click(object sender, RoutedEventArgs e)
        {
            Setup S1 = new Setup();
            S1.ShowDialog();
        }

        private void Buttonschließen_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Checkboxdbovdzobs_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Checkboxvdzobs_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Checkboxbot_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void Buttoninstall_Click(object sender, RoutedEventArgs e)
        {
            Install I1 = new Install(appManager);
            I1.ShowDialog();
        }

        private void Comboboxportal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Ein und Ausblenden der Checkboxen
            //TBD bei Zurückfallen auf "Auswählen" alle Boxen verstecken
            if (Comboboxportal.SelectedIndex == 1)
            {
                Checkboxbot.Visibility = Visibility.Visible;
                Checkboxvdzobs.Visibility = Visibility.Collapsed;
                Checkboxdbovdzobs.Visibility = Visibility.Collapsed;
                Checkboxvdzobs.IsChecked = false;
                Checkboxdbovdzobs.IsChecked = false;
            }
            else if (Comboboxportal.SelectedIndex == 2)
            {
                Properties.Settings.Default.portal = "lidarts";
                Properties.Settings.Default.Save();
                Checkboxvdzobs.Visibility = Visibility.Visible;
                Checkboxbot.Visibility = Visibility.Collapsed;
                Checkboxdbovdzobs.Visibility = Visibility.Collapsed;
                Checkboxbot.IsChecked = false;
                Checkboxdbovdzobs.IsChecked = false;
            }
            else if (Comboboxportal.SelectedIndex == 3)
            {
                Properties.Settings.Default.portal = "dartboards";
                Properties.Settings.Default.Save();
                Checkboxdbovdzobs.Visibility = Visibility.Visible;
                Checkboxbot.Visibility = Visibility.Collapsed;
                Checkboxvdzobs.Visibility = Visibility.Collapsed;
                Checkboxbot.IsChecked = false;
                Checkboxvdzobs.IsChecked = false;
            }
            else if (Comboboxportal.SelectedIndex == 4)
            {
                Properties.Settings.Default.portal = "nakka";
                Properties.Settings.Default.Save();
                Checkboxbot.Visibility = Visibility.Collapsed;
                Checkboxvdzobs.Visibility = Visibility.Collapsed;
                Checkboxdbovdzobs.Visibility = Visibility.Collapsed;
                Checkboxbot.IsChecked = false;
                Checkboxvdzobs.IsChecked = false;
                Checkboxdbovdzobs.IsChecked = false;
            }
        }

        private void Buttonrest_Click(object sender, RoutedEventArgs e)
        {
            // Setup erledigt -> NEIN
            Properties.Settings.Default.installdone = false;
            Properties.Settings.Default.setupdone = false;
            // Settings Speichern
            Properties.Settings.Default.Save();
        }

        
    }
}
