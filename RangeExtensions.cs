using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LightningMultifileDownloader
{
    internal static class RangeExtensions
    {
        internal static Range To(this long start, long end) => new Range { Start = start, End = end };
    }
}
