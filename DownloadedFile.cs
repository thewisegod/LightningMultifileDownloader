using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightningMultifileDownloader
{
    internal class DownloadedFile
    {
        internal long Size { get; set; }
        internal string SavePath { get; set; }
        internal TimeSpan DownloadDuration { get; set; }
        internal int ConcurrentDownloads { get; set; }
    }
}
