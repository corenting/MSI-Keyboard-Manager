using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace MSI_Keyboard_Manager
{
    class Utils
    {
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield break;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                if (child is T)
                {
                    yield return (T) child;
                }

                foreach (T childOfChild in FindVisualChildren<T>(child))
                {
                    yield return childOfChild;
                }
            }
        }

        public static void ShowAlert(string message)
        {
            MessageBox.Show(message, "Error !", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static string GetButtonName(bool isPrimaryPage, RegionSetting regionSetting)
        {
            string name = "Button" +
                          (isPrimaryPage
                              ? Enum.GetName(typeof (Constants.Colors), regionSetting.PrimaryColor)
                              : Enum.GetName(typeof (Constants.Colors), regionSetting.SecondaryColor)) +
                          Enum.GetName(typeof (Constants.Intensities), regionSetting.Intensity);
            switch (regionSetting.Region)
            {
                case Constants.Regions.Left:
                    name += "Left";
                    break;
                case Constants.Regions.Middle:
                    name += "Middle";
                    break;
                case Constants.Regions.Right:
                    name += "Right";
                    break;
            }


            if (name.Contains("Off") || name.Contains(("White")))
            {
                name = name.Replace(
                    Enum.GetName(typeof (Constants.Intensities), regionSetting.Intensity), "");
            }

            return name;
        }
    }
}