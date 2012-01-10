using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ConverterLib
{
    public class ApeWavConverter:WavConverterBase
    {
        public ApeWavConverter():base("-d",".\\tools\\mac.exe")
        {
        }


       
        protected override  void BuildArgument()
        {
            cmd.Append("\"");
            cmd.Append(SourceFile);
            cmd.Append("\"");
            cmd.Append(" \"");
            cmd.Append(TargetFile);
            cmd.Append("\"");

            cmd.Append(" ");
            cmd.Append(argswitch );
        }
        public override string Extension
        {
            get { return "APE"; }
        }
    }
}
