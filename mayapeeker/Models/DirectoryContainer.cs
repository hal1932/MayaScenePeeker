using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace mayapeeker.Models
{
    public class DirectoryContainer : ModelBase
    {
        public class Item
        {
            public FileSystemInfo FileSystemInfo { get; private set; }
            public FileInfo FileInfo { get; private set; }
            public DirectoryInfo DirectoryInfo { get; private set; }

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

                FileSystemInfo = info;
            }
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
                RaisePropertyChanged("ItemList");
            }
        }
        #endregion


        public DirectoryContainer()
        {
            Messenger.AddMessageFilter("CurrentDirectoryChanged");
            Messenger.AddMessageFilter("FileFilterChanged");

            Messenger.MessageReceived += (sender, e) =>
            {
                switch(e.Key)
                {
                    case "CurrentDirectoryChanged":
                        Reload(e.Content as DirectoryInfo);
                        break;

                    case "FileFilterChanged":
                        _currentFilterArray = e.Content as string[];
                        Reload();
                        break;
                }
            };
        }


        public bool TryGetValue(int index, out FileSystemInfo value)
        {
            value = null;
            if (ItemList == null) return false;
            if (index < 0 || ItemList.Count < index) return false;

            value = ItemList[index].FileSystemInfo;
            return true;
        }


        public void Reload(DirectoryInfo info = null)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                ReloadImpl(info);
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() => ReloadImpl(info));
            }
        }


        private void ReloadImpl(DirectoryInfo info)
        {
            if (info == null)
            {
                if (_currentDirectory == null) return;
                else info = _currentDirectory;
            }
            else
            {
                if (_currentDirectory != null)
                {
                    if (info.FullName == _currentDirectory.FullName) return;
                }
            }

            if (_currentFilterArray == null) return;


            var items = info.GetDirectories().Select(dir => new Item(dir));
            foreach (var filter in _currentFilterArray)
            {
                items = items.Concat(
                    info.GetFiles(filter).Select(file => new Item(file)));
            }
            ItemList = items.ToList();

            _currentDirectory = info;
            Messenger.DispatchMessage(
                new Interactivity.InteractionMessage("CurrentDirectoryChanged", info));
        }


        private DirectoryInfo _currentDirectory;
        private string[] _currentFilterArray;

    }
}
