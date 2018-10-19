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
    public partial class Form1 : Form
    {
        public string filename;
        Thread thread;
        public float offset, rastersize;
        public string SavePath = "";



        //private Bitmap Surface;


        public Form1()
        {
            InitializeComponent();
            openToolStripMenuItem.Enabled=false;
        }

        private void FileStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var folderBrowserDialog1 = new FolderBrowserDialog();

            
            string initpath = "d:\\__magán\\orto_mentések\\";
            folderBrowserDialog1.SelectedPath = initpath;
            DialogResult result = folderBrowserDialog1.ShowDialog();            
            
            
            if (result == DialogResult.OK)
            {
                
                SavePath = folderBrowserDialog1.SelectedPath;
                if (SavePath == "")
                {
                    if (MessageBox.Show("Close this and create new Project?", "Create new project?", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        SavePath = folderBrowserDialog1.SelectedPath;
                    }

                }
                else
                    SavePath = folderBrowserDialog1.SelectedPath;
            }
            openToolStripMenuItem.Enabled = true;

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
                //this.Hide();
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


}
}
