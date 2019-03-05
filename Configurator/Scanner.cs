using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Configurator
{
    public partial class Scanner : Form
    {
        public string scanDirectory { get; set; }

        public List<string> simpConf = new List<string> { "robot_server_right ", "scanner_right_hid ", "matrix_code_2 ", "matrix_code_1 ", "robot_server_left " };

        public Scanner()
        {
            InitializeComponent();
        }
        public Scanner(string dir) {

          
            InitializeComponent();
            this.scanDirectory = dir;

        }
        private void Scanner_Load(object sender, EventArgs e)
        {
            if (scanDirectory != "") {
               
                string[] lines = System.IO.File.ReadAllLines(@scanDirectory+ "\\config.txt");
                int counter = lines.Count();
                TextBox[] textboxes = new TextBox[counter];
                Label[] labels = new Label[counter];
                int i = 0;
                int locationL = 25;
                int locationT = 19;


                foreach (var lns in lines) {

                    string[] stsplit = lns.Split(Convert.ToChar("="));

                    if (stsplit[0].StartsWith(";"))
                    {

                        labels[i] = new Label();
                        labels[i].Text = stsplit[0];
                        labels[i].Visible = false;

                        labels[i].Location = new Point(21, locationL);
                        labels[i].Size = new Size(250, 20);

                        textboxes[i] = new TextBox();

                        textboxes[i].Visible = false;

                        textboxes[i].Text = stsplit[1];
                        textboxes[i].Location = new Point(280, locationT);
                        textboxes[i].Size = new Size(242, 26);
                        panel1.Controls.Add(labels[i]);
                        panel1.Controls.Add(textboxes[i]);

                    }
                    else
                    {

                        labels[i] = new Label();
                        textboxes[i] = new TextBox();
                        labels[i].Text = stsplit[0];

                        labels[i].Location = new Point(21, locationL);
                        labels[i].Size = new Size(250, 20);
                                             

                        textboxes[i].Text = stsplit[1];
                        textboxes[i].Location = new Point(280, locationT);
                        textboxes[i].Size = new Size(242, 26);

                        if (simpConf.Contains(stsplit[0]))
                        {

                            labels[i].Visible = true;
                            textboxes[i].Visible = true;
                            locationL += 47;
                            locationT += 47;
                        }
                        else {

                            labels[i].Visible = false;
                            textboxes[i].Visible = false;
                        }

                        panel1.Controls.Add(labels[i]);
                        panel1.Controls.Add(textboxes[i]);

                    }
                }


            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {

                int locationL = 25;
                int locationT = 19;
                Boolean comment = false;
                foreach (Control ctrl in panel1.Controls)
                {
                    if (ctrl.GetType() == typeof(Label))
                    {
                        if (ctrl.Text.StartsWith(";"))
                        {
                            comment = true;
                        }
                        else
                        {
                            ctrl.Location = new Point(21, locationL);
                            ctrl.Visible = true;
                            locationL += 47;
                        }

                    }
                    else if (ctrl.GetType() == typeof(TextBox))
                    {
                        if (!comment)
                        {
                            ctrl.Location = new Point(280, locationT);
                            ctrl.Visible = true;
                            locationT += 47;
                        }
                        else
                        {
                            comment = false;
                        }

                    }

                }

                this.panel1.Update();

            }
            else {

                int locationL = 25;
                int locationT = 19;
                Boolean found = false;
                foreach (Control ctrl in panel1.Controls)
                {
                    if (ctrl.GetType() == typeof(Label) && simpConf.Contains(ctrl.Text))
                    {
                        found = true;
                        ctrl.Location = new Point(21, locationL);
                        ctrl.Visible = true;
                        locationL += 47;

                    }
                    else if (ctrl.GetType() == typeof(TextBox) && found)
                    {

                        ctrl.Location = new Point(280, locationT);
                        ctrl.Visible = true;
                        locationT += 47;
                        found = false;
                    }
                    else
                    {
                        ctrl.Visible = false;
                    }

                }

                this.panel1.Update();


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string key = "";
            string value = "";
            Boolean cycle = false;
            foreach (Control ctrl in panel1.Controls) {

                if (ctrl.GetType() == typeof(Label))
                {
                    
                    key = ctrl.Text;
                }
                else if (ctrl.GetType() == typeof(TextBox))
                {
                    value = ctrl.Text;
                    cycle = true;
                }

                if (cycle) {

                    dict.Add(key, value);
                    cycle = false;
                }

            }

            if (dict.Count > 0) {
                string[] wrtfile = new string[dict.Count];
                int counter = 0;
                foreach (var elem in dict)
                {
                    string line = elem.Key + "=" + elem.Value;
                    wrtfile[counter++] = line;
                }

                using (StreamWriter outputFile = new StreamWriter(@scanDirectory + "\\config.txt"))
                {
                    foreach (string line in wrtfile)
                        outputFile.WriteLine(line);

                    string message = "The Scanner Configuration was updated";
                    string caption = "Update";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result;

                    // Displays the MessageBox.

                    result = MessageBox.Show(message, caption, buttons);
                }
            }

        }
    }
}
