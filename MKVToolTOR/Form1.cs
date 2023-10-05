using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MKVToolTOR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string query = " -o \"{1}\"  --audio-tracks th  -S   \"{0}\"";
        List<string> lstMKV;
        int index = 0;
        string mkvmerge_file = @"D:\Mega_TOR\Pro\MKVToolNix\mkvmerge.exe";
        private void Form1_Load(object sender, EventArgs e)
        {

            textBox1.Text = Properties.Settings.Default.strPath;
            textBox4.Text = Properties.Settings.Default.strReplace;
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fol = new FolderBrowserDialog())
            {
                fol.ShowDialog();
                textBox1.Text = fol.SelectedPath;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text)) return;

            // CreateCMD(textBox1.Text);



            lstMKV = System.IO.Directory.GetFiles(textBox1.Text, "*.mkv", SearchOption.AllDirectories)
                    .Where(f => !Path.GetFileNameWithoutExtension(f).ToLower().Contains("_.mkv"))
                    .ToList<string>();
            if (lstMKV == null || lstMKV.Count <= 0)
                MessageBox.Show("ConvertMKV All  Complete");
            progressBar1.Maximum = lstMKV.Count;
            progressBar1.Minimum = 0;
            progressBar1.Value = 0;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox4.Text)) return;


            System.IO.Directory.GetFiles(textBox1.Text, "*" + textBox4.Text + "*", SearchOption.AllDirectories).ToList<string>()
                     .ForEach(f =>
                     {
                         string _f = Path.GetDirectoryName(f) + "\\" + Path.GetFileNameWithoutExtension(f).Replace(textBox4.Text, "").Trim() + Path.GetExtension(f);
                         File.Move(f, _f);
                     }
                     );
            MessageBox.Show("Replace Complete");
        }


        private void textBox4_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox4.Text))
                Properties.Settings.Default.strReplace = textBox4.Text;
            Properties.Settings.Default.Save();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
                Properties.Settings.Default.strPath = textBox1.Text;
            Properties.Settings.Default.Save();
        }

   
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
         
            if (lstMKV.Count > 0)
            {

                index = 0;
                   ConvertMKVItem(lstMKV[index]);
            }
        }

        public void ConvertMKVItem(string file)
        {
            string _file = Path.GetDirectoryName(file) + "\\" + Path.GetFileName(file).Replace(".mkv", "_.mkv");
            this.Invoke(new Action(() => this.Text = "MKV FILE:" + file));
            /*  Process process = new Process
              {
                  StartInfo =
                {
                    FileName = mkvmerge_file,
                    Arguments = string.Format(query,file, _file),
                    CreateNoWindow = true,
                    WindowStyle =  ProcessWindowStyle.Hidden


                },
                  EnableRaisingEvents = true
              };

              process.Exited += new EventHandler((object _s, EventArgs _e) =>
              {

                  process.Dispose();
                  progressBar1.Invoke(new Action(() => progressBar1.Value++));
                  if (checkBox1.Checked)
                  { 
                      try { 
                          File.Delete(file);
                          File.Move(_file, file);
                      } catch { } 

                  }

                  index++;
                  if (index >= lstMKV.Count)
                  { MessageBox.Show("ConvertMKV All Complete"); return; }

                  ConvertMKVItem(lstMKV[index]);
              });

              process.Start();*/
            


        }
    }
}
