using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace ConverterLib
{
    public  class WavSplitter
    {
        Process process = new Process();
        readonly string exename=".\\tools\\cutter.exe";
        string filename;
        string path;
        string newFileName;
        long fileSize;
        int bytesPerSec;
        int soundDataLen;
        byte[] fileHead;
        int fileHeadLen;

        const int bufferLen = 1 * 1024 * 1024;
        byte[] buffer = new byte[bufferLen];


        public string NewFileName
        {
            get { return newFileName; }
           
        }
        public WavSplitter(string wavfile)
        {
            filename = wavfile;
            path = filename.Substring(0, filename.LastIndexOf('\\'));
            AnalyzeWav();
        }

        private void AnalyzeWav()
        {
            uint fmtlength;
            uint fmtend, factend;
            uint temp;
            byte[] buffer = new byte[64];
            FileStream stream = File.Open(filename, FileMode.Open);
            fileSize = stream.Length;
            
            stream.Read(buffer, 0, 64);
            //文件开始4个节点必须为RIFF
            if (!(buffer[0] == 0x52 && buffer[1] == 0x49 && buffer[2] == 0x46 && buffer[3] == 0x46))
                throw new ArgumentException("WAV文件无效：RIFF");
            //第4-7字节存放文件长度
            temp = BitConverter.ToUInt32(buffer, 4);
            if(temp==fileSize-8)
            {

            }
            //第8-11字节存放WAVE
            if (!(buffer[8] == 0x57 && buffer[9] == 0x41 && buffer[10] == 0x56 && buffer[11] == 0x45))
            {
                throw new ArgumentException("WAV文件无效：WAVE");
            }
            //第12-15字节存放fmt(20H)
            if (!(buffer[12] == 0x66 && buffer[13] == 0x6D && buffer[14] == 0x74 && buffer[15] == 0x20))
            {
                throw new ArgumentException("WAV文件无效：fmt_");
            }
            //第16-19字节存放fmt段长度（为16）为18时有2字节附加数据
            fmtlength = BitConverter.ToUInt32(buffer, 16);
            if (fmtlength == 16)
            {
            }
            else if (fmtlength == 18)
            {
            }
            else
            {
                throw new ArgumentException("WAV文件无效：fmt_段长度无效");
            }
            //第20-21字节存放文件编码格式，PCM=1
            temp = BitConverter.ToUInt16(buffer, 20);
            if(temp !=1)
            {
                throw new ArgumentException("WAV文件无效：编码格式不为PCM");
            }
            //第22-23字节存放声道数
            //temp=BitConverter.ToUInt16(buffer,22);
            //第24-27字节存放采样率  SamplesPerSec  
            //temp = BitConverter.ToUInt32(buffer, 24);
            //第28-31字节存放 BytesPerSec 
            //音频数据传送速率, 单位是字节。其值为采样率×每次采样大小。播放软件利用此值可以估计缓冲区的大小。
            bytesPerSec =(int) BitConverter.ToUInt32(buffer, 28);
            //第32-33字节存放每次采样的大小BlockAlign             
            //每次采样的大小 = 采样精度*声道数/8(单位是字节); 这也是字节对齐的最小单位, 譬如 16bit 立体声在这
            //temp = BitConverter.ToUInt16(buffer,32);
            //第34-35字节存放采样精度 BitsPerSample
            //每个声道的采样精度; 譬如 16bit 在这里的值就是16。如果有多个声道，则每个声道的采样精度大小都一样的。
            //temp = BitConverter.ToUInt16(buffer, 34);
            if (fmtlength == 16)
            {
                fmtend= 36;
            }
            else
            {
                fmtend = 38;
            }
            //判断是否存在fact段
            if(buffer[fmtend]==0x66 &&
                buffer[fmtend+1]==0x61 &&
                buffer[fmtend+2]==0x63 &&
                buffer[fmtend+3]==0x74)
            {
                //读取fact段长度：
                temp=BitConverter.ToUInt32(buffer,(int)fmtend+4);
                factend=fmtend+8+temp;
            }
            else 
            {
                   factend=fmtend;
            }
            //判断data段
            if (buffer[factend] == 0x64 &&
                 buffer[factend + 1] == 0x61 &&
                 buffer[factend + 2] == 0x74 &&
                 buffer[factend + 3] == 0x61)
            {
                //获取data段长度
                soundDataLen = (int)BitConverter.ToUInt32(buffer, (int)factend + 4);

                //将头部保存
                fileHead = buffer;
                fileHeadLen = (int)factend + 4;
            }
            else
            {
                throw new ArgumentException("WAV文件无效：data段");
            }
            stream.Close();
        }
        public void BeginSplit(CueSongInfo info)
        {

            long mstart = info.StartTime.ToMiliSeconds() ;
            long mend = info.EndTime.ToMiliSeconds();
            newFileName = path + "\\" + info.FileName + ".wav";
            string newfilename ="\""+path+"\\"+ info.FileName+".wav\"";

            long startpos = mstart / 1000 * bytesPerSec + bytesPerSec / 1000 * (mstart % 1000);
            long endpos = mend / 1000 * bytesPerSec + bytesPerSec / 1000 * (mend % 1000);
            if (endpos == 0)
            {
                endpos = fileSize;
            }
            Split(startpos, endpos, newFileName);
            //ProcessStartInfo psi = new ProcessStartInfo(exename);
            //psi.Arguments = "\""+filename + "\" " + mstart.ToString() +
            //    " " + mend.ToString () + " "+newfilename;
            //psi.UseShellExecute = false;
            //psi.WindowStyle = ProcessWindowStyle.Hidden;
            //psi.RedirectStandardOutput = true;
            //psi.CreateNoWindow = true;
            //psi.RedirectStandardError = true;

            //process.StartInfo = psi;
            //process.ErrorDataReceived += new DataReceivedEventHandler(process_ErrorDataReceived);
            //process.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);
           
            //process.Start();
            //process.BeginOutputReadLine();
            //process.BeginErrorReadLine();
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
            //if (abort)
            //{
            //    process.Kill();
            //}
            //process.WaitForExit();
            //process.CancelErrorRead();
            //process.CancelOutputRead();
        }
        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (OutputUpdated != null)
            {
                OutputUpdated(sender, e);
            }
        }
        void Split(long startPos, long endPos, string newFileName)
        {
            FileStream rfs = File.Open(filename, FileMode.Open);
            FileStream wfs = File.Open(newFileName, FileMode.Create);
            long newfileSize;
            if (startPos >= fileSize || endPos > fileSize)
            {
                throw new ArgumentException("分割长度超出文件大小。");
            }
            if (startPos > endPos)
            {
                throw new ArgumentException("起始位置不能大于结束位置");
            }
            //将wav文件头写入新文件
            wfs.Write(fileHead, 0, fileHeadLen);
            //写入文件大小
            wfs.Seek(4, SeekOrigin.Begin);
            newfileSize = endPos - startPos ;
            wfs.Write(BitConverter.GetBytes((int)(newfileSize+fileHeadLen-8)), 0, 4);
            wfs.Seek(fileHeadLen, SeekOrigin.Begin);
           //写入data段长度
            wfs.Write(BitConverter.GetBytes((int)newfileSize), 0, 4);
            //写入data数据
            rfs.Seek(fileHeadLen + 4+startPos, SeekOrigin.Begin);
            int total=(int)newfileSize;
            while(total >= bufferLen)
            {
                rfs.Read(buffer, 0, bufferLen);
                wfs.Write(buffer, 0, bufferLen);
                total -= bufferLen;
            }
            rfs.Read(buffer, 0, total);
            wfs.Write(buffer, 0, total);
            rfs.Close();
            wfs.Close();
        }
        public event DataReceivedEventHandler OutputUpdated;
    }
}
