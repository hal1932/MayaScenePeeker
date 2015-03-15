using System.Diagnostics;
using System.IO;
using System.Windows;

namespace mayapeeker.ViewModels
{
    class FileViewModel : ViewModelBase
    {
        public Models.AssociatedFiletype AssociatedFiletype { get; set; }

        #region CurrentItem変更通知プロパティ
        private FileInfo _CurrentItem;

        public FileInfo CurrentItem
        {
            get { return _CurrentItem; }
            set
            { 
                if (_CurrentItem == value) return;
                _CurrentItem = value;
                RaisePropertyChanged();
            }
        }
        #endregion



        public FileViewModel()
        {
            AssociatedFiletype = new Models.AssociatedFiletype();
        }


        public override void Initialize()
        {
            base.Initialize();

            AssociatedFiletype.Load();
            Messenger.AddMessageHandler(
                Properties.Resources.MsgKey_SelectedItemChanged,
                (msg) => ChangeSelectedFile(msg.Content as FileSystemInfo));
        }


        public void CloseApp()
        {
            Application.Current.Shutdown();
        }


        public void OpenFile()
        {
            if (CurrentItem != null)
            {
                Debug.WriteLine("  +++ open +++ " + CurrentItem.FullName);
            }
        }


        private void ChangeSelectedFile(FileSystemInfo info)
        {
            if (File.Exists(info.FullName))
            {
                CurrentItem = new FileInfo(info.FullName);
            }
            else
            {
                CurrentItem = null;
            }
        }
    }
}
