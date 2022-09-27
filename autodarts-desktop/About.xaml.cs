using System;
using System.Diagnostics;
using System.Windows;
using Button = System.Windows.Controls.Button;
using Clipboard = System.Windows.Forms.Clipboard;
using MessageBox = System.Windows.Forms.MessageBox;

namespace autodarts_desktop
{
    /// <summary>
    /// Interaktionslogik für About.xaml
    /// </summary>
    public partial class About : Window
    {
        private const string donationAdress = "bc1qr7wsvmmgaj6dle8gae2dl0dcxu5yh8vqlv34x4";

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
                    Clipboard.SetDataObject(donationAdress);
                    MessageBox.Show($"{donationAdress} copied to clipboard - Thank you!");
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
