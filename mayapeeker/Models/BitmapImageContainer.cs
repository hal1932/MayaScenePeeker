using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace mayapeeker.Models
{
    class BitmapImageContainer
    {
        public BitmapImage this[string key]
        {
            get { return _cache[key].BitmapImage; }
            set
            {
                var item = _cache[key];
                item.BitmapImage = value;
                item.StreamSource = value.StreamSource;
            }
        }


        public bool TryGetValue(string key, out BitmapImage image)
        {
            ImageContainer item;
            if(_cache.TryGetValue(key, out item))
            {
                image = item.BitmapImage;
                return true;
            }

            image = null;
            return false;
        }


        public BitmapImage CreateImage(string key, Bitmap bitmap)
        {
            BitmapImage image;
            if(TryGetValue(key, out image))
            {
                return image;
            }

            var item = CreateImageItem(bitmap);
            _cache[key] = item;

            return item.BitmapImage;
        }


        public BitmapImage CreateImage(string key, Uri uri)
        {
            BitmapImage image;
            if (TryGetValue(key, out image))
            {
                return image;
            }

            image = new BitmapImage();
            image.BeginInit();
            image.UriSource = uri;
            image.EndInit();
            
            image.Freeze();

            _cache[key] = new ImageContainer() { BitmapImage = image };

            return image;
        }


        public BitmapImage CreateImage(string key, FileSystemInfo info)
        {
            BitmapImage image;
            if (TryGetValue(key, out image))
            {
                return image;
            }

            var icon = Icon.ExtractAssociatedIcon(info.FullName);
            var item = CreateImageItem(icon.ToBitmap());

            _cache[key] = item;

            return item.BitmapImage;
        }


        private ImageContainer CreateImageItem(Bitmap bitmap)
        {
            var image = new BitmapImage();

            var stream = new MemoryStream();
            {
                bitmap.Save(stream, ImageFormat.Png);
            }

            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            image.Freeze();

            return new ImageContainer() { BitmapImage = image, StreamSource = stream };
        }


        private class ImageContainer
        {
            public BitmapImage BitmapImage { get; internal set; }
            public Stream StreamSource { get; internal set; }
        }

        private Dictionary<string, ImageContainer> _cache = new Dictionary<string, ImageContainer>();

    }
}
