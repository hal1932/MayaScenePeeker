using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mayapeeker.ViewModels
{
    class PathHistoryBoxViewModel : ViewModelBase
    {
        public delegate void OnSelectedItemChanged(DirectoryInfo index);
        public event OnSelectedItemChanged SelectedItemChanged;

        #region ItemArray変更通知プロパティ
        private string[] _ItemArray;

        public string[] ItemArray
        {
            get { return _ItemArray; }
            set
            { 
                if (_ItemArray == value) return;
                _ItemArray = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region SelectedIndex変更通知プロパティ
        private int _SelectedIndex;

        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            { 
                if (_SelectedIndex == value) return;
                _SelectedIndex = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        private DirectoryInfo[] _itemInfoArray;


        public PathHistoryBoxViewModel(DirectoryInfo[] itemInfoArray)
        {
            _itemInfoArray = itemInfoArray;
            ItemArray = itemInfoArray.Select(info => info.FullName).ToArray();
        }

        public void ChangeSelectedIndex()
        {
            if (SelectedItemChanged != null)
            {
                SelectedItemChanged(_itemInfoArray[SelectedIndex]);
            }
        }
    }
}
