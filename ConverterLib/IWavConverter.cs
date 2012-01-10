using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConverterLib
{
    public interface IWavConverter
    {
        string Extension { get; }
        void BeginConvert();
        void BeginConvert(string src);
        void EndConvert(bool abort);
    }
}
