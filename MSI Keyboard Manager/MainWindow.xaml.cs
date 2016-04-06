using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ComboBox = System.Windows.Controls.ComboBox;
using MessageBox = System.Windows.MessageBox;

namespace MSI_Keyboard_Manager
{
    public partial class MainWindow
    {
        private HidManager _hidManager;
        private RegionSetting _leftRegionSetting;
        private RegionSetting _middleRegionSetting;
        private RegionSetting _rightRegionSetting;
        private Constants.Modes _mode = Constants.Modes.Normal;
        private int _speed;
        private bool[] _isPrimaryPage = {true, true, true};

        private void MainWindow_OnInitialized(object sender, EventArgs e)
        {
            //Setup the comboBox with the modes from the enum
            foreach (string mode in Enum.GetNames(typeof (Constants.Modes)))
            {
                ComboBoxMode.Items.Add(mode);
            }
            ComboBoxMode.SelectedIndex = 0;
            ComboBoxMode.SelectionChanged += OnModeChanged;

            _leftRegionSetting = new RegionSetting(Constants.Regions.Left, Constants.Intensities.High,
                Constants.Colors.Blue, Constants.Colors.Red);
            _middleRegionSetting = new RegionSetting(Constants.Regions.Middle, Constants.Intensities.High,
                Constants.Colors.Blue, Constants.Colors.Red);
            _rightRegionSetting = new RegionSetting(Constants.Regions.Right, Constants.Intensities.High,
                Constants.Colors.Blue, Constants.Colors.Red);

            _hidManager = new HidManager();

            //System tray icon setup
            TrayCommand tray = new TrayCommand(this);
            NotifyIcon.LeftClickCommand = tray;
            NotifyIcon.DoubleClickCommand = tray;
        }

        #region Events

        private void TrayOpenClick(object sender, RoutedEventArgs e)
        {
            NotifyIcon.Visibility = Visibility.Collapsed;
            Visibility = Visibility.Visible;
        }

        private void TrayQuitClick(object sender, RoutedEventArgs e)
        {
            NotifyIcon.Dispose();
            Application.Current.Shutdown();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            NotifyIcon.Visibility = Visibility.Visible;
            Hide();
        }

        private void ColorButtonClick(object sender, RoutedEventArgs e)
        {
            Constants.Regions region;
            string buttonName = ((Button) sender).Name;

            //Get region according to button name
            if (buttonName.Contains("Left")) region = Constants.Regions.Left;
            else if (buttonName.Contains("Middle")) region = Constants.Regions.Middle;
            else region = Constants.Regions.Right;


            //Get color
            Constants.Colors color = (from c in Enum.GetNames(typeof (Constants.Colors))
                where buttonName.Contains(c)
                select (Constants.Colors) Enum.Parse(typeof (Constants.Colors), c)).FirstOrDefault();

            //Get intensity
            Constants.Intensities intensity = (from i in Enum.GetNames(typeof (Constants.Intensities))
                where buttonName.Contains(i)
                select (Constants.Intensities) Enum.Parse(typeof (Constants.Intensities), i)).FirstOrDefault();

            //Get primary or secondary according to array
            bool isPrimary = _isPrimaryPage[(int) region - 1];

            //Store it
            switch (region)
            {
                case Constants.Regions.Left:
                    _leftRegionSetting.Intensity = intensity;
                    if (isPrimary) _leftRegionSetting.PrimaryColor = color;
                    else _leftRegionSetting.SecondaryColor = color;
                    break;
                case Constants.Regions.Middle:
                    _middleRegionSetting.Intensity = intensity;
                    if (isPrimary) _middleRegionSetting.PrimaryColor = color;
                    else _middleRegionSetting.SecondaryColor = color;
                    break;
                case Constants.Regions.Right:
                    _rightRegionSetting.Intensity = intensity;
                    if (isPrimary) _rightRegionSetting.PrimaryColor = color;
                    else _rightRegionSetting.SecondaryColor = color;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            TransferSettingsToKeyboard();
        }

        private void OnModeChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetPagesAndButtons();
            if (Enum.TryParse(((ComboBox) sender).SelectedItem.ToString(), out _mode))
            {
                //Display speed or not
                if (_mode == Constants.Modes.Normal || _mode == Constants.Modes.Gaming || _mode == Constants.Modes.Audio)
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
                if (_mode == Constants.Modes.Wave || _mode == Constants.Modes.Breathe ||
                    _mode == Constants.Modes.DualColor)
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

                //Display colors or not
                if (_mode == Constants.Modes.Audio)
                {
                    GroupBoxLeft.Visibility = Visibility.Hidden;
                    GroupBoxMiddle.Visibility = Visibility.Hidden;
                    GroupBoxRight.Visibility = Visibility.Hidden;
                }
                else if (_mode == Constants.Modes.Gaming)
                {
                    GroupBoxLeft.Visibility = Visibility.Visible;
                    GroupBoxMiddle.Visibility = Visibility.Hidden;
                    GroupBoxRight.Visibility = Visibility.Hidden;
                }
                else
                {
                    GroupBoxLeft.Visibility = Visibility.Visible;
                    GroupBoxMiddle.Visibility = Visibility.Visible;
                    GroupBoxRight.Visibility = Visibility.Visible;
                }

                TransferSettingsToKeyboard();
            }
            else
            {
                ShowAlert("Invalid mode selected");
            }
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

        private void OnSpeedChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(TextBoxSpeed.Text, out _speed)) return;
            _speed = 5;
            TransferSettingsToKeyboard();
        }

        #endregion

        #region Utils & misc

        private void ResetPagesAndButtons()
        {
            _isPrimaryPage = new[] {true, true, true};
            GroupBoxLeft.Header = "Left (primary)";
            ButtonPageLeft.Content = "Secondary color";
            GroupBoxMiddle.Header = "Middle (primary)";
            ButtonPageMiddle.Content = "Secondary color";
            GroupBoxRight.Header = "Right (primary)";
            ButtonPageRight.Content = "Secondary color";
        }

        private void ShowAlert(string message)
        {
            MessageBox.Show(message, "Error !", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void TransferSettingsToKeyboard()
        {
            _hidManager.SetMode(_mode, _leftRegionSetting, _middleRegionSetting, _rightRegionSetting, _speed);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        #endregion
    }
}