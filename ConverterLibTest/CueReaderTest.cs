using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConverterLib;
namespace ConverterLibTest
{
    [TestClass]
    public class CueReaderTest
    {
        [TestMethod]
        public void TestRead()
        {
            string cue = "D:\\test.cue";
            ConverterLib.CueReader reader = new ConverterLib.CueReader(cue);
            List<CueSongInfo> list=reader.ReadAllSounds() as List<CueSongInfo>;
            Assert.AreNotEqual(list.Count, 0);

        }
    }
}
