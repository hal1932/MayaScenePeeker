using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace mayapeeker.Views
{
    class ImageIcon : Image
    {
        public static readonly RoutedEvent MouseDoubleClickEvent =
            EventManager.RegisterRoutedEvent(
                "MouseDoubleClick",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(ImageIcon));

        public event RoutedEventHandler MouseDoubleClick
        {
            add { AddHandler(MouseDoubleClickEvent, value); }
            remove { RemoveHandler(MouseDoubleClickEvent, value); }
        }


        #region SourceInfo
        public string SourceInfo
        {
            get { return (string)GetValue(SourcePathProperty); }
            set { SetValue(SourcePathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SourcePath.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourcePathProperty =
            DependencyProperty.Register(
                "SourceInfo", typeof(string),
                typeof(ImageIcon),
                new PropertyMetadata(OnSourceInfoChanged));
        #endregion


        private static void OnSourceInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imageIcon = (ImageIcon)d;

            // set tmp image
            var bitmap = new BitmapImage();
            {
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(@"D:\home\Pictures\ScreenShot\2014y09m20d_135341036.jpg");
                bitmap.EndInit();
                bitmap.Freeze();
            }
            imageIcon.Source = bitmap;
        }


        public ImageIcon()
        {
            PreviewMouseDown += (sender, e) =>
            {
                if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
                {
                    RaiseEvent(
                        new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton)
                        {
                            RoutedEvent = MouseDoubleClickEvent,
                            Source = this,
                        });
                }
            };
        }

    }
}
