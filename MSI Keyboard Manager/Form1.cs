using HidSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSI_Keyboard_Manager
{
    public partial class Form1 : Form
    {
        private HidManager _hidManager;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _hidManager = new HidManager();
            var normal = new Tuple<Constants.Colors, Constants.Colors>(Constants.Colors.Blue, Constants.Colors.Red);
            var inverted = new Tuple<Constants.Colors, Constants.Colors>(Constants.Colors.Red, Constants.Colors.Blue);

            _hidManager.SetDualColorMode(Constants.Intensities.High,normal, inverted, normal, 2);
        }
    }
}
