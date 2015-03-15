
using mayapeeker.Models.Interactivity;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System;
using System.Windows;

namespace mayapeeker.ViewModels
{
    class PreviewViewModel : ViewModelBase
    {
        #region ItemList変更通知プロパティ
        private FileSystemInfo[] _ItemList;

        public FileSystemInfo[] ItemList
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

        #region SelectedIndex変更通知プロパティ
        private int _SelectedIndex;

        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            { 
                if (_SelectedIndex == value) return;
                _SelectedIndex = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        public PreviewViewModel()
        {
            Messenger.AddMessageHandler(
                Properties.Resources.MsgKey_SelectedItemChanged, (msg) =>
                {
                    UpdateItemList(msg.Content as FileSystemInfo);
                });
            Messenger.AddMessageHandler(
                Properties.Resources.MsgKey_CurrentDirectoryChanged, (msg) =>
                {
                    UpdateItemList(null);
                });
        }


        public override void Initialize()
        {
            base.Initialize();
            Messenger.SetAsDispatcher(this);
        }


        public void OpenItem()
        {
            if (SelectedIndex < 0) return;

            var selected = ItemList[SelectedIndex];
            if (File.Exists(selected.FullName))
            {
                Debug.WriteLine("  +++ open +++ " + ItemList[SelectedIndex].FullName);
            }
            else
            {
                Messenger.DispatchMessage(
                    Properties.Resources.MsgKey_RequestChangeCurrentDirectory, selected);
            }
        }


        private void UpdateItemList(FileSystemInfo info)
        {
            if (info is DirectoryInfo)
            {
                var dirInfo = (DirectoryInfo)info;

                var items = new FileSystemInfo[] { };
                try
                {
                    ItemList = items
                        .Concat(dirInfo.GetDirectories())
                        .Concat(dirInfo.GetFiles())
                        .ToArray();
                }
                catch(UnauthorizedAccessException)
                {
                    MessageBox.Show("フォルダへのアクセス権限がありません");
                }
            }
            else
            {
                ItemList = new FileSystemInfo[] { };
            }
        }

    }
}
