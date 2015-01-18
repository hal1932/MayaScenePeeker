using Microsoft.Practices.Prism.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace mayapeeker.ViewModels
{
    class FileViewModel : ViewModelBase
    {
        public Models.AssociatedFiletype AssociatedFiletype { get; set; }

        #region CloseAppCommand
        private DelegateCommand _CloseAppCommand;

        public DelegateCommand CloseAppCommand
        {
            get
            {
                if (_CloseAppCommand == null)
                {
                    _CloseAppCommand = new DelegateCommand(CloseApp);
                }
                return _CloseAppCommand;
            }
        }

        public void CloseApp()
        {
            Application.Current.Shutdown();
        }
        #endregion


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
