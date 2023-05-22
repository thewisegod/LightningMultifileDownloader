using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LightningMultifileDownloader
{
    internal class ServicePointManagerWrapper
    {
        internal ServicePointManagerWrapper(bool expect100Continue = false, int defaultConnectionLimit = 100, int maxServicePointIdleTime = 1000)
        {
            ServicePointManager.Expect100Continue = expect100Continue;
            ServicePointManager.DefaultConnectionLimit = defaultConnectionLimit;
            ServicePointManager.MaxServicePointIdleTime = maxServicePointIdleTime;
        }
    }
}
