

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MKVToolTOR
{
    public partial class frmMain : Form
    {
        string mkvmerge_query = " -o \"{1}\"  --audio-tracks th  -S   \"{0}\"";
        string ffmpeg_query = "   -i \"{0}\" -s {2}  \"{1}\"";
        string mkvmerge_file = System.Windows.Forms.Application.StartupPath + @"\MKVToolNix\mkvmerge.exe";
        string ffmpeg_file = System.Windows.Forms.Application.StartupPath + @"\ffmpeg\ffmpeg.exe";
        // string strmkvmerge = @"Progress: (\d+)%";
        double _progress = 0; int index;

        string display = "";

        List<string> cris = new List<string>() {
            ".1080p.BluRay.x264.DD5.1.THM",
            ".1080p.BrRip.x264.DTS.5.1.THD_ZEZASiambit",
            ".1080p.BrRip.x264.DD.5.1.THD_ZEZASiambit",
            ".1080p.BrRip.x264.DDP.5.1.Atmos.THD_ZEZASiambit",
            ".1080p.BluRay Rip.H264-Boot @nalog",
            ".1080p.IMAX.BrRip.DTS.x264.THB",
            ".1080p.x264.BrRip..DTS 5.1.THD_ZEZA",
            ".1080p.x264.BrRip.DTS.5.1.THM_ZEZASiambit",
            ".1080p.BrRip.x264.DTS-5.1.THM_",
            ".1080p.BrRip.x264.DD.5.1.THM_ZEZASiambit",
            "_(BDRip_1920x1080_HEVC_Hi10P_FLAC)",
            ".1080p.WEB-DL.H265.AAC-(Spark751)",
            ".1080p.WeTV.WEB-DL.DDP2.0.x.264_SkiesIT",
            ".1080p.NF.APP-DL.DDP5.1.H264",
            ".MONOMAX.WEB-DL.DDP2.0.x264_SkiesIT", 
            ".1080p.iQIYI.WEB-DL.DDP5.1.x264_SkiesIT",  
            ".1080p.iQIYI.WEB-DL.DDP2.0.x264_SkiesIT",
            "2160p.WEB-DL.H265.AAC-(Spark751)",
            "1080p.YOUKU.WEB-DL.DDP2.0.x264_SkiesIT",
            "1080p.TrueID.WEB-DL.DDP2.0.x264_SkiesIT",
            "(WEBRip1080p@H264.E-AC3.AC3)",
            "[HD1080p@H264.DTS.AC3]",
            "1080p BrRip DTS x264",
            "1080p Bluray TH DTS x264-Estb",
            "1080p BluRay _©GOLD CUP™",
            "1080p BrRip DTS x264_NongZEZACtHts",
            "1080p BrRip x264 DTS-5 1 THD_ZEZA",
            "1080p BrRip DTS x264 THB_ZEZASiambit",
            "1080p BrRip DTS x264 THM_",
            "1080p BrRip DTS x264Load2up com",
            "BrRip DTS x264_NongZEZACtHts",
            "BluRay 1080p DTS x264 {Super Mini-HD 1080p HQ}",
            "1080p BluRay H264 AC3 _©GOLD CUP™",
            "1080p BluRay DTS x264_by NongZEZACtHts" ,
            "1080p WEB-DL_©GOLD CUP™",
            "1080p BrRip DTS-TH x264_by NongZEZACtHts",
            "1080p WEB-DL AC3 x264_NongZEZACtHts",
            "1080p BluRay x264 EN DTS TH AC-3",
            "HQ1080pH265 DTS-HD AC3",
            "1080p HDRH264 DTS AC3",
            "HD1080pH264 DTS","1080p_DTS_x264",
            "HQ1080pH265 WEB-DL-AC3",
            "1080p DTS BrRip x264",
            ".1080p.NF.APP-DL.DDP5.1.h264",
            ".1080p.WEB-DL.AC3.x264_by SkiesIT",
            ".1080p.WEB-DL.AC3.x264_SkiesIT",
            "1080p WEB-DL H 264 AAC-Spark751",
            "1080p BrRip x264 DTS-5 1 THM_ZEZA",
            "1080p BrRip x264 TrueHD 7 1 Atmos THD_ZEZASiambit",
            "1080p BrRip x264 DTS-HD MA 5 1 THM_ZEZA",
            "Mini-Hidef-HQ",
            "1080p HDRip H264 AAC",
            "1080p WEB-DL AC3 x264 THM_ZEZASiambit",
            "1080p BrRip x264 DTS-5 1 THB_ZEZASiambit",
            "1080p BrRip x264 TrueHD 7 1 Atmos THD_ZEZASiambit",
            "1080p WEB-DL Monomax H 264 AAC-Spark751",
            "1080p.WEB.H264.TH",
            "1080p NF APP-DL DDP5 1 H264",
            "1080p WEBRip x264_GOLD CUP",
            "1080p BrRip DTS x264",
            "1080p NF APP-DL DDP5 1 H264",
            "1080 DDP5 1 x264 NF-MESSI",
            "1080p NF WEB-DL DDP5 1 x264",
            "[Netflix]","Netflix",
            "THM One2loadup com",
            "1080p WEB-DL AC3 x264 THM_ZEZA",
            "1080p BrRip DTS THM2 0",
            "[60fps ลื่นหัวแตก]",
            "60fps ลื่นหัวแตก",
            "1080p.BrRip.DTS.x264.One2loadup.com",
            "[MINI]",
            "{MINI}",
            "{MINI Super-HQ}_",
            "[Mini Super-1080p.HQ]",
             "MINI-HD_",
             "[Mini HD]", 
            "[MINI Super-HQ]_",
            "TT ",
            "TT- ",
            "TT_","{MINI Super-HQ}",
            "[MINI-HQ]_",
            "[Mini-hidef]",
            "[Mini-HD]",
            "[HQ]",
            "{MINI-HQ}_","[AB13]_"};


        public frmMain()
        {
            InitializeComponent();
            dataGridView1.SetDefaultCellStyle();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fol = new FolderBrowserDialog())
            {
                fol.ShowDialog();
                textBox1.Text = fol.SelectedPath;

                SetData();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
        void DeleteCri()
        {
            //
            string fl = textBox1.Text;

            cris.ForEach(cri =>
            {
                (from file in System.IO.Directory.GetFiles(fl, "*", SearchOption.AllDirectories)
                 where Path.GetFileNameWithoutExtension(file).Contains(cri)
                 select file).ToList<string>()
                     .ForEach(f =>
                     {
                         string _f = Path.GetDirectoryName(f) + "\\" + Path.GetFileNameWithoutExtension(f).Replace(cri, " ").Trim() + Path.GetExtension(f);
                         txtStatus.Text = "Move File:" + f;
                         if (_f != f)
                         {
                             try
                             {
                                 System.IO.File.Move(f, TORServices.PathFile.FileTor.MakeValidFileName(_f));
                             }
                             catch { }
                         }

                     });

            });
            (from file in System.IO.Directory.GetFiles(textBox1.Text, "*", SearchOption.AllDirectories)
             where Path.GetFileNameWithoutExtension(file).Contains(".")
             select file).ToList<string>()
                      .ForEach(f =>
                      {
                          string _f = Path.GetDirectoryName(f) + "\\" + Path.GetFileNameWithoutExtension(f).Replace(".", " ").Trim() + Path.GetExtension(f);
                          txtStatus.Text = "Move File:" + f;

                          if (_f != f)
                          {
                              try
                              {
                                  System.IO.File.Move(f, TORServices.PathFile.FileTor.MakeValidFileName(_f));
                              }
                              catch { }
                          }
                      });
           

                Directory.GetFiles(textBox1.Text, "*", SearchOption.TopDirectoryOnly).ToList<string>()
                .ForEach(f =>
                {
                    txtStatus.Text = "Set Year File:" + f;
                    for (int i = 1900; i < 2030; i++)
                    {
                        if (Path.GetFileName(f).Contains(i.ToString()))
                        {
                            //  MessageBox.Show(f);
                            string _f = textBox1.Text + "\\" + i + " " + Path.GetFileName(f).Replace(i.ToString(), "").Replace("()", "").Replace("[]", "").Trim();
                            if (_f != f)
                            {
                                if (File.Exists(_f))
                                {
                                    _f = TORServices.PathFile.FileTor.RenameFileDup(_f);
                                }
                                File.Move(f, _f);
                            }

                            return;

                        }

                    }
                });

          

            txtStatus.Text = "Delete Cri Complete";
        }
        void AddDatainGrid()
        {

            dataGridView1.Invoke(new Action(() => dataGridView1.Rows.Clear()));

            Directory.GetFiles(textBox1.Text, "*", SearchOption.AllDirectories)
            .Where(f => Path.GetExtension(f).ToLower() == ".mp4" ||
                      Path.GetExtension(f).ToLower() == ".mkv" ||
                      Path.GetExtension(f).ToLower() == ".mpg")
            .ToList<string>()
            .ForEach(f =>
            {
                txtStatus.Text = "Add File:" + f;
                dataGridView1.Invoke(new Action(() => dataGridView1.Rows.Add(f)));

            });

            txtStatus.Text = "Add File Complete";
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox4.Text)) return;

            Task.Run(() =>
            {
                (from file in System.IO.Directory.GetFiles(textBox1.Text, "*", SearchOption.AllDirectories)
                 where Path.GetFileNameWithoutExtension(file).Contains(textBox4.Text)
                 select file).ToList<string>()
                       .ForEach(f =>
                       {
                           string _f = Path.GetDirectoryName(f) + "\\" + Path.GetFileNameWithoutExtension(f).Replace(textBox4.Text, textBox2.Text).Trim() + Path.GetExtension(f);
                           txtStatus.Text = "Move File:" + f;

                           try
                           {
                               System.IO.File.Move(f, TORServices.PathFile.FileTor.MakeValidFileName(_f));
                           }
                           catch { }
                       }
              );
                txtStatus.Text = "Move File Complete";

                AddDatainGrid();

            });

        }

    
        string inputFile = @"F:\test\2002 3 The Mother.mkv";
        string outputFile = @"F:\test\2002 3 The Mother_.mkv";

        void UpdateLabelWithProgress(string data)
        {
            if (data != null)
            {
                Match match = Regex.Match(data, @"Progress: (\d+)%");
                if (match.Success)
                {
                    int progressValue = int.Parse(match.Groups[1].Value);
                    // ต้องเปลี่ยน label1 เป็นชื่อ label ที่คุณใช้งานจริง
                    this.Invoke((MethodInvoker)(() => this.Text = $"Progress: {progressValue}%"));
                    // txtStatus.Text = $"{inputFile}\nto {outputFile}\nProgress: {progressValue}%";
                }
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text)) return;
            if (dataGridView1.Rows.Count <= 0)
            {
                MessageBox.Show("ConvertMKV All Complete");
                return;
            }

            button4.Enabled = false;

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                int r = i; // นำค่า i มาใช้ในเธรดอื่น
                await ConvertMKVItemAsync(r);
            }

            button4.Enabled = true;
            MessageBox.Show("ConvertMKV All Complete");
        }
       

        private async Task ConvertMKVItemAsync(int r)
        {
            string file = dataGridView1[0,r].Value.ToString();
            if (!File.Exists(file)) return;
           // string folComplete = textBox1.Text + "\\Complete";
          //  Directory.CreateDirectory(folComplete);
            string extfile = Path.GetExtension(file);

            string _file = Path.GetDirectoryName(file) + "\\"+Path.GetFileName(file).Replace(".mkv", "_.mkv") ;
            
            txtStatus.GetCurrentParent().BeginInvoke(new Action(() => txtStatus.Text = file));
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

            if (File.Exists(_file))
            {
                File.Delete(file);
                dataGridView1.Invoke(new Action(() => dataGridView1[1, r].Value = 100));
                taskCompletionSource.TrySetResult(true);
                return;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(_file));
            Process process = new Process
            {
                StartInfo =
        {
            FileName = mkvmerge_file,
            Arguments = string.Format(mkvmerge_query, file, _file),
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            UseShellExecute = false
        },
                EnableRaisingEvents = true
            };
            
            Action<string> UpdateLabelWithProgress = data =>
            {
                System.Threading.Thread.Sleep(2000);
                if (data != null)
                {
                    Match match = Regex.Match(data, @"Progress: (\d+)%");
                   
                    if (match.Success)
                    {
                        int progressValue = int.Parse(match.Groups[1].Value);
                        if (progressValue < 100)
                        { 
                            dataGridView1.Invoke(new Action(() => dataGridView1[1, r].Value = progressValue));
                            txtStatus.GetCurrentParent().BeginInvoke(new Action(() => txtStatus.Text = file + " " + progressValue + " %"));
                            // txtStatus.Text = file + " " + progressValue + " %";
                        }

                        if (progressValue == 100)
                        {
     
                            dataGridView1.Invoke(new Action(() => dataGridView1[1, r].Value = 100));
                            taskCompletionSource.TrySetResult(true);
                        }
                    }
                }
            };

            process.OutputDataReceived += (s, _e) => UpdateLabelWithProgress(_e.Data);
            process.ErrorDataReceived += (s, _e) => UpdateLabelWithProgress(_e.Data);

            process.Exited += new EventHandler((object _s, EventArgs _e) =>
            {
                process.Dispose();
                System.Threading.Thread.Sleep(1000);
                try
                {
                   /*if (Path.GetDirectoryName(file).Replace(textBox1.Text, "").Trim() == "" || Path.GetDirectoryName(file).Replace(textBox1.Text, "") == "\\")
                    {
                        folComplete = textBox1.Text + "\\Complete";
                    }
                    else
                    {
                        folComplete = textBox1.Text + "\\Complete\\" + Path.GetDirectoryName(file).Replace(textBox1.Text, "").Trim();
                    }
                    if (!Directory.Exists(folComplete))
                        Directory.CreateDirectory(folComplete);
                    */
                    
                    File.Delete(file);
                    System.Threading.Thread.Sleep(200);
                   // if(Path.GetDirectoryName(file) != textBox1.Text.Trim())
                   File.Move(_file, file);
                }
                catch { }

                dataGridView1.Invoke(new Action(() => dataGridView1[1, r].Value = 100));
                taskCompletionSource.TrySetResult(true);
            });

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await taskCompletionSource.Task;
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox4.Text))
                Properties.Settings.Default.strReplace = textBox4.Text;
            Properties.Settings.Default.Save();
        }
        void SetData()
        {
            if (string.IsNullOrEmpty(textBox1.Text) || !Directory.Exists(textBox1.Text)) return;

            Properties.Settings.Default.strPath = textBox1.Text;
            Properties.Settings.Default.Save();

            Task.Run(() =>
            {
                txtStatus.Text = "Get File in " + textBox1.Text;
                Directory.GetDirectories(textBox1.Text, "*", SearchOption.TopDirectoryOnly)
                .ToList<string>()
                .ForEach(fol =>
                {
                    txtStatus.Text = "Get File in " + fol;
                    var lstFile = Directory.GetFiles(fol, "*", SearchOption.AllDirectories)
                            .Where(f => Path.GetExtension(f).ToLower() == ".mp4" ||
                                        Path.GetExtension(f).ToLower() == ".mkv" ||
                                        Path.GetExtension(f).ToLower() == ".mpg")
                            .ToList<string>();

                    if (lstFile.Count == 1)
                    {

                        try
                        {
                            File.Move(lstFile[0], textBox1.Text + "\\" + Path.GetFileName(lstFile[0]));
                            Directory.Delete(fol, true);
                        }
                        catch { }

                    }



                });


                txtStatus.Text = "DeleteCri in " + textBox1.Text;

                DeleteCri();

                txtStatus.Text = "AddDatainGrid in " + textBox1.Text;
                AddDatainGrid();

            });
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            SetData();

        }

        bool checkRun = true;
        private async void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (dataGridView1.Rows.Count < 0) return;
           /* index = 0;
                ConvertMKVItem(index);*/

        }

       /* void ConvertMKVItem(int _r)
        {
            int r = _r;
            string file = dataGridView1.Rows[r].ToString();
            string _file = Path.GetDirectoryName(file) + "\\" + Path.GetFileName(file).Replace(".mkv", "_.mkv");
            Process process = new Process
            {
                StartInfo =
               {
                   FileName = mkvmerge_file,
                   Arguments = string.Format(mkvmerge_query,file, _file),
                   RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    UseShellExecute = false
               },
                EnableRaisingEvents = true
            };
            Action<string> UpdateLabelWithProgress = data =>
            {
                if (data != null)
                {
                    Match match = Regex.Match(data, @"Progress: (\d+)%");
                    if (match.Success)
                    {
                        int progressValue = int.Parse(match.Groups[1].Value);
                        txtStatus.Text = $"{file}Progress: {progressValue}%";
                        if(progressValue<100)
                        dataGridView1.Invoke(new Action(() => dataGridView1[1, r].Value = progressValue));

                    }
                }
            };
            process.OutputDataReceived += (s, _e) => UpdateLabelWithProgress(_e.Data);
            process.ErrorDataReceived += (s, _e) => UpdateLabelWithProgress(_e.Data);
            
            process.Exited += new EventHandler((object _s, EventArgs _e) =>
            {

                process.Dispose();
                backgroundWorker1.Invoke(new Action(() => backgroundWorker1.Value++));
                
                    try { 
                    File.Delete(file);
                    File.Move(_file, file);
                } catch { }
                dataGridView1.Invoke(new Action(() => dataGridView1[1, r].Value = 100));
                index++;
                if (index >= dataGridView1.RowCount-1)
                {
                    button4.Invoke(new Action(() => button4.Enabled = true));
                    MessageBox.Show("ConvertMKV All Complete"); 
                    return;
                }

                ConvertMKVItem(index);
            });

            process.Start();


        }
       */

        void ConvertffmpegItem(string file)
        {

            string str = "", str1 = "";
            Task.Run(() =>
            {
                Regex myRegex = new Regex(@"Output #0.*?\n\s{0,}.*?\n\s{0,}.*?\n\s{0,}.*?\n\s{0,}.*?\n\s{0,}.*?\n\s{0,}.*?\n\s{0,}NUMBER_OF_FRAMES.*?:\s{1,}(\d+)\n", RegexOptions.None);
                Regex myRegex_ = new Regex(@"frame=\s{0,}(\d+)", RegexOptions.None);
                long frameAll = 0;
                Process proc = new Process();
                proc.StartInfo.FileName = ffmpeg_file;
                proc.StartInfo.Arguments = string.Format(ffmpeg_query, file, Path.GetDirectoryName(file) + Path.GetFileNameWithoutExtension(file) + ".mp4", display);
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

                    str1 = line;

                    if (str1.Contains("frame="))
                    {
                        if (frameAll <= 0)
                        {
                            Match match = myRegex.Match(str);
                            if (match.Length > 0 && match.Groups.Count >= 1)
                            {
                                frameAll = long.Parse(match.Groups[1].Value);

                            }

                        }
                        Match match_ = myRegex_.Match(str1);
                        if (match_.Length > 0 && match_.Groups.Count >= 1)
                        {
                            long frame = long.Parse(match_.Groups[1].Value);
                            if (frame >= 0)
                            {
                                dataGridView1.Invoke(new Action(() => dataGridView1[1, index].Value = 30d + (Convert.ToDouble(frame) / Convert.ToDouble(frameAll) * 100d) * 70d));
                                //  this.Invoke(new Action(() => this.Text = "Frame:" + frame + "/" + frameAll + "  " + (Convert.ToDouble(frame) / Convert.ToDouble(frameAll) * 100d).ToString("0.000") + " %"));
                            }

                        }
                    }



                }
                proc.Close();



            }).Wait();
            dataGridView1.Invoke(new Action(() => dataGridView1[1, index].Value = 100));

        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            CenterToScreen();
            textBox1.Text = Properties.Settings.Default.strPath;
            textBox4.Text = Properties.Settings.Default.strReplace;

            SetData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                Directory.GetFiles(textBox1.Text, "*.mkv_", SearchOption.TopDirectoryOnly).ToList<string>()
                .ForEach(f =>
                {
                    txtStatus.Text = f;
                    string _f = f.Replace(".mkv_", ".mkv");
                    try
                    {
                        if (File.Exists(_f))
                            File.Delete(_f);

                        File.Move(f, _f);
                    }
                    catch { }
                    
                 
                });
                Directory.GetFiles(textBox1.Text, "*.mp4_", SearchOption.TopDirectoryOnly).ToList<string>()
                .ForEach(f =>
                {
                    txtStatus.Text = f;
                    string _f = f.Replace(".mp4_", ".mp4");
                    try
                    {
                        if (File.Exists(_f))
                            File.Delete(_f);

                        File.Move(f, _f);
                    }
                    catch { }


                });

                txtStatus.Text = "Complete";
            });

        }
    }
}
