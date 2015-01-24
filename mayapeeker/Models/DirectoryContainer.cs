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


        #region ItemArray変更通知プロパティ
        private Item[] _ItemArray;

        public Item[] ItemArray
        {
            get
            { return _ItemArray; }
            set
            { 
                if (_ItemArray == value)
                    return;
                _ItemArray = value;
                OnPropertyChanged("ItemArray");
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
                }
            };
        }


        public void Reload(DirectoryInfo info)
        {
            var itemList = new List<Item>();
            foreach (var dir in info.GetDirectories())
            {
                itemList.Add(new Item(dir));
            }
            foreach (var file in info.GetFiles())
            {
                itemList.Add(new Item(file));
            }
            ItemArray = itemList.ToArray();
        }

    }
}
