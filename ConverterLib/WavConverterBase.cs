using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ConverterLib
{
   public abstract  class WavConverterBase:IWavConverter
    {
       protected WavConverterBase(string argswch, string exe)
       {
           this.exename = exe;
           this.argswitch = argswch;

           cmd = new StringBuilder();
          
       }

       void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
       {
           if (OutputUpdated != null)
           {
               OutputUpdated(sender, e);
           }
       }

        protected  Process process = new Process();
        protected  StringBuilder cmd;
        private string sourceFile;

        protected string SourceFile
        {
            get { return sourceFile; }
            set 
            { 
                sourceFile = value;
                targetFile = value.Replace(value.Substring(value.LastIndexOf(".")), ".wav") ;
            }
        }
        private string targetFile;

        protected string TargetFile
        {
            get { return targetFile; }
            set { targetFile = value; }
        }

        protected readonly string argswitch ;
        protected readonly string exename;

        protected abstract void BuildArgument();
        public virtual void BeginConvert()
        {
            
            BuildArgument();

            ProcessStartInfo psi = new ProcessStartInfo(exename);
            psi.UseShellExecute = false;
            psi.Arguments = cmd.ToString();
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
        public virtual void BeginConvert(string src)
        {
            this.SourceFile = src;
            BeginConvert();
        }
        public virtual void EndConvert(bool abort)
        {
            if (abort)
            {
                process.Kill();
            }
            process.WaitForExit();
        }
        public event DataReceivedEventHandler OutputUpdated;
        public abstract string Extension { get; }
    }
}
