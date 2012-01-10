using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConverterLib
{
    public class CueSongInfo:ICloneable,IComparable<CueSongInfo>
    {
        int track;

        public int Track
        {
            get { return track; }
            set { track = value; }
        }
        string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        string artist;

        public string Artist
        {
            get { return artist; }
            set { artist = value; }
        }
        string album;

        public string Album
        {
            get { return album; }
            set { album = value; }
        }
        string genre;

        public string Genre
        {
            get { return genre; }
            set { genre = value; }
        }
        CueTime startTime;

        public CueTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        CueTime endTime;

        public CueTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        string year;

        public string Year
        {
            get { return year; }
            set { year = value; }
        }

        public object Clone()
        {
            return base.MemberwiseClone();
        }

        public int CompareTo(CueSongInfo other)
        {
            if (track < other.track) return -1;
            else if (track > other.track) return 1;
            else return 0;
        }
        public string FileName
        {
            get {
                string fn = title.Replace('/', '／');
                fn = fn.Replace('\\', '＼');
                fn = fn.Replace(':', '：');
                fn=fn.Replace('*','＊');
                fn=fn.Replace ('\"','＂');
                fn=fn.Replace('<','＜');
                fn=fn.Replace('>','＞');
                fn=fn.Replace('|','｜');
                fn=fn.Replace('?','？');
                return track.ToString() +"."+ fn;
            }
        }
    }
}
