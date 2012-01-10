using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConverterLib
{
    public class WavConverterFactory
    {
        public static IWavConverter GetWavConverter(string src)
        {
            IWavConverter conv=null;
            string extUp = System.IO.Path.GetExtension(src).ToUpper();
            switch (extUp)
            {
                case ".APE": 
                    conv = new ApeWavConverter();
                    break;
                case ".FLAC":
                    conv = new FlacWavConverter();
                    break;
                case ".TTA":
                    conv = new TtaWavConverter();
                    break;
                case ".WAV":
                    conv = new WavWavConverter();
                    break;
            }
            return conv;
        }
    }
}
