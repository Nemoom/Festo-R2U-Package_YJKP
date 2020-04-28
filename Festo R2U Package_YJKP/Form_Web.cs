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
        string URL;
        public Form_Web(string WebLink)
        {
            InitializeComponent();
            URL = WebLink;
        }

        private void Form_Web_Load(object sender, EventArgs e)
        {
            webKitBrowser1.Navigate(URL);
        }
    }
}
