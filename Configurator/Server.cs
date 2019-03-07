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

        public int mousePX = 0;

        public int mousePY = 0;

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
           
            //induction = 0;
            //parking = 0;
            //enterindqueue = 0;
            //indqueue = 0;
            //charging = 0;
            //working = 0;
            //chutes = 0;
            //robots = 0;
            

            if (serverDirectory != "")
            {
                if (File.Exists(serverDirectory + "\\carlist.txt"))
                {
                    button1.Visible = true;
                    label1.Visible = true;
                }

                if (File.Exists(serverDirectory + "\\grid.config")&& File.Exists(serverDirectory + "\\grid.physical"))
                {
                    button4.Visible = true;
                    label22.Visible = true;

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



                    //Point pos = new Point(1, 1);
                    ////writing the button matrix

                    //Graphics g = panel2.CreateGraphics();

                    //foreach (string line in linesGrid)
                    //{
                    //    string[] elems = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    //    if (elems[0] != "0" && elems[2] != "0")
                    //    {


                    //        //using GDI

                          
                    //        Pen blackPen = new Pen(Color.Black, 2);

                    //        SolidBrush solidBrush = new SolidBrush(Color.White);
                    //        int posX = Convert.ToInt32(elems[0]);
                    //        int posY = Convert.ToInt32(elems[1]);

                    //        switch (elems[2])
                    //        {
                    //            case "12":
                    //               solidBrush.Color = Color.FromArgb(88, 142, 12);
                    //               elements++;
                                    
                    //                Rectangle penRect = new Rectangle(posX*btnW, posY*btnH, btnH, btnW);
                    //                g.DrawRectangle(blackPen, penRect);

                                    
                    //                chutes++;

                    //                break;
                    //            case "1":
                                  
                    //                Rectangle wRect = new Rectangle(posX, posY, btnH, btnW);
                    //                g.DrawRectangle(blackPen, wRect);
                    //                working++;
                    //                break;
                    //            case "11":
                    //                Rectangle pRect = new Rectangle(posX, posY, btnH, btnW);
                    //                g.DrawRectangle(blackPen, pRect);
                    //                parking++;
                    //                break;
                    //            case "5":
                    //                Rectangle fivRect = new Rectangle(posX, posY, btnH, btnW);
                    //                g.DrawRectangle(blackPen, fivRect);

                    //                break;
                    //            case "7":
                    //                Rectangle indRect = new Rectangle(posX, posY, btnH, btnW);
                    //                g.DrawRectangle(blackPen, indRect);
                    //                induction++;
                    //                break;
                    //            case "2":
                    //                Rectangle inqRect = new Rectangle(posX, posY, btnH, btnW);
                    //                g.DrawRectangle(blackPen, inqRect);
                    //                indqueue++;
                    //                break;
                    //            case "10":
                    //                Rectangle entiRect = new Rectangle(posX, posY, btnH, btnW);
                    //                g.DrawRectangle(blackPen, entiRect);
                    //                enterindqueue++;
                    //                break;
                    //            case "4":
                    //                Rectangle chRect = new Rectangle(posX, posY, btnH, btnW);
                    //                g.DrawRectangle(blackPen, chRect);
                    //                charging++;
                    //                break;
                    //            default:
                    //                break;
                    //        }






                    //        //Using Controls



                    //        //PictureBox btn = new PictureBox();
                    //        //Point newpos = new Point();
                    //        //newpos.X = Convert.ToInt32(elems[0]) * btnW;
                    //        //newpos.Y = Convert.ToInt32(elems[1]) * btnH;
                    //        //btn.Location = newpos;
                    //        //btn.Height = btnH;
                    //        //btn.Width = btnW;
                    //        //btn.Enabled = false;
                    //        //btn.ForeColor = Color.Black;
                    //        //btn.BorderStyle = BorderStyle.FixedSingle;

                    //        //ToolTip ToolTip1 = new ToolTip();
                    //        //ToolTip1.SetToolTip(btn, (newpos.X / btnW).ToString() + "-" + (newpos.Y / btnH).ToString());

                    //        //switch (elems[2])
                    //        //{
                    //        //    case "12":
                    //        //        btn.BackColor = Color.FromArgb(88, 142, 12);
                    //        //        btn.Enabled = true;

                    //        //        elements++;

                    //        //        int posX = Convert.ToInt32(elems[0]);
                    //        //        int posY = Convert.ToInt32(elems[1]);
                    //        //        GridMatrix node = matrix.Where(p => p.PosX == posX && p.PosY == posY).FirstOrDefault();
                    //        //        if (node != null)
                    //        //        {
                    //        //            btn.Text = node.GridValue;
                    //        //        }
                    //        //        chutes++;

                    //        //        break;
                    //        //    case "1":
                    //        //        btn.BackColor = Color.FromArgb(252, 240, 5);
                    //        //        working++;
                    //        //        break;
                    //        //    case "11":
                    //        //        btn.BackColor = Color.FromArgb(255, 255, 255);
                    //        //        parking++;
                    //        //        break;
                    //        //    case "5":
                    //        //        btn.BackColor = Color.FromArgb(242, 109, 96);

                    //        //        break;
                    //        //    case "7":
                    //        //        btn.BackColor = Color.FromArgb(34, 201, 56);
                    //        //        induction++;
                    //        //        break;
                    //        //    case "2":
                    //        //        btn.BackColor = Color.FromArgb(202, 116, 237);
                    //        //        indqueue++;
                    //        //        break;
                    //        //    case "10":
                    //        //        btn.BackColor = Color.FromArgb(0, 255, 225);
                    //        //        enterindqueue++;
                    //        //        break;
                    //        //    case "4":
                    //        //        btn.BackColor = Color.FromArgb(255, 114, 0);
                    //        //        charging++;
                    //        //        break;
                    //        //    default:
                    //        //        break;
                    //        //}

                    //        //panel2.Controls.Add(btn);

                    //        label11.Text = induction.ToString();
                    //        label12.Text = charging.ToString();
                    //        label13.Text = parking.ToString();
                    //        label14.Text = chutes.ToString();
                    //        label15.Text = indqueue.ToString();
                    //        label16.Text = enterindqueue.ToString();
                    //        label18.Text = working.ToString();
                    //        label20.Text = (charging * 10).ToString();
                    //        label21.Text = (robots - 1).ToString();

                          

                    //    }

                    //}

                    //g.Dispose();
                }

               
            }
        }

        
        private void button4_Click_1(object sender, EventArgs e)
        {
            Grid grid = new Grid(serverDirectory);

            grid.Show();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            
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
                if (File.Exists(serverDirectory + "\\carlist.txt"))
                {
                    button1.Visible = true;
                    label1.Visible = true;
                }

                if (File.Exists(serverDirectory + "\\grid.config") && File.Exists(serverDirectory + "\\grid.physical"))
                {
                    button4.Visible = true;
                    label22.Visible = true;

                    string[] linesGrid = System.IO.File.ReadAllLines(serverDirectory + "\\grid.config");


                    string[] gridPhi = System.IO.File.ReadAllLines(serverDirectory + "\\grid.physical");

                   

                    string[] linesCart = System.IO.File.ReadAllLines(serverDirectory + "\\carlist.txt");




                    Point pos = new Point(1, 1);
                    //writing the button matrix

                    Graphics g = panel2.CreateGraphics();

                    foreach (string line in linesGrid)
                    {
                        string[] elems = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (elems[0] != "0" && elems[2] != "0")
                        {


                            //using GDI


                            Pen blackPen = new Pen(Color.Black, 2);

                            SolidBrush solidBrush = new SolidBrush(Color.White);
                            int posX = Convert.ToInt32(elems[0]);
                            int posY = Convert.ToInt32(elems[1]);

                            switch (elems[2])
                            {
                                case "12":
                                    
                                    elements++;

                                    Rectangle penRect = new Rectangle(posX * btnW, posY * btnH, btnH, btnW);
                                    g.DrawRectangle(blackPen, penRect);

                                    //Filling Rectangle
                                    Rectangle rect2 = new Rectangle(posX * btnW + 2, posY * btnH + 2, btnH - 2, btnW - 2);
                                    solidBrush.Color = Color.FromArgb(100, 100, 0);
                                    g.FillRectangle(solidBrush, rect2);

                                    chutes++;

                                    break;
                                case "1":

                                    Rectangle wRect = new Rectangle(posX * btnW, posY * btnH, btnH, btnW);
                                    g.DrawRectangle(blackPen, wRect);

                                    //Filling Rectangle
                                    Rectangle rectwF = new Rectangle(posX * btnW + 2, posY * btnH + 2, btnH - 2, btnW - 2);
                                    solidBrush.Color = Color.FromArgb(255, 255, 130);
                                    g.FillRectangle(solidBrush, rectwF);
                                    working++;
                                    break;
                                case "11":
                                    Rectangle pRect = new Rectangle(posX * btnW, posY * btnH, btnH, btnW);
                                    g.DrawRectangle(blackPen, pRect);

                                    //Filling Rectangle
                                    Rectangle rectpF = new Rectangle(posX * btnW + 2, posY * btnH + 2, btnH - 2, btnW - 2);
                                    solidBrush.Color = Color.FromArgb(255, 255, 255);
                                    g.FillRectangle(solidBrush, rectpF);
                                    parking++;
                                    break;
                                case "5":
                                    Rectangle fivRect = new Rectangle(posX * btnW, posY * btnH, btnH, btnW);
                                    g.DrawRectangle(blackPen, fivRect);

                                    //Filling Rectangle
                                    Rectangle rectfF = new Rectangle(posX * btnW + 2, posY * btnH + 2, btnH - 2, btnW - 2);
                                    solidBrush.Color = Color.FromArgb(242, 109, 96);
                                    g.FillRectangle(solidBrush, rectfF);

                                    break;
                                case "7":
                                    Rectangle indRect = new Rectangle(posX * btnW, posY * btnH, btnH, btnW);
                                    g.DrawRectangle(blackPen, indRect);

                                    //Filling Rectangle
                                    Rectangle rectinF = new Rectangle(posX * btnW + 2, posY * btnH + 2, btnH - 2, btnW - 2);
                                    solidBrush.Color = Color.FromArgb(150, 255, 150);
                                    g.FillRectangle(solidBrush, rectinF);
                                    induction++;
                                    break;
                                case "2":
                                    Rectangle inqRect = new Rectangle(posX * btnW, posY * btnH, btnH, btnW);
                                    g.DrawRectangle(blackPen, inqRect);

                                    //Filling Rectangle
                                    Rectangle rectinqF = new Rectangle(posX * btnW + 2, posY * btnH + 2, btnH - 2, btnW - 2);
                                    solidBrush.Color = Color.FromArgb(202, 116, 237);
                                    g.FillRectangle(solidBrush, rectinqF);
                                    indqueue++;
                                    break;
                                case "10":
                                    Rectangle entiRect = new Rectangle(posX * btnW, posY * btnH, btnH, btnW);
                                    g.DrawRectangle(blackPen, entiRect);

                                    //Filling Rectangle
                                    Rectangle rectEinqF = new Rectangle(posX * btnW + 2, posY * btnH + 2, btnH - 2, btnW - 2);
                                    solidBrush.Color = Color.FromArgb(31, 226, 226);
                                    g.FillRectangle(solidBrush, rectEinqF);
                                    enterindqueue++;
                                    break;
                                case "4":
                                    Rectangle chRect = new Rectangle(posX * btnW, posY * btnH, btnH, btnW);
                                    g.DrawRectangle(blackPen, chRect);

                                    //Filling Rectangle
                                    Rectangle rectchF = new Rectangle(posX * btnW + 2, posY * btnH + 2, btnH - 2, btnW - 2);
                                    solidBrush.Color = Color.FromArgb(255, 128, 0);
                                    g.FillRectangle(solidBrush, rectchF);
                                    charging++;
                                    break;

                                case "15":
                                    Rectangle echRect = new Rectangle(posX * btnW, posY * btnH, btnH, btnW);
                                    g.DrawRectangle(blackPen, echRect);

                                    //Filling Rectangle
                                    Rectangle rectechF = new Rectangle(posX * btnW + 2, posY * btnH + 2, btnH - 2, btnW - 2);
                                    solidBrush.Color = Color.FromArgb(255, 160, 66);
                                    g.FillRectangle(solidBrush, rectechF);
                                    
                                    break;

                                case "13":
                                    Rectangle hRect = new Rectangle(posX * btnW, posY * btnH, btnH, btnW);
                                    g.DrawRectangle(blackPen, hRect);

                                    //Filling Rectangle
                                    Rectangle recthF = new Rectangle(posX * btnW + 2, posY * btnH + 2, btnH - 2, btnW - 2);
                                    solidBrush.Color = Color.FromArgb(255, 100, 255);
                                    g.FillRectangle(solidBrush, recthF);

                                    break;
                                default:

                                    Rectangle defRect = new Rectangle(posX * btnW, posY * btnH, btnH, btnW);
                                    g.DrawRectangle(blackPen, defRect);

                                    break;
                            }
                                                       

                            label11.Text = induction.ToString();
                            label12.Text = charging.ToString();
                            label13.Text = parking.ToString();
                            label14.Text = chutes.ToString();
                            label15.Text = indqueue.ToString();
                            label16.Text = enterindqueue.ToString();
                            label18.Text = working.ToString();
                            label20.Text = (charging * 10).ToString();
                            label21.Text = (robots - 1).ToString();



                        }

                    }

                    g.Dispose();
                }


            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            btnH = trackBar1.Value;
            btnW = trackBar1.Value;
            panel2.Refresh();
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {

            mousePX = e.X;
            mousePY = e.Y;
            int gridPX = e.X/ btnW;
            int gridPY = e.Y/ btnH;
            string nodeType = "Block";

            label23.Text = "X: "+mousePX.ToString()+";"+"Y: "+ mousePY.ToString();


            System.Drawing.Point mousePosition = new Point(mousePX, mousePY);

            ////finding node type
            //var node = matrix.Where(p => p.PosX == gridPX && p.PosY == gridPY).FirstOrDefault();

            //if (node!=null)
            //{
            //    nodeType = "something";
            //}

            IWin32Window win = this;
            mousePosition.X += 90;

            toolTip1.Show(nodeType + " X:" + gridPX.ToString() + "Y:" + gridPY.ToString(), win, mousePosition, 1000);
        }

        private void panel2_MouseHover(object sender, EventArgs e)
        {
            //System.Drawing.Point mousePosition = new Point(mousePX,mousePY);
            

           

            //    IWin32Window win = this;
            //    mousePosition.X += 90;

            //    toolTip1.Show("Tooltip " + "X:" + mousePosition.X / btnW + "Y:" + mousePosition.Y / btnH, win, mousePosition, 1000);
           
        }
    }
}
