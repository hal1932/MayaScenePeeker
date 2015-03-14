using mayapeeker.Models;
using mayapeeker.Models.Interactivity;
using System.IO;
using System.Windows.Forms;

namespace mayapeeker.ViewModels
{
    class BookmarkViewModel : ViewModelBase
    {
        public BookmarkContainer Container { get; set; }
        public int SelectedIndex { get; set; }


        public BookmarkViewModel()
        {
            Container = new BookmarkContainer();

            Messenger.AddMessageHandler(
                Properties.Resources.MsgKey_CurrentDirectoryChanged, (msg) =>
                {
                    _currentDirectory = msg.Content as DirectoryInfo;
                });
        }


        public override void Initialize()
        {
            Container.Load();
            base.Initialize();
        }


        protected override void DisposeUnmanagedResource()
        {
            Messenger.RemoveMessageHandler();
            Container.Save();
        }


        public void SelectItem()
        {
            Container.Open(SelectedIndex);
        }


        public void AddCurrent()
        {
            if (_currentDirectory != null)
            {
                Container.AddItem(new BookmarkContainer.Item(_currentDirectory));
            }
        }


        public void AddItem()
        {
            var dialog = new FolderBrowserDialog()
            {
                RootFolder = System.Environment.SpecialFolder.MyComputer,
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Container.AddItem(new BookmarkContainer.Item(
                    new DirectoryInfo(dialog.SelectedPath)));
            }
        }


        public void RemoveItem()
        {
            Container.RemoveItem(SelectedIndex);
        }


        private DirectoryInfo _currentDirectory;

    }
}
