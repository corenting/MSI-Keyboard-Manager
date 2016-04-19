using System;
using System.Linq;
using System.Windows;
using HidSharp;

namespace MSI_Keyboard_Manager
{
    internal class HidManager
    {
        private readonly HidStream _stream;

        public HidManager()
        {
            try
            {
                var loader = new HidDeviceLoader();
                var device = loader.GetDevices(0x1770, 0xff00).First();
                device.TryOpen(out _stream);
            }
            catch
            {
                MessageBox.Show("Error while loading the USB device, the program will now quit.", "Error !",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                Environment.Exit(1);
            }
        }

        public void SetMode(Constants.Modes mode, RegionSetting left, RegionSetting middle, RegionSetting right,
            int speed)
        {
            switch (mode)
            {
                case Constants.Modes.Normal:
                    SendCommand(66, (byte) left.Region, (byte) left.PrimaryColor, (byte) left.Intensity);
                    SendCommand(66, (byte) middle.Region, (byte) middle.PrimaryColor, (byte) middle.Intensity);
                    SendCommand(66, (byte) right.Region, (byte) right.PrimaryColor, (byte) right.Intensity);
                    break;
                case Constants.Modes.Gaming:
                    SendCommand(66, (byte) left.Region, (byte) left.PrimaryColor, (byte) left.Intensity);
                    break;
                case Constants.Modes.Wave:
                case Constants.Modes.DualColor:
                case Constants.Modes.Breathe:
                    SendCommandForDualModeRegion(Constants.Regions.Left, (byte) left.PrimaryColor,
                        (byte) left.SecondaryColor,
                        (byte) left.Intensity, (byte) speed);
                    SendCommandForDualModeRegion(Constants.Regions.Middle, (byte) middle.PrimaryColor,
                        (byte) middle.SecondaryColor,
                        (byte) middle.Intensity, (byte) speed);
                    SendCommandForDualModeRegion(Constants.Regions.Right, (byte) right.PrimaryColor,
                        (byte) right.SecondaryColor,
                        (byte) right.Intensity, (byte) speed);
                    break;
            }
            SendCommand(65, (byte) mode, 0, 0);
        }

        private void SendCommandForDualModeRegion(Constants.Regions region, byte firstColor, byte secondColor,
            byte intensity, byte speed)
        {
            int modifier;
            switch (region)
            {
                case Constants.Regions.Left:
                    modifier = 0;
                    break;
                case Constants.Regions.Middle:
                    modifier = 3;
                    break;
                case Constants.Regions.Right:
                    modifier = 6;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(region), region, null);
            }
            SendCommand(67, (byte) (Constants.Regions.Left + modifier), firstColor, intensity);
            SendCommand(67, (byte) (Constants.Regions.Middle + modifier), secondColor, intensity);
            SendCommand(67, (byte) (Constants.Regions.Right + modifier), speed, speed, speed);
        }

        private void SendCommand(byte command, byte areaOrMode, byte color, byte intensity, byte last = 0)
        {
            byte[] array = {1, 2, command, areaOrMode, color, intensity, last, 236};
            _stream.SetFeature(array);
        }

        public void Close()
        {
            _stream.Close();
        }
    }
}