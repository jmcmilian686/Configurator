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
    public partial class NewMapGrid : Form
    {
        public NewMapGrid()
        {
            InitializeComponent();
        }

        private void NewMapGrid_Load(object sender, EventArgs e)
        {
            textBox1.Text = "0";
            textBox2.Text = "0";
            textBox4.Text = "10";
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //======creating the grid
            int counter = 1;
            int height = Convert.ToInt32(textBox2.Text);
            int width = Convert.ToInt32(textBox1.Text);
            string[,] matrix = new string[height, width];
            string grid ="//Grid Map File";
            if (height>0 && width>0)
            {
               
                //==establish the value lenght

                int charLenght = Convert.ToInt32(textBox4.Text)-1;
                for (int i = 0; i < height; i++)
                {
                    //string gridline = "";
                    for (int k = 0; k < width; k++)
                    {
                        int fillingLngth = charLenght - counter.ToString().Length;
                        string filling = "";
                        for (int l = 0; l < fillingLngth; l++)
                        {
                            filling = filling + "0";
                        }
                        matrix[i, k] = "1" + filling + counter.ToString();
                        //gridline = gridline + ("1" + filling + counter.ToString()+ ",");
                        counter++;
                    }

                    //grid = grid + Environment.NewLine + gridline;
                    //gridline = "";
                }

                for (int i = 0; i < width; i++)
                {
                    string gridline = "";
                    for (int k = 0; k < height; k++)
                    {
                        gridline = gridline + matrix[k,i] + ",";
                    }
                    grid = grid + Environment.NewLine + gridline;
                    gridline = "";
                }


            }
           
            //=====end Grid creation======


            // Displays a SaveFileDialog so the user can save the file
            // assigned to Button2.  
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            //saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            saveFileDialog1.Title = "Save the grid map";
            saveFileDialog1.ShowDialog();
            //saveFileDialog1.FileName = "file.map";
            // If the file name is not an empty string open it for saving.  
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.  
                System.IO.FileStream fs =(System.IO.FileStream)saveFileDialog1.OpenFile();
                // Saves the Image in the appropriate ImageFormat based upon the  
                // File type selected in the dialog box.  

                string dataasstring = grid; //your data
                byte[] info = new UTF8Encoding(true).GetBytes(dataasstring);
                fs.Write(info, 0, info.Length);

                // writing data in bytes already
                byte[] data = new byte[] { 0x0 };
                fs.Write(data, 0, data.Length);

                
                fs.Close();
            }

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
