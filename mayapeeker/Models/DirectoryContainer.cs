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
            get { return _ItemList; }
            set
            { 
                if (_ItemList == value) return;
                _ItemList = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        public DirectoryContainer()
        {
            Messenger.AddMessageHandler(
                Properties.Resources.MsgKey_CurrentDirectoryChanged, (msg) =>
                {
                    Reload(msg.Content as DirectoryInfo, false);
                });
            Messenger.AddMessageHandler(
                Properties.Resources.MsgKey_FileFilterChanged, (msg) =>
                {
                    _currentFilterArray = msg.Content as string[];
                    Reload();
                });
        }


        public bool TryGetValue(int index, out FileSystemInfo value)
        {
            value = null;
            if (ItemList == null) return false;
            if (index < 0 || ItemList.Count < index) return false;

            value = ItemList[index].FileSystemInfo;
            return true;
        }


        public void ApplyFilter(string filter = null)
        {
            if (_baseList == null) return;

            if (filter == null) filter = _currentFilter;

            if (filter == null)
            {
                ItemList = _baseList;
                return;
            }

            ItemList = _baseList
                .Where(item => item.FileSystemInfo.Name.Contains(filter))
                .ToList();

            _currentFilter = filter;
        }


        public void Reload(DirectoryInfo info = null, bool dispatchMessage = true)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                ReloadImpl(info, dispatchMessage);
            }
            else
            {
                Application.Current.Dispatcher.Invoke(() => ReloadImpl(info, dispatchMessage));
            }
        }


        private void ReloadImpl(DirectoryInfo info, bool dispatchMessage)
        {
            if (info == null)
            {
                if (_currentDirectory == null) return;
                else info = _currentDirectory;
            }
            //else
            //{
            //    if (_currentDirectory != null)
            //    {
            //        if (info.FullName == _currentDirectory.FullName
            //            && info.LastWriteTime == _currentDirectory.LastWriteTime)
            //        {
            //            return;
            //        }
            //    }
            //}

            var items = info.GetDirectories().Select(dir => new Item(dir));
            if (_currentFilterArray != null)
            {
                foreach (var filter in _currentFilterArray)
                {
                    items = items.Concat(
                        info.GetFiles(filter).Select(file => new Item(file)));
                }
            }
            _baseList = items.ToList();

            ApplyFilter();

            _currentDirectory = info;

            if (dispatchMessage)
            {
                Messenger.DispatchMessage(
                    Properties.Resources.MsgKey_CurrentDirectoryChanged, info);
            }
        }


        private DirectoryInfo _currentDirectory;
        private string[] _currentFilterArray;
        private List<Item> _baseList;

        private string _currentFilter;

    }
}
