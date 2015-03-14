
using System.IO;
namespace mayapeeker.ViewModels
{
    class ShelfViewModel : ViewModelBase
    {
        #region CurrentDirectoryPath変更通知プロパティ
        private string _CurrentDirectoryPath;

        public string CurrentDirectoryPath
        {
            get { return _CurrentDirectoryPath; }
            set
            { 
                if (_CurrentDirectoryPath == value) return;
                _CurrentDirectoryPath = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region ExistsBackward変更通知プロパティ
        private bool _ExistsBackward;

        public bool ExistsBackward
        {
            get { return _ExistsBackward; }
            set
            { 
                if (_ExistsBackward == value) return;
                _ExistsBackward = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region ExistsForward変更通知プロパティ
        private bool _ExistsForward;

        public bool ExistsForward
        {
            get { return _ExistsForward; }
            set
            { 
                if (_ExistsForward == value) return;
                _ExistsForward = value;
                RaisePropertyChanged();
            }
        }
        #endregion



        public ShelfViewModel()
        {
            _history = new Models.FilepathHistory();
        }


        public override void Initialize()
        {
            _history.Load();
            CurrentDirectoryPath = _history.CurrentDirectoryInfo.FullName;
            
            _history.AddPropertyChangedHandler(
                "CurrentDirectoryInfo", (e) =>
                {
                    CurrentDirectoryPath = _history.CurrentDirectoryInfo.FullName;
                    ExistsBackward = _history.ExistsBackward;
                    ExistsForward = _history.ExistsForward;
                });

            base.Initialize();
        }


        protected override void DisposeUnmanagedResource()
        {
            _history.Save();
        }


        public void ChangeToBackwardDirectory()
        {
            _history.SetPrevious();
        }

        public void ChangeToForwardDirectory()
        {
            _history.SetForward();
        }


        public void PushCurrentToHistory()
        {
            _history.Push(new DirectoryInfo(CurrentDirectoryPath));
        }


        public void ChangeToParentDirectory()
        {
            if(!Directory.Exists(CurrentDirectoryPath)) return;

            var info = Directory.GetParent(CurrentDirectoryPath);
            CurrentDirectoryPath = info.FullName;

            PushCurrentToHistory();
        }


        public Models.FilepathHistory _history;

    }
}
