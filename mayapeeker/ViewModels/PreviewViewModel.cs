
using mayapeeker.Models;
using System.Diagnostics;
namespace mayapeeker.ViewModels
{
    class PreviewViewModel : ViewModelBase
    {
        public DirectoryContainer DirectoryContainer { get; set; }

        // OneWayToSource だから PropertyChanged 対応しない
        public int SelectedDirectoryIndex { get; set; }



        public PreviewViewModel()
        {
            DirectoryContainer = new DirectoryContainer();
        }


        public override void Initialize()
        {
            base.Initialize();
        }


        public void SelectItem()
        {
            var item = DirectoryContainer.ItemList[SelectedDirectoryIndex];
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
