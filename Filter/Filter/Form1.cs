using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using MathNet.Numerics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Filter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)//open
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            string Path = openFileDialog1.FileName;

           Bm = new Bitmap(Path);

            output f = new output(Bm);
            f.ShowDialog();


        }



        private void button3_Click(object sender, EventArgs e)//show
        {
            output f = new output(Bm);
            f.ShowDialog();
        }

        private void button2_Click_1(object sender, EventArgs e)// black white
        {
            Bitmap filter = new Bitmap(Bm);
            for (int x = 0; x < filter.Width; x++)
                for (int y = 0; y < filter.Height; y++)
                {
                    Color c = filter.GetPixel(x, y);
                    byte bw = (byte)(0.3 * c.R + 0.59 * c.G + 0.11 * c.B);

                    filter.SetPixel(x, y, Color.FromArgb(c.A, bw, bw, bw));
                }
            output up = new output(filter);
            up.ShowDialog();
            Bm = filter;
        }

        private void button5_Click(object sender, EventArgs e) //impuls
        {
            Bitmap filter = new Bitmap(Bm);
            Random r = new Random();
            for (int x = 0; x < filter.Width; x+=r.Next()%50+50)
                for (int y = 0; y < filter.Height; y+=r.Next()%50+50)
                {
                    Color c = filter.GetPixel(x, y);
                    

                    filter.SetPixel(x, y, Color.FromArgb(c.A,  255, 255, 255));
                }
            output up = new output(filter);
            up.ShowDialog();
            Bm = filter;

        }

        private void button4_Click(object sender, EventArgs e)//gauss
        {
            Bitmap filter = new Bitmap(Bm);
            Random r = new Random();
           // double[,] m = new double[1000,1000];
             for (int x = 0; x < filter.Width; x += r.Next() % 10+5)
                 for (int y = 0; y < filter.Height; y += r.Next() % 10+5)
                 {


                    double coef = Math.Abs(SampleGaussian(r, 0, 2));
                    try
                    {
                        Color c = filter.GetPixel(x, y);
                        filter.SetPixel(x, y,
                            Color.FromArgb(c.A, (int)(c.R*coef), (int)(c.G * coef), (int)(c.B * coef)));
                    }
                    catch (Exception)
                    {

                    }
                           
                 }


    
            output up = new output(filter);
            up.ShowDialog();
            Bm = filter;
        }
        public static double SampleGaussian(Random random, double mean, double stddev)
        {
            // The method requires sampling from a uniform random of (0,1]
            // but Random.NextDouble() returns a sample of [0,1).
            double x1 = 1 - random.NextDouble();
            double x2 = 1 - random.NextDouble();

            double y1 = Math.Sqrt(-2.0 * Math.Log(x1)) * Math.Cos(2.0 * Math.PI * x2);
            return y1 * stddev + mean;
        }

        private void button6_Click(object sender, EventArgs e)// gauss filet
        {
            double[,] gauss = new double[3, 3];
            double sum = 0;
            for(int i=0;i<3;i++)
                for (int j = 0; j < 3; j++)
                {
                    double index = i * i + j * j; //((i-1) * (i-1) + (j-1) * (j-1)) * -1;
                    gauss[i, j] = Math.Pow(Constants.E,
                        (index) / 18.0) / Constants.Pi2 / 9.0*10/1.9;    //1.47;
                    sum += gauss[i, j];
                }
            Bitmap filter = new Bitmap(Bm);
            for (int x = 1; x < filter.Width-1; x++)
                for (int y = 1; y < filter.Height-1; y++)
                {
                    double coef = 0;
                    for (int i = -1; i < 2; i++)
                        for (int j = -1; j < 2; j++)
                        {
                            Color c1 = filter.GetPixel(x+i, y+j);
                            coef += gauss[i + 1, j + 1] * c1.R;

                        }
                   // coef = coef / 9;
                    Color c = filter.GetPixel(x, y);
                    filter.SetPixel(x, y,
                            Color.FromArgb(c.A, (int)coef, (int)coef, (int)coef));
                }
            output up = new output(filter);
            up.ShowDialog();
            Bm = filter;

        }

        private void button8_Click(object sender, EventArgs e)//mediana
        {
            Bitmap filter = new Bitmap(Bm);
            for (int x = 1; x < filter.Width - 1; x++)
                for (int y = 1; y < filter.Height - 1; y++)
                {
                    int[] mediana = new int[9];

                    for (int i = 0; i < 3; i++)
                        for (int j = 0; j < 3; j++)
                        {
                            Color c1 = filter.GetPixel(x + i-1, y + j-1);
                            mediana[i * 3 + j] = c1.R;
                            
                        }
                    Array.Sort(mediana);
                    Color c = filter.GetPixel(x, y);
                    filter.SetPixel(x, y,
                            Color.FromArgb(c.A, mediana[4], mediana[4], mediana[4]));
                }
            output up = new output(filter);
            up.ShowDialog();
            Bm = filter;
        }

        private void button7_Click(object sender, EventArgs e)//rectangl
        {
            double[,] gauss = new double[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    gauss[i, j] = 1.0/ 9.0;
                }
            Bitmap filter = new Bitmap(Bm);
            for (int x = 1; x < filter.Width - 1; x++)
                for (int y = 1; y < filter.Height - 1; y++)
                {
                    double coef = 0;
                    for (int i = -1; i < 2; i++)
                        for (int j = -1; j < 2; j++)
                        {
                            Color c1 = filter.GetPixel(x + i, y + j);
                            coef += gauss[i + 1, j + 1] * c1.R;

                        }
                    // coef = coef / 9;
                    Color c = filter.GetPixel(x, y);
                    filter.SetPixel(x, y,
                            Color.FromArgb(c.A, (int)coef, (int)coef, (int)coef));
                }
            output up = new output(filter);
            up.ShowDialog();
            Bm = filter;

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
