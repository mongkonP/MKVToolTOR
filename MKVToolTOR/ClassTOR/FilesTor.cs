
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TORServices.PathFile
{
  

    public static class FileTor
    {
        public static string videoFilter = ".avi .mpg .mpeg .mov .flv .3gp .rm .rmvb .mp4 .0gm .mkv .mov .wmv .vob";
        public static string audioFilter = ".mp3 .wav .ogg .mid .rm .wma m4a";
        public static string imagesFilter = ".jpg .jpeg .bmp .gif .ico .tga .png";
        public static string codeFilter = ".c .cpp .h .java .class .jar .cs .csproj .vbproj";
        public static string softwareFilter = ".exe .msi .rpm .bin .deb .iso .nrg .zip .rar";
        public static string documentsFilter = ".pdf .doc .htm .html .mht .txt .ppt .xl .pps .tex .dvi";

        public static string FilterFile = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png" +
                                    "|txt files (*.txt)|*.txt" +
                                    "|Document files (*.doc, *.docx, *.xls,  *.xlsx, *.ppt, *.pptx, *.pdf )|*.doc, *.docx, *.xls,  *.xlsx, *.ppt, *.pptx, *.pdf " +
                                    "|Audio/Vdio files (*.AVI, *.XVID, *.DivX *.3GP *.MKV *.MP4 *.FLV *.MP3 *.DAT)|*.AVI, *.XVID, *.DivX *.3GP *.MKV *.MP4 *.FLV *.MP3 *.DAT" +
                                    "|All files (*.*)|*.*";
        public static string SelectFile(string _path, string InitialDirectory = "", string ExtFile = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png" +
                                    "|txt files (*.txt)|*.txt" +
                                    "|Document files (*.doc, *.docx, *.xls,  *.xlsx, *.ppt, *.pptx, *.pdf )|*.doc, *.docx, *.xls,  *.xlsx, *.ppt, *.pptx, *.pdf " +
                                    "|Audio/Vdio files (*.AVI, *.XVID, *.DivX *.3GP *.MKV *.MP4 *.FLV *.MP3 *.DAT)|*.AVI, *.XVID, *.DivX *.3GP *.MKV *.MP4 *.FLV *.MP3 *.DAT" +
                                    "|All files (*.*)|*.*")
        {
            string _F = "";
            try
            {
                System.Windows.Forms.OpenFileDialog fb = new System.Windows.Forms.OpenFileDialog();
                if(!string.IsNullOrEmpty(InitialDirectory))
                {
                    fb.InitialDirectory = InitialDirectory;
                }
                fb.ShowDialog();
                _F = fb.FileName;
            }
            catch { }
            return _F;
        }
        public static string TimeToString(this TimeSpan time)
        {
            string st = "00";

           /* if (time.TotalSeconds > 0)
                st = time.TotalSeconds.ToString("00");

            if (time.TotalMinutes > 0)
                st = time.TotalMinutes.ToString("00") + ":" + st;

            if (time.TotalHours > 0)
                st = time.TotalHours.ToString("00") + ":" + st;*/
            return ((time.TotalHours > 0)? time.TotalHours.ToString("00") + ":":"") + ((time.Minutes > 0) ? time.Minutes.ToString("00") + ":" : "")  + ((time.TotalSeconds > 0) ? time.TotalSeconds.ToString("00") + ":" : "");

        }
        public static TimeSpan ToTimeSpan(this string ts)
        {
            if (ts == null || ts=="") return TimeSpan.Parse("00:00:00");
            string[] sts = ts.Split(':');
         //  System.Windows.Forms.MessageBox.Show("ts = " + ts + "\n" +  TimeSpan.Parse(ts.Trim()).TotalMinutes.ToString());
            TimeSpan _ts = TimeSpan.Parse("00:00:00");
              try
              {

                  if (sts.Length == 1)
                  {
                      _ts = TimeSpan.Parse("00:00:" + ts);
                  }
                  else if (sts.Length == 2)
                  {
                      _ts = TimeSpan.Parse("00:" + ts);
                      // MessageBox.Show(_ts.ToString());
                  }
                  else if (sts.Length == 3)
                  {
                      _ts = TimeSpan.Parse(ts);
                  }
                  else if (!ts.Contains(":"))
                  {
                      _ts = TimeSpan.Parse("00:00:00");
                  }
              }
              catch { _ts = TimeSpan.Parse("00:00:00"); }

            /*  try
              {
                  _ts = TimeSpan.Parse(ts.Trim());
              }
              catch { _ts = TimeSpan.Parse("00:00:00"); }
            */
          // System.Windows.Forms.MessageBox.Show("ts = " + ts  + "\n ttt \n" + _ts.TotalMinutes.ToString());
            return _ts;
        }
     
        public static string bytetobigger(long b)
        {
          
                string final;
            float c = (float)b;
            
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (c == 0)
                return "0" + suf[0];
            long bytes = (long)Math.Abs(c);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            final = (Math.Sign(c) * num).ToString("0.000") +" "+ suf[place];
            return final;

           
           
        }
        public static string GetMD5HashFromFile(this string filename)
                {
                    if (!System.IO.File.Exists(filename)) return null;
               
                    using (var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider())
                    {
                        var buffer = md5.ComputeHash(System.IO.File.ReadAllBytes(filename));
                        var sb = new StringBuilder();
                        for (int i = 0; i < buffer.Length; i++)
                        {
                            sb.Append(buffer[i].ToString("x2"));
                        }
                        return sb.ToString();
                    }

             
                    
                }
        public static async Task< List<string>> GetMD5HashFromFiles(this string Dir, string searchPattern = "*.*", System.IO.SearchOption searchOption = System.IO.SearchOption.TopDirectoryOnly)
        {
            if (!System.IO.Directory.Exists(Dir)) return null;
        return  await Task.Run(() =>
            {
                List<string> result = new List<string>();
                using (var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider())
                {
                    System.IO.Directory.GetFiles(Dir, searchPattern, searchOption).ToList<string>()
                     .ForEach(f =>
                     {

                         var buffer = md5.ComputeHash(System.IO.File.ReadAllBytes(f));
                         var sb = new StringBuilder();
                         for (int i = 0; i < buffer.Length; i++)
                         {
                             sb.Append(buffer[i].ToString("x2"));
                         }
                         result.Add(sb.ToString());
                     });
                }
                return result;
            });
           

        }
        public static System.Data.DataTable ToDatatableMD5Hash(this string Dir, string searchPattern = "*.*", System.IO.SearchOption searchOption = System.IO.SearchOption.TopDirectoryOnly)
        {

            if (string.IsNullOrEmpty(Dir)) return null;
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add(new System.Data.DataColumn("File", typeof(string)));
            dt.Columns.Add(new System.Data.DataColumn("Path", typeof(string)));
            dt.Columns.Add(new System.Data.DataColumn("Size", typeof(string)));
            dt.Columns.Add(new System.Data.DataColumn("MD5", typeof(string)));
            dt.Columns.Add(new System.Data.DataColumn("Date Modified", typeof(string)));
            dt.Columns.Add(new System.Data.DataColumn("Selected", typeof(Boolean)));
            System.IO.Directory.GetFiles(Dir, searchPattern, searchOption).ToList<string>()
                .ForEach(
                f =>
                {
                    System.Data.DataRow dr = dt.NewRow();
                    dr["File"] = System.IO.Path.GetFileNameWithoutExtension(f);
                    dr["Path"] = f;
                    dr["Size"] = new System.IO.FileInfo(f).Length;
                    dr["MD5"] = FileTor.GetMD5HashFromFile(f);
                    dr["Date Modified"] = System.IO.File.GetCreationTime(f);
                    dr["Selected"] = false;
                    dt.Rows.Add(dr);
                });
            return dt;
        }


        public static System.Diagnostics.Process IsProcessOpen(this string name)
        {
            foreach (System.Diagnostics.Process clsProcess in System.Diagnostics.Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains(name))
                {
                    return clsProcess;
                }
            }
            return null;
        }
        public static void OpenFile(string _Path, Boolean _Edit = false)
        {
            string FileOnTemp;
            if (_Edit)
            {
                FileOnTemp = _Path;
            }
            else
            {
                FileOnTemp = System.IO.Path.GetTempPath() + "FileOnTemp" + string.Format("{0:ddMMyyyhhmmss}", DateTime.Now) + System.IO.Path.GetExtension(_Path);
                System.IO.File.Copy(_Path, FileOnTemp);
            }
            System.Diagnostics.Process.Start(FileOnTemp);
            try
            {
                System.Diagnostics.Process.Start(FileOnTemp);

            }
            catch
            {
                if (!System.IO.File.Exists(FileOnTemp))
                {
                    System.Windows.Forms.MessageBox.Show("Can't Find File " + FileOnTemp);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Can't Open File " + FileOnTemp);

                }

            }

        }
		public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }
		
        public static string RenameFileDup(string _File)
        {
            string stg = _File;
            if (System.IO.File.Exists(_File))
            {
                int i = 1;
                do
                {
                    stg = System.IO.Path.GetDirectoryName(_File) + "\\" + System.IO.Path.GetFileNameWithoutExtension(_File) + "_" + i + System.IO.Path.GetExtension(_File);
                    i++;
                } while (System.IO.File.Exists(stg));
            }
            return stg;
        }
        public static string MakeValidFileName(this string path)
        {
            return System.IO.Path.GetFileNameWithoutExtension(path).MakeValidFileName(System.IO.Path.GetDirectoryName(path), System.IO.Path.GetExtension(path));
        }
        public static string MakeValidFileName(this string file, string path)
        {
            return file.MakeValidFileName(path, System.IO.Path.GetExtension(file));
        }
        public static string MakeValidFileName(this string file, string path, string Extension)
        {
            string _file;

            _file = file.Replace("/", "_").Replace(@"\", "_").Replace("'", "")
                .Replace("\"", "").Replace("*", "").Replace(":", "").Replace(";", "")
                .Replace("[", "").Replace("]", "").Replace("(", "").Replace(")", "")
                .Replace("~", "").Replace("+", "").Replace("@", "").Replace("$", "")
                .Replace("%", "").Replace("^", "").Replace("฿", "").Replace("|", "")
                .Replace("{", "").Replace("}", "").Replace("?", "").Replace("<", "")
                .Replace(">", "").Replace("?","");


            if (!string.IsNullOrEmpty(path))
                _file = path + "\\" + _file;
            if (!string.IsNullOrEmpty(Extension))
                _file =  _file  + ((Extension.IndexOf(".") != 0) ? "." + Extension : Extension);

            return  _file;
        }
        public static string getFilerex(this string file)
        {
            string strRegex = @"([\w\s\.]+)";
            Regex myRegex = new Regex(strRegex, RegexOptions.Compiled);

            string _f = "";
            foreach (Match myMatch in myRegex.Matches(file))
            {
                _f += myMatch.Value;
            }
            return _f.Trim();
        }
        public  static Task< long> GetDirectorySize( string dir )
        {
            // 1.
            // Get array of all file names.
            string[] a = Directory.GetFiles(dir, "*.*");

            // 2.
            // Calculate total bytes of all files in a loop.
            long b = 0;
            return Task.Run(() =>
            {
           foreach (string name in a)
            {
                // 3.
                // Use FileInfo to get length of each file.
                FileInfo info = new FileInfo(name);
                b += info.Length;
            }
            // 4.
            // Return total size
            return b;

            });
 
        }
        public static async Task DelEmptyFolder(string fol)
        {
             await Task.Run(() =>
            {
                (from _fol in Directory.GetDirectories(fol, "*", SearchOption.TopDirectoryOnly).ToList<string>()
                 where (long)GetDirectorySize(fol).Result <= 0
                 select _fol).ToList<string>()
                     .ForEach(f =>
                     Directory.Delete(f)
                     );
            });
        }
        public static async Task DelEmptyFile(string fol)
        {
            await Task.Run(() =>
            {
                (from file in Directory.GetFiles(fol, "*", SearchOption.TopDirectoryOnly).ToList<string>()
                 where (long)new FileInfo(file).Length <= 0
                 select file).ToList<string>()
             .ForEach(f =>
               Directory.Delete(f)
             );
            });
        }

        public static string SelectFolder()
        {
            string _p = "";
            try
            {
                System.Windows.Forms.FolderBrowserDialog fb = new System.Windows.Forms.FolderBrowserDialog();
                fb.ShowDialog();
                _p = fb.SelectedPath;

                if (_p.Substring(_p.Length - 1) != "\\")
                {
                    _p = _p + "\\";
                }
            }
            catch { _p = ""; }
            return _p;
        }
       
        public static async Task< bool> RenameFileDup(string _Dir, string filters = "*.*", System.IO.SearchOption searchOption = System.IO.SearchOption.TopDirectoryOnly)
        {
            if (!System.IO.Directory.Exists(_Dir)) return false;
            return await Task.Run(() =>
            {
                System.IO.Directory.GetFiles(_Dir, filters, searchOption).ToList<string>()
                    .ForEach(f => FileTor.RenameFileDup(f));
                return true;

            });
         
        }
        private static async Task< IEnumerable<string>> GetFiles(string _Dir, string filter, System.IO.SearchOption searchOption = System.IO.SearchOption.TopDirectoryOnly)
        {
            return await Task.Run(()=>  System.IO.Directory.GetFiles(_Dir, filter, searchOption).ToList<string>());
        }
        private static async Task<IEnumerable<string>> GetFiles(string _Dir, IEnumerable<string> filters, System.IO.SearchOption searchOption = System.IO.SearchOption.TopDirectoryOnly)
        {
            return await Task.Run(() => filters.SelectMany(filter => System.IO.Directory.GetFiles(_Dir, filter, searchOption)).ToList<string>());
        }
        private static async Task<IEnumerable<string>> GetFiles(string _Dir, string[] filters, System.IO.SearchOption searchOption = System.IO.SearchOption.TopDirectoryOnly)
        {
            return await Task.Run(() => filters.SelectMany(filter => System.IO.Directory.GetFiles(_Dir, filter, searchOption)).ToList<string>());
        }
       

public static async Task DeleteDirectory(string path, bool recursive)
		{
            await Task.Run(() =>
            {

                if (recursive)
                {
                    var subfolders = Directory.GetDirectories(path);
                    List<Task> tasks = new List<Task>();
                    foreach (var s in subfolders)
                    {
                        tasks.Add( DeleteDirectory(s, recursive));
                    }
                    Task.WaitAll(tasks.ToArray());
                }
                var files = Directory.GetFiles(path);
                foreach (var f in files)
                {
                    try
                    {
                        var attr = File.GetAttributes(f);
                        if ((attr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                        {
                            File.SetAttributes(f, attr ^ FileAttributes.ReadOnly);
                        }
                        File.Delete(f);
                    }
                    catch (IOException)
                    {
                        //IOErrorOnDelete = true;
                    }
                }

                // At this point, all the files and sub-folders have been deleted.
                // So we delete the empty folder using the OOTB Directory.Delete method.
                Directory.Delete(path);
            });
			
		}
        public static async Task DeleteFile(string _Dir,
            string filter = "*.*",
            System.IO.SearchOption searchOption = System.IO.SearchOption.TopDirectoryOnly,
            double size = 0)
        {
            await Task.Run(() =>
            {
               System.IO.Directory.EnumerateFiles(_Dir, filter, searchOption)
              .ToList<string>()
               .ForEach(file =>
                {
                  if (new System.IO.FileInfo(file).Length == size)
                  try { System.IO.File.Delete(file); }
                  catch { }
                });
            });
           
        }
        public static async Task DeleteFile(string _Dir,
            string filter = "*.*",
            System.IO.SearchOption searchOption = System.IO.SearchOption.TopDirectoryOnly,
            double sizeMin = 0, double sizeMax = 100)
        {
            await Task.Run(() =>
            {
                System.IO.Directory.EnumerateFiles(_Dir, filter, searchOption)
                               .ToList<string>()
                               .ForEach(file =>
                               {
                                   if (new System.IO.FileInfo(file).Length >= sizeMin && new System.IO.FileInfo(file).Length <= sizeMax)
                                       try { System.IO.File.Delete(file); }
                                       catch { }
                               });
            }
            );
            
        }

    }


    
}
