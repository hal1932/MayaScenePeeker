using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mayapeeker.Models
{
    public class FilepathHistory : ModelBase
    {
        #region CurrentPath変更通知プロパティ
        private DirectoryInfo _CurrentDirectoryInfo;

        public DirectoryInfo CurrentDirectoryInfo
        {
            get
            { return _CurrentDirectoryInfo; }
            set
            { 
                if (_CurrentDirectoryInfo == value)
                    return;

                if (_CurrentDirectoryInfo != null)
                {
                    var historyList = HistoryArray.ToList();
                    historyList.Insert(0, value);
                    HistoryArray = historyList.ToArray();
                }

                _CurrentDirectoryInfo = value;
                OnPropertyChanged("CurrentDirectoryInfo");
                Messenger.DispatchMessage(
                    new Coordination.InterModelMessage("SelectedDirectoryChanged", value));
            }
        }
        #endregion

        #region HistoryArray変更通知プロパティ
        private DirectoryInfo[] _HistoryArray;

        public DirectoryInfo[] HistoryArray
        {
            get
            { return _HistoryArray; }
            set
            { 
                if (_HistoryArray == value)
                    return;
                _HistoryArray = value;
                OnPropertyChanged("HistoryArray");
            }
        }
        #endregion


        public FilepathHistory()
        {
            HistoryArray = new DirectoryInfo[0];
        }


        public void Load()
        {
            var lastItem = AppCache.Get("LastFilepath");
            if (lastItem == null)
            {
                lastItem = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            CurrentDirectoryInfo = new DirectoryInfo(lastItem);
        }


        public void Save()
        {
            AppCache.Set("LastFilepath", CurrentDirectoryInfo.FullName);
        }
    }
}
