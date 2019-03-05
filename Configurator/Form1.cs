using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Configurator
{
    public partial class Configurator : Form
    {
        public Configurator()
        {
            InitializeComponent();
        }

        public string scanDir;
        public int scanVers;
        public string servDir;
        public string gridDir;

        private void Form1_Load(object sender, EventArgs e)
        {
            scanDir = "";
            servDir = "";
            scanVers = 1;
            gridDir = "";
        }

        private void servbtn_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
               
                servDir = folderBrowserDialog1.SelectedPath;
            }

            if (servDir != "") {
                Boolean found = false;
                string actualD = servDir;
                foreach (string st in Directory.GetFiles(actualD))
                {
                    
                    if (st == actualD + "\\LabZ_Server.exe")
                    {
                        confServer.Visible = true;
                        found = true;
                    }


                }

                if (!found) {

                    string message = "The selected folder is not a LabZ Server Directory";
                    string caption = "Wrong Path";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result;

                    // Displays the MessageBox.

                    result = MessageBox.Show(message, caption, buttons);

                    /*if (result == System.Windows.Forms.DialogResult.OK)
                    {

                        // Closes the parent form.

                        this.Close();
                       

                    }*/
                    confServer.Visible = false;

                }

            } 
           

        }

        private void scannbtn_Click(object sender, EventArgs e)
        {
            scanVers = 1;
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                scanDir = folderBrowserDialog1.SelectedPath;
            }

            if (scanDir != "")
            {
                Boolean found = false;
                string actualD = scanDir;
                foreach (string st in Directory.GetFiles(actualD))
                {

                    if (st == actualD + "\\LabZ_Scanner.exe")
                    {
                        confScan.Visible = true;
                        found = true;
                    }

                    if (st == actualD + "\\config.txt")
                    {
                        scanVers = 0;
                    }


                }

                if (!found)
                {

                    string message = "The selected folder is not a LabZ Scanner Directory";
                    string caption = "Wrong Path";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result;

                    // Displays the MessageBox.

                    result = MessageBox.Show(message, caption, buttons);

                    /*if (result == System.Windows.Forms.DialogResult.OK)
                    {

                        // Closes the parent form.

                        this.Close();
                       

                    }*/
                    confScan.Visible = false;

                }

            }

        }

        private void confScan_Click(object sender, EventArgs e)
        {
            if (scanVers == 0)
            {
                Scanner formscan = new Scanner(scanDir);
                formscan.Show();
            }
            else
            {
                ScannerV2 formScannV2 = new ScannerV2(scanDir);
                formScannV2.Show();
            }
                                       
        }

        private void confServer_Click(object sender, EventArgs e)
        {
            Server servForm = new Server(servDir);

            servForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Grid grid = new Grid(gridDir);

            grid.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                gridDir = folderBrowserDialog1.SelectedPath;
            }

            if (gridDir != "")
            {
                Boolean found = false;
                Boolean physical = false, map = false, config = false;
                string actualD = gridDir;
                foreach (string st in Directory.GetFiles(actualD))
                {

                    if (st == actualD + "\\grid.config")
                    {
                        config = true;
                    }
                    if (st == actualD + "\\grid.map")
                    {
                        map = true;
                    }
                    if (st == actualD + "\\grid.physical")
                    {
                        physical = true;
                    }


                    if (config && map && physical)
                    {
                        found = true;
                        confGrid.Visible = true;
                    }


                }

                if (!found)
                {
                    string message = "";
                    string caption = "Missing File";

                    if (!config)
                    {
                        message = "The grid.config file is missing";

                    }
                    else if (!map)
                    {
                        message = "The grid.map file is missing";
                    }
                    else if (!physical)
                    {
                        message = "The grid.physical file is missing";
                    }

                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result;

                    // Displays the MessageBox.

                    result = MessageBox.Show(message, caption, buttons);

                    /*if (result == System.Windows.Forms.DialogResult.OK)
                    {

                        // Closes the parent form.

                        this.Close();
                       

                    }*/
                    confScan.Visible = false;

                }

            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void newGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewMapGrid newMG = new NewMapGrid();
            newMG.Show();
        }
    }
}
