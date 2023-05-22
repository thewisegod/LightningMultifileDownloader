using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;

namespace LightningMultifileDownloader
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private static int _grdFileRowCount;

        private static string _fileSavePath;

        public MainWindow()
        {
            InitializeComponent();

            _fileSavePath = $"{Environment.GetEnvironmentVariable("USERPROFILE")}\\Downloads";

            lblDownloadPath.Content = $"{Environment.GetEnvironmentVariable("USERPROFILE")}\\Downloads";
        }

        private void StackPanel_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                var grdRow = new RowDefinition()
                {
                    Name = $"rd{_grdFileRowCount}",
                    Height = new GridLength(30)
                };
                grdFiles.RowDefinitions.Add(grdRow);

                var chkSelect = new CheckBox()
                {
                    Name = "chkSelect",
                    Margin = new Thickness()
                    {
                        Left = 4
                    },
                    Padding = new Thickness()
                    {
                        Right = 6,
                    },
                    Cursor = Cursors.Hand
                };
                chkSelect.Checked += (s, revt) =>
                {
                    chkSelect.IsChecked = true;

                    var allChecked = true;

                    var rows = grdFiles.Children.Cast<UIElement>().Count() / Constants.ROW_CONTROL_COUNT;

                    for (var rowIndex = 0; rowIndex < rows; rowIndex++)
                    {
                        var foundChkBox = FindGridControl<CheckBox>(Constants.CHECKBOX_INDEX, rowIndex);

                        if (foundChkBox == chkSelect) continue;

                        allChecked = foundChkBox.IsChecked == true;

                        if (!allChecked) break;
                    }

                    if (allChecked)
                        ToggleSelectAll(allChecked);
                };
                chkSelect.Unchecked += (s, revt) =>
                {
                    chkSelect.IsChecked = false;

                    var anyChecked = false;

                    var rows = grdFiles.Children.Cast<UIElement>().Count() / Constants.ROW_CONTROL_COUNT;

                    for (var rowIndex = 0; rowIndex < rows; rowIndex++)
                    {
                        var chk = FindGridControl<CheckBox>(Constants.CHECKBOX_INDEX, rowIndex);

                        if (chk == chkSelect) continue;

                        anyChecked = chk.IsChecked == true;

                        if (anyChecked) break;
                    }

                    if (!anyChecked)
                        ToggleSelectAll(anyChecked);
                };

                Grid.SetRow(chkSelect, _grdFileRowCount);
                Grid.SetColumn(chkSelect, 0);

                var seperator = CreateSeperator("sep1", _grdFileRowCount, 1);

                grdFiles.Children.Add(chkSelect);
                grdFiles.Children.Add(seperator);

                string fileName = e.Data.GetData(DataFormats.StringFormat).ToString();
                fileName = fileName.StartsWith("http") ? fileName : $"http://{fileName}";

                var lblFileName = new Label()
                {
                    Name = "lblFileName",
                    Padding = new Thickness()
                    {
                        Left = 6,
                        Right = 6,
                    },
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Content = fileName,
                    ToolTip = fileName
                };

                Grid.SetRow(lblFileName, _grdFileRowCount);
                Grid.SetColumn(lblFileName, 2);

                seperator = CreateSeperator("sep2", _grdFileRowCount, 3);

                grdFiles.Children.Add(lblFileName);
                grdFiles.Children.Add(seperator);

                var lblFileSize = new Label()
                {
                    Name = "lblFileSize",
                    Padding = new Thickness()
                    {
                        Left = 6,
                        Right = 6,
                    },
                    VerticalContentAlignment = VerticalAlignment.Center,
                };

                Grid.SetRow(lblFileSize, _grdFileRowCount);
                Grid.SetColumn(lblFileSize, 4);

                seperator = CreateSeperator("sep3", _grdFileRowCount, 5);

                grdFiles.Children.Add(lblFileSize);
                grdFiles.Children.Add(seperator);

                var txtConcurrentDownloads = new TextBox()
                {
                    Name = "txtConcurrentDownloads",
                    HorizontalContentAlignment = HorizontalAlignment.Right,
                    Width = 63,
                    Height = 2,
                    Margin = new Thickness()
                    {
                        Left = 6,
                        Right = 6
                    }
                };
                txtConcurrentDownloads.KeyDown += (s, evt) =>
                {
                    evt.Handled = !((evt.Key >= Key.D0 && evt.Key <= Key.D9) || (evt.Key >= Key.NumPad0 && evt.Key <= Key.NumPad9));
                };
                txtConcurrentDownloads.AddHandler(CommandManager.ExecutedEvent, new RoutedEventHandler((s, revta) => 
                {
                    if ((revta as ExecutedRoutedEventArgs).Command == ApplicationCommands.Paste)
                    {
                        if (!int.TryParse(txtConcurrentDownloads.Text, out int result))
                        {
                            txtConcurrentDownloads.Text = string.Empty;
                        }
                    }
                }), true);

                Grid.SetRow(txtConcurrentDownloads, _grdFileRowCount);
                Grid.SetColumn(txtConcurrentDownloads, 6);

                seperator = CreateSeperator("sep4", _grdFileRowCount, 7);

                grdFiles.Children.Add(txtConcurrentDownloads);
                grdFiles.Children.Add(seperator);

                var pbDownload = new ProgressBar()
                {
                    Name = "pbDownload",
                    Minimum = 0,
                    Maximum = 100,
                    Height = 16,
                    Margin = new Thickness()
                    {
                        Left = 4,
                        Right = 4,
                    }
                };

                var tbProgress = new TextBlock()
                {
                    Name = "tbDownloadProgress",
                    Padding = new Thickness()
                    {
                        Left = 6,
                        Top = 7,
                        Right = 6,
                    },
                    Foreground = Brushes.White,
                    TextAlignment = TextAlignment.Center
                };
                tbProgress.SetBinding(TextBlock.TextProperty, new Binding("Value")
                {
                    Source = pbDownload,
                    Mode = BindingMode.OneWay,
                    StringFormat = "{0:0}%"
                });

                Grid.SetRow(pbDownload, _grdFileRowCount);
                Grid.SetColumn(pbDownload, 8);

                Grid.SetRow(tbProgress, _grdFileRowCount);
                Grid.SetColumn(tbProgress, 8);

                seperator = CreateSeperator("sep5", _grdFileRowCount, 9);

                grdFiles.Children.Add(pbDownload);
                grdFiles.Children.Add(tbProgress);
                grdFiles.Children.Add(seperator);

                var lblSeconds = new Label()
                {
                    Name = "lblSeconds",
                    Padding = new Thickness()
                    {
                        Left = 6,
                        Right = 6,
                    },
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center
                };

                Grid.SetRow(lblSeconds, _grdFileRowCount);
                Grid.SetColumn(lblSeconds, 10);

                grdFiles.Children.Add(lblSeconds);

                _grdFileRowCount++;
            }
        }

        private Separator CreateSeperator(string name, int rowIndex, int colIndex)
        {
            var seperator = new Separator()
            {
                Name = name,
                LayoutTransform = new RotateTransform()
                {
                    Angle = 90
                }
            };

            Grid.SetRow(seperator, rowIndex);
            Grid.SetColumn(seperator, colIndex);

            return seperator;
        }

        private void StackPanel_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.None;
        }

        private void ImgDownload_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var rows = grdFiles.Children.Cast<UIElement>().Count() / Constants.ROW_CONTROL_COUNT;

            for(var rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                var chkSelect = FindGridControl<CheckBox>(Constants.CHECKBOX_INDEX, rowIndex);
                if (chkSelect.IsChecked == true)
                {
                    var lblFileName = FindGridControl<Label>(Constants.FILENAME_INDEX, rowIndex);                 
                    var lblFileSize = FindGridControl<Label>(Constants.FILESIZE_INDEX, rowIndex);
                    var txtConcurrentDownloads = FindGridControl<TextBox>(Constants.CONCURRENT_DOWNLOAD_INDEX, rowIndex);
                    var pbDownload = FindGridControl<ProgressBar>(Constants.PROGRESS_INDEX, rowIndex);
                    var lblSeconds = FindGridControl<Label>(Constants.SECONDS_INDEX, rowIndex);

                    int.TryParse(txtConcurrentDownloads.Text, out var concurrentDownloads);

                    DownloadFilesInBackground(chkSelect, lblFileName, lblFileSize, concurrentDownloads, pbDownload, lblSeconds);
                }
            }
        }

        private void DownloadFilesInBackground(CheckBox chkSelect, Label lblFileName, Label lblFileSize, int concurrentDownloads, ProgressBar pbDownload, Label lblSeconds)
        {
            var worker = new BackgroundWorker()
            {
                WorkerReportsProgress = true
            };
            worker.RunWorkerAsync();
            worker.DoWork += (s, rea) =>
            {
                Downloader.Download(this.Dispatcher.Invoke(() => chkSelect), this.Dispatcher.Invoke(() => lblFileName.Content.ToString()), _fileSavePath,
                    this.Dispatcher.Invoke(() => lblFileSize), this.Dispatcher.Invoke(() => pbDownload), this.Dispatcher.Invoke(() => lblSeconds),
                    this.Dispatcher, concurrentDownloads, false);
            };
        }

        private void ImgSavePath_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var fbd = new System.Windows.Forms.FolderBrowserDialog();

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _fileSavePath = fbd.SelectedPath;

                lblDownloadPath.Content = _fileSavePath;
            }
            else
            {
                _fileSavePath = lblDownloadPath.Content.ToString();
            }
        }

        private void ImgRefresh_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var rows = _grdFileRowCount;
            var rowIndex = rows - 1;  
            var rowsToRemove = new List<int>();
        
            while (rowIndex >= 0)
            {
                var chkSelect = FindGridControl<CheckBox>(Constants.CHECKBOX_INDEX, rowIndex);

                if (chkSelect.IsChecked != true)
                {
                    var checkBoxIndex = Constants.CHECKBOX_INDEX + (rowIndex * Constants.ROW_CONTROL_COUNT);

                    RemoveGridRow(checkBoxIndex, rowIndex);

                    rows = _grdFileRowCount;
                }

                rowIndex--;
            }
        }

        private T FindGridControl<T>(int ctrlIndex, int rowIndex) where T : class =>
            grdFiles.Children.Cast<UIElement>().ElementAt(ctrlIndex + (rowIndex * Constants.ROW_CONTROL_COUNT)) as T;

        private List<CheckBox> FindCheckedBoxes() => grdFiles.Children.Cast<CheckBox>().Where(_ => _.IsChecked != true).ToList<CheckBox>();

        private void RemoveGridRow(int ctrlIndex, int rowIndex)
        {
            var itemsRemoved = 0;

            while (itemsRemoved < Constants.ROW_CONTROL_COUNT)
            {
                grdFiles.Children.RemoveAt(ctrlIndex);

                itemsRemoved++;
            }

            grdFiles.RowDefinitions.RemoveAt(rowIndex);

            _grdFileRowCount--;
        }

        private void ChkAllSelect_Checked(object sender, RoutedEventArgs e)
        {
            ToggleFileDownloadSelection(true);
        }

        private void ChkAllSelect_Unchecked(object sender, RoutedEventArgs e)
        {
            ToggleFileDownloadSelection(false);
        }

        private void ToggleFileDownloadSelection(bool select)
        {
            var rows = grdFiles.Children.Cast<UIElement>().Count() / Constants.ROW_CONTROL_COUNT;

            for (var rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                var chkSelect = FindGridControl<CheckBox>(Constants.CHECKBOX_INDEX, rowIndex);

                chkSelect.IsChecked = select;
            }
        }

        private void ToggleSelectAll(bool allChecked)
        {
            chkAllSelect.IsChecked = allChecked;
        }
    }
}
