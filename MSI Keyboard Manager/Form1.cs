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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var loader = new HidDeviceLoader();
            var device = loader.GetDevices(0x1770, 0xff00).First();
            HidStream stream;
            device.TryOpen(out stream);
            byte[] array = {1, 2, 66, 1, (int) Constants.Colors.Red, 3, 0, 236};
            stream.SetFeature(array);
        }
    }
}
