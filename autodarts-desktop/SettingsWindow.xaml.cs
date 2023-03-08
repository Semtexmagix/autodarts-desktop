using autodarts_desktop.control;
using autodarts_desktop.model;
using Ookii.Dialogs.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using Brushes = System.Windows.Media.Brushes;
using Microsoft.Win32;
using Binding = System.Windows.Data.Binding;
using TextBox = System.Windows.Controls.TextBox;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using Xceed.Wpf.Toolkit;

namespace autodarts_desktop
{
    /// <summary>
    /// Interaktionslogik für Settings.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        // ATTRIBUTES

        private ProfileManager profileManager;
        private AppBase app;
        private double fontSize;
        private System.Windows.Media.Brush fontColor;
        private int marginTop;
        private int elementWidth;
        private double elementOffsetRight;
        private double elementOffsetLeft;
        private double elementClearedOpacity;
        private HorizontalAlignment elementHoAl;
        



        // METHODES

        public SettingsWindow(ProfileManager profileManager, AppBase app)
        {
            InitializeComponent();

            this.profileManager = profileManager;
            this.app = app;

            fontSize = 22.0;
            fontColor = Brushes.White;
            marginTop = (int)fontSize + 14;
            elementWidth = (int)(Width * 0.80);
            elementHoAl = HorizontalAlignment.Left;
            elementOffsetRight = 0.0;
            elementOffsetLeft = 20.0;
            elementClearedOpacity = 0.3;
            Title = "Configuration - " + this.app.Name;

            RenderAppConfiguration();
        }



        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FocusManager.SetFocusedElement(this, this);   

