using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YoutubeDownloader.Lib
{
    public partial class MyProgressBar : ProgressBar
    {
        public MyProgressBar()
        {//
         //  InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            //Draw percentage
            Rectangle rect = this.ClientRectangle;
            Graphics g = pe.Graphics;
            ProgressBarRenderer.DrawHorizontalBar(g, rect);

            if (this.Value > 0)
            {
                Rectangle clip = new Rectangle(rect.X, rect.Y, (int)Math.Round(((float)this.Value / this.Maximum) * rect.Width), rect.Height);
                ProgressBarRenderer.DrawHorizontalChunks(g, clip);
            }
            double v = Convert.ToDouble((float)this.Value / (float)this.Maximum * 100.00);
            using (Font f = new Font(FontFamily.GenericMonospace, 18))
            {
                string _v = Value + " / " + Maximum + "  " + string.Format("{0:0.000} %", v);
                SizeF size = g.MeasureString(_v, f);
                Point location = new Point((int)((rect.Width / 2) - (size.Width / 2)), (int)((rect.Height / 2) - (size.Height / 2) + 2));
                g.DrawString(_v, f, Brushes.Black, location);
            }
            //base.OnPaint(pe);
        }
    }
}
