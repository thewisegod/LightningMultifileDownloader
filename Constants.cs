using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightningMultifileDownloader
{
    internal static class Constants
    {
        internal const long BINARY_KBS_IN_MBS = 1024;

        internal const int ROW_CONTROL_COUNT = 12;

        internal const int CHECKBOX_INDEX = 0;

        internal const int FILENAME_INDEX = 2;

        internal const int FILESIZE_INDEX = 4;

        internal const int CONCURRENT_DOWNLOAD_INDEX = 6;

        internal const int PROGRESS_INDEX = 8;

        internal const int SECONDS_INDEX = 11;
    }
}
