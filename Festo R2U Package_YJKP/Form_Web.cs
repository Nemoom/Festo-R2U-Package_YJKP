using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Festo_R2U_Package_YJKP
{
    public partial class Form_Web : Form
    {
        public Form_Web()
        {
            InitializeComponent();
        }

        private void Form_Web_Load(object sender, EventArgs e)
        {
            webKitBrowser1.Navigate("http://172.16.141.56:8080/servo_press_kit.htm");
        }
    }
}
