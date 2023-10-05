using MediaToolkit.Model;
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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
       

    
        Regex myRegex = new Regex(@"Output #0.*?\n\s{0,}.*?\n\s{0,}.*?\n\s{0,}.*?\n\s{0,}.*?\n\s{0,}.*?\n\s{0,}.*?\n\s{0,}NUMBER_OF_FRAMES.*?:\s{1,}(\d+)\n", RegexOptions.None);
        Regex myRegex_ = new Regex(@"frame=\s{0,}(\d+)", RegexOptions.None);
        long frameAll = 0;
        string mkvmerge_query = " -o \"{1}\"  --audio-tracks th  -S   \"{0}\"";
        string ffmpeg_query = "   -i \"{0}\" -s {2}  \"{1}\"";
        string mkvmerge_file = Application.StartupPath + @"\MKVToolNix\mkvmerge.exe";
        string ffmpeg_file = Application.StartupPath + @"\ffmpeg\ffmpeg.exe";

        private void Form2_Load(object sender, EventArgs e)
        {

            


        }
        private void mkvmerge_Run(string file)
        {
         
            string _file = Path.GetDirectoryName(file) + "\\" + Path.GetFileName(file).Replace(".mkv", "_.mkv");

            string str = "";
            Task.Run(() =>
            {

                Process proc = new Process();
                proc.StartInfo.FileName = mkvmerge_file;
                proc.StartInfo.Arguments = string.Format(mkvmerge_query, file,_file);
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
              
                if (!proc.Start())
                {

                    return;
                }
                StreamReader reader = proc.StandardError;
              
                string line = "";


                while ((line = reader.ReadLine()) != null)
                {
                    str += "\n" + line;
                    richTextBox1.Invoke(new Action(() =>
                    {
                        richTextBox1.Text = line;

                       

                    }));
                    // System.Threading.Thread.Sleep(1000);
                }
                proc.Close();



            });
        }


        void ffmpeg_Run(string file,string target)
        {

            string str = "";
            Task.Run(() =>
            {

                Process proc = new Process();
                proc.StartInfo.FileName = ffmpeg_file;
                proc.StartInfo.Arguments = string.Format(ffmpeg_query, file, target, "1280x720");
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                if (!proc.Start())
                {

                    return;
                }
                StreamReader reader = proc.StandardError;
                //  richTextBox1.Invoke(new Action(() => richTextBox1.Text += "\n" + reader.ReadLine()));
                string line = "";


                while ((line = reader.ReadLine()) != null)
                {
                    str += "\n" + line;
                    richTextBox1.Invoke(new Action(() =>
                    {
                        richTextBox1.Text = line;

                        if (richTextBox1.Text.Contains("frame="))
                        {
                            if (frameAll <= 0)
                            {
                                Match match = myRegex.Match(str);
                                if (match.Length > 0 && match.Groups.Count >= 1)
                                {
                                    frameAll = long.Parse(match.Groups[1].Value);

                                }

                            }
                            Match match_ = myRegex_.Match(richTextBox1.Text);
                            if (match_.Length > 0 && match_.Groups.Count >= 1)
                            {
                                long frame = long.Parse(match_.Groups[1].Value);
                                if (frame >= 0)
                                {
                                    this.Invoke(new Action(() => this.Text = "Frame:" + frame + "/" + frameAll + "  " + (Convert.ToDouble(frame) / Convert.ToDouble(frameAll) * 100d).ToString("0.000") + " %"));
                                }

                            }
                        }

                    }));
                    // System.Threading.Thread.Sleep(1000);
                }
                proc.Close();



            }).Wait();

        }
    }
}
