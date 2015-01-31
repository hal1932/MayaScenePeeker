
using mayapeeker.Models.Interactivity;
using System.IO;

namespace mayapeeker.ViewModels
{
    class PreviewViewModel : ViewModelBase
    {
        public PreviewViewModel()
        {
            Messenger.AddMessageFilter("CurrentDirectoryChanged");
            Messenger.MessageReceived += (sender, e) =>
            {
                _currentDirectory = e.Content as DirectoryInfo;
            };
        }


        public override void Initialize()
        {
            base.Initialize();
        }


        private DirectoryInfo _currentDirectory;

    }
}
