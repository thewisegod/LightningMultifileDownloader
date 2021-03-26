using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightningMultifileDownloader
{
    public class DownloadResult
    {
        public long FileSize { get; set; }
        public string FileSavePath { get; set; }
        public TimeSpan FileDownloadDuration { get; set; }
        public int ConcurrentDownloads { get; set; }
    }
}
