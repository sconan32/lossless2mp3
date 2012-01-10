using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConverterLib
{
    public class WavWavConverter:WavConverterBase
    {
        public WavWavConverter()
            : base("", "")
        { }
        public override void BeginConvert()
        {
            //base.BeginConvert();
        }
        public override void EndConvert(bool abort)
        {
            //base.EndConvert(abort);
        }
        protected override void BuildArgument()
        {
            
            //throw new NotImplementedException();
        }
        public override string Extension
        {
            get { return "WAV"; }
        }
    }
}
