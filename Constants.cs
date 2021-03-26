using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightningMultifileDownloader
{
    public static class Constants
    {
        public const long BINARY_KBS_IN_MBS = 1024;

        public const int ROW_CONTROL_COUNT = 12;

        public const int CHECKBOX_INDEX = 0;

        public const int FILENAME_INDEX = 2;

        public const int FILESIZE_INDEX = 4;

        public const int CONCURRENT_DOWNLOAD_INDEX = 6;

        public const int PROGRESS_INDEX = 8;

        public const int SECONDS_INDEX = 11;
    }
}
