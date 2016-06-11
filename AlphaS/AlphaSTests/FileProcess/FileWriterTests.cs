using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlphaS.FileProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
namespace AlphaS.FileProcess.Tests
{
    [TestClass()]
    public class FileWriterTests
    {
        [TestMethod()]
        public void WriteFileTest_TestWriteWithFilePath()
        {
            string testFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + "AlphaSTest.txt";
            if (File.Exists(testFilePath)) File.Delete(testFilePath);
            string testContent = "測試測試\r\n123456";

            FileWriter FW = new FileWriter();
            bool workdone = false;
            FW.OnAllWorkDone += (s, e) => workdone = true;

            FW.WriteFile(testFilePath, testContent);
            while (!workdone) { }

            string result;
            try
            {
                using (var sr = new StreamReader(testFilePath, Encoding.Default))
                {
                    result = sr.ReadToEnd();
                }
                Assert.AreEqual(testContent, result);
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void WriteFileTest1()
        {
            Assert.Fail();
        }
    }
}