using autodarts_desktop.control;
using autodarts_desktop.model;
using autodarts_desktop.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Button = System.Windows.Controls.Button;
using CheckBox = System.Windows.Controls.CheckBox;
using MessageBox = System.Windows.MessageBox;

namespace autodarts_desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // ATTRIBUTES

        private ProfileManager profileManager;
        private Profile? selectedProfile;
        private List<UIElement> selectedProfileElements;



        // METHODS

        public MainWindow()
        {
            InitializeComponent();


            selectedProfileElements = new();
            CheckBoxStartProfileOnProgramStart.IsChecked = Settings.Default.start_profile_on_start;

            try
            {
                profileManager = new ProfileManager();
                profileManager.AppDownloadStarted += ProfileManager_AppDownloadStarted;
                profileManager.AppDownloadFinished += ProfileManager_AppDownloadFinished;
                profileManager.AppDownloadFailed += ProfileManager_AppDownloadFailed;
                profileManager.AppDownloadProgressed += ProfileManager_AppDownloadProgressed;
                profileManager.AppInstallStarted += ProfileManager_AppInstallStarted;
                profileManager.AppInstallFinished += ProfileManager_AppInstallFinished;
                profileManager.AppInstallFailed += ProfileManager_AppInstallFailed;
                profileManager.AppConfigurationRequired += ProfileManager_AppConfigurationRequired;

                profileManager.LoadAppsAndProfiles();

                RenderProfiles();

                Updater.NewReleaseFound += Updater_NewReleaseFound;
                Updater.ReleaseInstallInitialized += Updater_ReleaseInstallInitialized;
                Updater.ReleaseDownloadStarted += Updater_ReleaseDownloadStarted;
                Updater.ReleaseDownloadFailed += Updater_ReleaseDownloadFailed;
                Updater.ReleaseDownloadProgressed += Updater_ReleaseDownloadProgressed;
                Updater.CheckNewVersion();
            }
            catch(ConfigurationException ex)
            {
                if (MessageBox.Show($"Configuration-file '{ex.File}' not readable. You can fix it by yourself or let it go to hell and I recreate it for you. Do you want me to reset it? (All of your settings will be lost)", "Configuration Error", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        profileManager.DeleteConfigurationFile(ex.File);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Configuration-file-deletion failed. Please delete it by yourself. " + e.Message);
                    }
                }
                MessageBox.Show("Please restart the application.");
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong: " + ex.Message);
                Environment.Exit(1);
            }
        }



        private void Buttonstart_Click(object sender, RoutedEventArgs e)
        {
            RunSelectedProfile();
        }

        private void Buttonabout_Click(object sender, RoutedEventArgs e)
        {
            new About().ShowDialog();
        }
        
        private void Comboboxportal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectedProfile != null) selectedProfile.IsTaggedForStart = false;
            selectedProfile = ((ComboBoxItem)Comboboxportal.SelectedItem).Tag as Profile;
            if (selectedProfile == null) return;
            selectedProfile.IsTaggedForStart = true;
            RenderProfile();
        }
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.Default.start_profile_on_start = (bool)CheckBoxStartProfileOnProgramStart.IsChecked;
            Settings.Default.Save();

            try
            {
                profileManager.StoreApps();
                profileManager.CloseApps();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occured: " + ex.Message);
            }
        }

        private void WaitingText_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            WaitingText.Visibility = Visibility.Hidden;
        }



        private void Updater_NewReleaseFound(object? sender, ReleaseEventArgs e)
        {
            if (MessageBox.Show($"New Version '{e.Version}' available! Do you want to update?", "New Version", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    Updater.UpdateToNewVersion();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Update to new version failed: " + ex.Message);
                }
            }
        }

        private void Updater_ReleaseDownloadStarted(object? sender, ReleaseEventArgs e)
        {
            SetWait(true, "Downloading " + e.Version + "..");
        }

        private void Updater_ReleaseDownloadFailed(object? sender, ReleaseEventArgs e)
        {
            Hide();
            MessageBox.Show("Checking for new release failed! Please check your internet-connection and try again. " + e.Message);
            Close();
            return;
        }

        private void Updater_ReleaseDownloadProgressed(object? sender, DownloadProgressChangedEventArgs e)
        {
            SetWait(true);
        }

        private void Updater_ReleaseInstallInitialized(object? sender, ReleaseEventArgs e)
        {
            Close();
        }



        private void ProfileManager_AppDownloadStarted(object? sender, AppEventArgs e)
        {
            SetWait(true, "Downloading " + e.App.Name + "..");
        }

        private void ProfileManager_AppDownloadFinished(object? sender, AppEventArgs e)
        {
            SetWait(false);
        }

        private void ProfileManager_AppDownloadFailed(object? sender, AppEventArgs e)
        {
            SetWait(false, "Download " + e.App.Name + " failed. Please check your internet-connection and try again. " + e.Message);
        }

        private void ProfileManager_AppDownloadProgressed(object? sender, DownloadProgressChangedEventArgs e)
        {
            SetWait(true);
        }

        private void ProfileManager_AppInstallStarted(object? sender, AppEventArgs e)
        {
            SetWait(true, "Installing " + e.App.Name + "..");
        }

        private void ProfileManager_AppInstallFinished(object? sender, AppEventArgs e)
        {
            SetWait(false);
        }

        private void ProfileManager_AppInstallFailed(object? sender, AppEventArgs e)
        {
            SetWait(false, "Install " + e.App.Name + " failed. " + e.Message);
        }

        private void ProfileManager_AppConfigurationRequired(object? sender, AppEventArgs e)
        {
            MessageBox.Show(e.Message);
            new SettingsWindow(profileManager, e.App).ShowDialog();
            RunSelectedProfile();
        }


        private void RunSelectedProfile()
        {
            try
            {
                scroller.ScrollToTop();
                SetWait(true, "Starting profile ..");
                if (ProfileManager.RunProfile(selectedProfile)) WindowState = WindowState.Minimized;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error ocurred: " + ex.Message);
            }
            finally
            {
                SetWait(false);
            }
        }

        private void SetWait(bool wait, string waitingText = "")
        {
            string waitingMessage = String.IsNullOrEmpty(waitingText) ? WaitingText.Text : waitingText;

            if (wait)
            {
                Opacity = 0.5;
                GridMain.IsEnabled = false;
                Waiting.Visibility = Visibility.Visible;
                WaitingText.Visibility = Visibility.Visible;
            }
            else
            {
                Opacity = 1.0;
                GridMain.IsEnabled = true;
                Waiting.Visibility = Visibility.Hidden;
                WaitingText.Visibility = String.IsNullOrEmpty(waitingText) ? Visibility.Hidden : Visibility.Visible;
            }
            WaitingText.Text = waitingMessage;
        }

        private void RenderProfiles()
        {
            Comboboxportal.Items.Clear();
            ComboBoxItem lastItemTaggedForStart = null;
            var profiles = profileManager.GetProfiles();
            if(profiles.Count == 0)
            {
                MessageBox.Show($"No profiles available.");
                Environment.Exit(1);
            }
            foreach (var profile in profiles)
            {
                var comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = profile.Name;
                comboBoxItem.Tag = profile;         
                Comboboxportal.Items.Add(comboBoxItem);
                if (profile.IsTaggedForStart) lastItemTaggedForStart = comboBoxItem;  
            }
            if(lastItemTaggedForStart != null) Comboboxportal.SelectedItem = lastItemTaggedForStart;
            RenderProfile();

            if (Settings.Default.start_profile_on_start) RunSelectedProfile();
        }

        private void RenderProfile()
        {
            if (selectedProfile == null) return;

            foreach (var e in selectedProfileElements) GridMain.Children.Remove(e);
            selectedProfileElements.Clear();

            var startMargin = Comboboxportal.Margin;
            int top = 25;
            int counter = 1;

            foreach (var app in selectedProfile.Apps.OrderByDescending(a => a.Value.TaggedForStart))
            {
                var marginTop = counter * top + 10;
                selectedProfile.Apps.TryGetValue(app.Key, out ProfileState? appProfile);
                var nextMargin = new Thickness(startMargin.Left, startMargin.Top + marginTop, startMargin.Right, startMargin.Bottom);

                var imageConfiguration = new Image();
                imageConfiguration.HorizontalAlignment = HorizontalAlignment.Left;
                imageConfiguration.Width = 18;
                imageConfiguration.Height = 18;
                imageConfiguration.Source = new BitmapImage(new Uri("pack://application:,,,/images/configuration.png"));

                var buttonConfiguration = new Button();
                buttonConfiguration.Margin = new Thickness(nextMargin.Left, nextMargin.Top, nextMargin.Right, nextMargin.Bottom);
                buttonConfiguration.Style = (Style)GridMain.Resources["BtnStyle"];
                buttonConfiguration.Content = imageConfiguration;
                buttonConfiguration.HorizontalAlignment = HorizontalAlignment.Left;
                buttonConfiguration.VerticalAlignment = VerticalAlignment.Top;
                buttonConfiguration.Background = Brushes.Transparent;
                buttonConfiguration.BorderThickness = new Thickness(0);
                buttonConfiguration.IsEnabled = appProfile.App.IsConfigurable() || appProfile.App.IsInstallable();

                buttonConfiguration.Click += (s, e) =>
                {
                    new SettingsWindow(profileManager, app.Value.App).ShowDialog();
                    scroller.ScrollToTop();
                };
                GridMain.Children.Add(buttonConfiguration);
                selectedProfileElements.Add(buttonConfiguration);

                var checkBoxTagger = new CheckBox();
                checkBoxTagger.Margin = new Thickness(nextMargin.Left + 25, nextMargin.Top + 2, nextMargin.Right, nextMargin.Bottom);
                checkBoxTagger.Content = appProfile.App.Name;
                checkBoxTagger.HorizontalAlignment = HorizontalAlignment.Left;
                checkBoxTagger.VerticalAlignment = VerticalAlignment.Top;
                checkBoxTagger.DataContext = appProfile;
                checkBoxTagger.SetBinding(CheckBox.IsCheckedProperty, new Binding("TaggedForStart"));
                checkBoxTagger.IsEnabled = !appProfile.IsRequired;
                checkBoxTagger.Foreground = appProfile.TaggedForStart ? Brushes.White : Brushes.Gray;
                checkBoxTagger.FontWeight = appProfile.TaggedForStart ? FontWeights.Bold : FontWeights.Normal;
                checkBoxTagger.Checked += (s, e) =>
                {
                    checkBoxTagger.Foreground = Brushes.White;
                    checkBoxTagger.FontWeight = FontWeights.Bold;
                };
                checkBoxTagger.Unchecked += (s, e) =>
                {
                    checkBoxTagger.Foreground = Brushes.Gray;
                    checkBoxTagger.FontWeight = FontWeights.Normal;
                };
                if (!String.IsNullOrEmpty(appProfile.App.DescriptionShort)) checkBoxTagger.ToolTip = appProfile.App.DescriptionShort;
                GridMain.Children.Add(checkBoxTagger);
                selectedProfileElements.Add(checkBoxTagger);

                counter += 1;
            }


        }


    }
}
