using System;
using System.Collections.Generic;
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
        private ScanPoints sc;
        public Bitmap image;
        string filename;
        public Thread thread;
        float offset, rastersize;
        Form1 obj;

        public Surface(string filename, float offset, float rastersize, Form1 obj)
        {
            this.filename = filename;
            this.offset = offset;
            this.rastersize = rastersize;
            this.obj = obj;
            //thread = new Thread(Run);
            //Thread.CurrentThread.IsBackground = true;
            //thread.Start();
            //Run();


          
            
            ;
            //sc.Surface();
        }

        public void Run()
        {
            
            sc = new ScanPoints(filename, rastersize, offset, obj);
            //sc.Surface(0.01f, 0.5);
            image = sc.SurfaceMap;
        }
    }
}
