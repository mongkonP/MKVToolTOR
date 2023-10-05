using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MKVToolTOR
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        Random random = new Random();
       

     

        private void Form3_Load(object sender, EventArgs e)
        {

            // string mkvmerge_file = @"C:\Program Files\MKVToolNix\mkvmerge.exe";
            // กำหนดตัวแปร sourcePath ให้เป็น path ของไฟล์ที่ต้องการเปลี่ยน
            // string sourcePath = @"E:\Test\2007 The Kingdom  1080p NF APP-DL DDP5 1 H264.mkv";

            /* var processInfo = new ProcessStartInfo
             {
                 FileName = mkvmerge_file,
                 Arguments = $"-i \"{sourcePath}\"",
                 RedirectStandardOutput = true,
                 UseShellExecute = false
             };

             var process = Process.Start(processInfo);
             process.WaitForExit();

             while (!process.StandardOutput.EndOfStream)
             {
                 richTextBox1.Text+="\n" +  process.StandardOutput.ReadLine();

             }*/
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "C:\\Program Files\\MKVToolNix\\mkvinfo.exe";
            startInfo.Arguments = " \"E:\\Test\\2007 The Kingdom  1080p NF APP-DL DDP5 1 H264.mkv\"";
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;

            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();

            richTextBox1.Text = process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            /*

                        // กำหนดตัวแปร destinationPath ให้เป็น path ของไฟล์ที่จะถูกสร้างขึ้น
                        string destinationPath = @"E:\Test\2007 The Kingdom  1080p NF APP-DL DDP5 1 H264 - Converted.mkv";

                        // สร้าง ProcessStartInfo object เพื่อกำหนดค่าสำหรับการเรียกโปรแกรม mkvmerge
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.FileName = path;
                        startInfo.Arguments = $"-o \"{destinationPath}\" --no-subtitles \"--audio-tracks th \"{sourcePath}\"";
                        startInfo.UseShellExecute = false;
                        startInfo.CreateNoWindow = true;
                        startInfo.RedirectStandardOutput = true;

                        // เริ่มการเรียกโปรแกรม mkvmerge
                        Process process = new Process();
                        process.StartInfo = startInfo;
                        process.Start();

                        // อ่านข้อมูลที่ถูกพิมพ์ออกมาจากโปรแกรม mkvmerge
                        string output = process.StandardOutput.ReadToEnd();

                        // รอจนกว่าโปรแกรม mkvmerge จะทำงานเสร็จสิ้น
                        process.WaitForExit();

                        // แสดงข้อความที่พิมพ์ออกมาจากโปรแกรม mkvmerge ใน text box
                        richTextBox1.Text = output;*/
        }

        
    }
}
