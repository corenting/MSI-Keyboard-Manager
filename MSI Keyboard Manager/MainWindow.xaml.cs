using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ComboBox = System.Windows.Controls.ComboBox;
using MessageBox = System.Windows.MessageBox;

namespace MSI_Keyboard_Manager
{
    public partial class MainWindow : Window
    {
        private HidManager _hidManager;
        private Constants.Colors[] _leftColors = {Constants.Colors.Off, Constants.Colors.Off, Constants.Colors.Off};
        private Constants.Colors[] _middleColors = {Constants.Colors.Off, Constants.Colors.Off, Constants.Colors.Off};
        private Constants.Colors[] _rightColors = {Constants.Colors.Off, Constants.Colors.Off, Constants.Colors.Off};
        private bool[] _isPrimaryPage = {true, true, true};

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnInitialized(object sender, EventArgs e)
        {
            //Setup the comboBox with the modes from the enum
            foreach (string mode in Enum.GetNames(typeof (Constants.Modes)))
            {
                ComboBoxMode.Items.Add(mode);
            }
            ComboBoxMode.SelectedIndex = 0;
            ComboBoxMode.SelectionChanged += OnModeChanged;
            _hidManager = new HidManager();
        }

        private void ColorButtonClick(object sender, RoutedEventArgs e)
        {
            Constants.Regions region;

            //Get region according to button name
            string buttonName = ((Button) sender).Name;
            if (buttonName.Contains("Left")) region = Constants.Regions.Left;
            else if (buttonName.Contains("Middle")) region = Constants.Regions.Middle;
            else region = Constants.Regions.Right;


        }

        private void OnModeChanged(object sender, SelectionChangedEventArgs e)
        {
            Constants.Modes mode;
            if (Enum.TryParse(((ComboBox) sender).SelectedItem.ToString(), out mode))
            {
                //Display speed or not
                if (mode == Constants.Modes.Normal || mode == Constants.Modes.Gaming || mode == Constants.Modes.Audio)
                {
                    LabelSpeed.Visibility = Visibility.Hidden;
                    TextBoxSpeed.Visibility = Visibility.Hidden;
                }
                else
                {
                    LabelSpeed.Visibility = Visibility.Visible;
                    TextBoxSpeed.Visibility = Visibility.Visible;
                }

                //Display page button or not
                if (mode == Constants.Modes.Wave || mode == Constants.Modes.Breathe || mode == Constants.Modes.DualColor)
                {
                    ButtonPageLeft.Visibility = Visibility.Visible;
                    ButtonPageMiddle.Visibility = Visibility.Visible;
                    ButtonPageRight.Visibility = Visibility.Visible;
                }
                else
                {
                    ButtonPageLeft.Visibility = Visibility.Hidden;
                    ButtonPageMiddle.Visibility = Visibility.Hidden;
                    ButtonPageRight.Visibility = Visibility.Hidden;
                }

                //Display middle and right or not
                if (mode == Constants.Modes.Gaming)
                {
                    GroupBoxMiddle.Visibility = Visibility.Hidden;
                    GroupBoxRight.Visibility = Visibility.Hidden;
                }
                else
                {
                    GroupBoxMiddle.Visibility = Visibility.Visible;
                    GroupBoxRight.Visibility = Visibility.Visible;
                }
            }
            else
            {
                ShowAlert("Invalid mode selected");
            }
        }

        private void ShowAlert(string message)
        {
            MessageBox.Show(message, "Error !", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void OnPageChanged(object sender, RoutedEventArgs e)
        {
            string name = ((Button) sender).Name;
            if (name.Contains("Left"))
            {
                _isPrimaryPage[0] = !_isPrimaryPage[0];
                if (_isPrimaryPage[0])
                {
                    GroupBoxLeft.Header = "Left (primary)";
                    ButtonPageLeft.Content = "Secondary color";
                }
                else
                {
                    GroupBoxLeft.Header = "Left (secondary)";
                    ButtonPageLeft.Content = "Primary color";
                }
            }
            else if (name.Contains("Middle"))
            {
                _isPrimaryPage[1] = !_isPrimaryPage[1];
                if (_isPrimaryPage[1])
                {
                    GroupBoxMiddle.Header = "Middle (primary)";
                    ButtonPageMiddle.Content = "Secondary color";
                }
                else
                {
                    GroupBoxMiddle.Header = "Middle (secondary)";
                    ButtonPageMiddle.Content = "Primary color";
                }
            }
            else
            {
                _isPrimaryPage[2] = !_isPrimaryPage[2];
                if (_isPrimaryPage[2])
                {
                    GroupBoxRight.Header = "Right (primary)";
                    ButtonPageRight.Content = "Secondary color";
                }
                else
                {
                    GroupBoxRight.Header = "Right (secondary)";
                    ButtonPageRight.Content = "Primary color";
                }
            }
        }
    }
}