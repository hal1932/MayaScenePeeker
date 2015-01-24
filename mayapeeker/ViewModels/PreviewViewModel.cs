
using mayapeeker.Models;
namespace mayapeeker.ViewModels
{
    class PreviewViewModel : ViewModelBase
    {
        public DirectoryContainer DirectoryContainer { get; set; }


        public PreviewViewModel()
        {
            DirectoryContainer = new DirectoryContainer();
        }


        public override void Initialize()
        {
            base.Initialize();
        }
    }
}
