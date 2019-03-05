using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Configurator
{
    public partial class ScannerV2 : Form
    {
        public string scannerDirectory { get; set; }

       
        public ScannerV2(string dir)
        {
            this.scannerDirectory = dir; 
            InitializeComponent();
        }

        private void ScannerV2_Load(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.Load(scannerDirectory + "\\LabZ_Scanner.exe.config");
            Dictionary<string, string> dict = new Dictionary<string, string>();

            XmlNodeList setings = doc.GetElementsByTagName("appSettings");

            int counter = 0;
            foreach (XmlNode node in setings[0].ChildNodes)
            {

                if (node.NodeType == XmlNodeType.Element)
                {
                    dict.Add(node.Attributes[0].Value, node.Attributes[1].Value);
                }
                else if (node.NodeType == XmlNodeType.Comment)
                {
                    dict.Add("comment" + counter++, node.InnerText);
                }

            }

            if (dict.Count > 0)
            {


                foreach (var elem in dict)
                {
                    if (elem.Key.StartsWith("comm"))
                    {

                        int id = dataGridView1.Rows.Add(elem.Value);
                        dataGridView1.Rows[id].DefaultCellStyle = new DataGridViewCellStyle { Font = new Font("Times New Roman", 12.0f, FontStyle.Bold), BackColor = Color.LightSkyBlue };

                    }
                    else
                    {

                        dataGridView1.Rows.Add(elem.Key, elem.Value);
                    }
                    //str.Add(new RobotIps { ip = elem });


                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count > 0 && this.dataGridView1.SelectedRows[0].Index != this.dataGridView1.Rows.Count - 1 && this.dataGridView1.SelectedRows[0].Index != 0)
            {
                this.dataGridView1.Rows.RemoveAt(
                    this.dataGridView1.SelectedRows[0].Index);
            }
        }

        private void cancBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> savestrs = new Dictionary<string, string>();
            int counter = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {

                if (counter < dataGridView1.Rows.Count - 1)
                {
                    if (row.Cells[1].FormattedValue.ToString() == "")
                    {
                        savestrs.Add("comment" + counter, row.Cells[0].FormattedValue.ToString());

                    }
                    else
                    {

                        savestrs.Add(row.Cells[0].FormattedValue.ToString(), row.Cells[1].FormattedValue.ToString());
                    }


                    counter++;
                }


            }

            //working the file

            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.Load(scannerDirectory + "\\LabZ_Scanner.exe.config");

            XmlNodeList node = doc.GetElementsByTagName("appSettings");


            if (node.Count > 0)
            {

                doc.GetElementsByTagName("appSettings").Item(0).RemoveAll();

            }



            foreach (var elem in savestrs)
            {

                if (elem.Key.StartsWith("comm"))
                {

                    XmlNode newnode = doc.CreateNode(XmlNodeType.Comment, null, null);
                    newnode.InnerText = elem.Value;
                    XmlWhitespace ws = doc.CreateWhitespace("\r\n\t");
                    doc.GetElementsByTagName("appSettings").Item(0).AppendChild(ws);
                    doc.GetElementsByTagName("appSettings").Item(0).AppendChild(newnode);


                }
                else
                {

                    XmlNode newnode = doc.CreateNode(XmlNodeType.Element, "add", null);
                    //Create a new attribute
                    XmlAttribute attr1 = doc.CreateAttribute("key");
                    attr1.Value = elem.Key;

                    XmlAttribute attr2 = doc.CreateAttribute("value");
                    attr2.Value = elem.Value;

                    //Add the attribute to the node     
                    newnode.Attributes.SetNamedItem(attr1);
                    newnode.Attributes.SetNamedItem(attr2);
                    XmlWhitespace ws = doc.CreateWhitespace("\r\n\t");
                    doc.GetElementsByTagName("appSettings").Item(0).AppendChild(ws);
                    doc.GetElementsByTagName("appSettings").Item(0).AppendChild(newnode);

                }


            }
            XmlWhitespace wse = doc.CreateWhitespace("\r\n");
            doc.GetElementsByTagName("appSettings").Item(0).AppendChild(wse);
            doc.PreserveWhitespace = true;
            doc.Save(scannerDirectory + "\\LabZ_Scanner.exe.config");
            string message = "Configuration File Saved Successfully ";
            string caption = "File Saved";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result;

            // Displays the MessageBox.

            result = MessageBox.Show(message, caption, buttons);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (text1.Text != "")
            {
                if (text2.Text == "")
                {
                    int id = dataGridView1.Rows.Add(text1.Text);
                    dataGridView1.Rows[id].DefaultCellStyle = new DataGridViewCellStyle { Font = new Font("Times New Roman", 12.0f, FontStyle.Bold), BackColor = Color.LightSkyBlue };

                }
                else
                {

                    dataGridView1.Rows.Add(text1.Text, text2.Text);

                }


            }
            else
            {

                string message = "Please enter the required information to add";
                string caption = "Blank field";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;

                // Displays the MessageBox.

                result = MessageBox.Show(message, caption, buttons);

            }
        }
    }
}
