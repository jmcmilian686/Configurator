using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Configurator
{
    public partial class Rfid : Form
    {
        public string rfidString { get; set; }
        public string TheValue
        {
            get { return rfidString; }
        }

        public Rfid(string rfid)
        {
            this.rfidString = rfid;
            InitializeComponent();
           
        }

        private void Rfid_Load(object sender, EventArgs e)
        {
            label3.Text = rfidString;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (textBox1.Text!="")
            {
                rfidString = textBox1.Text;
                label3.Text = textBox1.Text;
                
            }
            this.Close();
        }
    }
}
