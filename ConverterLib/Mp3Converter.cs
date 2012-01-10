using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ConverterLib
{
    public  class Mp3Converter
    {
        Process process = new Process();
        readonly string exename = ".\\tools\\lame.exe";
        string filename;
        string newname;
        CueSongInfo songinfo;
    

        public   Mp3Converter(string wavfile,CueSongInfo info)
        {
            this.filename = wavfile;
            newname = System.IO.Path.ChangeExtension(wavfile, "mp3");
            this.songinfo = info;
        }
        public void BeginConvert(Mp3Config config)
        {

           
            ProcessStartInfo psi = new ProcessStartInfo(exename);

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

            process.StartInfo = psi;
            process.ErrorDataReceived += new DataReceivedEventHandler(process_ErrorDataReceived);
            process.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
        }
        void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (OutputUpdated != null)
            {
                OutputUpdated(sender, e);
            }
        }
        public void EndConvert(bool abort)
        {
            if (abort)
            {
                process.Kill();
            }
            process.WaitForExit();
            process.CancelErrorRead();
            process.CancelOutputRead();

            System.IO.File.Delete(filename);
        }
        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (OutputUpdated != null)
            {
                OutputUpdated(sender, e);
            }
        }
        public event DataReceivedEventHandler OutputUpdated;
        

    }
}
