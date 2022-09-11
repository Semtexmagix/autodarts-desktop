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
        }


        private void Comboboxportal_DropDownClosed(object sender, EventArgs e)
        {



            // Ein und Ausblenden der Checkboxen je nach Combobox auswahl funktioniert nicht

            /*
            if (Comboboxportal.SelectedIndex == 1)
            {
                Checkboxbot.Visibility = Visibility.Visible;
                Checkboxvdzobs.Visibility = Visibility.Collapsed;
                Checkboxvdzobs.IsChecked = false;

            }
            else if (Comboboxportal.SelectedIndex == 2)
            {
                Checkboxvdzobs.Visibility = Visibility.Visible;
                Checkboxbot.Visibility = Visibility.Hidden;
                Checkboxbot.IsChecked = false;
            } 
            */
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


            if (Checkboxvdzobs.IsChecked == true)
            {

                string obsphat = Properties.Settings.Default.obs;

                if (obsphat == "Bitte Datei auswählen")
                {
                    MessageBox.Show("OBS Ordner im Setup einstellen!");
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
                    MessageBox.Show("VDZ Ordner im Setup einstellen!");
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

            if (Checkboxbot.IsChecked == true)
            {
                string botphat = Properties.Settings.Default.bot;

                if (botphat == "Bitte Datei auswählen")
                {
                    MessageBox.Show("VDZ Ordner im Setup einstellen!");
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
            string port = "8080";

            // Beispielaufruf für autodarts-caller
            //string callerpath = Properties.Settings.Default.pathcaller;
            string callerpath = "C:\\Users\\danie\\Desktop";




            string callerProcess = "python";
            string callerPath = callerpath + "\\autodarts-caller-master";
            string callerArgumentDelimitter = " ";

            IDictionary<string, string> callerArguments = new Dictionary<string, string>
            {
                    { "autodarts-caller.py", "" },
                    { "-U", autodartsUser },
                    { "-P", autodartsPassword },
                    { "-B", autodartsBoardId },
                    { "-M", autodartsSounds },
                    { "-WTT", $"http://localhost:{port}/throw" }
            };

            // Beispielaufruf für autodarts-extern
            string lidartsUser = Properties.Settings.Default.emaillidarts;
            string lidartsPassword = Properties.Settings.Default.pwlidarts;
            string skipdarts = Properties.Settings.Default.skipdarts;
            string timer = Properties.Settings.Default.timetoend;
            string messagestart = Properties.Settings.Default.messagestart;
            string messageend = Properties.Settings.Default.messageend;
            string externProcess = "node";
            string externPath = callerpath + "/autodarts-extern-master";
            string externArgumentDelimitter = "=";


            IDictionary<string, string> externArguments = new Dictionary<string, string>
            {
                { ".", "" },
                { "--host_port", port },
                { "--autodarts_user", autodartsUser },
                { "--autodarts_password", autodartsPassword },
                { "--autodarts_board_id", autodartsBoardId },
                { "--extern_platform", "lidarts" },
                { "--time_before_exit", timer },
                { "--lidarts_user", lidartsUser },
                { "--lidarts_password", lidartsPassword },
                { "--lidarts_skip_dart_modals", skipdarts },
                { "--lidarts_chat_message_start", messagestart },
                { "--lidarts_chat_message_end", messageend }
            };

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
            else if (Comboboxportal.SelectedIndex == 2)
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

        }
    }
}
