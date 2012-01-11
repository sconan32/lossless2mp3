using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace Converter
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.btnStart.IsEnabled = false;
        }
        string[] supportType = { "WAV","APE", "FLAC", "TTA" };
        string cueFile;
        string audioFile;
        IList<ConverterLib.CueSongInfo> songs;
        int stepcnt = 0;
        int step=0;
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {

            if (songs == null || songs.Count <= 0)
            {
                return;
            }
            Thread th = new Thread(() =>
            {
                //ConverterLib.CueReader reader = new ConverterLib.CueReader("d:\\abc.cue");
               // IList<ConverterLib.CueSongInfo> songs= reader.ReadAllSounds();
                txtOutput.Dispatcher.Invoke(new Action(() => { txtOutput.Text = "[√]正在解压缩..."; }));
                //string audiofile = "";
                //foreach (string t in supportType)
                //{
                //    string tmpaudio = System.IO.Path.ChangeExtension(cueFile, t);
                //    if (System.IO.File.Exists(tmpaudio))
                //    {
                //        audiofile = tmpaudio;
                //        break;
                //    }
                //    this.Dispatcher.Invoke(new Action(() =>
                //    {
                //        this.btnStart.IsEnabled = true;
                //        this.btnOpen.IsEnabled = true;
                //    }));
                //}
               
                ConverterLib.IWavConverter converter = ConverterLib.WavConverterFactory.GetWavConverter(audioFile);
                //ConverterLib.ApeWavConverter converter = new ConverterLib.ApeWavConverter();
                //converter.OutputUpdated += new System.Diagnostics.DataReceivedEventHandler(converter_OutputUpdated);
                
                converter.BeginConvert(audioFile);
                converter.EndConvert(false);
                progressBar1.Dispatcher.Invoke(new Action(()=>{progressBar1.Value=(++step)*100/stepcnt;}));
                ConverterLib.WavSplitter split = new ConverterLib.WavSplitter(System.IO.Path.ChangeExtension(audioFile,"wav"));
                //split.OutputUpdated+=new System.Diagnostics.DataReceivedEventHandler(converter_OutputUpdated);
                foreach (ConverterLib.CueSongInfo info in songs)
                {
                    txtOutput.Dispatcher.Invoke(new Action(() => { txtOutput.Text = "[√]正在切割...[" + info.Title + "]"; }));
                    split.BeginSplit(info);
                    split.EndSplit(false);
                    ConverterLib.Mp3Converter con = new ConverterLib.Mp3Converter(split.NewFileName,info);
                    con.ProgressChanged += new ConverterLib.ProgressChangedEventHandler(con_ProgressChanged);
                    txtOutput.Dispatcher.Invoke(new Action(() => { txtOutput.Text = "[√]正在转换...[" + info.Title + "]"; }));
                    con.BeginConvert(new ConverterLib.Mp3Config());
                    con.EndConvert(false);
                     progressBar1.Dispatcher.Invoke(new Action(()=>{progressBar1.Value=(++step)*100/stepcnt;}));

                }
                System.IO.File.Delete(System.IO.Path.ChangeExtension(cueFile, "wav"));
                this.Dispatcher.Invoke(new Action(() =>
                {
                    this.btnStart.IsEnabled = true;
                    this.btnOpen.IsEnabled = true;
                    txtOutput.Text = "[√]全部完成";
                }));
            });
            
            this.btnOpen.IsEnabled = false;
            this.btnStart.IsEnabled = false;
            th.Start();
        }

        void con_ProgressChanged(object sender, ConverterLib.ProgressChangedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                SubProgressBar.Value = e.Progress;
            }));
        }
     
        void converter_OutputUpdated(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
                {
                    this.txtOutput.Text = e.Data+Environment.NewLine ;
                    this.txtOutput.ScrollToEnd();
                }));
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.Clear();
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "CUE文件(*.cue)|*.cue|无损音乐文件(*.wav,*.ape,*.flac,*.tta)|"+
                "*.wav;*.ape;*.flac;*.tta|所有文件(*.*)|*.*";
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == true)
            {
                audioFile = string.Empty;
                cueFile = string.Empty;
                txtType.Text = " - - - -";
                FindAudioFile(ofd.FileName);
                
               
                if (!string.IsNullOrEmpty(cueFile))
                {
                    this.txtCueFile.Text = cueFile;
                    //ConverterLib.CueReader reader = new ConverterLib.CueReader(cueFile);
                    //songs = reader.ReadAllSounds();

                    lstSongs.DataContext = songs;

                    stepcnt = songs.Count + 1;
                }
                else
                {
                    this.txtCueFile.Text = "没有匹配的CUE文件";
                    stepcnt = 1;
                }
                step = 0;
                progressBar1.Value = 0;
                if (string.IsNullOrEmpty(audioFile))
                {

                    txtOutput.Text = "[×]找不到音频文件";
                    btnStart.IsEnabled = false;
                }
                else
                {
                    btnStart.IsEnabled = true;
                    this.txtType.Text = System.IO.Path.GetExtension(audioFile).Substring(1).ToUpper();
                }
            }
        }
        private void FindAudioFile(string file)
        {
            string srcExt = System.IO.Path.GetExtension(file).ToUpper();
            if (srcExt == ".CUE")
            {
                cueFile = file;
            }
            else
            {
                string tmpcue = System.IO.Path.ChangeExtension(file, "cue");
                if (System.IO.File.Exists(tmpcue))
                {
                    cueFile = tmpcue;
                }
            }
            if (!string.IsNullOrEmpty(cueFile))
            {
                ConverterLib.CueReader reader = new ConverterLib.CueReader(cueFile);
                songs = reader.ReadAllSounds();
                string tmpaudio = System.IO.Path.GetDirectoryName(file) + "\\" + reader.AudioFile;
                if (System.IO.File.Exists(tmpaudio))
                {
                    audioFile = tmpaudio;
                    return;
                }
                else
                {
                    txtOutput.Text = "[!]CUE文件中指定了错误的音频文件";
                }
            }
            else
            {
                txtOutput.Text = "[!]找不到CUE文件";
            }

            if (Array.IndexOf(supportType, srcExt.Substring(1)) >= 0)
            {
                audioFile = file;
            }
            else
            {
                foreach (string sup in supportType)
                {
                    string tmpfile = System.IO.Path.ChangeExtension(file, sup);
                    if (System.IO.File.Exists(tmpfile))
                    {
                        audioFile = tmpfile;
                        break;
                    }
                }
            }
        }

    }
}
