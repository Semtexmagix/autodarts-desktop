using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaktionslogik für About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button helpButton = sender as Button;

            switch (helpButton.Name)
            {
                case "contact1":
                    VisitHelpPage("https://discordapp.com/users/Reepa86#1149");
                    break;
                case "contact2":
                    VisitHelpPage("https://discordapp.com/users/wusaaa#0578");
                    break;
                case "bug":
                    VisitHelpPage("https://github.com/Semtexmagix/autodarts-desktop/issues");
                    break;
                case "donation":
                    VisitHelpPage("https://github.com/lbormann/autodarts-caller");
                    break;
            }
        }

        private void VisitHelpPage(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo(url)
                {
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


    }
}
