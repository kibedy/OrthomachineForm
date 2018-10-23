﻿using Emgu.CV;
using Emgu.CV.Structure;
using OM_Form;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OM_Form.View;
using System.ComponentModel;

//using Emgu.CV;
//using Emgu.Util;
//using Emgu.CV.Structure;

namespace ortomachine.Model
{
    public class ScanPoints
    {
        LinkedList<Points> PointList = new LinkedList<Points>();
        //string path = "";
        UInt16[,] surface;
        byte[,,] RGBsurface;
        ushort xwidth;
        ushort yheight;
        int filetype;
        float offset;    //offset: black border
        float rastersize;
        double Xmax, Zmax, X0, Z0;
        public Bitmap SurfaceMap;
        string filename;
        Form1 form1;
        //Form1 obj= null;
        public int lc;
        int actline;
        public int procbarvalue;
        //BackgroundWorker worker;
        //public int progress;
        //public delegate void ProgressUpdate(int value);
        //public event ProgressUpdate OnProgressupdate;


        //public ScanPoints(string filename, float rastersize, float offset, Form1 obj, BackgroundWorker worker, DoWorkEventArgs e)
        public ScanPoints(string filename, float rastersize, float offset, Form1 obj)
        {
            this.rastersize = rastersize;
            this.offset = offset;
            this.filename = filename;
            form1 = obj;
            procbarvalue = 0;            
            startprocess(filename);
            procbarvalue = 0;
            //this.worker = worker;
            lc = 0;            

        }

        public void startprocess(string filename)
        {
            try
            {
                preprocess(filename);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Invalid file");
            }

            BoundingBox();
            Surface(this.rastersize, this.offset);
            SurfaceMap = saveSurface();

        }

        private void preprocess(string fn)
        {
            StreamReader srl = new StreamReader(fn);
            while (!srl.EndOfStream)
            {
                srl.ReadLine();
                lc++;
            }
            srl.Close();


            StreamReader sr = new StreamReader(fn);
            char[] delimiterChars = { ' ', ',', '\t' };            

            string line = sr.ReadLine();
            string[] numbers = line.Split(delimiterChars);
            filetype = numbers.Length;
            actline = 1;
            ;
            if (filetype == 3)
            {
                double X = double.Parse(numbers[0], System.Globalization.CultureInfo.InvariantCulture);
                double Y = double.Parse(numbers[1], System.Globalization.CultureInfo.InvariantCulture);
                double Z = double.Parse(numbers[2], System.Globalization.CultureInfo.InvariantCulture);

                Points point = new Points(X, Y, Z);
                PointList.AddFirst(point);
                procbarvalue = 0;

                while (!sr.EndOfStream)
                {                                        
                    line = sr.ReadLine();
                    numbers = line.Split(delimiterChars);

                    //procbarvalue = CountLines(lc);
                    CountLines(lc);
                    ;
                    //form1.backgroundWorker1.ReportProgress(pro);
                    

                    {
                        point = new Points(
                            double.Parse(numbers[0], System.Globalization.CultureInfo.InvariantCulture),
                            double.Parse(numbers[1], System.Globalization.CultureInfo.InvariantCulture),
                            double.Parse(numbers[2], System.Globalization.CultureInfo.InvariantCulture));
                        PointList.AddLast(point);
                    }
                }
            }

            if (filetype == 4)      //intensity only
            {
                Points point = new Points(
                    double.Parse(numbers[0], System.Globalization.CultureInfo.InvariantCulture),
                    double.Parse(numbers[1], System.Globalization.CultureInfo.InvariantCulture),
                    double.Parse(numbers[2], System.Globalization.CultureInfo.InvariantCulture),
                    int.Parse(numbers[3]));
                PointList.AddFirst(point);

                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    numbers = line.Split(delimiterChars);
                    {
                        point = new Points(
                            double.Parse(numbers[0], System.Globalization.CultureInfo.InvariantCulture),
                            double.Parse(numbers[1], System.Globalization.CultureInfo.InvariantCulture),
                            double.Parse(numbers[2], System.Globalization.CultureInfo.InvariantCulture),
                            int.Parse(numbers[3]));
                        PointList.AddLast(point);
                    }
                }
            }

