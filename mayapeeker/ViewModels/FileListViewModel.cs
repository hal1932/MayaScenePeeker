
using mayapeeker.Models;
using mayapeeker.Models.Interactivity;
using System.Diagnostics;
using System.IO;
namespace mayapeeker.ViewModels
{
    class FileListViewModel : ViewModelBase
    {
        public DirectoryContainer DirectoryContainer { get; set; }

        #region SelectedItemIndex
        // OneWayToSource だから PropertyChanged 対応しない
        public int SelectedItemIndex
        {
            get { return _selectedItemIndex; }
            set
            {
                if (value != _selectedItemIndex)
                {
                    _selectedItemIndex = value;

                    FileSystemInfo info;
                    if(DirectoryContainer.TryGetValue(value, out info))
                    {
                        Messenger.DispatchMessage(new InteractionMessage(
                            "SelectedItemChanged", info));
                    }
                }
            }
        }
        private int _selectedItemIndex;
        #endregion

        #region ImplViewModel変更通知プロパティ
        private FileListImpl.FileListImplBase _ImplViewModel;

        public FileListImpl.FileListImplBase ImplViewModel
        {
            get
            { return _ImplViewModel; }
            set
            { 
                if (_ImplViewModel == value)
                    return;
                _ImplViewModel = value;
                RaisePropertyChanged("ImplViewModel");
            }
        }
        #endregion


        public string SearchFilter
        {
            set
            {
                DirectoryContainer.ApplyFilter(value);
            }
        }



        public FileListViewModel()
        {
            Messenger.SetAsDispatcher(this);
            DirectoryContainer = new DirectoryContainer();
        }


        public override void Initialize()
        {
            base.Initialize();
        }


        public void SelectItem()
        {
            var item = DirectoryContainer.ItemList[SelectedItemIndex];
            if (item.DirectoryInfo != null)
            {
                DirectoryContainer.Reload(item.DirectoryInfo);
            }
            else
            {
                Debug.WriteLine("  +++ open +++ " + item.FileInfo.FullName);
            }
        }


        public void SwitchToIconView()
        {
            if (!(ImplViewModel is FileListImpl.IconView))
            {
                ImplViewModel = new FileListImpl.IconView()
                {
                    DirectoryContainer = DirectoryContainer,
                };
            }
        }


        public void SwitchToNameView()
        {
            if (!(ImplViewModel is FileListImpl.NameView))
            {
                ImplViewModel = new FileListImpl.NameView()
                {
                    DirectoryContainer = DirectoryContainer,
                };
            }
        }

    }
}
