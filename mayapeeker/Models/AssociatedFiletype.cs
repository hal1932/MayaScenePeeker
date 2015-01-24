using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mayapeeker.Models
{
    public class AssociatedFiletype : ModelBase
    {
        #region LabelArray変更通知プロパティ
        private string[] _LabelArray;

        public string[] LabelArray
        {
            get
            { return _LabelArray; }
            set
            { 
                if (_LabelArray == value)
                    return;
                _LabelArray = value;
                OnPropertyChanged("LabelArray");
            }
        }
        #endregion

        #region SelectedIndex変更通知プロパティ
        private int _SelectedIndex = -1;

        public int SelectedIndex
        {
            get
            { return _SelectedIndex; }
            set
            { 
                if (_SelectedIndex == value)
                    return;
                _SelectedIndex = value;
                OnPropertyChanged("SelectedIndex");

                OnPropertyChanged("SelectedFilter");
                Messenger.DispatchMessage(
                    new Coordination.InterModelMessage("FileFilterChanged", SelectedFilter));
            }
        }
        #endregion

        #region SelectedFilter変更通知プロパティ
        public string[] SelectedFilter
        {
            get { return _filterArrayList[SelectedIndex]; }
        }
        #endregion


        public AssociatedFiletype()
        {
            LabelArray = new string[0];
        }


        public void Load()
        {
            var filename = Properties.Resources.File_AssociatedFiletype;

            _filterArrayList.Clear();

            var labelList = new List<string>();
            using (var reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Trim();
                    var items = line.Split(',');
                    labelList.Add(items[0]);
                    _filterArrayList.Add(items[1].Split('+'));
                }
            }

            LabelArray = labelList.ToArray();
            SelectedIndex = 0;
        }


        public string[] GetFilterArray(int labelIndex)
        {
            return _filterArrayList[labelIndex];
        }


        private List<string[]> _filterArrayList = new List<string[]>();

    }
}
