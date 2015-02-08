using mayapeeker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mayapeeker.ViewModels.FileListImpl
{
    class FileListImplBase : ViewModelBase
    {
        #region DirectoryContainer変更通知プロパティ
        private DirectoryContainer _DirectoryContainer;

        public DirectoryContainer DirectoryContainer
        {
            get
            { return _DirectoryContainer; }
            set
            { 
                if (_DirectoryContainer == value)
                    return;
                _DirectoryContainer = value;
                RaisePropertyChanged("DirectoryContainer");
            }
        }
        #endregion

    }
}
