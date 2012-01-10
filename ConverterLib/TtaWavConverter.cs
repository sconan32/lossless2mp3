using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConverterLib
{
    public class TtaWavConverter:WavConverterBase
    {
        public TtaWavConverter()
            : base("-d", ".\\tools\\ttaenc.exe")
        { }
        protected override void BuildArgument()
        {
            cmd.Append(argswitch);
            cmd.Append(" \"");
            cmd.Append(SourceFile);
            cmd.Append("\"");
        }
        public override string Extension
        {
            get { return "TTA"; }
        }
    }

}
