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
    public partial class Server : Form
    {
        public string serverDirectory { get; set; }

        public int elements { get; set; }

        public int btnW { get; set; }

        public int btnH { get; set; }

        public int induction { get; set; }

        public int working { get; set; }

        public int chutes { get; set; }

        public int parking { get; set; }

        public int charging { get; set; }

        public int indqueue { get; set; }

        public int enterindqueue { get; set; }

        public int rightmost { get; set; }

        public string saveDir { get; set; }

        public int robots { get; set; }


        public List<GridMatrix> matrix { get; set; }
        public Server(string dir)
        {
            this.serverDirectory = dir;
            InitializeComponent();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            RobotList rbform = new RobotList(serverDirectory);

            rbform.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ConfFile confFile = new ConfFile(serverDirectory);

            confFile.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Grid grid = new Grid(serverDirectory);

            grid.Show();
        }

        private void Server_Load(object sender, EventArgs e)
        {
            btnH = 20;
            btnW = 20;
            induction = 0;
            parking = 0;
            enterindqueue = 0;
            indqueue = 0;
            charging = 0;
            working = 0;
            chutes = 0;
            robots = 0;
            

            if (serverDirectory != "")
            {



                string[] linesGrid = System.IO.File.ReadAllLines(serverDirectory + "\\grid.config");


                string[] gridPhi = System.IO.File.ReadAllLines(serverDirectory + "\\grid.physical");

                matrix = new List<GridMatrix>();
                foreach (string line in linesGrid)
                {

                    string[] elems = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (elems[2] == "12")
                    {
                        GridMatrix node = new GridMatrix();
                        node.PosX = Convert.ToInt32(elems[0]);
                        node.PosY = Convert.ToInt32(elems[1]);
                        matrix.Add(node);
                    }

                }

                string[] linesCart = System.IO.File.ReadAllLines(serverDirectory + "\\carlist.txt");

                


                List<RobotIps> str = new List<RobotIps>();

                foreach (string elem in linesCart)
                {
                    //str.Add(new RobotIps { ip = elem });
                    robots++;

                }

                // filling matrix with physical data

                foreach (string linePh in gridPhi)
                {
                    string[] elems = linePh.Split('\t');
                    if (!elems[0].StartsWith("//"))
                    {
                        int posX = Convert.ToInt32(elems[2]);
                        int posY = Convert.ToInt32(elems[3]);
                        GridMatrix node = matrix.Where(p => p.PosX == posX && p.PosY == posY).FirstOrDefault();
                        if (node != null)
                        {
                            node.GridValue = elems[0];
                        }


                    }
                }



                Point pos = new Point(1, 1);
                //writing the button matrix
                foreach (string line in linesGrid)
                {
                    string[] elems = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (elems[0] != "0" && elems[2] != "0")
                    {


                        PictureBox btn = new PictureBox();
                        Point newpos = new Point();
                        newpos.X = Convert.ToInt32(elems[0]) * btnW;
                        newpos.Y = Convert.ToInt32(elems[1]) * btnH;
                        btn.Location = newpos;
                        btn.Height = btnH;
                        btn.Width = btnW;
                        btn.Enabled = false;
                        btn.ForeColor = Color.Black;
                        btn.BorderStyle = BorderStyle.FixedSingle;

                        ToolTip ToolTip1 = new ToolTip();
                        ToolTip1.SetToolTip(btn, (newpos.X / btnW).ToString() + "-" + (newpos.Y / btnH).ToString());

                        switch (elems[2])
                        {
                            case "12":
                                btn.BackColor = Color.FromArgb(88, 142, 12);
                                btn.Enabled = true;

                                elements++;

                                int posX = Convert.ToInt32(elems[0]);
                                int posY = Convert.ToInt32(elems[1]);
                                GridMatrix node = matrix.Where(p => p.PosX == posX && p.PosY == posY).FirstOrDefault();
                                if (node != null)
                                {
                                    btn.Text = node.GridValue;
                                }
                                chutes++;

                                break;
                            case "1":
                                btn.BackColor = Color.FromArgb(252, 240, 5);
                                working++;
                                break;
                            case "11":
                                btn.BackColor = Color.FromArgb(255, 255, 255);
                                parking++;
                                break;
                            case "5":
                                btn.BackColor = Color.FromArgb(242, 109, 96);

                                break;
                            case "7":
                                btn.BackColor = Color.FromArgb(34, 201, 56);
                                induction++;
                                break;
                            case "2":
                                btn.BackColor = Color.FromArgb(202, 116, 237);
                                indqueue++;
                                break;
                            case "10":
                                btn.BackColor = Color.FromArgb(0, 255, 225);
                                enterindqueue++;
                                break;
                            case "4":
                                btn.BackColor = Color.FromArgb(255, 114, 0);
                                charging++;
                                break;
                            default:
                                break;
                        }

                        panel2.Controls.Add(btn);

                        label11.Text = induction.ToString();
                        label12.Text = charging.ToString();
                        label13.Text = parking.ToString();
                        label14.Text = chutes.ToString();
                        label15.Text = indqueue.ToString();
                        label16.Text = enterindqueue.ToString();
                        label18.Text = working.ToString();
                        label20.Text = (charging*10).ToString();
                        label21.Text = (robots-1).ToString();

                       

                    }

                }
            }
        }

        
        private void button4_Click_1(object sender, EventArgs e)
        {
            NewGrid grid = new NewGrid(serverDirectory);

            grid.Show();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
