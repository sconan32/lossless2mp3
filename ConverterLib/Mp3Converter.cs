using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;
namespace ConverterLib
{
    public  class Mp3Converter
    {
        Process process = new Process();
        Thread lameThread;
        readonly string exename = ".\\tools\\lame.exe";
        int percentDone;
        bool isRunning;
        Regex regexLine;
        string filename;
        string newname;
        CueSongInfo songinfo;
        ProcessStartInfo psi;
        

        public   Mp3Converter(string wavfile,CueSongInfo info)
        {
            this.filename = wavfile;
            newname = System.IO.Path.ChangeExtension(wavfile, "mp3");
            this.songinfo = info;
            InitStartInfo();
            regexLine = new Regex(
                @"(\d+)/(\d+)\D+(\d+)\D+([\d:]+)\D+([\d:]+)\D+([\d:]+)\D+([\d:]+)\D+([\d\.]+)\D+([\d:]+)",
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant |
                RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        }
        private void Reset()
        {
            percentDone = 0;
            process = new Process();
            process.StartInfo = psi;
        }
        private void InitStartInfo()
        {
            psi = new ProcessStartInfo(exename);

            string arg = string.Format("-b 320 \"{0}\" \"{1}\" " +
                "--add-id3v2 --id3v2-ucs2 --tt \"{2}\" --ta \"{3}\" --tl \"{4}\" --ty \"{5}\" " +
                "--tn \"{6}\" --tg \"{7}\" ",
                filename, newname, songinfo.Title, songinfo.Artist,
                songinfo.Album, songinfo.Year, songinfo.Track, songinfo.Genre);
            psi.Arguments = arg;
            psi.UseShellExecute = false;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            psi.RedirectStandardError = true;
        }
        public bool BeginConvert(Mp3Config config)
        {
            if (isRunning)
                return false;
            Reset();
            try
            {
                lameThread = new Thread(new ThreadStart(LameReader));
                lameThread.IsBackground = true;
                lameThread.Name = "LameReader";
                lameThread.Start();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
       
        public void EndConvert(bool abort)
        {
            if (abort)
            {
                lameThread.Abort();
            }
           
            lameThread.Join();
            lameThread = null;
            System.IO.File.Delete(filename);
        }
       
        private void LameReader()
        {
            Match m;
            string line;
            try
            {
                process.Start();
                line = process.StandardError.ReadLine();
                while (line != null && lameThread.ThreadState != System.Threading.ThreadState.Running)
                {
                    m = regexLine.Match(line);
                    if (m.Success)
                    {
                        percentDone = int.Parse(m.Groups[3].Value);
                        OnProcessChanged(percentDone);
                    }
                    else
                    { }
                    //取得一行可能会阻塞线程
                    line = process.StandardError.ReadLine();
                }
                process.Close();
                process = null;

                if (percentDone == 100)
                {
                    OnTaskFinished();
                }
                else
                {
                    OnTaskCanceled();
                }
            }
            catch (Exception ex)
            {
                if (process != null)
                {
                    process.Close();
                    process = null;
                }
            }
            finally
            {
                isRunning = false;
            }
        }
        protected void OnProcessChanged(int progress)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(this, new ProgressChangedEventArgs() { Progress = progress });
            }
        }
        protected void OnTaskFinished()
        {
            if (TaskFinished != null)
            {
                TaskFinished(this, EventArgs.Empty);
            }
        }
        protected void OnTaskCanceled()
        {
            if (TaskCanceled != null)
            {
                TaskCanceled(this, EventArgs.Empty);
            }
        }
       
        public event ProgressChangedEventHandler ProgressChanged;
        public event EventHandler TaskFinished;
        public event EventHandler TaskCanceled;
    }
}
