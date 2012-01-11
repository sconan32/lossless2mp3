using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConverterLib
{
    public delegate void ProgressChangedEventHandler(object sender,ProgressChangedEventArgs e);

    public class ProgressChangedEventArgs:
        EventArgs
    {
        public int Progress { get; set; }
    }
}