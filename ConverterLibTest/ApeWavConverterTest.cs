using ConverterLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ConverterLibTest
{
    
    
    /// <summary>
    ///这是 ApeWavConverterTest 的测试类，旨在
    ///包含所有 ApeWavConverterTest 单元测试
    ///</summary>
    [TestClass()]
    public class ApeWavConverterTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///ApeWavConverter 构造函数 的测试
        ///</summary>
        [TestMethod()]
        public void ApeWavConverterConstructorTest()
        {
            ApeWavConverter target = new ApeWavConverter();
            
            Assert.Inconclusive("TODO: 实现用来验证目标的代码");
        }

        /// <summary>
        ///BuildArgument 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("ConverterLib.dll")]
        public void BuildArgumentTest()
        {
            ApeWavConverter_Accessor target = new ApeWavConverter_Accessor(); // TODO: 初始化为适当的值
            target.BuildArgument();
            Assert.Inconclusive("无法验证不返回值的方法。");
        }
        [TestMethod()]
        public void ConvertTest()
        {
            ApeWavConverter tar = new ApeWavConverter();
            tar.BeginConvert("d:\\abc.ape");
            tar.OutputUpdated += new System.Diagnostics.DataReceivedEventHandler(tar_OutputUpdated);
            tar.EndConvert(false);
            
        }

        void tar_OutputUpdated(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.Data);
        }
    }
}
