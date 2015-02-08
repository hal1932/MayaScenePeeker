using mayapeeker.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
                "SourceInfo", typeof(FileSystemInfo),
                typeof(ImageIcon),
                new PropertyMetadata(OnSourceInfoChanged));
        #endregion


        private static void OnSourceInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imageIcon = (ImageIcon)d;
            var info = (FileSystemInfo)e.NewValue;

            BitmapImage image;
            if (info.Attributes == FileAttributes.Directory)
            {
                image = _imageContainer.CreateImage("directory", Properties.Resources.folder);
                imageIcon.Stretch = System.Windows.Media.Stretch.None;
            }
            else
            {
                image = _imageContainer.CreateImage(info.FullName, info);
                imageIcon.Stretch = System.Windows.Media.Stretch.Uniform;
            }
            imageIcon.Source = image;
        }


        public ImageIcon()
        {
            PreviewMouseDown += ImageIcon_PreviewMouseDown;
        }


        ~ImageIcon()
        {
            PreviewMouseDown -= ImageIcon_PreviewMouseDown; 
        }


        void ImageIcon_PreviewMouseDown(object sender, MouseButtonEventArgs e)
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
        }


        private static BitmapImageContainer _imageContainer = new BitmapImageContainer();

    }
}
