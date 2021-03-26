using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LightningMultifileDownloader
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void StackPanel_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                MessageBox.Show(e.Data.GetData(DataFormats.StringFormat) as string);
                //        < CheckBox Name = "chkSelect" Grid.Column = "0" Margin = "4,0,0,0" Padding = "0,0,6,0" />

                //< Separator Grid.Column = "1" >

                //    < Separator.LayoutTransform >

                //        < RotateTransform Angle = "90" />

                //        </ Separator.LayoutTransform >

                //    </ Separator >

                //    < Label Grid.Column = "2" Padding = "6,0,6,0" VerticalContentAlignment = "Top" > File Url </ Label >
                var grdRow = new RowDefinition()
                {
                    Height = new GridLength(20)
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
                        Left = 6,
                        Top = 0,
                        Right = 6,
                        Bottom = 0
                    }
                };

                Grid.SetRow(chkSelect, 0);
                Grid.SetColumn(chkSelect, 0);

                var seperator = new Separator()
                {
                    LayoutTransform = new RotateTransform()
                    {
                        Angle = 90
                    }
                };

                Grid.SetRow(seperator, 0);
                Grid.SetColumn(seperator, 1);

                var lblFileName = new Label()
                {
                    Name = "lblFileName",
                    Padding = new Thickness()
                    {
                        Right = 6,
                    },
                    VerticalContentAlignment = VerticalAlignment.Top,
                    Content = e.Data.GetData(DataFormats.StringFormat)
                };

                Grid.SetRow(lblFileName, 0);
                Grid.SetColumn(lblFileName, 2);

                grdFiles.Children.Add(chkSelect);
                grdFiles.Children.Add(seperator);
                grdFiles.Children.Add(lblFileName);
            }
        }

        private void StackPanel_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.None;
        }
    }
}
