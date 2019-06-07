using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Configurator
{
    static class ComboImage
    {
        //ComboBox Image=========================================================================
        // Set up the ComboBox to display color samples and their names.
        public static void DisplayColorSamples(this ComboBox cbo, Dictionary<Color,string> colors)
        {
            // Make the ComboBox owner-drawn.
            cbo.DrawMode = DrawMode.OwnerDrawFixed;

            // Add the colors to the ComboBox's items.
            cbo.Items.Clear();
            cbo.DataSource = new BindingSource(colors, null);
            cbo.DisplayMember = "Value";
            cbo.ValueMember = "Value";
            //foreach (var color in colors)
            //{
            //    cbo.Items.Add(color);
              

            //}
               

            // Subscribe to the DrawItem event.
            cbo.DrawItem += cboColorSample_DrawItem;
        }

        // Margins around owner drawn ComboBoxes.
        private const int MarginWidth = 2;
        private const int MarginHeight = 2;

        // Draw a ComboBox item that is displaying a color sample
        private static void cboColorSample_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            // Clear the background appropriately.
            e.DrawBackground();

            // Draw the color sample.
            int hgt = e.Bounds.Height - 2 * MarginHeight;
            Rectangle rect = new Rectangle(
                e.Bounds.X + MarginWidth,
                e.Bounds.Y + MarginHeight,
                hgt, hgt);
            ComboBox cbo = sender as ComboBox;
            var elem = (KeyValuePair<Color, string>)cbo.Items[e.Index];
            Color color = (Color)elem.Key;
            using (SolidBrush brush = new SolidBrush(color))
            {
                e.Graphics.FillRectangle(brush, rect);
            }

            // Outline the sample in black.
            e.Graphics.DrawRectangle(Pens.Black, rect);

            // Draw the color's name to the right.
            using (Font font = new Font(cbo.Font.FontFamily,cbo.Font.Size , FontStyle.Regular))
            {
                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Center;
                    int x = hgt + 2 * MarginWidth;
                    int y = e.Bounds.Y + e.Bounds.Height / 2;
                    e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    e.Graphics.DrawString(elem.Value, font, Brushes.Black, x, y, sf);
                }
            }

            // Draw the focus rectangle if appropriate.
            e.DrawFocusRectangle();
        }
        // Set up the ComboBox to display images.
        public static void DisplayImages(this ComboBox cbo, Image[] images)
        {
            // Make the ComboBox owner-drawn.
            cbo.DrawMode = DrawMode.OwnerDrawVariable;

            // Add the images to the ComboBox's items.
            cbo.Items.Clear();
            foreach (Image image in images) cbo.Items.Add(image);

            // Subscribe to the DrawItem event.
            cbo.MeasureItem += cboDrawImage_MeasureItem;
            cbo.DrawItem += cboDrawImage_DrawItem;
        }

        // Measure a ComboBox item that is displaying an image.
        private static void cboDrawImage_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index < 0) return;

            // Get this item's image.
            ComboBox cbo = sender as ComboBox;
            Image img = (Image)cbo.Items[e.Index];
            e.ItemHeight = img.Height + 2 * MarginHeight;
            e.ItemWidth = img.Width + 2 * MarginWidth;
        }
        // Draw a ComboBox item that is displaying an image.
        private static void cboDrawImage_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            // Clear the background appropriately.
            e.DrawBackground();

            // Draw the image.
            ComboBox cbo = sender as ComboBox;
            Image img = (Image)cbo.Items[e.Index];
            float hgt = e.Bounds.Height - 2 * MarginHeight;
            float scale = hgt / img.Height;
            float wid = img.Width * scale;
            RectangleF rect = new RectangleF(
                e.Bounds.X + MarginWidth,
                e.Bounds.Y + MarginHeight,
                wid, hgt);
            e.Graphics.InterpolationMode =
                InterpolationMode.HighQualityBilinear;
            e.Graphics.DrawImage(img, rect);

            // Draw the focus rectangle if appropriate.
            e.DrawFocusRectangle();
        }
    }
}
