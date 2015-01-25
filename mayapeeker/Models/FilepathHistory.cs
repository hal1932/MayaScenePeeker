using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mayapeeker.Models
{
    public class FilepathHistory : ModelBase
    {
        public DirectoryInfo CurrentDirectoryInfo
        {
            get { return _itemHistory[_currentItemIndex]; }
        }

        public bool ExistsBackward
        {
            get { return _currentItemIndex > 0; }
        }

        public bool ExistsForward
        {
            get { return _currentItemIndex < _itemHistory.Count - 1; }
        }


        public FilepathHistory()
        {
            Messenger.AddMessageFilter("CurrentDirectoryChanged");
            Messenger.MessageReceived += (sender, e) =>
            {
                var info = e.Content as DirectoryInfo;
                if (info.FullName == CurrentDirectoryInfo.FullName) return;
                Push(info);
            };
        }


        public void Load()
        {
            var lastItem = AppCache.Get("LastFilepath");
            if (lastItem == null)
            {
                lastItem = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            _itemHistory.Add(new DirectoryInfo(lastItem));

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
            if (_currentItemIndex < _itemHistory.Count - 1)
            {
                _itemHistory.RemoveRange(
                    _currentItemIndex + 1, _itemHistory.Count - _currentItemIndex - 1);
            }

            _itemHistory.Add(info);
            _currentItemIndex = _itemHistory.Count - 1;

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
            if (_currentItemIndex >= _itemHistory.Count - 1) return;
            ++_currentItemIndex;

            SendCurrentChangedMessage();
        }


        private int FindItemIndex(DirectoryInfo info)
        {
            return _itemHistory.FindIndex(item => item.FullName == info.FullName);
        }


        private void SendCurrentChangedMessage()
        {
            OnPropertyChanged("CurrentDirectoryInfo");
            //OnPropertyChanged("ExistsBackward");
            //OnPropertyChanged("ExistsForward");
            Messenger.DispatchMessage(
                new Coordination.InterModelMessage("CurrentDirectoryChanged", CurrentDirectoryInfo));
        }


        private List<DirectoryInfo> _itemHistory = new List<DirectoryInfo>();
        private int _currentItemIndex;

    }
}