            if (filetype == 7)
            {
                Points point = new Points(
                        double.Parse(numbers[0], System.Globalization.CultureInfo.InvariantCulture),
                        double.Parse(numbers[1], System.Globalization.CultureInfo.InvariantCulture),
                        double.Parse(numbers[2], System.Globalization.CultureInfo.InvariantCulture),
                        int.Parse(numbers[3]),
                        int.Parse(numbers[4]),
                        int.Parse(numbers[5]),
                        int.Parse(numbers[6]));


                PointList.AddFirst(point);

                while (!sr.EndOfStream)
                {

                    line = sr.ReadLine();
                    numbers = line.Split(delimiterChars);
                    {
                        point = new Points(
                            double.Parse(numbers[0], System.Globalization.CultureInfo.InvariantCulture),
                            double.Parse(numbers[1], System.Globalization.CultureInfo.InvariantCulture),
                            double.Parse(numbers[2], System.Globalization.CultureInfo.InvariantCulture),
                            int.Parse(numbers[3]),
                            int.Parse(numbers[4]),
                            int.Parse(numbers[5]),
                            int.Parse(numbers[6]));
                        PointList.AddLast(point);
                    }
                }
            }
        }

        public void Surface(float rastersize, double offset)
        {
            //Points actual = PointList.First();
            xwidth = (ushort)((((Xmax - X0) + 2 * offset) / rastersize) + 1);
            yheight = (ushort)((((Zmax - Z0) + 2 * offset) / rastersize) + 1);

            surface = new UInt16[xwidth, yheight];
            if (filetype == 7)
            {
                RGBsurface = new byte[xwidth, yheight, 3];

            }


            double maxY = -99999999;

            foreach (Points item in PointList)
            {
                uint i = (uint)(((item.X - X0) + offset) / rastersize);
                uint j = (uint)(((item.Z - Z0) + offset) / rastersize);
                if (item.Y > maxY || surface[i, j] > item.Y)
                {
                    surface[i, j] = (ushort)(item.Y * 1000);    //computing in mm                
                }

            }
            ;

        }

        private void CountLines(int lineCount)
        {
            actline++;
            if (actline-2>lineCount/10)
            {
                procbarvalue += 10;
                actline = 0;
                form1.backgroundWorker1.ReportProgress(procbarvalue);
                /*if (form1.progressBar1.InvokeRequired)
                {
                    form1.progressBar1.BeginInvoke((MethodInvoker)delegate ()
                    {
                        form1.progressBar1.Value = procbarvalue;
                        Application.DoEvents();
                    });
                }
                form1.progressBar1.Value = procbarvalue;
                //form1.progressBar1.Value = procbarvalue;
                */
                ;
            }
            
            //int ddd= (int)((actline*100) / lineCount) ;
            //return ddd;
            
            ;
        }

        private void BoundingBox()
        {
            Xmax = 0; Zmax = 0; X0 = 999999999; Z0 = 999999999; //global coordinates
            foreach (Points item in PointList)
            {
                if (item.X < X0)
                {
                    X0 = item.X;
                }
                if (item.X > Xmax)
                {
                    Xmax = item.X;
                }
                if (item.Z < Z0)
                {
                    Z0 = item.Z;
                }
                if (item.Z > Zmax)
                {
                    Zmax = item.Z;
                }
            }


        }

        public Bitmap saveSurface()
        {
            
             

            Image<Gray, ushort> image = new Image<Gray, ushort>(xwidth, yheight);
            for (int x = 0; x < xwidth; x++)
            {
                for (int y = 0; y < yheight; y++)
                {
                    image.Data[yheight - y - 1, x, 0] = surface[x, y];
                }
            }
            image.Save(form1.SavePath+"\\"+"surface.png");
            StreamWriter sw = new StreamWriter(form1.SavePath + "\\" + "surface.xyz");
            sw.WriteLine("{0} {1} {2} {3}",X0.ToString(), Z0.ToString(), rastersize.ToString(), offset.ToString());
            sw.Close();
            return image.ToBitmap();


        }



    }
}
