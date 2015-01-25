using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mayapeeker.Models
{
    public class BookmarkContainer : ModelBase
    {
        public class Item
        {
            public DirectoryInfo DirectoryInfo { get; private set; }

            public Item(DirectoryInfo info)
            {
                DirectoryInfo = info;
            }
        }


        #region ItemList変更通知プロパティ
        private ObservableCollection<Item> _ItemList;

        public ObservableCollection<Item> ItemList
        {
            get
            { return _ItemList; }
            set
            { 
                if (_ItemList == value)
                    return;
                _ItemList = value;
                OnPropertyChanged("ItemList");
            }
        }
        #endregion



        public BookmarkContainer()
        {
            ItemList = new ObservableCollection<Item>();
        }


        public void Load()
        {
            var itemList = new List<Item>();

            _csv.Open(Properties.Resources.File_Bookmark);
            foreach (var row in _csv.RowList)
            {
                itemList.Add(new Item(new DirectoryInfo(row[0])));
            }

            ItemList = new ObservableCollection<Item>(itemList);
        }


        public void Save()
        {
            _csv.Save();
        }


        public void AddItem(Item item)
        {
            var found = ItemList.FirstOrDefault(
                i => i.DirectoryInfo.FullName == item.DirectoryInfo.FullName);
            if (found != default(Item)) return;

            _csv.RowList.Add(new string[] { item.DirectoryInfo.FullName });
            ItemList.Add(item);
        }


        public void RemoveItem(int index)
        {
            _csv.RowList.RemoveAt(index);
            ItemList.RemoveAt(index);
        }


        public void Open(int index)
        {
            var item = ItemList[index];
            if (!item.DirectoryInfo.Exists) return;

            Messenger.DispatchMessage(
                new Interactivity.InteractionMessage("CurrentDirectoryChanged", item.DirectoryInfo));
        }


        private CsvFile _csv = new CsvFile();

    }
}
