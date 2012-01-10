using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ConverterLib
{
    public class CueReader:IDisposable
    {
        FileStream cuefile;
        IList<CueSongInfo> songs;
        string audioFile;

        public string AudioFile
        {
            get { return audioFile; }
            set { audioFile = value; }
        }
        public CueReader(string cuefile)
        {
            this.cuefile = File.Open(cuefile, FileMode.Open);
            songs = new List<CueSongInfo>();
        }
        public IList<CueSongInfo> ReadAllSounds()
        {
            CueSongInfo model = new CueSongInfo();
            StreamReader reader = new StreamReader(cuefile,Encoding.Default);
            while (!reader.EndOfStream)
            {
                string str = reader.ReadLine().Trim();
                int splitpos=str.IndexOf(' ');
                string cmd = str.Substring(0, splitpos).ToUpper();
                string value = str.Substring(splitpos).Replace('\"', ' ').Trim() ;
                
                switch (cmd)
                {
                    case "TRACK": 
                        int t ;
                        if (int.TryParse(value.Substring(0, 2),out t))
                            model.Track = t;
                        CueSongInfo song=ReadTrackInfo(reader,model);
                        songs.Add(song);
                        break;
                    case "TITLE":
                        model.Album = value.Trim();
                        break;
                    case "PERFORMER":
                        model.Artist = value.Trim ();
                        break;
                    case "FILE":
                        this.audioFile = value.Substring(0, value.LastIndexOf(' ')).Trim();
                        break;
                    case "REM":
                        string v2cmd = value.Substring(0, value.IndexOf(' ')).ToUpper();
                        string v2value = value.Substring(value.IndexOf(' '));
                        if (v2cmd == "DATE")
                        {
                            model.Year = v2value.Trim ();
                        }
                        else if (v2cmd == "GENRE")
                        {
                            model.Genre = v2value.Trim();
                        }
                        break;
                        
                }
            }
            (songs as List<CueSongInfo>).Sort();
            AdjustTime();

            return songs;
        }
        private void AdjustTime()
        {
            CueTime t ;
            for (int i = 0; i < songs.Count; i++)
            {
                t = songs[i].StartTime+new CueTime(0,0,2);
                songs[i].StartTime = songs[i].EndTime;
                if (i > 0)
                {
                    if (t.ToMiliSeconds() != 0)
                    {
                        songs[i - 1].EndTime = t;
                    }
                    else
                    {
                        songs[i - 1].EndTime = songs[i].StartTime;
                    }
                }
            }
            
        }
        private CueSongInfo ReadTrackInfo(StreamReader reader, CueSongInfo model)
        {
            CueSongInfo info = model.Clone() as CueSongInfo ;
            while (!reader.EndOfStream)
            {
                string str = reader.ReadLine().Trim();
                int splitpos = str.IndexOf(' ');
                string cmd = str.Substring(0, splitpos).ToUpper();
                string value = str.Substring(splitpos).Replace('\"', ' ').Trim();

                switch (cmd)
                {
                    case "TITLE":
                        info.Title = value.Trim ();
                        break;
                    case "PERFORMER":
                        info.Artist = value.Trim();
                        break;
                    case "GERNE":
                        info.Genre = value.Trim();
                        break;
                    case "YEAR":
                        info.Year = value.Trim();
                        break;
                    case "INDEX":
                        int timesplit = value.IndexOf(' ');
                        string type = value.Substring(0, timesplit);
                        string tstr = value.Substring(timesplit).Trim();
                        CueTime time = new CueTime(0,0,0);
                        if (CueTime.TryParse(tstr, out time))
                        {
                            if (type == "00")
                            {

                                info.StartTime = time;
                            }
                            else
                            {
                                info.EndTime = time;
                            }

                        }


                        break;
                    case "REM":
                        string v2cmd = value.Substring(0, value.IndexOf(' ')).ToUpper();
                        string v2value = value.Substring(value.IndexOf(' '));
                        if (v2cmd == "DATE")
                        {
                            model.Year = v2value.Trim ();
                        }
                        else if (v2cmd == "GENRE")
                        {
                            model.Genre = v2value.Trim();
                        }
                        break;
                    case "TRACK":
                        int t;
                        if (int.TryParse(value.Substring(0, 2), out t))
                            model.Track = t;
                        CueSongInfo song = ReadTrackInfo(reader, model);
                        songs.Add(song);
                        break;

                }
            }
            return info;
        }
        private void Dispose(bool fromUser)
        {
            cuefile.Close();
            cuefile.Dispose();
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
