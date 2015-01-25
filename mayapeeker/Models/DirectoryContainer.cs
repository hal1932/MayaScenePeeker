using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mayapeeker.Models
{
    public class DirectoryContainer : ModelBase
    {
        public class Item
        {
            public FileInfo FileInfo { get; private set; }
            public DirectoryInfo DirectoryInfo { get; private set; }
            public string FullName { get { return _baseInfo.FullName; } }

            public Item(FileSystemInfo info)
            {
                if ((info.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    Debug.Assert(info is DirectoryInfo);
                    DirectoryInfo = info as DirectoryInfo;
                }
                else
                {
                    Debug.Assert(info is FileInfo);
                    FileInfo = info as FileInfo;
                }

                _baseInfo = info;
            }

            private FileSystemInfo _baseInfo;
        }


        #region ItemList変更通知プロパティ
        private List<Item> _ItemList;

        public List<Item> ItemList
        {
            get
            { return _ItemList; }
            set
            { 
                if (_ItemList == value)
                    return;
                _ItemList = value;
                OnPropertyChanged("ItemList");
            }
        }
        #endregion


        public DirectoryContainer()
        {
            Messenger.AddMessageFilter("SelectedDirectoryChanged");
            Messenger.AddMessageFilter("FileFilterChanged");

            Messenger.MessageReceived += (sender, e) =>
            {
                switch(e.Key)
                {
                    case "SelectedDirectoryChanged":
                        Reload(e.Content as DirectoryInfo);
                        break;

                    case "FileFilterChanged":
                        _currentFilterArray = e.Content as string[];
                        Reload();
                        break;
                }
            };
        }


        public void Reload(DirectoryInfo info = null)
        {
            if (_currentFilterArray == null) return;

            if (info == null) info = _currentDirectory;
            if (info == null) return;

            var items = info.GetDirectories().Select(dir => new Item(dir));
            foreach (var filter in _currentFilterArray)
            {
                items = items.Concat(
                    info.GetFiles(filter).Select(file => new Item(file)));
            }
            ItemList = items.ToList();

            _currentDirectory = info;
        }


        private DirectoryInfo _currentDirectory;
        private string[] _currentFilterArray;

    }
}
