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

            _messageListener.AddMessageFilter("CurrentDirectoryChanged");
            _messageListener.MessageReceived += (sender, e) =>
            {
                _currentDirectory = e.Content as DirectoryInfo;
            };
        }


        public override void Initialize()
        {
            Container.Load();
            base.Initialize();
        }


        protected override void DisposeUnmanagedResource()
        {
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
        private InteractionMessageListener _messageListener = new InteractionMessageListener();

    }
}
