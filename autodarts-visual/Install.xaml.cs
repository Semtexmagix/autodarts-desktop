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

namespace autodarts_visual
{
    /// <summary>
    /// Interaktionslogik für Install.xaml
    /// </summary>
    public partial class Install : Window
    {
        public Install()
        {
            InitializeComponent();
        }

        private void Checkboxcallerinstall_Checked(object sender, RoutedEventArgs e)
        {
            Checkboxexterninstall.Visibility = Visibility.Visible;
            Labelextern.Visibility = Visibility.Visible;
        }

        private void Checkboxcallerinstall_Unchecked(object sender, RoutedEventArgs e)
        {
            Checkboxexterninstall.Visibility = Visibility.Hidden;
            Labelextern.Visibility = Visibility.Hidden;
        }

    }
}
