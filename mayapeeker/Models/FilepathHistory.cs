using System;
using System.Collections.Generic;
using System.IO;

namespace mayapeeker.Models
{
    public class FilepathHistory : ModelBase
    {
        public List<DirectoryInfo> ItemStack { get; private set; }

        public DirectoryInfo CurrentDirectoryInfo
        {
            get { return ItemStack[_currentItemIndex]; }
        }

        public bool ExistsBackward
        {
            get { return _currentItemIndex > 0; }
        }

        public bool ExistsForward
        {
            get { return _currentItemIndex < ItemStack.Count - 1; }
        }


        public FilepathHistory()
        {
            Messenger.AddMessageHandler(
                Properties.Resources.MsgKey_CurrentDirectoryChanged, (msg) =>
                {
                    var info = msg.Content as DirectoryInfo;
                    if (info.FullName == CurrentDirectoryInfo.FullName) return;
                    Push(info);
                });
            ItemStack = new List<DirectoryInfo>();
        }


        public void Load()
        {
            var lastItem = AppCache.Get("LastFilepath");
            if (lastItem == null)
            {
                lastItem = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            ItemStack.Add(new DirectoryInfo(lastItem));

            SendCurrentChangedMessage();
        }


        public void Save()
        {
            AppCache.Set("LastFilepath", CurrentDirectoryInfo.FullName);
        }


        public void Push(DirectoryInfo info)
        {
            if (info == null) return;

            // 現在地が「最新のアイテム」を指していない場合、
            // 履歴の整合性を保つために、現在地から先の履歴を削除して、
            // 新しく追加されたものが「最新」になるようにする。
            if (_currentItemIndex < ItemStack.Count - 1)
            {
                ItemStack.RemoveRange(
                    _currentItemIndex + 1, ItemStack.Count - _currentItemIndex - 1);
            }

            ItemStack.Add(info);
            _currentItemIndex = ItemStack.Count - 1;

            SendCurrentChangedMessage();
        }


        public void SetPrevious()
        {
            if (_currentItemIndex <= 0) return;
            --_currentItemIndex;

            SendCurrentChangedMessage();
        }


        public void SetForward()
        {
            if (_currentItemIndex >= ItemStack.Count - 1) return;
            ++_currentItemIndex;

            SendCurrentChangedMessage();
        }


        private int FindItemIndex(DirectoryInfo info)
        {
            return ItemStack.FindIndex(item => item.FullName == info.FullName);
        }


        private void SendCurrentChangedMessage()
        {
            RaisePropertyChanged("CurrentDirectoryInfo");
            Messenger.DispatchMessage(
                Properties.Resources.MsgKey_CurrentDirectoryChanged, CurrentDirectoryInfo);
        }


        private int _currentItemIndex;

    }
}
