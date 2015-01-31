
using mayapeeker.Models;
using mayapeeker.Models.Interactivity;
using System.Diagnostics;
using System.IO;
namespace mayapeeker.ViewModels
{
    class FileListViewModel : ViewModelBase
    {
        public DirectoryContainer DirectoryContainer { get; set; }

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

    }
}
