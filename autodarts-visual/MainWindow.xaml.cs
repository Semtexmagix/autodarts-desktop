using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.NetworkInformation;
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
using Windows.ApplicationModel.Store.Preview.InstallControl;
using Windows.Foundation.Numerics;

namespace autodarts_visual
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppManager appManager;
        private Dictionary<string, bool> appsInstallState;

        public MainWindow()
        {
            InitializeComponent();
            appManager = new AppManager();
            SetInstallStateApps();
        }


        private void Buttonstart_Click(object sender, RoutedEventArgs e)
        {
            var selectedTag = ((ComboBoxItem)Comboboxportal.SelectedItem).Tag.ToString();

            try
            {
                appManager.RunAutodartsCaller();

                switch (selectedTag)
                {
                    case "caller":
                        appManager.RunAutodartsPortal();
                        break;
                    case "lidarts":
                        appManager.RunAutodartsExtern(AutodartsExternPlatforms.lidarts);
                        break;
                    case "nakka":
                        appManager.RunAutodartsExtern(AutodartsExternPlatforms.nakka);
                        break;
                    case "dartboards":
                        appManager.RunAutodartsExtern(AutodartsExternPlatforms.dartboards);
                        break;
                }

                if (Checkboxbot.IsChecked == true)
                {
                    appManager.RunAutodartsBot();
                }
                if (Checkboxvdz.IsChecked == true)
                {
                    appManager.RunVirtualDartsZoom();
                }
                if (Checkboxdboc.IsChecked == true)
                {
                    appManager.RunDartboardsClient();
                }
                if (Checkboxcustom.IsChecked == true)
                {
                    AppManager.RunCustomApp();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
    
        }

        private void Buttonsetup_Click(object sender, RoutedEventArgs e)
        {
            OpenConfigurationForm();
        }

        private void Buttoninstall_Click(object sender, RoutedEventArgs e)
        {
            OpenInstallForm();
        }

        private void Comboboxportal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Comboboxportal.SelectedIndex == -1) return;

            bool appState;

            
            if (String.IsNullOrEmpty(Properties.Settings.Default.obs))
            {
                Checkboxcustom.IsEnabled = false;
                Checkboxcustom.IsChecked = false;
            }
            else
            {
                Checkboxcustom.IsEnabled = true;
                Checkboxcustom.IsChecked = Properties.Settings.Default.custom_start_default;
            }

            appsInstallState.TryGetValue("autodarts", out appState);
            Checkboxad.IsEnabled = appState;
            Checkboxad.IsChecked = appState ? Properties.Settings.Default.ad_start_default : false;



            var selectedTag = ((ComboBoxItem)Comboboxportal.SelectedItem).Tag.ToString();

            switch (selectedTag)
            {
                case "caller":
                    appsInstallState.TryGetValue("autodarts-bot", out appState);
                    Checkboxbot.IsEnabled = appState;
                    Checkboxbot.IsChecked = appState ? Properties.Settings.Default.bot_start_default : false;

                    Checkboxvdz.IsEnabled = false;
                    Checkboxvdz.IsChecked = false;

                    Checkboxdboc.IsEnabled = false;
                    Checkboxdboc.IsChecked = false;
                    break;
                case "lidarts":
                    appsInstallState.TryGetValue("virtual-darts-zoom", out appState);
                    Checkboxvdz.IsEnabled = appState;
                    Checkboxvdz.IsChecked = appState ? Properties.Settings.Default.vdz_start_default : false;

                    Checkboxbot.IsEnabled = false;
                    Checkboxbot.IsChecked = false;

                    Checkboxdboc.IsEnabled = false;
                    Checkboxdboc.IsChecked = false;
                    break;
                case "nakka":
                    appsInstallState.TryGetValue("virtual-darts-zoom", out appState);
                    Checkboxvdz.IsEnabled = appState;
                    Checkboxvdz.IsChecked = appState ? Properties.Settings.Default.vdz_start_default : false; ;

                    Checkboxbot.IsEnabled = false;
                    Checkboxbot.IsChecked = false;

                    Checkboxdboc.IsEnabled = false;
                    Checkboxdboc.IsChecked = false;

                    break;
                case "dartboards":
                    appsInstallState.TryGetValue("dartboards-client", out appState);
                    Checkboxdboc.IsEnabled = appState;
                    Checkboxdboc.IsChecked = appState ? Properties.Settings.Default.dboc_start_default : false;

                    appsInstallState.TryGetValue("virtual-darts-zoom", out appState);
                    Checkboxvdz.IsEnabled = appState;
                    Checkboxvdz.IsChecked = appState ? Properties.Settings.Default.vdz_start_default : false; ;

                    Checkboxbot.IsEnabled = false;
                    Checkboxbot.IsChecked = false;

                    break;
            }
        }



        private void OpenInstallForm()
        {
            Install I1 = new Install(appManager);
            I1.ShowDialog();
            SetInstallStateApps();

            if (Properties.Settings.Default.setupdone == false)
            {
                MessageBox.Show("App-configuration not found - redirect to configuration page");
                OpenConfigurationForm();
            }
        }
        
        private void OpenConfigurationForm()
        {
            Setup S1 = new Setup();
            S1.ShowDialog();
            SetInstallStateApps();
        }

        private void SetInstallStateApps()
        {
            Comboboxportal.Items.Clear();
            appsInstallState = appManager.GetAppsInstallState();
            
            foreach (var appInstallState in appsInstallState)
            {
                switch (appInstallState.Key)
                {
                    case "autodarts-caller":
                        if(appInstallState.Value == true)
                        {
                            AddComboBoxItem("autodarts.io + autodarts-caller", "caller");
                        }
                        break;
                    case "autodarts-extern":
                        if (appInstallState.Value == true)
                        {
                            AddComboBoxItem("autodarts-extern: lidarts.org", "lidarts");
                            AddComboBoxItem("autodarts-extern: nakka.com/n01/online", "nakka");
                            AddComboBoxItem("autodarts-extern: dartboards.online", "dartboards");
                        }
                        break;

                    default:
                        break;
                }
            }

            bool autodartsCallerInstalled = false;
            appsInstallState.TryGetValue("autodarts-caller", out autodartsCallerInstalled);
            if (autodartsCallerInstalled == false)
            {
                MessageBox.Show("Requirements (autodarts-caller) not satisfied - redirect to download page");
                OpenInstallForm();
            }
            else
            {
                Comboboxportal.SelectedIndex = 0;
            }
        }

        private void AddComboBoxItem(string content, String value)
        {
            var appItem = new ComboBoxItem();
            appItem.Content = content;
            appItem.Tag = value;
            Comboboxportal.Items.Add(appItem);
        }

        private void Checkboxad_Clicked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ad_start_default = (bool)Checkboxad.IsChecked;
            Properties.Settings.Default.Save();
        }

        private void Checkboxbot_Clicked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.bot_start_default = (bool)Checkboxbot.IsChecked;
            Properties.Settings.Default.Save();
        }

        private void Checkboxvdz_Clicked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.vdz_start_default = (bool)Checkboxvdz.IsChecked;
            Properties.Settings.Default.Save();
        }

        private void Checkboxdboc_Clicked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.dboc_start_default = (bool)Checkboxdboc.IsChecked;
            Properties.Settings.Default.Save();
        }

        private void Checkboxcustom_Clicked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.custom_start_default = (bool)Checkboxcustom.IsChecked;
            Properties.Settings.Default.Save();
        }
    }
}
