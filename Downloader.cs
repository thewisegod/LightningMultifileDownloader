using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace LightningMultifileDownloader
{
    internal static class Downloader
    {  
        static Downloader()
        {
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.DefaultConnectionLimit = 100;
            ServicePointManager.MaxServicePointIdleTime = 1000;
        }

        internal static void Download(CheckBox chkSelect, string fileUrl, string saveFolder, Label lblFileSize, ProgressBar progressBar, Label lblSeconds, Dispatcher dispatcher, int concurrentDownloads = 0, bool validateSSL = false)
        {
            if (!validateSSL) { ServicePointManager.ServerCertificateValidationCallback = delegate { return true; }; };

            var uri = new Uri(fileUrl);
 
            var savePath = Path.Combine(saveFolder, uri.Segments.Last());

            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }

            if (concurrentDownloads <= 0)
            {
                concurrentDownloads = Environment.ProcessorCount;
            }

            var progressPercentage = Math.Round((100d / concurrentDownloads), 1);
 
            var fileSize = GetFileSize(fileUrl);

            if (fileSize > 0)
            {
                dispatcher.Invoke(() => lblFileSize.Content = $"{(fileSize / Constants.BINARY_KBS_IN_MBS):n0} MBs");

                if (string.IsNullOrWhiteSpace(Path.GetExtension(savePath)))
                {
                    if (savePath.EndsWith("/"))
                    {
                        savePath = $"{savePath.Remove(savePath.LastIndexOf("\\") + 1)}file.exe";
                    }
                }

                using (var destinationStream = new FileStream(savePath, FileMode.Append))
                {
                    var tempFilesDictionary = new ConcurrentDictionary<long, string>();

                    var readRanges = BuildChunkRangeList(concurrentDownloads, fileSize);

                    var startTime = DateTime.Now;

                    Parallel.ForEach(readRanges, new ParallelOptions() { MaxDegreeOfParallelism = concurrentDownloads }, readRange =>
                    {
                        CreateTempFiles(fileUrl, readRange, tempFilesDictionary);

                        dispatcher.Invoke(() => progressBar.Value += progressPercentage);
                    });

                    CreateFile(destinationStream, tempFilesDictionary);

                    dispatcher.Invoke(() =>
                    {
                        lblSeconds.Content = DateTime.Now.Subtract(startTime).Seconds;
                        progressBar.Value = progressBar.Maximum;
                        chkSelect.IsChecked = false;
                    });
                }
            }
        }

        private static void CreateFile(FileStream destinationStream, ConcurrentDictionary<long, string> tempFilesDictionary)
        {
            foreach (var tempFile in tempFilesDictionary.OrderBy(_ => _.Key))
            {
                var tempFileBytes = File.ReadAllBytes(tempFile.Value);

                destinationStream.Write(tempFileBytes, 0, tempFileBytes.Length);

                File.Delete(tempFile.Value);
            }
        }

        private static List<Range> BuildChunkRangeList(int concurrentDownloads, long responseLength)
        {
            var readRanges = new List<Range>();

            for (var chunk = 0; chunk < concurrentDownloads - 1; chunk++)
            {
                readRanges.Add((chunk * (responseLength / concurrentDownloads)).To(((chunk + 1) * (responseLength / concurrentDownloads)) - 1));
            }

            readRanges.Add((readRanges.Any() ? readRanges.Last().End + 1 : 0).To(responseLength - 1));

            return readRanges;
        }

        private static void CreateTempFiles(string fileUrl, Range readRange, ConcurrentDictionary<long, string> segmentDictionary)
        {
            var webRequestGet = HttpWebRequest.Create(fileUrl) as HttpWebRequest;
            webRequestGet.Method = "GET";
            webRequestGet.AddRange(readRange.Start, readRange.End);

            using (var webResponse = webRequestGet.GetResponse() as HttpWebResponse)
            {
                var tempFilePath = Path.GetTempFileName();

                using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    webResponse.GetResponseStream().CopyTo(fileStream);

                    segmentDictionary.TryAdd(readRange.Start, tempFilePath);
                }
            }
        }

        internal static long GetFileSize(string fileUrl)
        {
            var webRequestHead = HttpWebRequest.Create(fileUrl);
            webRequestHead.Method = "HEAD";

            var fileSize = 0L;

            using (var webResponse = webRequestHead.GetResponse())
            {
                try
                {
                    fileSize = long.Parse(webResponse.Headers.Get("Content-Length"));
                }
                catch (ArgumentNullException anex)
                {
                    MessageBox.Show(anex.ToString());
                    return 0;
                }
            }

            return fileSize;
        }
    }
}