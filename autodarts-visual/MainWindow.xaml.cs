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
                if (Checkboxdbo.IsChecked == true)
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
            Setup S1 = new Setup();
            S1.ShowDialog();
        }

        private void Buttoninstall_Click(object sender, RoutedEventArgs e)
        {
            OpenInstallForm();
        }

        private void Comboboxportal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Comboboxportal.SelectedIndex == -1) return;

            bool appState;

            Checkboxcustom.IsChecked = false;
            if (String.IsNullOrEmpty(Properties.Settings.Default.obs))
            {
                Checkboxcustom.IsEnabled = false;
            }
            else
            {
                Checkboxcustom.IsEnabled = true;
            }

            appsInstallState.TryGetValue("autodarts", out appState);
            Checkboxad.IsEnabled = appState;
            Checkboxad.IsChecked = false;



            var selectedTag = ((ComboBoxItem)Comboboxportal.SelectedItem).Tag.ToString();

            switch (selectedTag)
            {
                case "caller":
                    appsInstallState.TryGetValue("autodarts-bot", out appState);
                    Checkboxbot.IsEnabled = appState;
                    Checkboxbot.IsChecked = false;

                    Checkboxvdz.IsEnabled = false;
                    Checkboxvdz.IsChecked = false;

                    Checkboxdbo.IsEnabled = false;
                    Checkboxdbo.IsChecked = false;
                    break;
                case "lidarts":
                    appsInstallState.TryGetValue("virtual-darts-zoom", out appState);
                    Checkboxvdz.IsEnabled = appState;
                    Checkboxvdz.IsChecked = false;

                    Checkboxbot.IsEnabled = false;
                    Checkboxbot.IsChecked = false;

                    Checkboxdbo.IsEnabled = false;
                    Checkboxdbo.IsChecked = false;
                    break;
                case "nakka":
                    appsInstallState.TryGetValue("virtual-darts-zoom", out appState);
                    Checkboxvdz.IsEnabled = appState;
                    Checkboxvdz.IsChecked = false;

                    Checkboxbot.IsEnabled = false;
                    Checkboxbot.IsChecked = false;

                    Checkboxdbo.IsEnabled = false;
                    Checkboxdbo.IsChecked = false;

                    break;
                case "dartboards":
                    appsInstallState.TryGetValue("dartboards-client", out appState);
                    Checkboxdbo.IsEnabled = appState;
                    Checkboxdbo.IsChecked = false;

                    appsInstallState.TryGetValue("virtual-darts-zoom", out appState);
                    Checkboxvdz.IsEnabled = appState;
                    Checkboxvdz.IsChecked = false;

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
                Setup S1 = new Setup();
                S1.ShowDialog();
            }
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



    }
}
