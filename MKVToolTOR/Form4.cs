using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MKVToolTOR
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        string mkvmergePath = @"C:\Program Files\MKVToolNix\mkvmerge.exe";
        string inputFile = @"F:\test\2008 Bedtime Stories.mkv";
        string outputFile = @"F:\test\2008 Bedtime Stories_.mkv";
        void UpdateLabelWithProgress(string data)
        {
            if (data != null)
            {
                Match match = Regex.Match(data, @"Progress: (\d+)%");
                if (match.Success)
                {
                    int progressValue = int.Parse(match.Groups[1].Value);
                    // ต้องเปลี่ยน label1 เป็นชื่อ label ที่คุณใช้งานจริง
                    label1.Invoke((MethodInvoker)(() => label1.Text =  $"{inputFile}\nto {outputFile}\nProgress: {progressValue}%"));
                }
            }
        }
        private void Form4_Load(object sender, EventArgs e)
        {
            Task.Run(() =>
            {

              

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = mkvmergePath,
                    Arguments = $"-o \"{outputFile}\" --audio-tracks th -S \"{inputFile}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    UseShellExecute = false
                };

                using (Process process = new Process { StartInfo = startInfo })
                {
                    process.OutputDataReceived += (s, _e) => UpdateLabelWithProgress(_e.Data);
                    process.ErrorDataReceived += (s, _e) => UpdateLabelWithProgress(_e.Data);

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    process.WaitForExit();
                }
            });
           
        }
    }
}
