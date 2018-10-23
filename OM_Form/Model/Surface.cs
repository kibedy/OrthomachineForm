using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OM_Form;
using ortomachine.Model;

namespace ortomachine.Model
{
    public class Surface
    {        
        public ScanPoints sc;
        public Bitmap image;
        string filename;
        public Thread thread;
        float offset, rastersize;
        Form1 obj;
        public int procbarvalue;


        public Surface(string filename, float offset, float rastersize, Form1 obj)
        {
            this.filename = filename;
            this.offset = offset;
            this.rastersize = rastersize;
            this.obj = obj;            
        }

        //public Bitmap Run(BackgroundWorker worker, DoWorkEventArgs e)
        public Bitmap Run()
        {            
            //sc = new ScanPoints(filename, rastersize, offset, obj);
            //sc = new ScanPoints(filename, rastersize, offset, obj, worker, e);
            sc = new ScanPoints(filename, rastersize, offset, obj);            
            image = sc.SurfaceMap;            
            return image;
            
        }
    }
}
