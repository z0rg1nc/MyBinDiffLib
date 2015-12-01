using System;
using System.IO;

namespace BtmI2p.MyBinDiff.Lib
{
    public class MyBinDiffHelper
    {
        public static byte[] GetPatch(byte[] oldData, byte[] newData)
        {
            using (var ms = new MemoryStream())
            {
                BinaryPatchUtility.Create(
                    oldData,
                    newData,
                    ms
                );
                return ms.ToArray();
            }
        }

        public static byte[] ApplyPatch(byte[] oldData, byte[] patchData)
        {
            using (var msInput = new MemoryStream(oldData))
            {
                var openPatchFuncStream = (Func<Stream>)(
                    () => new MemoryStream(patchData)
                );
                using (var outputStream = new MemoryStream())
                {
                    BinaryPatchUtility.Apply(
                        msInput,
                        openPatchFuncStream,
                        outputStream
                    );
                    return outputStream.ToArray();
                }
            }
        }
    }
}
