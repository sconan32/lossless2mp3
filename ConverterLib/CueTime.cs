using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConverterLib
{
    public struct  CueTime
    {
        public uint Minute;
        public uint Second;
        public uint Frames;
        public static readonly uint FramesPerSecond = 74;
        public CueTime(uint frames)
            :this(0,0,frames)
        {
        
        }
        public CueTime(uint min, uint sec, uint frame)
        {
            Minute = min;
            Second = sec;
            Frames = frame;
        }

        public static bool TryParse(string from, out CueTime time)
        {
            string[] arr = from.Split(':');
            if (arr.Length == 3)
            {
                uint m = 0, s = 0, f = 0;
                if (uint.TryParse(arr[0], out m) &&
                    uint.TryParse(arr[1], out s) &&
                    uint.TryParse(arr[2], out f))
                {
                    time.Minute = m;
                    time.Second = s;
                    time.Frames = f;
                    return true;
                }
            }
            time = new CueTime(0);          
            return false;
        }
        public CueTime Parse(string from)
        {
            
            CueTime time;
            if (TryParse(from,out  time))
            {
                return time;
            }
            throw new ArgumentException("不可解析的字符串格式");
        }
        static public CueTime operator -(CueTime lhs,CueTime rhs)
        {
            CueTime  c=(CueTime) lhs.MemberwiseClone()  ;
            if (c.Frames >= rhs.Frames)
            {
                c.Frames = c.Frames - rhs.Frames;
            }
            else
            {
                c.Frames = c.Frames + CueTime.FramesPerSecond - rhs.Frames;
                --c.Second;
            }
            if (c.Second >= rhs.Second)
            {
                c.Second = c.Second - rhs.Second;
            }
            else
            {
                c.Second = c.Second + 60 - rhs.Second;
                --c.Minute;
            }
            if (c.Minute >= rhs.Minute)
            {
                c.Minute -= rhs.Minute;

            }
            else
            {
                throw new ArgumentException("非法结果（不能从一个较小的时间减去一个较大的时间）");
            }
            return c;
        }
        static public CueTime operator +(CueTime lhs, CueTime rhs)
        {
            CueTime t =( CueTime )lhs.MemberwiseClone() ;
            t.Minute += rhs.Minute;
            t.Second += rhs.Second;
            t.Frames += rhs.Frames;
            if (t.Frames >= CueTime.FramesPerSecond)
            {
                ++t.Second;
                t.Frames -= FramesPerSecond;
            }
            if (t.Second >= 60)
            {
                ++t.Minute;
                t.Second -= 60;
            }
            return t;
        }
        public long ToMiliSeconds()
        {
            long a=0;
            a += 1000 * Frames / FramesPerSecond;
            a += Second * 1000;
            a += Minute * 60 * 1000;
            return a;
        }
        public override string ToString()
        {
            return Minute.ToString() + ":" + Second.ToString() + ":" + Frames.ToString() ;
        }
    }
}
