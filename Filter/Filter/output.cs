using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Filter
{
    public partial class output : Form
    {
        public output(Bitmap bm)
        {
            InitializeComponent();
            Width = bm.Width;
            Height = bm.Height+150;

            pictureBox1.Image = bm;
            pictureBox1.Width= bm.Width;
            pictureBox1.Height = bm.Height;
            label1.Location = new Point(label1.Location.X, bm.Height + 30);
            label2.Location = new Point(label2.Location.X, bm.Height + 30);
            label3.Location = new Point(label3.Location.X, bm.Height + 80);
            label4.Location = new Point(label4.Location.X, bm.Height + 80);
            label1.Text = "Контрастность = " + Contrast(bm);
            label2.Text = "Ср. кв. контрастность = " + sqContrast(bm);
            label3.Text = "Полнота яркости = " + Polnota(bm);
            label4.Text = "Уровень адаптации = " + adapt(bm);

        }
        public static double Contrast(Bitmap _bm)
        {
            int max = -1;
            int min = 255 * 3;
            for (int x = 0; x < _bm.Width; x++)
                for (int y = 0; y < _bm.Height; y++)
                {
                    Color c = _bm.GetPixel(x, y);
 
                    int current = c.R + c.G + c.B;

                    if (current > max)
                        max = current;
                    if (min > current)
                        min = current;

                }
            double d1 = max - min;
            double d2 = max + min;
            return d1/d2;
        }
        public static double sqContrast(Bitmap _bm)
        {
            double ave = 0;
            for (int x = 0; x < _bm.Width; x++)
                for (int y = 0; y < _bm.Height; y++)
                {
                    Color c = _bm.GetPixel(x, y);

                    ave+= c.R + c.G + c.B;
                }
            ave = ave / _bm.Width / _bm.Height;

            double res = 0;
            for (int x = 0; x < _bm.Width; x++)
                for (int y = 0; y < _bm.Height; y++)
                {
                    Color c = _bm.GetPixel(x, y);

                    res += (c.R + c.G + c.B- ave)*(c.R + c.G + c.B - ave);
                }
            res=res/ _bm.Width / _bm.Height;
            return Math.Sqrt(res);

        }
        public static double Polnota(Bitmap _bm)
        {

            int max = -1;
            int min = 255 * 3;
            for (int x = 0; x < _bm.Width; x++)
                for (int y = 0; y < _bm.Height; y++)
                {
                    Color c = _bm.GetPixel(x, y);

                    int current = c.R + c.G + c.B;

                    if (current > max)
                        max = current;
                    if (min > current)
                        min = current;

                }

            return 256.0/ (max + min);
        }
         public static double adapt(Bitmap _bm )
        {
            int max = -1;
            double ave = 0;
            for (int x = 0; x < _bm.Width; x++)
                for (int y = 0; y < _bm.Height; y++)
                {
                    Color c = _bm.GetPixel(x, y);

                    int current = c.R + c.G + c.B;
                    ave += c.R + c.G + c.B;
            if (current > max)
                        max = current;
                }
            double d1= max/2;
            ave = ave / _bm.Width / _bm.Height;
            return 1- (ave - d1) / d1;
        }
    }
}
