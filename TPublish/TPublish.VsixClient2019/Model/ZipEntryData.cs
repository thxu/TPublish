using ICSharpCode.SharpZipLib.Zip;

namespace TPublish.VsixClient2019.Model
{
    /// <summary>
    /// 单个Zip压缩文件数据
    /// </summary>
    public class ZipEntryData
    {
        public ZipEntry ZipEntry { get; set; }

        public byte[] Data { get; set; }
    }
}
