
namespace mayapeeker.ViewModels
{
    class ShelfViewModel : ViewModelBase
    {
        public Models.FilepathHistory PathContainer { get; set; }


        public ShelfViewModel()
        {
            PathContainer = new Models.FilepathHistory();
        }


        public override void Initialize()
        {
            PathContainer.Load();
            base.Initialize();
        }


        protected override void DisposeUnmanagedResource()
        {
            PathContainer.Save();
        }

    }
}
