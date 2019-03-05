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
    public partial class RobotList : Form
    {
        public string serverDirectory { get; set; }
       
        public RobotList(string dir)
        {
            this.serverDirectory = dir;
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void RobotList_Load(object sender, EventArgs e)
        {
            if (serverDirectory != "")
            {

                string[] lines = System.IO.File.ReadAllLines(serverDirectory + "\\carlist.txt");
                
                label3.Text = (lines.Count() -1).ToString();

                
                List<RobotIps> str = new List<RobotIps>();

                foreach (string elem in lines)
                {
                    //str.Add(new RobotIps { ip = elem });
                    dataGridView1.Rows.Add(elem);

                }

                 
                

            }
           

            // maskedTextBox1.Mask = "###.###";
            // maskedTextBox1.ValidatingType = typeof(System.Net.IPAddress);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (maskedTextBox1.Text != "")
            {
                
                string[] insert = maskedTextBox1.Text.Split(Convert.ToChar("."));
                if (insert.Count()>0 && insert[1].StartsWith("0"))
                {
                    insert[1] = insert[1].Substring(1);
                }
                string newaddres = "192.168." + insert[0]+"."+insert[1];

                Boolean found = false;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[0].FormattedValue.ToString() == newaddres)
                    {
                        found = true;
                    }
                }

                if (found)
                {
                    string message = "Robot is already registered";
                    string caption = "Repeated value";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result;

                    // Displays the MessageBox.

                    result = MessageBox.Show(message, caption, buttons);

                }
                else {
                    dataGridView1.Rows.Add(newaddres);
                    label3.Text = (Convert.ToInt32(label3.Text) + 1).ToString();

                   
                }

                
            }
            else {

                string message = "Please complete the Robot Address";
                string caption = "Blank field";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;

                // Displays the MessageBox.

                result = MessageBox.Show(message, caption, buttons);
            }

            


        }



        private void maskedTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count > 0 && this.dataGridView1.SelectedRows[0].Index != this.dataGridView1.Rows.Count - 1 && this.dataGridView1.SelectedRows[0].Index != 0)
            {
                this.dataGridView1.Rows.RemoveAt(
                    this.dataGridView1.SelectedRows[0].Index);
                label3.Text = (Convert.ToInt32(label3.Text) - 1).ToString();
            }
        }

        private void cancBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            string[] savestrs = new string[dataGridView1.Rows.Count -1];
            int counter = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows) {

                if (counter < dataGridView1.Rows.Count -1) {
                    savestrs[counter] = row.Cells[0].FormattedValue.ToString();
                    counter++;
                }
                
                
            }

           /* using (StreamWriter outputFile = new StreamWriter(@serverDirectory + "\\carlist.txt"))
            {
                foreach (string line in savestrs) {
                    outputFile.WriteLine(line);
                }
                


                string message = "The Robots List was updated";
                string caption = "Update";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;

                // Displays the MessageBox.

                result = MessageBox.Show(message, caption, buttons);
            }*/

            string newPath = serverDirectory + "\\carlist.txt";
            if (File.Exists(newPath))
            {
                File.WriteAllLines(newPath, savestrs);
                string message = "File saved successfully";
                string caption = "Saved";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;

                // Displays the MessageBox.

                result = MessageBox.Show(message, caption, buttons);

            }

            

            
        }
    }
}
