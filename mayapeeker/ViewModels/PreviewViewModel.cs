﻿
using mayapeeker.Models.Interactivity;
using System.Diagnostics;
using System.Linq;
using System.IO;

namespace mayapeeker.ViewModels
{
    class PreviewViewModel : ViewModelBase
    {
        #region ItemList変更通知プロパティ
        private string[] _ItemList;

        public string[] ItemList
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


        public PreviewViewModel()
        {
            Messenger.AddMessageFilter("SelectedItemChanged");
            Messenger.AddMessageFilter("CurrentDirectoryChanged");
            Messenger.MessageReceived += (sender, e) =>
            {
                if (e.Key == "CurrentDirectoryChanged")
                {
                    UpdateItemList(null);
                }
                else
                {
                    UpdateItemList(e.Content as FileSystemInfo);
                }
            };
        }


        public override void Initialize()
        {
            base.Initialize();
        }


        private void UpdateItemList(FileSystemInfo info)
        {
            if (info is DirectoryInfo)
            {
                var dirInfo = (DirectoryInfo)info;
                var items = dirInfo.GetDirectories().Select(item => item.Name);
                ItemList = items.Concat(dirInfo.GetFiles().Select(item => item.Name)).ToArray();
            }
            else
            {
                ItemList = new string[] { };
            }
        }

    }
}
