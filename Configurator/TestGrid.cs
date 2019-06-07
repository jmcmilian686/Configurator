using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Configurator
{
    public partial class TestGrid : Form
    {
        public string serverDirectory { get; set; }
        public TestGrid(string dir)
        {
            this.serverDirectory = dir;
            InitializeComponent();
  
        }
              
        public Bitmap BackBuffer;
        public int elements { get; set; }

        public int btnW { get; set; }

        public int btnH { get; set; }

        public int offsetX { get; set; }

        public int offsetY { get; set; }

        public int induction { get; set; }

        public int working { get; set; }

        public int chutes { get; set; }

        public int parking { get; set; }

        public int charging { get; set; }

        public int indqueue { get; set; }

        public int enterindqueue { get; set; }

        public int genCounter { get; set; }

        public List<GridMatrix> matrix { get; set; }

        public int mousePX = 0;

        public int mousePY = 0;

        public bool changed = false;

        public GridMatrix prevNode;

        public string actualTag;

        public BitmaskEnum newMask;

        public int selectedBlock;

        [Flags]
        public enum BitmaskEnum
        {
            Blnk = 0,
            RtLv = 1,
            RtEnt = 2,
            UpLv = 64,
            UpEnt = 128,
            LfEnt = 8,
            LfLv = 4,
            DoEnt = 32,
            DoLv= 16
        }
             

       
        Dictionary<Color, string> colorDict = new Dictionary<Color, string>()
        {
            {Color.FromArgb(242,242,242),"Empty"},
            {Color.FromArgb(255, 255, 130),"WorkingBlock" },
            {Color.FromArgb(200, 200, 255),"InductionQueue"},
            {Color.FromArgb(100, 100, 255),"ReturnBlock"},
            {Color.FromArgb(255, 128, 0),"ChargingStation"},
            {Color.FromArgb(255, 160, 56),"EnterCharging"},
            {Color.FromArgb(255, 202, 149),"QueueCharging"},
            {Color.FromArgb(150, 255, 150),"Induction"},
            {Color.FromArgb(210, 200, 21),"DivertQueue"},
            {Color.FromArgb(0, 200, 0),"EvenSpeed"},
            {Color.FromArgb(31, 226, 226),"EnterQueue"},
            {Color.FromArgb(255, 255, 255),"Parking"},
            {Color.FromArgb(100, 100, 0),"Chute"},
            {Color.FromArgb(255, 100, 255),"Hospital"},
            

        };
        private void Mouse_Over(Object s, EventArgs e)
        {

        }

        public string[] GetMasked(BitmaskEnum maskedRes)
        {
            return maskedRes.ToString().Split(new[] { ", " }, StringSplitOptions.None);
        }
      
        private void TestGrid_Load(object sender, EventArgs e)
        {
            btnH = 20;
            btnW = 20;
            offsetX = 0;
            offsetY = 0;
            prevNode = new GridMatrix();

            comboBox5.DisplayColorSamples(colorDict);
            comboBox5.SelectedIndex = 0;
            selectedBlock = 0;
            
            //label12.Text = enumerat.ToString();

            //var list = enumerat.ToString().Split(new[] { ", " }, StringSplitOptions.None);

            //foreach (var item in list)
            //{
            //    var buff = item;
            //}

            string[] a = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            string[] b = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "0" };
            foreach (var el in a)
            {
                comboBox2.Items.Add(el);
            }

            foreach (var el1 in b)
            {
                comboBox4.Items.Add(el1);
            }
            elements = 0;

            comboBox1.Items.Add("1");
            comboBox1.Items.Add("2");

            comboBox3.Items.Add("1");
            comboBox3.Items.Add("2");


            if (serverDirectory != "")
            {
              
                if (File.Exists(serverDirectory + "\\grid.config") && File.Exists(serverDirectory + "\\grid.physical"))
                {
                    
                    //Reading grid.config file
                    string[] linesGrid = System.IO.File.ReadAllLines(serverDirectory + "\\grid.config");

                    //Reading grid.physical file
                    string[] gridPhi = System.IO.File.ReadAllLines(serverDirectory + "\\grid.physical");

                    //Reading grid.map file
                    string[] gridMap = System.IO.File.ReadAllLines(serverDirectory + "\\grid.map");

                    matrix = new List<GridMatrix>();

                    int ind = 0;
                    foreach (string line in linesGrid)
                    {

                        string[] elems = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (elems.Count() == 1)
                        {
                            elems = line.Split('\t');
                        }

                        string rfidLine = gridMap[Convert.ToInt32(elems[1])];
                        string[] rfids = rfidLine.Count() > 0 ? rfidLine.Split(',') : new string[] {""} ;



                        //if (elems[2] == "12")
                        //{
                        GridMatrix node = new GridMatrix();
                        node.PosX = Convert.ToInt32(elems[0]);
                        node.PosY = Convert.ToInt32(elems[1]);
                        node.NodeType = Convert.ToInt32(elems[2]);
                        node.Dir = Convert.ToInt32(elems[3]);
                        if (rfids.Count()>0)
                        {
                            node.RFID = rfids[Convert.ToInt32(elems[0])].ToString();
                        }
                        matrix.Add(node);
                        //}
                        ind++;
                    }



                    List<RobotIps> str = new List<RobotIps>();

                  

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



                  
                }

                int width = panel4.Width / matrix.Last().PosX;
                int height = panel4.Height / matrix.Last().PosY;

                if (height <= width)
                {
                    btnH = height;
                    btnW = height;
                }
                else
                {
                    btnH = width;
                    btnW = width;
                }

            }


        }


        private void paintFlow(Graphics g, BitmaskEnum maskedNumber, int posX, int posY)
        {

            // Create a new pen.
            Pen GreenPen = new Pen(Brushes.Green);
            Pen BlackPen = new Pen(Brushes.Black);
            // Set the pen's width.
            GreenPen.Width = 2;
            BlackPen.Width = 2;
            // Set the LineJoin property.
            GreenPen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
            BlackPen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                       
            var resMask = GetMasked(maskedNumber);
            int posRectX = (posX * btnW + 1) - offsetX;
            int posRectY = (posY * btnH + 1) - offsetY;
            foreach (var iteMask in resMask)
            {
                switch (iteMask)
                {
                    case "RtLv":
                        g.DrawLine(BlackPen, (posRectX + (btnW * 11 / 20)), (posRectY + (btnW * 9 / 20)), posRectX + btnW - 2, posRectY + btnW * 9 / 20);
                        break;
                    case "RtEnt":
                        g.DrawLine(GreenPen, (posRectX + (btnW * 11 / 20)), (posRectY + (btnW * 11 / 20)), posRectX + btnW - 2, posRectY + btnW * 11 / 20);
                        break;
                    case "LfLv":
                        g.DrawLine(BlackPen, posRectX + 2, (posRectY + (btnW * 11 / 20)), (posRectX + btnW * 9 / 20), posRectY + btnW * 11 / 20);
                        break;
                    case "LfEnt":
                        g.DrawLine(GreenPen, posRectX + 2, (posRectY + (btnW * 9 / 20)), posRectX + btnW * 9 / 20, posRectY + btnW * 9 / 20);
                        break;
                    case "UpEnt":
                        g.DrawLine(GreenPen, posRectX + (btnW * 11 / 20), posRectY + 2, posRectX + btnW * 11 / 20, posRectY + btnW * 9 / 20);
                        break;
                    case "UpLv":
                        g.DrawLine(BlackPen, posRectX + (btnW * 9 / 20), posRectY + 2, (posRectX + btnW * 9 / 20), posRectY + btnW * 9 / 20);
                        break;
                    case "DoEnt":
                        g.DrawLine(GreenPen, posRectX + (btnW * 9 / 20), posRectY + (btnW * 11 / 20), posRectX + (btnW * 9 / 20), posRectY + btnW - 2);
                        break;
                    case "DoLv":
                        g.DrawLine(BlackPen, posRectX + (btnW * 11 / 20), posRectY + (btnW * 11 / 20), posRectX + btnW * 11 / 20, posRectY + btnW - 2);
                        break;
                    default:
                        break;
                }
            }


        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            int frameHeight = matrix.Last().PosX * btnH;
            int frameWidth = matrix.Last().PosY * btnH;
            

            BackBuffer = new Bitmap(frameHeight, frameWidth);

            //Graphics g = panel4.CreateGraphics();

            Graphics g = Graphics.FromImage(BackBuffer);

            Pen blackPen = new Pen(Color.Gray, 1);

            Font drawFont = new Font("Arial", 10);
            SolidBrush drawBrush = new SolidBrush(Color.White);

            foreach (var item in matrix)
            {
                //using GDI

                SolidBrush solidBrush = new SolidBrush(Color.Gray);
                int posX = Convert.ToInt32(item.PosX);
                int posY = Convert.ToInt32(item.PosY);
                // Create a new pen.
                Pen GreenPen = new Pen(Brushes.Green);
                Pen BlackPen = new Pen(Brushes.Black);
                // Set the pen's width.
                GreenPen.Width = 2;
                BlackPen.Width = 2;
                // Set the LineJoin property.
                GreenPen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;
                BlackPen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;

                switch (item.NodeType.ToString())
                {

                    case "1":

                        Rectangle wRect = new Rectangle(posX * btnW - offsetX, posY * btnH - offsetY, btnH, btnW);
                        g.DrawRectangle(blackPen, wRect);

                        //Filling Rectangle
                        Rectangle rectwF = new Rectangle((posX * btnW + 1) - offsetX, (posY * btnH + 1) - offsetY, btnH - 1, btnW - 1);
                        solidBrush.Color = Color.FromArgb(255, 255, 130);
                        g.FillRectangle(solidBrush, rectwF);

                        //flow drawing
                        BitmaskEnum enumerat = (BitmaskEnum)Convert.ToInt32(item.Dir);
                        paintFlow(g, enumerat, posX, posY);

                        working++;
                        break;
                    case "2":
                        Rectangle inqRect = new Rectangle(posX * btnW - offsetX, posY * btnH - offsetY, btnH, btnW);
                        g.DrawRectangle(blackPen, inqRect);

                        //Filling Rectangle
                        Rectangle rectinqF = new Rectangle((posX * btnW + 1) - offsetX, (posY * btnH + 1) - offsetY, btnH - 1, btnW - 1);
                        solidBrush.Color = Color.FromArgb(202, 116, 237);
                        g.FillRectangle(solidBrush, rectinqF);
                        //flow drawing
                        BitmaskEnum enumerat5 = (BitmaskEnum)Convert.ToInt32(item.Dir);
                        paintFlow(g, enumerat5, posX, posY);
                        indqueue++;
                        break;
                    case "3":
                        Rectangle retRect = new Rectangle(posX * btnW - offsetX, posY * btnH - offsetY, btnH, btnW);
                        g.DrawRectangle(blackPen, retRect);

                        //Filling Rectangle
                        Rectangle rectret = new Rectangle((posX * btnW + 1) - offsetX, (posY * btnH + 1) - offsetY, btnH - 1, btnW - 1);
                        solidBrush.Color = Color.FromArgb(100, 100, 255);
                        g.FillRectangle(solidBrush, rectret);
                        //flow drawing
                        BitmaskEnum enumerat23 = (BitmaskEnum)Convert.ToInt32(item.Dir);
                        paintFlow(g, enumerat23, posX, posY);
                        indqueue++;
                        break;
                    case "4":
                        Rectangle chRect = new Rectangle(posX * btnW - offsetX, posY * btnH - offsetY, btnH, btnW);
                        g.DrawRectangle(blackPen, chRect);

                        //Filling Rectangle
                        Rectangle rectchF = new Rectangle((posX * btnW + 1) - offsetX, (posY * btnH + 1) - offsetY, btnH - 1, btnW - 1);
                        solidBrush.Color = Color.FromArgb(255, 128, 0);
                        g.FillRectangle(solidBrush, rectchF);
                        //flow drawing
                        BitmaskEnum enumerat7 = (BitmaskEnum)Convert.ToInt32(item.Dir);
                        paintFlow(g, enumerat7, posX, posY);
                        charging++;
                        break;
                    case "5":
                        Rectangle fivRect = new Rectangle(posX * btnW - offsetX, posY * btnH - offsetY, btnH, btnW);
                        g.DrawRectangle(blackPen, fivRect);

                        //Filling Rectangle
                        Rectangle rectfF = new Rectangle((posX * btnW + 1) - offsetX, (posY * btnH + 1) - offsetY, btnH - 1, btnW - 1);
                        solidBrush.Color = Color.FromArgb(242, 109, 96);
                        g.FillRectangle(solidBrush, rectfF);
                        //flow drawing
                        BitmaskEnum enumerat3 = (BitmaskEnum)Convert.ToInt32(item.Dir);
                        paintFlow(g, enumerat3, posX, posY);

                        break;
                    case "6":
                        Rectangle queCRect = new Rectangle(posX * btnW - offsetX, posY * btnH - offsetY, btnH, btnW);
                        g.DrawRectangle(blackPen, queCRect);

                        //Filling Rectangle
                        Rectangle rectQc = new Rectangle((posX * btnW + 1) - offsetX, (posY * btnH + 1) - offsetY, btnH - 1, btnW - 1);
                        solidBrush.Color = Color.FromArgb(255, 202, 149);
                        g.FillRectangle(solidBrush, rectQc);
                        //flow drawing
                        BitmaskEnum enumerat26 = (BitmaskEnum)Convert.ToInt32(item.Dir);
                        paintFlow(g, enumerat26, posX, posY);

                        break;
                    case "7":
                        Rectangle indRect = new Rectangle(posX * btnW - offsetX, posY * btnH - offsetY, btnH, btnW);
                        g.DrawRectangle(blackPen, indRect);

                        //Filling Rectangle
                        Rectangle rectinF = new Rectangle((posX * btnW + 1) - offsetX, (posY * btnH + 1) - offsetY, btnH - 1, btnW - 1);
                        solidBrush.Color = Color.FromArgb(150, 255, 150);
                        g.FillRectangle(solidBrush, rectinF);
                        //flow drawing
                        BitmaskEnum enumerat4 = (BitmaskEnum)Convert.ToInt32(item.Dir);
                        paintFlow(g, enumerat4, posX, posY);
                        induction++;
                        break;
                    case "8":
                        Rectangle divQRect = new Rectangle(posX * btnW - offsetX, posY * btnH - offsetY, btnH, btnW);
                        g.DrawRectangle(blackPen, divQRect);

                        //Filling Rectangle
                        Rectangle rectDQ = new Rectangle((posX * btnW + 1) - offsetX, (posY * btnH + 1) - offsetY, btnH - 1, btnW - 1);
                        solidBrush.Color = Color.FromArgb(210, 200, 21);
                        g.FillRectangle(solidBrush, rectDQ);
                        //flow drawing
                        BitmaskEnum enumerat28 = (BitmaskEnum)Convert.ToInt32(item.Dir);
                        paintFlow(g, enumerat28, posX, posY);
                        induction++;
                        break;
                    case "9":
                        Rectangle evenRect = new Rectangle(posX * btnW - offsetX, posY * btnH - offsetY, btnH, btnW);
                        g.DrawRectangle(blackPen, evenRect);

                        //Filling Rectangle
                        Rectangle rectEv = new Rectangle((posX * btnW + 1) - offsetX, (posY * btnH + 1) - offsetY, btnH - 1, btnW - 1);
                        solidBrush.Color = Color.FromArgb(0, 200, 0);
                        g.FillRectangle(solidBrush, rectEv);
                        //flow drawing
                        BitmaskEnum enumerat29 = (BitmaskEnum)Convert.ToInt32(item.Dir);
                        paintFlow(g, enumerat29, posX, posY);
                        induction++;
                        break;

                    case "10":
                        Rectangle entiRect = new Rectangle(posX * btnW - offsetX, posY * btnH - offsetY, btnH, btnW);
                        g.DrawRectangle(blackPen, entiRect);

                        //Filling Rectangle
                        Rectangle rectEinqF = new Rectangle((posX * btnW + 1) - offsetX, (posY * btnH + 1) - offsetY, btnH - 1, btnW - 1);
                        solidBrush.Color = Color.FromArgb(31, 226, 226);
                        g.FillRectangle(solidBrush, rectEinqF);
                        //flow drawing
                        BitmaskEnum enumerat6 = (BitmaskEnum)Convert.ToInt32(item.Dir);
                        paintFlow(g, enumerat6, posX, posY);
                        enterindqueue++;
                        break;

                    case "11":
                        Rectangle pRect = new Rectangle(posX * btnW - offsetX, posY * btnH - offsetY, btnH, btnW);
                        g.DrawRectangle(blackPen, pRect);

                        //Filling Rectangle
                        Rectangle rectpF = new Rectangle((posX * btnW + 1) - offsetX, (posY * btnH + 1) - offsetY, btnH - 1, btnW - 1);
                        solidBrush.Color = Color.FromArgb(255, 234, 255);
                        g.FillRectangle(solidBrush, rectpF);
                        //flow drawing
                        BitmaskEnum enumerat2 = (BitmaskEnum)Convert.ToInt32(item.Dir);
                        paintFlow(g, enumerat2, posX, posY);
                        parking++;
                        break;
                    case "12":

                        elements++;

                        Rectangle penRect = new Rectangle(posX * btnW - offsetX, posY * btnH - offsetY, btnH, btnW);
                        g.DrawRectangle(blackPen, penRect);

                        //Filling Rectangle
                        Rectangle rect2 = new Rectangle((posX * btnW + 1) - offsetX, (posY * btnH + 1) - offsetY, btnH - 1, btnW - 1);
                        solidBrush.Color = Color.FromArgb(100, 100, 0);
                        g.FillRectangle(solidBrush, rect2);

                        //StringFormat drawFormat = new StringFormat();
                        //drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;

                        g.DrawString(item.GridValue, drawFont, drawBrush, (posX * btnW + 1) - offsetX, (posY * btnH + 1) - offsetY);

                        chutes++;

                        break;

                    case "13":
                        Rectangle hRect = new Rectangle(posX * btnW - offsetX, posY * btnH - offsetY, btnH, btnW);
                        g.DrawRectangle(blackPen, hRect);

                        //Filling Rectangle
                        Rectangle recthF = new Rectangle((posX * btnW + 1) - offsetX, (posY * btnH + 1) - offsetY, btnH - 1, btnW - 1);
                        solidBrush.Color = Color.FromArgb(255, 100, 255);
                        g.FillRectangle(solidBrush, recthF);
                        //flow drawing
                        BitmaskEnum enumerat9 = (BitmaskEnum)Convert.ToInt32(item.Dir);
                        paintFlow(g, enumerat9, posX, posY);

                        break;
                    case "15":
                        Rectangle echRect = new Rectangle(posX * btnW - offsetX, posY * btnH - offsetY, btnH, btnW);
                        g.DrawRectangle(blackPen, echRect);

                        //Filling Rectangle
                        Rectangle rectechF = new Rectangle((posX * btnW + 1) - offsetX, (posY * btnH + 1) - offsetY, btnH - 1, btnW - 1);
                        solidBrush.Color = Color.FromArgb(255, 160, 66);
                        g.FillRectangle(solidBrush, rectechF);
                        //flow drawing
                        BitmaskEnum enumerat8 = (BitmaskEnum)Convert.ToInt32(item.Dir);
                        paintFlow(g, enumerat8, posX, posY);

                        break;

                    
                    default:

                        Rectangle defRect = new Rectangle(posX * btnW - offsetX, posY * btnH - offsetY, btnH, btnW);
                        g.DrawRectangle(blackPen, defRect);
                        Rectangle recthFE = new Rectangle((posX * btnW + 1) - offsetX, (posY * btnH + 1) - offsetY, btnH - 1, btnW - 1);
                        solidBrush.Color = Color.FromArgb(242, 242, 242);
                        g.FillRectangle(solidBrush, recthFE);
                        break;
                }


            }

            g.Dispose();

            Graphics k = panel4.CreateGraphics();
            Point st = new Point();
            st.X = 0;
            st.Y = 0;
            k.DrawImage(BackBuffer,st);
            k.Dispose();
        }





        private void TrackBar2_Scroll(object sender, EventArgs e)
        {
            //textBox1.Text = trackBar2.Value.ToString();
            btnH = trackBar2.Value;
            btnW = trackBar2.Value;
            int offsetH = ((matrix.Last().PosX * btnW) - panel4.Width) / btnH;
            int offsetV = ((matrix.Last().PosY * btnH) - panel4.Height) / btnW;

            if (offsetH > 0)
            {
                hScrollBar1.Minimum = 0;
                hScrollBar1.Maximum = offsetH *2;
                hScrollBar1.SmallChange = 2;
                hScrollBar1.LargeChange = 5;
                hScrollBar1.Visible = true;
            }

            if (offsetV > 0)
            {
                vScrollBar1.Minimum = 0;
                vScrollBar1.Maximum = offsetV * 2;
                vScrollBar1.SmallChange = 2;
                vScrollBar1.LargeChange = 5;
                vScrollBar1.Visible = true;
            }
            panel4.Refresh();
        }

        private void VScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
           
            offsetY = vScrollBar1.Value * btnH;
            panel4.Refresh();
        }

        private void HScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            offsetX = hScrollBar1.Value * btnW;
            panel4.Refresh();
        }

        private void Panel4_MouseMove(object sender, MouseEventArgs e)
        {
            mousePX = offsetX > 0 ? e.X + offsetX : e.X;
            mousePY = offsetY > 0 ? e.Y + offsetY : e.Y;
            int gridPX = mousePX / btnW;
            int gridPY = mousePY / btnH;
            string nodeType = "Block";

            label1.Text = "X: " + mousePX.ToString() + ";" + "Y: " + mousePY.ToString();


            System.Drawing.Point mousePosition = new Point(e.X, e.Y);

            ////finding node type
            var nodeT = matrix.Where(p => p.PosX == gridPX && p.PosY == gridPY).FirstOrDefault();
            int indexT = matrix.IndexOf(nodeT);

            //if (node!=null)
            //{
            //    nodeType = "something";
            //}
            if (nodeT != null)
            {
                switch (nodeT.NodeType)
                {
                    case 12:
                        nodeType = "Chute :" + nodeT.GridValue+ "--"+nodeT.RFID;
                        if ((ModifierKeys & Keys.Control) == Keys.Control)
                        {
                            //nodeT.GridValue = "Test";
                           
                           
                                if (!checkBox2.Checked)
                                {
                                    if (prevNode != nodeT)
                                    {
                                        if (comboBox2.SelectedItem != null)
                                        {
                                            genCounter++;
                                            textBox1.Text = genCounter.ToString();
                                            if (checkBox1.Checked && comboBox4.SelectedItem != null)
                                            {
                                                nodeT.GridValue = comboBox4.SelectedItem.ToString() + comboBox1.SelectedItem.ToString() + comboBox2.SelectedItem.ToString() + comboBox3.SelectedItem.ToString() + genCounter.ToString("d2");

                                            }
                                            else
                                            {

                                                nodeT.GridValue = comboBox1.SelectedItem.ToString() + comboBox2.SelectedItem.ToString() + comboBox3.SelectedItem.ToString() + genCounter.ToString("d2");
                                            }


                                        }
                                        matrix[indexT] = nodeT;
                                        prevNode = nodeT;
                                        panel4.Refresh();
                                    }
                                }
                                else
                                {
                                    nodeT.GridValue = textBox2.Text;
                                    matrix[indexT] = nodeT;
                                    prevNode = nodeT;
                                    panel4.Refresh();
                            }
                                
                               
                         
                            
                        }
                            break;
                    case 0:
                        nodeType = "Empty :" + "--" + nodeT.RFID; ;
                        break;
                    case 1:
                        nodeType = "Working Block :"  + nodeT.GridValue + "--" + nodeT.RFID;
                        break;
                    case 11:
                        nodeType = "Parking :" + nodeT.GridValue + "--" + nodeT.RFID;
                        break;
                    case 7:
                        nodeType = "Induction Station:" + nodeT.GridValue + "--" + nodeT.RFID;
                        if ((ModifierKeys & Keys.Control) == Keys.Control)
                        {
                            
                              if (checkBox2.Checked)
                              {
                                  nodeT.GridValue = textBox2.Text;
                              }

                                matrix[indexT] = nodeT;
                                prevNode = nodeT;
                                panel4.Refresh();
                            

                        }
                        break;
                    case 4:
                        nodeType = "Charging Station:" + nodeT.GridValue + "--" + nodeT.RFID;
                        break;
                    case 13:
                        nodeType = "Hospital:" + nodeT.GridValue + "--" + nodeT.RFID;
                        break;
                    case 5:
                        nodeType = "Enter Charging:" + nodeT.GridValue + "--" + nodeT.RFID;
                        break;
                    case 10:
                        nodeType = "Enter Queue:" + nodeT.GridValue + "--" + nodeT.RFID;
                        break;
                    case 9:
                        nodeType = "Even Speed:" + nodeT.GridValue + "--" + nodeT.RFID;
                        break;
                    default:
                        break;
                }
                
            }


            IWin32Window win = this;
            mousePosition.X += 90;
            
            toolTip1.Show(nodeType + " X:" + gridPX.ToString() + "Y:" + gridPY.ToString(), win, mousePosition, 1000);
        }

        private void Panel4_Click(object sender, EventArgs e)
        {
           
            int gridPX = mousePX / btnW;
            int gridPY = mousePY / btnH;
            string nodeType = "Block";

            ////finding node type
            var nodeT = matrix.Where(p => p.PosX == gridPX && p.PosY == gridPY).FirstOrDefault();
            int indexT = matrix.IndexOf(nodeT);

            //if (node!=null)
            //{
            //    nodeType = "something";
            //}
            if (nodeT != null)
            {
                if (checkBox11.Checked)
                {
                    if (nodeT != null)
                    {
                        if (selectedBlock == 0 || selectedBlock == 12)
                        {
                            nodeT.Dir = 0;
                            nodeT.NodeType = selectedBlock;

                        }
                        else
                        {

                            nodeT.Dir = (int)newMask;
                            nodeT.NodeType = selectedBlock;

                        }


                        panel4.Refresh();
                    }
                }
                else
                {
                    switch (nodeT.NodeType)
                    {
                        case 12:

                            if (!checkBox2.Checked)
                            {
                                if (comboBox2.SelectedItem != null)
                                {
                                    genCounter++;
                                    textBox1.Text = genCounter.ToString();
                                    if (checkBox1.Checked && comboBox4.SelectedItem != null)
                                    {
                                        nodeT.GridValue = comboBox4.SelectedItem.ToString() + comboBox1.SelectedItem.ToString() + comboBox2.SelectedItem.ToString() + comboBox3.SelectedItem.ToString() + genCounter.ToString("d2");

                                    }
                                    else
                                    {

                                        nodeT.GridValue = comboBox1.SelectedItem.ToString() + comboBox2.SelectedItem.ToString() + comboBox3.SelectedItem.ToString() + genCounter.ToString("d2");
                                    }


                                }
                                matrix[indexT] = nodeT;
                                prevNode = nodeT;
                                panel4.Refresh();

                            }
                            else
                            {
                                nodeT.GridValue = textBox2.Text;
                                matrix[indexT] = nodeT;
                                prevNode = nodeT;
                                panel4.Refresh();
                            }
                            break;
                        case 0:
                            nodeType = "Empty :";
                            break;
                        case 1:
                            nodeType = "Working Block :" + nodeT.GridValue;
                            break;
                        case 11:
                            nodeType = "Parking :" + nodeT.GridValue;
                            break;
                        case 7:

                            if (checkBox2.Checked)
                            {
                                nodeT.GridValue = textBox2.Text;
                            }

                            matrix[indexT] = nodeT;
                            prevNode = nodeT;
                            panel4.Refresh();
                            break;
                        case 4:
                            nodeType = "Charging Station:" + nodeT.GridValue;
                            break;
                        default:
                            break;
                    }
                }
                

               
            }
            
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                comboBox4.Enabled = true;
            }
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            textBox1.Text = "0";
            genCounter = 0;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            string[] gridPhi = System.IO.File.ReadAllLines(serverDirectory + "\\grid.physical");

            int actualY = 0;
            int maxY = matrix.Last().PosY;
            string[] saveRfid = new string[maxY+1];
            string lineRfid = "";
            string gridConfig = "";
            int counter = 0;
         
            int size = 0;

            foreach (GridMatrix gm in matrix)
            {
                if ((gm.NodeType == 12 && gm.GridValue != null) || gm.NodeType == 7)
                {
                    size++;
                }
                if (gm.PosY== actualY)
                {
                    
                    if (gm!=matrix.Last())
                    {
                        lineRfid += gm.RFID + ",";
                    }
                    else
                    {
                        lineRfid += gm.RFID + ",";
                        saveRfid[actualY] = lineRfid;
                    }
                }
                else
                {
                    saveRfid[actualY] = lineRfid;
                    actualY = gm.PosY;
                    lineRfid = "";
                    lineRfid += gm.RFID + ",";
                }

                gridConfig +=" "+ gm.PosX +"   " + gm.PosY + "  " + gm.NodeType + "    " + gm.Dir + Environment.NewLine;

            }

           
            string[] savestr = new string[size];

          
            foreach (GridMatrix cnt in matrix)
            {
                if ((cnt.NodeType == 12 && cnt.GridValue != null) || cnt.NodeType == 7)
                {
                    string nodetype = "";

                    switch (cnt.NodeType)
                    {
                        case 12:
                            nodetype = "CHUTE";
                            break;
                        case 7:
                            nodetype = "Induction";
                            break;
                        default:
                            break;
                    }

                    string newline = cnt.GridValue + "\t" + nodetype + "\t" + (cnt.PosX).ToString() + "\t" + (cnt.PosY).ToString();
                    savestr[counter++] = newline;
                }


            }

            

                string newPath = serverDirectory + "\\grid.physical";
                if (File.Exists(newPath))
                {
                    File.WriteAllLines(newPath, savestr);

                    string message = "File saved successfully ";
                    string caption = "Save Results";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result;

                    string newPathMap = serverDirectory + "\\grid.map";
                    if (File.Exists(newPathMap))
                    {
                        File.WriteAllLines(newPathMap, saveRfid);

                    }
                    string newPathconfig = serverDirectory + "\\grid.config";
                    if (File.Exists(newPathconfig))
                    {
                        File.WriteAllText(newPathconfig, gridConfig);

                    }

                // Displays the MessageBox.

                result = MessageBox.Show(message, caption, buttons);

                }
                


        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
               
                textBox2.Enabled = true;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
            }
            else
            {
                textBox2.Text = "";
                textBox2.Enabled = false;
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;
            }

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            genCounter = Convert.ToInt32(textBox1.Text);
        }

        private void TextBox1_TextChanged_1(object sender, EventArgs e)
        {
            genCounter = Convert.ToInt32(textBox1.Text);

        }

        private void Panel4_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button==MouseButtons.Right)// in case of a right click on the picture area
            {
                int gridPX = mousePX / btnW;
                int gridPY = mousePY / btnH;
               
                ////finding node type
                var nodeT = matrix.Where(p => p.PosX == gridPX && p.PosY == gridPY).FirstOrDefault();
                int indexT = matrix.IndexOf(nodeT);
                if (nodeT!= null)
                {
                    actualTag = nodeT.RFID;

                    using (Rfid rfidForm = new Rfid(actualTag))
                    {
                        if (rfidForm.ShowDialog() == DialogResult.Cancel)
                        {
                            actualTag = rfidForm.TheValue;
                            nodeT.RFID = actualTag;
                            matrix[indexT] = nodeT;
                            panel4.Refresh();
                        }
                    }


                }



            }
            else
            {
                //if (checkBox11.Checked)
                //{
                //    int gridPX = mousePX / btnW;
                //    int gridPY = mousePY / btnH;

                //    ////finding node type
                //    var nodeT = matrix.Where(p => p.PosX == gridPX && p.PosY == gridPY).FirstOrDefault();
                //    int indexT = matrix.IndexOf(nodeT);
                //    if (nodeT != null)
                //    {
                //        if (selectedBlock ==0||selectedBlock ==12)
                //        {
                //            nodeT.Dir = 0;
                //            nodeT.NodeType = selectedBlock;

                //        }
                //        else
                //        {
                            
                //                nodeT.Dir = (int)newMask;
                //                nodeT.NodeType = selectedBlock;
                           
                //        }
                        

                //        panel4.Refresh();
                //    }
                //}
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CheckBox11_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox11.Checked)
            {
                newMask = BitmaskEnum.Blnk;
                checkBox3.Enabled = true;
                checkBox4.Enabled = true;
                checkBox5.Enabled = true;
                checkBox6.Enabled = true;
                checkBox7.Enabled = true;
                checkBox8.Enabled = true;
                checkBox9.Enabled = true;
                checkBox10.Enabled = true;
            }
            else
            {
                newMask = BitmaskEnum.Blnk;
                checkBox3.Enabled = false;
                checkBox3.Checked = false;
                checkBox4.Enabled = false;
                checkBox4.Checked = false;
                checkBox5.Enabled = false;
                checkBox5.Checked = false;
                checkBox6.Enabled = false;
                checkBox6.Checked = false;
                checkBox7.Enabled = false;
                checkBox7.Checked = false;
                checkBox8.Enabled = false;
                checkBox8.Checked = false;
                checkBox9.Enabled = false;
                checkBox9.Checked = false;
                checkBox10.Enabled = false;
                checkBox10.Checked = false;
            }
        }

        public void NewMaskCheck()
        {
            newMask = BitmaskEnum.Blnk;
            if (checkBox3.Checked)
            {
                newMask = newMask | BitmaskEnum.RtLv;
            }
            if (checkBox4.Checked)
            {
                newMask = newMask | BitmaskEnum.RtEnt;
            }
            if (checkBox5.Checked)
            {
                newMask = newMask | BitmaskEnum.LfLv;
            }
            if (checkBox6.Checked)
            {
                newMask = newMask | BitmaskEnum.LfEnt;
            }
            if (checkBox7.Checked)
            {
                newMask = newMask | BitmaskEnum.UpLv;
            }
            if (checkBox8.Checked)
            {
                newMask = newMask | BitmaskEnum.UpEnt;
            }
            if (checkBox9.Checked)
            {
                newMask = newMask | BitmaskEnum.DoLv;
            }
            if (checkBox10.Checked)
            {
                newMask = newMask | BitmaskEnum.DoEnt;
            }

        }

        private void CheckBox5_CheckedChanged(object sender, EventArgs e)
        {
            NewMaskCheck();
        }

        private void ComboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            var valuIndex = (KeyValuePair<Color, string>)comboBox5.SelectedItem;

            
            Bitmap rectBuffer = new Bitmap(37, 37);
            Pen blackPen = new Pen(Color.Gray, 1);
            Graphics g = Graphics.FromImage(rectBuffer);
            Rectangle penRect = new Rectangle(0, 0, 37, 37);
            g.DrawRectangle(blackPen, penRect);
            SolidBrush solidBrush = new SolidBrush(Color.Gray);
            Rectangle rect3 = new Rectangle(1, 1, 37, 37);
            switch (valuIndex.Value)
            {
                case "WorkingBlock":
                    solidBrush.Color = Color.FromArgb(255, 255, 130);
                    g.FillRectangle(solidBrush, rect3);
                    selectedBlock = 1;
                    break;
                case "InductionQueue":
                    solidBrush.Color = Color.FromArgb(200, 200, 255);
                    g.FillRectangle(solidBrush, rect3);
                    selectedBlock = 2;
                    break;
                case "ReturnBlock":
                    solidBrush.Color = Color.FromArgb(100, 100, 255);
                    g.FillRectangle(solidBrush, rect3);
                    selectedBlock = 3;
                    break;
                case "ChargingStation":
                    solidBrush.Color = Color.FromArgb(255, 128, 0);
                    g.FillRectangle(solidBrush, rect3);
                    selectedBlock = 4;
                    break;
                case "EnterCharging":
                    solidBrush.Color = Color.FromArgb(255, 160, 56);
                    g.FillRectangle(solidBrush, rect3);
                    selectedBlock = 5;
                    break;
                case "QueueCharging":
                    solidBrush.Color = Color.FromArgb(255, 202, 149);
                    g.FillRectangle(solidBrush, rect3);
                    selectedBlock = 6;
                    break;
                case "Induction":
                    solidBrush.Color = Color.FromArgb(150, 255, 150);
                    g.FillRectangle(solidBrush, rect3);
                    selectedBlock = 7;
                    break;
                case "DivertQueue":
                    solidBrush.Color = Color.FromArgb(210, 200, 21);
                    g.FillRectangle(solidBrush, rect3);
                    selectedBlock = 8;
                    break;
                case "EvenSpeed":
                    solidBrush.Color = Color.FromArgb(0, 200, 0);
                    g.FillRectangle(solidBrush, rect3);
                    selectedBlock = 9;
                    break;
                case "EnterQueue":
                    solidBrush.Color = Color.FromArgb(31, 226, 226);
                    g.FillRectangle(solidBrush, rect3);
                    selectedBlock = 10;
                    break;
                case "Parking":
                    solidBrush.Color = Color.FromArgb(255, 255, 255);
                    g.FillRectangle(solidBrush, rect3);
                    selectedBlock = 11;
                    break;
                case "Chute":
                    solidBrush.Color = Color.FromArgb(100, 100, 0);
                    g.FillRectangle(solidBrush, rect3);
                    selectedBlock = 12;
                    break;
                case "Hospital":
                    solidBrush.Color = Color.FromArgb(255, 100, 255);
                    g.FillRectangle(solidBrush, rect3);
                    selectedBlock = 13;
                    break;
                case "Empty":
                    solidBrush.Color = Color.FromArgb(242, 242, 242);
                    g.FillRectangle(solidBrush, rect3);
                    selectedBlock = 0;
                    break;
                default:
                    break;
            }

            ////Filling Rectangle
            //Rectangle rect2 = new Rectangle((posX * btnW + 1) - offsetX, (posY * btnH + 1) - offsetY, btnH - 1, btnW - 1);
            //solidBrush.Color = Color.FromArgb(100, 100, 0);
            //g.FillRectangle(solidBrush, rect2);

            pictureBox1.Image = rectBuffer;
        }
    }
}