            try
            {
                profileManager.StoreApps();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error occured: " + ex.Message);
            }
        }


        private void RenderAppConfiguration()
        {
            var labelHeader = new Label();
            labelHeader.Content = app.Name;
            labelHeader.HorizontalAlignment = HorizontalAlignment.Center;
            labelHeader.VerticalAlignment = VerticalAlignment.Top;
            labelHeader.FontSize = fontSize;
            labelHeader.FontWeight = FontWeights.ExtraBold;
            labelHeader.Margin = new Thickness(elementOffsetLeft, 24, elementOffsetRight, 0);
            labelHeader.Foreground = fontColor;
            GridMain.Children.Add(labelHeader);

            if (!String.IsNullOrEmpty(app.HelpUrl))
            {
                var ímageHelp = new Image();
                ímageHelp.Width = 24;
                ímageHelp.Height = 24;
                ímageHelp.Source = new BitmapImage(new Uri("pack://application:,,,/images/help.png"));

                var buttonHelp = new Button();
                buttonHelp.Margin = new Thickness(0, 10, 10, 0);
                buttonHelp.Style = (Style)GridMain.Resources["BtnStyle"];
                buttonHelp.Content = ímageHelp;
                buttonHelp.HorizontalAlignment = HorizontalAlignment.Right;
                buttonHelp.VerticalAlignment = VerticalAlignment.Top;
                buttonHelp.FontSize = fontSize;
                buttonHelp.Background = Brushes.Transparent;
                buttonHelp.BorderThickness = new Thickness(0, 0, 0, 0);
                buttonHelp.Click += (s, e) =>
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo(app.HelpUrl)
                        {
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show("Error: " + ex.Message);
                    }
                };
                GridMain.Children.Add(buttonHelp);
            }



            if (!app.IsConfigurable()) return;

            var appConfiguration = app.Configuration;
            var argumentsBySection = appConfiguration.Arguments.GroupBy(a => a.Section);

            int counter = 1;
            foreach (var section in argumentsBySection)
            {
                counter += 2;

                if (!String.IsNullOrEmpty(section.Key))
                {
                    var textBlockSectionHeader = new TextBlock();
                    textBlockSectionHeader.Text = section.Key;
                    textBlockSectionHeader.HorizontalAlignment = HorizontalAlignment.Center;
                    textBlockSectionHeader.VerticalAlignment = VerticalAlignment.Top;
                    textBlockSectionHeader.FontSize = fontSize - 3;
                    textBlockSectionHeader.FontWeight = FontWeights.Bold;
                    textBlockSectionHeader.Margin = new Thickness(elementOffsetLeft, counter * marginTop, elementOffsetRight, 0);
                    textBlockSectionHeader.Foreground = fontColor;
                    textBlockSectionHeader.TextDecorations = TextDecorations.Underline;
                    GridMain.Children.Add(textBlockSectionHeader);
                }

                foreach (var argument in section)
                {
                    if (argument.IsRuntimeArgument) continue;
                    
                    var borderColor = Brushes.Transparent;
                    var borderThickness = new Thickness(1);
                    if (argument.Required && 
                        (String.IsNullOrEmpty(argument.Value) && !argument.EmptyAllowedOnRequired) ||
                        (argument.Value == null && argument.EmptyAllowedOnRequired))
                    {
                        borderColor = Brushes.Red;
                        borderThickness = new Thickness(3);
                    }

                    counter += 1;

                    string type = argument.GetTypeClear();

                    if (type == Argument.TypeString ||
                        type == Argument.TypePassword ||
                        type == Argument.TypeFloat ||
                        type == Argument.TypeInt ||
                        type == Argument.TypeFile ||
                        type == Argument.TypePath ||
                        type == Argument.TypeSelection
                        )
                    {
                        var textBlock = new TextBlock();
                        textBlock.Text = argument.NameHuman + (argument.Required ? " * " : "");
                        textBlock.HorizontalAlignment = elementHoAl;
                        textBlock.VerticalAlignment = VerticalAlignment.Top;
                        textBlock.FontSize = fontSize - 6;
                        textBlock.Margin = new Thickness(elementOffsetLeft, counter * marginTop, elementOffsetRight, 0);
                        textBlock.Foreground = fontColor;
                        textBlock.ToolTip = argument.Description;
                        if (argument.Value == null) textBlock.Opacity = elementClearedOpacity;

                        GridMain.Children.Add(textBlock);
                    }

                    counter += 1;



                    FrameworkElement customElement = null;

                    if (type == Argument.TypeString || type == Argument.TypePath || type == Argument.TypeFile)
                    {
                        var textBox = new TextBox();
                        textBox.HorizontalAlignment = elementHoAl;
                        textBox.VerticalAlignment = VerticalAlignment.Top;
                        textBox.FontSize = fontSize - 6;
                        textBox.Margin = new Thickness(elementOffsetLeft, counter * marginTop, elementOffsetRight, 0);
                        textBox.Width = elementWidth;
                        textBox.BorderBrush = borderColor;
                        textBox.BorderThickness = borderThickness;
                        textBox.TextChanged += (s, e) => textBox.Opacity = 1.0;
                        textBox.DataContext = argument;
                        textBox.SetBinding(TextBox.TextProperty, new Binding("Value"));
                        HighlightElement(textBox, argument);
                        customElement = textBox;

                        if (type == Argument.TypePath)
                        {
                            textBox.GotFocus += (s, e) =>
                            {
                                var ookiiDialog = new VistaFolderBrowserDialog();
                                if (ookiiDialog.ShowDialog() == true)
                                {
                                    textBox.Text = ookiiDialog.SelectedPath;
                                }
                            };
                        }
                        else if (type == Argument.TypeFile)
                        {
                            textBox.GotFocus += (s, e) =>
                            {
                                var openFileDialog = new OpenFileDialog();
                                openFileDialog.Title = "Select File";
                                openFileDialog.Filter = "All files (*.*)|*.*|Anwendung (*.exe)|*.exe";
                                openFileDialog.FilterIndex = 2;
                                openFileDialog.ShowDialog();
                                textBox.Text = openFileDialog.FileName;
                            };
                        }

                        GridMain.Children.Add(textBox);
                    }
                    else if (type == Argument.TypePassword)
                    {
                        var passwordBox = new TextBox();
                        passwordBox.HorizontalAlignment = elementHoAl;
                        passwordBox.VerticalAlignment = VerticalAlignment.Top;
                        passwordBox.FontSize = fontSize - 6;
                        passwordBox.Margin = new Thickness(elementOffsetLeft, counter * marginTop, elementOffsetRight, 0);
                        passwordBox.Width = elementWidth;
                        passwordBox.BorderBrush = borderColor;
                        passwordBox.BorderThickness = borderThickness;
                        passwordBox.TextChanged += (s, e) => passwordBox.Opacity = 1.0;
                        passwordBox.DataContext = argument;
                        passwordBox.SetBinding(TextBox.TextProperty, new Binding("Value"));
                        HighlightElement(passwordBox, argument);
                        GridMain.Children.Add(passwordBox);
                        customElement = passwordBox;
                    }
                    else if (type == Argument.TypeFloat || type == Argument.TypeInt)
                    {
                        if (!String.IsNullOrEmpty(argument.RangeBy))
                        {
                            var textBoxSlider = new TextBox();
                            textBoxSlider.HorizontalAlignment = elementHoAl;
                            textBoxSlider.VerticalAlignment = VerticalAlignment.Top;
                            textBoxSlider.FontSize = fontSize - 6;
                            textBoxSlider.Margin = new Thickness(elementOffsetLeft, counter * marginTop, elementOffsetRight, 0);
                            textBoxSlider.Width = elementWidth;
                            textBoxSlider.MaxLength = 5;
                            textBoxSlider.IsEnabled = false;
                            textBoxSlider.TextChanged += (s, e) => textBoxSlider.Opacity = 1.0;
                            textBoxSlider.DataContext = argument;
                            textBoxSlider.SetBinding(TextBox.TextProperty, new Binding("Value"));

                            counter += 1;

                            var slider = new Slider();
                            slider.HorizontalAlignment = elementHoAl;
                            slider.VerticalAlignment = VerticalAlignment.Top;
                            slider.FontSize = fontSize - 6;
                            slider.Margin = new Thickness(elementOffsetLeft, counter * marginTop, elementOffsetRight, 0);
                            slider.Width = elementWidth;
                            slider.BorderBrush = borderColor;
                            slider.BorderThickness = borderThickness;
                            slider.IsSnapToTickEnabled = true;
                            slider.ValueChanged += (s, e) => slider.Opacity = 1.0;
                            slider.Tag = textBoxSlider;

                            if (type == Argument.TypeFloat)
                            {
                                slider.TickFrequency = 0.1;
                                slider.Minimum = Helper.GetDoubleByString(argument.RangeBy);
                                slider.Maximum = Helper.GetDoubleByString(argument.RangeTo);
                            }
                            else if (type == Argument.TypeInt)
                            {
                                slider.TickFrequency = 1.0;
                                slider.Minimum = Helper.GetIntByString(argument.RangeBy);
                                slider.Maximum = Helper.GetIntByString(argument.RangeTo);
                            }
                            slider.DataContext = argument;
                            slider.SetBinding(Slider.ValueProperty, new Binding("Value"));
                            HighlightElement(slider, argument);

                          
                            GridMain.Children.Add(textBoxSlider);
                            GridMain.Children.Add(slider);
                            customElement = slider;
                        }
                        else if(type == Argument.TypeInt)
                        {
                            var integerUpDown = new IntegerUpDown();
                            integerUpDown.HorizontalAlignment = elementHoAl;
                            integerUpDown.VerticalAlignment = VerticalAlignment.Top;
                            integerUpDown.FontSize = fontSize - 6;
                            integerUpDown.Margin = new Thickness(elementOffsetLeft, counter * marginTop, elementOffsetRight, 0);
                            integerUpDown.Width = elementWidth;
                            integerUpDown.BorderBrush = borderColor;
                            integerUpDown.BorderThickness = borderThickness;
                            integerUpDown.ValueChanged += (s, e) => integerUpDown.Opacity = 1.0;
                            integerUpDown.DataContext = argument;
                            integerUpDown.SetBinding(IntegerUpDown.ValueProperty, new Binding("Value"));
                            HighlightElement(integerUpDown, argument);
                            GridMain.Children.Add(integerUpDown);
                            customElement = integerUpDown;
                        }
                        else if (type == Argument.TypeFloat)
                        {
                            var decimalUpDown = new DecimalUpDown();
                            decimalUpDown.HorizontalAlignment = elementHoAl;
                            decimalUpDown.VerticalAlignment = VerticalAlignment.Top;
                            decimalUpDown.FontSize = fontSize - 6;          
                            decimalUpDown.Margin = new Thickness(elementOffsetLeft, counter * marginTop, elementOffsetRight, 0);
                            decimalUpDown.Width = elementWidth;
                            decimalUpDown.BorderBrush = borderColor;
                            decimalUpDown.Increment = (decimal)0.1;
                            decimalUpDown.FormatString = "F1";
                            decimalUpDown.BorderThickness = borderThickness;
                            decimalUpDown.ValueChanged += (s, e) => decimalUpDown.Opacity = 1.0;
                            decimalUpDown.DataContext = argument;
                            decimalUpDown.SetBinding(DecimalUpDown.ValueProperty, new Binding("Value"));
                            HighlightElement(decimalUpDown, argument);
                            GridMain.Children.Add(decimalUpDown);
                            customElement = decimalUpDown;
                        }

                    }
                    else if (type == Argument.TypeBool)
                    {
                        var checkBox = new CheckBox();
                        checkBox.Margin = new Thickness(elementOffsetLeft, counter * marginTop, elementOffsetRight, 0);
                        var checkBoxContent = new TextBox();
                        checkBoxContent.FontSize = fontSize - 6;
                        checkBoxContent.Text = argument.NameHuman;
                        checkBoxContent.Background = Brushes.Transparent;
                        checkBoxContent.Foreground = fontColor;
                        checkBoxContent.BorderThickness = new Thickness(0, 0, 0, 0);
                        checkBoxContent.VerticalAlignment = VerticalAlignment.Center;
                        checkBoxContent.HorizontalAlignment = elementHoAl;
                        checkBoxContent.Margin = new Thickness(0, -4, elementOffsetRight, 0);
                        checkBoxContent.IsReadOnly= true;
                        checkBoxContent.Cursor = Cursors.Arrow;

                        checkBox.Content = checkBoxContent;
                        checkBox.HorizontalAlignment = elementHoAl;
                        checkBox.VerticalAlignment = VerticalAlignment.Top;
                        checkBox.FontSize = fontSize - 6;
                        checkBox.Foreground = Brushes.White;
                        checkBox.BorderBrush = borderColor;
                        checkBox.BorderThickness = borderThickness;
                        checkBox.Checked += (s, e) => checkBox.Opacity = 1.0;
                        checkBox.DataContext = argument;
                        checkBox.SetBinding(CheckBox.IsCheckedProperty, new Binding("Value"));
                        HighlightElement(checkBox, argument);
                        GridMain.Children.Add(checkBox);
                        customElement = checkBox;
                    }
                    else if (type == Argument.TypeSelection)
                    {
                        // TODO..
                    }

                    if (customElement != null)
                    {
                        var imageClear = new Image();
                        imageClear.Width = 24;
                        imageClear.Height = 24;
                        imageClear.Source = new BitmapImage(new Uri("pack://application:,,,/images/clear.png"));

                        var button = new Button();
                        button.Margin = new Thickness(0, counter * marginTop - (imageClear.Height / 6), (imageClear.Width / 2), 0);
                        button.Style = (Style)GridMain.Resources["BtnStyle"];
                        button.Content = imageClear;
                        button.HorizontalAlignment = HorizontalAlignment.Right;
                        button.VerticalAlignment = VerticalAlignment.Top;
                        button.Background = Brushes.Transparent;
                        button.BorderThickness = new Thickness(0, 0, 0, 0);
                        button.Click += (s, e) =>
                        {
                            argument.Value = null;
                            customElement.DataContext = null;
                            customElement.UpdateDefaultStyle();
                            customElement.UpdateLayout();
                            customElement.DataContext = argument;
                            customElement.Opacity = elementClearedOpacity;
                        };
                        if (argument.Value == null)
                        {
                            customElement.Opacity = elementClearedOpacity;
                            if(customElement.Tag != null) { ((FrameworkElement)customElement.Tag).Opacity = elementClearedOpacity; }
                        }
                        GridMain.Children.Add(button);

                    }
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HighlightElement(FrameworkElement element, Argument argument)
        {
            if (app.ArgumentRequired != null && app.ArgumentRequired == argument)
            {
                var offset = element.Margin.Top - 25;
                if(offset < 0) offset = 0;
                scroller.ScrollToVerticalOffset(offset);
            }
        }


    }

}
