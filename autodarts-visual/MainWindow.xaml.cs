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
        
        public MainWindow()
        {
            InitializeComponent();



            ////////////////////////////////// Abfrage ob Setup und Install durchgeführt wurden

            if (Properties.Settings.Default.installdone == false)
            {
                MessageBox.Show("Autodarts-Visual hat erkannt das noch keine Programme installiert wurden, daher wirst du zum Install weitergeleitet");
                Install I1 = new Install();
                I1.ShowDialog();
            }

            if (Properties.Settings.Default.setupdone == false)
            {    
                MessageBox.Show("Autodarts-Visual hat erkannt das noch keine Setup Daten eingetragen wurden, daher wirst du zum Setup weitergeleitet");
                Setup S1 = new Setup();
                S1.ShowDialog();
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

        private void Buttonstart_Click(object sender, RoutedEventArgs e)
        {



            ///////////////////////////////////////// Mitstarten von VDZ und OBS
            ///

            if (Checkboxvdzobs.IsChecked == true)
            {

                string obsphat = Properties.Settings.Default.obs;
                if (obsphat == "Bitte Datei auswählen")
                {
                    MessageBox.Show("OBS.exe im Setup einstellen!");
                }
                else
                {
                    Process obsprocess = new Process();
                    obsprocess.StartInfo.FileName = "obs64.exe";
                    obsprocess.StartInfo.UseShellExecute = true;
                    obsprocess.StartInfo.WorkingDirectory = "D:\\obs-studio\\bin\\64bit";
                    //obsprocess.StartInfo.Verb = "runas";
                    obsprocess.Start();
                }


                string vdzphat = Properties.Settings.Default.vdz;
                if (vdzphat == "Bitte Datei auswählen")
                {
                    MessageBox.Show("VDZ.exe im Setup einstellen!");
                }
                else
                {
                    Process vdzprocess = new Process();
                    vdzprocess.StartInfo.FileName = vdzphat;
                    vdzprocess.StartInfo.UseShellExecute = true;
                    //vdzprocess.StartInfo.Verb = "runas";
                    vdzprocess.Start();
                }
            }



            ///////////////////////////////////////// Mitstarten von DBO, VDZ und OBS
            ///

            if (Checkboxdbovdzobs.IsChecked == true)
            {

                string obsphat = Properties.Settings.Default.obs;

                if (obsphat == "Bitte Datei auswählen")
                {
                    MessageBox.Show("OBS.exe im Setup einstellen!");
                }
                else
                {
                    Process obsprocess = new Process();
                    obsprocess.StartInfo.FileName = obsphat;
                    obsprocess.StartInfo.UseShellExecute = true;
                    obsprocess.StartInfo.WorkingDirectory = "D:\\obs-studio\\bin\\64bit";
                    //obsprocess.StartInfo.Verb = "runas";
                    obsprocess.Start();
                }



                string vdzphat = Properties.Settings.Default.vdz;

                if (vdzphat == "Bitte Datei auswählen")
                {
                    MessageBox.Show("VDZ.exe im Setup einstellen!");
                }
                else
                {
                    Process vdzprocess = new Process();
                    vdzprocess.StartInfo.FileName = vdzphat;
                    vdzprocess.StartInfo.UseShellExecute = true;
                    //vdzprocess.StartInfo.Verb = "runas";
                    vdzprocess.Start();
                }

                string dbophat = Properties.Settings.Default.dbo;

                if (dbophat == "Bitte Datei auswählen")
                {
                    MessageBox.Show("DBO.exe im Setup einstellen!");
                }
                else
                {
                    Process dboprocess = new Process();
                    dboprocess.StartInfo.FileName = dbophat;
                    dboprocess.StartInfo.UseShellExecute = true;
                    //dboprocess.StartInfo.Verb = "runas";
                    dboprocess.Start();
                }
            }



            ///////////////////////////////////////// Mitstarten von Autodarts.io Bot
            ///


            if (Checkboxbot.IsChecked == true)
            {
                string botphat = Properties.Settings.Default.bot;

                if (botphat == "Bitte Datei auswählen")
                {
                    MessageBox.Show("Bot Ordner im Setup einstellen!");
                }
                else
                {
                    Process vdzprocess = new Process();
                    vdzprocess.StartInfo.FileName = botphat;
                    vdzprocess.StartInfo.UseShellExecute = true;
                    //vdzprocess.StartInfo.Verb = "runas";
                    vdzprocess.Start();
                }
            }




            if (Comboboxportal.SelectedIndex == 0)
            {
                MessageBox.Show("Bitte Portal auswählen");
            }


            // Werte, die für beide Apps benötigt werden
            string autodartsUser = Properties.Settings.Default.emailautodarts;
            string autodartsPassword = Properties.Settings.Default.pwautodarts;
            string autodartsBoardId = Properties.Settings.Default.boardid;
            string autodartsSounds = Properties.Settings.Default.media;
            string autodartsCallerVol = Properties.Settings.Default.callervol;
            string autodartsRandomCaller = Properties.Settings.Default.randomcaller;
            string autodartsRandomCallerEachLeg = Properties.Settings.Default.randomcallereachleg;

            string port = "8080";

            
            
            
            
            /////////////////////////  Autodarts-caller Starten mit Args aus den Settings
            ///
            
            //string callerpath = Properties.Settings.Default.pathcaller;
            string callerProcess = "autodarts-caller.exe";
            string callerpath = ".\\";
            string callerPath = callerpath + "caller";
            string callerArgumentDelimitter = " ";


            IDictionary<string, string> callerArguments = new Dictionary<string, string>
            {
                    { "-U", autodartsUser },
                    { "-P", autodartsPassword },
                    { "-B", autodartsBoardId },
                    { "-M", autodartsSounds },
                    { "-V", autodartsCallerVol },
                    { "-R", autodartsRandomCaller },
                    { "-L", autodartsRandomCallerEachLeg },
                    { "-WTT", $"http://localhost:{port}/throw" }
            };

            /////////////////////////  Autodarts-Extern Starten mit Args aus den Settings
            ///

            string lidartsUser = Properties.Settings.Default.emaillidarts;
            string lidartsPassword = Properties.Settings.Default.pwlidarts;
            string skipdarts = Properties.Settings.Default.skipdarts;
            string timer = Properties.Settings.Default.timetoend;
            string messagestart = Properties.Settings.Default.messagestart;
            string messageend = Properties.Settings.Default.messageend;
            string browserpath = Properties.Settings.Default.browserpath;
            string dboUser = Properties.Settings.Default.dbouser;
            string dboPassword = Properties.Settings.Default.dbopw;

            string externProcess = "autodarts-extern.exe";
            string externpath = ".\\";
            string externPath = externpath + "extern";
            string externArgumentDelimitter = "=";

            string portalextern = Properties.Settings.Default.portal;

            IDictionary<string, string> externArguments = new Dictionary<string, string>
            {
                { "--browser_path", browserpath },
                { "--host_port", port },
                { "--autodarts_user", autodartsUser },
                { "--autodarts_password", autodartsPassword },
                { "--autodarts_board_id", autodartsBoardId },
                { "--extern_platform", portalextern },
                { "--time_before_exit", timer },
                { "--lidarts_user", lidartsUser },
                { "--lidarts_password", lidartsPassword },
                { "--lidarts_skip_dart_modals", skipdarts },
                { "--lidarts_chat_message_start", messagestart },
                { "--lidarts_chat_message_end", messageend },
                { "--nakka_skip_dart_modals", skipdarts },
                { "--dartboards_user", dboUser },
                { "--dartboards_password", dboPassword },
                { "--dartboards_skip_dart_modals", skipdarts }
            };

            // Combobox Autodarts
            if (Comboboxportal.SelectedIndex == 1)
            {
                var ps = new ProcessStartInfo("http://Autodarts.io")
                {
                    UseShellExecute = true,
                    Verb = "open"
                };
                Process.Start(ps);

                RunApp(callerProcess, callerPath, callerArguments, callerArgumentDelimitter);
            }
            // Combobox Lidarts
            else if (Comboboxportal.SelectedIndex == 2)
            {
                RunApp(callerProcess, callerPath, callerArguments, callerArgumentDelimitter);
                RunApp(externProcess, externPath, externArguments, externArgumentDelimitter);
            }
            // Combobox Dartbords.online
            else if (Comboboxportal.SelectedIndex == 3)
            {
                RunApp(callerProcess, callerPath, callerArguments, callerArgumentDelimitter);
                RunApp(externProcess, externPath, externArguments, externArgumentDelimitter);
            }
            // Combobox Nakka
            else if (Comboboxportal.SelectedIndex == 4)
            {              
                RunApp(callerProcess, callerPath, callerArguments, callerArgumentDelimitter);
                RunApp(externProcess, externPath, externArguments, externArgumentDelimitter);
            }
        }

        private void RunApp(string processName, string workingDirectory, IDictionary<string, string> arguments, string argumentDelimitter)
        {
            // Wir setzen die übergebenen 'arguments' zu einen String zusammen, der dann beim Prozess starten genutzt werden kann
            string composedArguments = "";

            // Wir durchlaufen alle übergebenen Argumente und hängen diese dem String an
            foreach (KeyValuePair<string, string> a in arguments)
            {
                // Das Start Argument hat kein Key-Value, deshalb unterscheiden wir hier und nehmen an,
                // dass es kein Value gibt, wenn der Wert ein leerer String ist.
                if (String.IsNullOrEmpty(a.Value))
                {
                    composedArguments += " " + a.Key;
                }
                // ... sonst hängen wir den Value an
                else
                {
                    composedArguments += " " + a.Key + argumentDelimitter + "\"" + a.Value + "\"";
                }
            }

            Console.WriteLine(composedArguments);

            using (var process = new Process())
            {
                try
                {
                    process.StartInfo.WorkingDirectory = workingDirectory;
                    process.StartInfo.FileName = processName;
                    process.StartInfo.Arguments = composedArguments;
                    process.StartInfo.RedirectStandardOutput = false;
                    process.StartInfo.RedirectStandardError = false;
                    process.StartInfo.UseShellExecute = true;
                    process.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred trying to start \"{processName}\" with \"{composedArguments}\":\n{ex.Message}");
                    throw;
                }
            }
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
            Install I1 = new Install();
            I1.ShowDialog();
        }

        private void Comboboxportal_DropDownClosed(object sender, SelectionChangedEventArgs e)
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
            // Setup erledigt -> NEIN -> Reset TEST
            Properties.Settings.Default.installdone = false;
            Properties.Settings.Default.setupdone = false;
            // Settings Speichern
            Properties.Settings.Default.Save();
        }
    }
}
