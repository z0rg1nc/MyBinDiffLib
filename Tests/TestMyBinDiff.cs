using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BtmI2p.MiscUtils;
using BtmI2p.MyBinDiff.Lib;
using Xunit;
using Xunit.Abstractions;

namespace BtmI2p.MyBinDiff.Tests
{
    public class TestMyBinDiff
    {
        private readonly ITestOutputHelper _output;
        public TestMyBinDiff(ITestOutputHelper output)
        {
	        _output = output;
        }
        [Fact]
        public void TestZipArchives()
        {
            const string zip1Name = "0.0.1.0(1).zip";
            const string zip2Name = "0.0.1.0(2).zip";
            var zip1Content = File.ReadAllBytes(zip1Name);
            var zip2Content = File.ReadAllBytes(zip2Name);
            byte[] zipDelta;
            using (var sw = new StopWatchDisposable("GetPatch"))
            {
                zipDelta = MyBinDiffHelper.GetPatch(
                    zip1Content,
                    zip2Content
                );
            }
            File.WriteAllBytes("delta.bin",zipDelta);
            _output.WriteLine(
                "{0} {1} {2}", 
                zip1Content.Length, 
                zip2Content.Length, 
                zipDelta.Length
            );
            byte[] zip2ContentCopy;
            using (var sw = new StopWatchDisposable("ApplyPatch"))
            {
                zip2ContentCopy = MyBinDiffHelper.ApplyPatch(
                    zip1Content,
                    zipDelta
                );
            }
            Assert.Equal(zip2Content,zip2ContentCopy);
        }

        [Fact]
        public async Task Test1()
        {
            var data1 = new byte[1000000];
            MiscFuncs.GetRandomBytes(data1);
            var data2 = new byte[1000000];
            MiscFuncs.GetRandomBytes(data2);
            var xData1 = data1.ToArray();
            var xData2 = data1.Concat(data2).ToArray();
            _output.WriteLine("{0}",xData2.Length);
            byte[] xDataDiff;
            using (var sw = new StopWatchDisposable("GetPatch"))
            {
                xDataDiff = MyBinDiffHelper.GetPatch(
                    xData1,
                    xData2
                );
            }
            _output.WriteLine("{0}",xDataDiff.Length);
            byte[] xData2Copy;
            using (var sw = new StopWatchDisposable("ApplyPatch"))
            {
                xData2Copy = MyBinDiffHelper.ApplyPatch(
                    xData1,
                    xDataDiff
                );
            }
            _output.WriteLine("{0}",xData2Copy.Length);
            Assert.Equal(xData2,xData2Copy);
        }
    }
}
