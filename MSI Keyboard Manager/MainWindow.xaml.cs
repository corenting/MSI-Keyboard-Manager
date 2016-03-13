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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MSI_Keyboard_Manager
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HidManager _hidManager;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnInitialized(object sender, EventArgs e)
        {
            //Setup the comboBox with the modes from the enum
            foreach (string mode in Enum.GetNames(typeof(Constants.Modes)))
            {
                ComboBoxMode.Items.Add(mode);
            }
            ComboBoxMode.SelectedIndex = 0;


            _hidManager = new HidManager();
            var normal = new Tuple<Constants.Colors, Constants.Colors>(Constants.Colors.Blue, Constants.Colors.Red);
            var inverted = new Tuple<Constants.Colors, Constants.Colors>(Constants.Colors.Red, Constants.Colors.Blue);
            _hidManager.SetComplexMode(Constants.Modes.DualColor, Constants.Intensities.High, normal, inverted, normal, 2);
        }
    }
}
