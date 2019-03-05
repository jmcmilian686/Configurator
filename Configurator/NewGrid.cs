using Configurator.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Configurator
{
    public partial class NewGrid : Form
    {
        public string serverDirectory { get; set; }
        public int gridW { get; set; }
        public int gridH { get; set; }
        public Dictionary<Point, string> map { get; set; }
        public struct block
        {
            public int blkType;
            public int access;
        };
        public Dictionary<Point, block> config { get; set; }
        public int imgH { get; set; }
        public int imgW { get; set; }


        public NewGrid(string dir)
        {
                this.serverDirectory = dir;
                InitializeComponent();
        }

        private void NewGrid_Load(object sender, EventArgs e)
        {
           
            




        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            
        }

        private void NewGrid_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            string[] linesGrid = System.IO.File.ReadAllLines(serverDirectory + "\\grid.map");
            imgH = 27;
            imgW = 27;

            Graphics g;
            Pen pen1 = new Pen(Color.Black, 1F);
            pen1.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
            g = this.CreateGraphics();


            if (linesGrid.Count() > 0)
            {
                gridH = linesGrid.Count();


                int lineCnt = 0;
                foreach (string line in linesGrid)
                {
                    string[] row = linesGrid[0].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    gridW = row.Count();

                    int cellCnt = 0;
                    foreach (string cell in row)
                    {
                        Point location = new Point()
                        {
                            X = cellCnt,
                            Y = lineCnt
                        };
                        map = new Dictionary<Point, string>();
                        map.Add(location, cell);

                        Rectangle rect = new Rectangle(location.X*imgW, location.Y*imgH, imgW, imgH);

                        Pen blackPen = new Pen(Color.Black, 2);

                        e.Graphics.DrawRectangle(blackPen, rect);
                       // e.Graphics.FillRectangle(Brushes.Blue, rect);



                        cellCnt++;

                    }
                    lineCnt++;
                }

            }
        }
    }
}
