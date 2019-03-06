using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace Configurator
{
    public partial class Grid : Form
    {
        public string serverDirectory { get; set; }


        public Grid(string dir)
        {
            this.serverDirectory = dir;
            InitializeComponent();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
        }

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

        public Boolean pressed { get; set; }

        public int genCounter { get; set; }

        public int botommost { get; set; }

        public string actualRfid { get; set; }

        public string[] mapFile { get; set; }


        public List<GridMatrix> matrix { get; set; }

        private void RFID_Rep(Object s, EventArgs e)
        {

            MenuItem b = s as MenuItem;
            string tag = b.Text;
            actualRfid = tag;
            var tagBtn = b.Parent.Tag;

            using (Rfid rfidForm = new Rfid(tag))
            {
                if (rfidForm.ShowDialog() == DialogResult.Cancel)
                {
                    actualRfid = rfidForm.TheValue;
                    var selBtn = panel2.Controls.Find(tagBtn.ToString(), true);
                    if (selBtn.Count() > 0)
                    {
                        selBtn[0].Text = "RFID";

                    }

                }
            }

            //Search and replace the value
            if (mapFile.Count() > 0)
            {
                for (int i = 0; i < mapFile.Count(); i++)
                {
                    bool flag = false;
                    var strSearch = mapFile[i].Split(',').ToList();

                    if (strSearch.Contains(tag))
                    {
                        int index = strSearch.IndexOf(tag);
                        strSearch.RemoveAt(index);
                        strSearch.Insert(index, actualRfid);
                        flag = true;
                    }

                    if (flag)
                    {
                        string newStr = String.Join(",", strSearch.ToArray());
                        mapFile[i] = newStr;
                        break;
                    }
                }
            }
            b.Text = actualRfid;


        }

        private void Mouse_Click(Object s, EventArgs e)
        {

            Button btn = new Button();
            btn = (Button)s;

            if (comboBox2.SelectedItem != null && pressed)
            {
                genCounter++;
                textBox1.Text = genCounter.ToString();
                if (checkBox1.Checked && comboBox4.SelectedItem != null)
                {
                    btn.Text = comboBox4.SelectedItem.ToString() + comboBox1.SelectedItem.ToString() + comboBox2.SelectedItem.ToString() + comboBox3.SelectedItem.ToString() + genCounter.ToString("d2");

                }
                else
                {

                    btn.Text = comboBox1.SelectedItem.ToString() + comboBox2.SelectedItem.ToString() + comboBox3.SelectedItem.ToString() + genCounter.ToString("d2");
                }


            }

            else
            {

                string message = "Please Select a Section letter.";
                string caption = "No Section";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;

                // Displays the MessageBox.

                result = MessageBox.Show(message, caption, buttons);

            }

        }

        private void Mouse_Over(Object s, EventArgs e)
        {

            Button btn = new Button();
            btn = (Button)s;

            if ((ModifierKeys & Keys.Control) == Keys.Control)
            {

                if (comboBox2.SelectedItem != null && pressed)
                {
                    genCounter++;
                    textBox1.Text = genCounter.ToString();
                    if (checkBox1.Checked && comboBox4.SelectedItem != null)
                    {
                        btn.Text = comboBox4.SelectedItem.ToString() + comboBox1.SelectedItem.ToString() + comboBox2.SelectedItem.ToString() + comboBox3.SelectedItem.ToString() + genCounter.ToString("d2");

                    }
                    else
                    {

                        btn.Text = comboBox1.SelectedItem.ToString() + comboBox2.SelectedItem.ToString() + comboBox3.SelectedItem.ToString() + genCounter.ToString("d2");
                    }


                }
            }






        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {


        }

        private void Grid_Load(object sender, EventArgs e)
        {
            //Write here all the initial code that should be in the Load event handler.
            btnH = 35;
            btnW = 47;
            induction = 0;
            parking = 0;
            enterindqueue = 0;
            indqueue = 0;
            charging = 0;
            working = 0;
            chutes = 0;
            pressed = false;
            genCounter = 0;
            textBox1.Text = "0";
            rightmost = 0;
            botommost = 0;


            if (serverDirectory != "")
            {
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

                string[] linesGrid = System.IO.File.ReadAllLines(serverDirectory + "\\grid.config");
                string[] linesMap = System.IO.File.ReadAllLines(serverDirectory + "\\grid.map");

                mapFile = linesMap;


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


                        Button btn = new Button();
                        Point newpos = new Point();
                        newpos.X = Convert.ToInt32(elems[0]) * btnW;
                        newpos.Y = Convert.ToInt32(elems[1]) * btnH;
                        btn.Location = newpos;
                        btn.Height = btnH;
                        btn.Width = btnW;
                        btn.Enabled = false;
                        btn.ForeColor = Color.Black;
                        btn.Name = "btn" + newpos.X.ToString() + newpos.Y.ToString();


                        ToolTip ToolTip1 = new ToolTip();
                        ToolTip1.SetToolTip(btn, (newpos.X / btnW).ToString() + "-" + (newpos.Y / btnH).ToString());
                        var xList = linesMap[Convert.ToInt32(elems[1])].ToString().Split(',');
                        string rfid = xList[Convert.ToInt32(elems[0])].ToString();


                        ContextMenu cm = new ContextMenu();
                        cm.Tag = btn.Name;
                        cm.MenuItems.Add(rfid, new EventHandler(RFID_Rep));

                        btn.ContextMenu = cm;
                        btn.Enabled = true;
                        switch (elems[2])
                        {
                            case "12":
                                btn.BackColor = Color.FromArgb(88, 142, 12);
                                //btn.Enabled = true;
                                btn.Click += new EventHandler(Mouse_Click);
                                btn.MouseEnter += new EventHandler(Mouse_Over);

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

                    }

                }

                foreach (Control elem1 in panel2.Controls)
                {

                    if (elem1.GetType() == typeof(Button))
                    {
                        if (elem1.Location.X / btnW > rightmost)
                        {
                            rightmost = elem1.Location.X / btnW;
                        }
                        if (elem1.Location.Y / btnH > botommost)
                        {
                            botommost = elem1.Location.Y / btnH;
                        }

                    }

                }

                for (int i = 1; i <= rightmost; i++)
                {
                    Label lab1 = new Label();
                    Point newpos = new Point();
                    newpos.X = i * btnW;
                    newpos.Y = 0;
                    lab1.Location = newpos;
                    lab1.Height = btnH;
                    lab1.Width = btnW;
                    lab1.Text = i.ToString();
                    lab1.TextAlign = ContentAlignment.MiddleCenter;
                    panel2.Controls.Add(lab1);
                }
                for (int i = rightmost; i > 0; i--)
                {
                    Label lab1 = new Label();
                    Point newpos = new Point();
                    newpos.X = i * btnW;
                    newpos.Y = (botommost + 1) * btnH;
                    lab1.Location = newpos;
                    lab1.Height = btnH;
                    lab1.Width = btnW;
                    lab1.Text = (rightmost - i + 1).ToString();
                    lab1.TextAlign = ContentAlignment.MiddleCenter;
                    panel2.Controls.Add(lab1);
                }

                for (int k = 1; k <= botommost; k++)
                {
                    Label lab2 = new Label();
                    Point newpos = new Point();
                    newpos.X = 0;
                    newpos.Y = k * btnH;
                    lab2.Location = newpos;
                    lab2.Height = btnH;
                    lab2.Width = btnW;
                    lab2.Text = k.ToString();
                    lab2.TextAlign = ContentAlignment.MiddleCenter;
                    panel2.Controls.Add(lab2);
                }

                for (int k = botommost; k > 0; k--)
                {
                    Label lab2 = new Label();
                    Point newpos = new Point();
                    newpos.X = (rightmost + 1) * btnW;
                    newpos.Y = k * btnH;
                    lab2.Location = newpos;
                    lab2.Height = btnH;
                    lab2.Width = btnW;
                    lab2.Text = (botommost - k + 1).ToString();
                    lab2.TextAlign = ContentAlignment.MiddleCenter;
                    panel2.Controls.Add(lab2);
                }


            }


            //info labels

        }





        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Control btn in panel2.Controls)
            {
                if (btn.GetType() == typeof(Button))
                {
                    btn.Text = "";
                }
            }

            foreach (GridMatrix gm in matrix)
            {
                gm.GridValue = "";
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {

            if (comboBox1.SelectedItem != null && comboBox2 != null && comboBox3 != null)
            {
                string message = "You are about to start, please press Ctrl while you move the Mouse over the blocks";
                string caption = "Starting";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;

                // Displays the MessageBox.

                result = MessageBox.Show(message, caption, buttons);

                pressed = true;


            }
            else
            {

                string message = "Please Select a Section letter a Level and a Side to start.";
                string caption = "No Section, Level or Side";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;

                // Displays the MessageBox.

                result = MessageBox.Show(message, caption, buttons);
            }




        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel2.VerticalScroll.Value = 0;
            panel2.HorizontalScroll.Value = 0;
            string[] gridPhi = System.IO.File.ReadAllLines(serverDirectory + "\\grid.physical");

            string[] header = new string[2];
            int counter = 0;
            header[0] = gridPhi[0];
            if (gridPhi[1].StartsWith("//"))
            {
                header[1] = gridPhi[1];
            }
            int size = 0;

            foreach (GridMatrix gm in matrix)
            {
                if (gm.GridValue != "")
                {
                    size++;
                }

            }

            if (header[1] != "")
            {
                size += 2;
            }
            else
            {
                size += 1;
            }
            string[] savestr = new string[size];

            savestr[0] = header[0];
            if (header[1] != "")
            {
                savestr[1] = header[1];
                counter = 2;
            }
            else
            {
                counter = 1;
            }

            // foreach (GridMatrix gm in matrix.OrderBy(p=>p.GridValue))
            foreach (Control cnt in panel2.Controls)
            {
                if (cnt is Button && cnt.Text != "")
                {
                    string newline = cnt.Text + "\t" + "Chute" + "\t" + (cnt.Location.X / btnW).ToString() + "\t" + (cnt.Location.Y / btnH).ToString();
                    savestr[counter++] = newline;
                }


            }

            int empties = matrix.Where(p => p.GridValue == "").Count();
            if (empties > 1)
            {
                string message = "You have empty locations in the Grid. Do you want to proceed?";
                string caption = "Empty locations";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                // Displays the MessageBox.

                result = MessageBox.Show(message, caption, buttons);

                if (result == DialogResult.Yes)
                {

                    string newPath = serverDirectory + "\\grid.physical";
                    if (File.Exists(newPath))
                    {
                        File.WriteAllLines(newPath, savestr);

                    }


                }


            }
            else
            {

                string newPath = serverDirectory + "\\grid.physical";
                if (File.Exists(newPath))
                {
                    File.WriteAllLines(newPath, savestr);

                    string message = "File saved successfully ";
                    string caption = "Save Results";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result;

                    // Displays the MessageBox.

                    result = MessageBox.Show(message, caption, buttons);

                }

            }

            string newPathMap = serverDirectory + "\\grid.map";
            if (File.Exists(newPathMap))
            {
                File.WriteAllLines(newPathMap, mapFile);

            }

        }



        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                saveDir = folderBrowserDialog1.SelectedPath;
            }

            if (saveDir != null)
            {

                foreach (GridMatrix gm in matrix)
                {
                    if (gm.GridValue != "")
                    {
                        var barcodeWriter = new BarcodeWriter();

                        // set the barcode format
                        barcodeWriter.Format = BarcodeFormat.QR_CODE;

                        // write text and generate a 2-D barcode as a bitmap
                        barcodeWriter
                            .Write(gm.GridValue)
                            .Save(@saveDir + "\\" + gm.GridValue + ".bmp");
                    }

                }

                string message = "Barcodes generated successfully ";
                string caption = "Generate Barcode";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;

                // Displays the MessageBox.

                result = MessageBox.Show(message, caption, buttons);

            }
        }



        public Dictionary<string, int> PatternId()
        {
            Dictionary<string, int> letters = new Dictionary<string, int>();

            List<Control> btns = new List<Control>();

            foreach (Control elem1 in panel2.Controls)
            {

                if (elem1.GetType() == typeof(Button))
                {
                    if (elem1.Text != "")
                    {
                        btns.Add(elem1);
                        if (elem1.Text.Length == 1)
                        {
                            if (letters.Where(p => p.Key == elem1.Text).Count() == 0)
                            {
                                letters.Add(elem1.Text, 0);
                            }
                        }
                        else if (elem1.Text.Length > 1)
                        {

                            string lett = elem1.Text.Substring(1, 1);
                            if (letters.Where(p => p.Key == lett).Count() == 0)
                            {
                                letters.Add(lett, 0);
                            }

                        }
                    }
                }

            }
            Dictionary<string, int> auXletters = new Dictionary<string, int>(letters);

            if (btns.Count > 0)
            {
                foreach (var lett in letters)
                {
                    int Xlft = 0;
                    int Xrgt = 0;
                    int Ytop = 0;
                    int Ybot = 0;

                    List<Control> btnsLet = btns.Where(p => p.Text == lett.Key || (p.Text.Length > 2 && p.Text.Substring(1, 1) == lett.Key)).ToList();

                    foreach (Button btn in btnsLet)
                    {
                        if (Xlft == 0)
                        {
                            Xlft = btn.Location.X;
                            Ytop = btn.Location.Y;
                        }
                        else
                        {

                            if (btn.Location.X > Xrgt)
                            {
                                Xrgt = btn.Location.X;
                            }
                            if (btn.Location.Y > Ybot)
                            {
                                Ybot = btn.Location.Y;
                            }

                        }
                    }

                    //Analizing coordinates to establish case.

                    if ((Xlft == Xrgt) && (Ytop < Ybot)) //case 1 vertical line
                    {
                        var ctrls = panel2.Controls.Cast<Control>();

                        int elemCount = ctrls.Where(l => l.Location.X < Xlft && l.Location.Y < Ybot && l.Location.Y > Ytop).Count();

                        if (elemCount > 0)
                        {
                            auXletters[lett.Key] = 7;
                        }
                        else
                        {

                            auXletters[lett.Key] = 1;
                        }


                    }

                    if ((Xrgt - Xlft) < (Ybot - Ytop))//case 2 U shape
                    {

                        int countBt = btnsLet.Where(k => k.Location.Y == Ytop).Count();
                        var ctrls = panel2.Controls.Cast<Control>();

                        int elemCount = ctrls.Where(l => l.Location.Y > Ybot && l.Location.X > Xlft && l.Location.X < Xrgt).Count();

                        if (countBt <= 2 && elemCount > 0)
                        {
                            auXletters[lett.Key] = 2;
                        }

                    }

                    if ((Xrgt - Xlft) < (Ybot - Ytop))//case 3 n shape
                    {

                        int countBt = btnsLet.Where(k => k.Location.Y == Ytop).Count();
                        var ctrls = panel2.Controls.Cast<Control>();

                        int elemCount = ctrls.Where(l => l.Location.Y < Ytop && l.Location.X > Xlft && l.Location.X < Xrgt).Count();

                        if (countBt >= 2 && elemCount > 0)
                        {
                            auXletters[lett.Key] = 3;
                        }

                    }

                    if ((Xrgt - Xlft) > (Ybot - Ytop))//case 4 C shape
                    {

                        int countBt = btnsLet.Where(k => k.Location.X == Xlft).Count();

                        var ctrls = panel2.Controls.Cast<Control>();

                        int elemCount = ctrls.Where(l => l.Location.X < Xlft && l.Location.Y > Ytop && l.Location.Y < Ybot).Count();

                        if (countBt >= 2 && elemCount > 0)
                        {
                            auXletters[lett.Key] = 4;
                        }

                    }

                    if ((Xrgt - Xlft) > (Ybot - Ytop))//case 5  inverted C shape
                    {

                        int countBt = btnsLet.Where(k => k.Location.X == Xlft).Count();

                        var ctrls = panel2.Controls.Cast<Control>();

                        int elemCount = ctrls.Where(l => l.Location.X > Xrgt && l.Location.Y > Ytop && l.Location.Y < Ybot).Count();

                        if (countBt <= 2 && elemCount > 0)
                        {
                            auXletters[lett.Key] = 5;
                        }

                    }

                    if ((Xlft < Xrgt) && (Ytop == Ybot)) //case 6 horizontal line
                    {
                        var ctrls = panel2.Controls.Cast<Control>();

                        int elemCount = ctrls.Where(l => l.Location.Y < Ytop && l.Location.X < Xrgt && l.Location.X > Xlft).Count();

                        if (elemCount > 0)
                        {
                            auXletters[lett.Key] = 8;
                        }
                        else
                        {
                            auXletters[lett.Key] = 6;
                        }

                    }

                }
            }

            return auXletters;
        }



        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Text = "0";
            genCounter = 0;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            genCounter = Convert.ToInt32(textBox1.Text);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                comboBox4.Enabled = true;
            }
            else
            {
                comboBox4.Enabled = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //// Hide the form so that it does not appear in the screenshot
                //this.Hide();
                //// Set the bitmap object to the size of the screen
                //bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
                //// Create a graphics object from the bitmap
                //gfxScreenshot = Graphics.FromImage(bmpScreenshot);
                //// Take the screenshot from the upper left corner to the right bottom corner
                //gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
                //// Save the screenshot to the specified path that the user has chosen
                //bmpScreenshot.Save(saveFileDialog1.FileName, ImageFormat.Png);
                //// Show the form again
                //this.Show();

                //var frm = Form.ActiveForm;
                var frm = panel2;
                using (var bmp = new Bitmap(frm.Width, frm.Height))
                {
                    frm.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                    bmp.Save(saveFileDialog1.FileName, ImageFormat.Png);
                }

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;
            List<string> barcodesData = new List<string>();
            string[] gridPhi = System.IO.File.ReadAllLines(serverDirectory + "\\grid.physical");
            List<string> locations = new List<string>();
            List<string> barcodeFile = new List<string>();
            Random random = new Random();

            string[] actions = { "PUT", "PUT BACK", "PUT FRONT" };

            foreach (var item in gridPhi)
            {
                var newstring = item.Split('\t');
                if (newstring.Length > 1)
                {
                    locations.Add(newstring[0]);
                }
            }
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog1.FileName;
                barcodesData = System.IO.File.ReadAllLines(filePath).ToList();
            }

            if (barcodesData.Count() > 0)
            {
                int bcIndex = 0;
                foreach (var location in locations)
                {
                    if (bcIndex < barcodesData.Count())
                    {
                        string newLine = barcodesData[bcIndex++] + '\t' + location + '\t' + actions[random.Next(0, actions.Length)] + '\t' + '\t' + "default";
                        barcodeFile.Add(newLine);
                    }

                    if (bcIndex >= barcodesData.Count())
                    {
                        bcIndex = 0;
                    }

                }


            }

            if (barcodeFile.Count() > 0)//File was populated
            {
                DialogResult result2 = MessageBox.Show("Do you want to save the barcode.txt file?",
               "Save the file",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Question);

                if (result2 == DialogResult.Yes)
                {
                    saveFileDialog1.FileName = "barcode";
                    saveFileDialog1.DefaultExt = "txt";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {

                        using (StreamWriter sw = new StreamWriter(saveFileDialog1.FileName))
                        {
                            sw.WriteLine("//barcode(*)" + '\t' + "chute(*)" + '\t' + "user action(*)" + '\t' + "tray angle");
                            foreach (var item in barcodeFile)
                            {
                                sw.WriteLine(item);
                            }
                            sw.Flush();
                            sw.Close();

                        }




                    }

                }

            }
        }
    }
}
