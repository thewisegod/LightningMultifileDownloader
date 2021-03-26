using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LightningMultifileDownloader
{
    public static class RangeExtensions
    {
        public static Range To(this long start, long end) => new Range { Start = start, End = end };
    }
}
