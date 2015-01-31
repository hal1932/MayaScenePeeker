using System.Windows;

namespace mayapeeker.ViewModels
{
    class FileViewModel : ViewModelBase
    {
        public Models.AssociatedFiletype AssociatedFiletype { get; set; }


        public FileViewModel()
        {
            AssociatedFiletype = new Models.AssociatedFiletype();
        }


        public override void Initialize()
        {
            AssociatedFiletype.Load();
            base.Initialize();
        }

    }
}
