using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ortomachine.Model;
using Microsoft.Win32;
using System.Threading;
using OM_Form.View;


namespace OM_Form
{
    public partial class Form1: Form
    {
        public string filename;
        //Thread thread;
        public float offset, rastersize;
        public string SavePath;
        Surface sf;
        public BackgroundWorker backgroundWorker1;
        

        
        public Form1()
        {
            InitializeComponent();
            openToolStripMenuItem.Enabled = false;
            SavePath = null;
            InitializeBackgroundWorker();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
        }

        private void InitializeBackgroundWorker()
        {
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.ProgressChanged +=new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
        }

        private void openToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            if (GetPC_Params()== true)
            {
                // Start the asynchronous operation.
                backgroundWorker1.RunWorkerAsync();               
            }
            
            
        }

      
      

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            //e.Result= sf.Run(worker, e);
            e.Result = sf.Run();
            //progressBar1.BeginInvoke((MethodInvoker)delegate { progressBar1.Value =sf.sc.procbarvalue; });


        }

        private bool GetPC_Params()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".asc";
            ofd.Filter = "Point cloud (*.txt)|*.txt|ASCII file (*.asc)|*.asc";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filename = ofd.FileName;
                using (var form2 = new Form2(this))
                {
                    form2.ShowDialog();
                    if (form2.DialogResult == DialogResult.OK)
                    {
                        sf = new Surface(filename, offset, rastersize, this);
                        
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            else
            {
                return false;
            }
        }

        // This event handler deals with the results of the
        // background operation.
        private void backgroundWorker1_RunWorkerCompleted(
            object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown.
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {

            }
            else
            {
      
            }
            pictureBox1.Image = (Bitmap)e.Result;
            this.progressBar1.Value = 100;
        }

        // This event handler updates the progress bar.
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ;
            //progressBar1.BeginInvoke((MethodInvoker)delegate { progressBar1.Value = sf.sc.procbarvalue; });
            this.progressBar1.Value = e.ProgressPercentage;
        }
      

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var folderBrowserDialog1 = new FolderBrowserDialog();


            string initpath = "g:\\_Magán\\_Óbudai Egyetem\\Szakdolgozat 1\\OrthomachineForm\\ortomachine_project\\";
            folderBrowserDialog1.SelectedPath = initpath;
            DialogResult result = folderBrowserDialog1.ShowDialog();


            if (result == DialogResult.OK)
            {

                if (SavePath != null)
                {
                    if (MessageBox.Show("Close this and create new Project?", "Create new project?", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        SavePath = folderBrowserDialog1.SelectedPath;
                        openToolStripMenuItem.Enabled = true;
                    }
                }
                else
                {
                    SavePath = folderBrowserDialog1.SelectedPath;
                    openToolStripMenuItem.Enabled = true;
                }
            }
        }
       

        #region unused
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void FileStripMenuItem1_Click(object sender, EventArgs e)
        {

        }
        #endregion


        
        #region backup
        /*

        public Form1()
        {
            InitializeComponent();
            openToolStripMenuItem.Enabled = false;
            SavePath = null;
        }



        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var folderBrowserDialog1 = new FolderBrowserDialog();


            string initpath = "g:\\_Magán\\_Óbudai Egyetem\\Szakdolgozat 1\\OrthomachineForm\\ortomachine_project\\";
            folderBrowserDialog1.SelectedPath = initpath;
            DialogResult result = folderBrowserDialog1.ShowDialog();


            if (result == DialogResult.OK)
            {

                if (SavePath != null)
                {
                    if (MessageBox.Show("Close this and create new Project?", "Create new project?", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        SavePath = folderBrowserDialog1.SelectedPath;
                        openToolStripMenuItem.Enabled = true;
                    }
                }
                else
                {
                    SavePath = folderBrowserDialog1.SelectedPath;
                    openToolStripMenuItem.Enabled = true;
                }
            }
        }




        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();            
            ofd.DefaultExt = ".asc";
            ofd.Filter = "Point cloud (*.txt)|*.txt|ASCII file (*.asc)|*.asc";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filename = ofd.FileName;
                using (var form2 = new Form2(this))
                {
                    form2.ShowDialog();
                    if (form2.DialogResult == DialogResult.OK)
                    {
                        Surface sf = new Surface(filename, offset, rastersize, this);
                        thread = new Thread(() =>
                        {
                            sf.Run();
                            if (pictureBox1.InvokeRequired)
                            {
                                pictureBox1.BeginInvoke((MethodInvoker)delegate ()
                                {
                                    pictureBox1.Image = sf.image;
                                    Application.DoEvents();
                                });
                            }
                        });
                        thread.Start();
                        pictureBox1.Image = sf.image;
                    }

                }
            }
        }

        #region unused
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void FileStripMenuItem1_Click(object sender, EventArgs e)
        {

        }
        #endregion
        */
        #endregion
    }
}
