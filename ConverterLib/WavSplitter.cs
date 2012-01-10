using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ConverterLib
{
    public  class WavSplitter
    {
        Process process = new Process();
        readonly string exename=".\\tools\\cutter.exe";
        string filename;
        string path;
        string newFileName;

        public string NewFileName
        {
            get { return newFileName; }
           
        }
        public WavSplitter(string wavfile)
        {
            filename = wavfile;
            path = filename.Substring(0, filename.LastIndexOf('\\'));
        }
        

        public void BeginSplit(CueSongInfo info)
        {

            long mstart = info.StartTime.ToMiliSeconds() ;
            long mend = info.EndTime.ToMiliSeconds();
            newFileName = path + "\\" + info.FileName + ".wav";
            string newfilename ="\""+path+"\\"+ info.FileName+".wav\"";
            
            ProcessStartInfo psi = new ProcessStartInfo(exename);
            psi.Arguments = "\""+filename + "\" " + mstart.ToString() +
                " " + mend.ToString () + " "+newfilename;
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
        public void EndSplit(bool abort)
        {
            if (abort)
            {
                process.Kill();
            }
            process.WaitForExit();
            process.CancelErrorRead();
            process.CancelOutputRead();
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
