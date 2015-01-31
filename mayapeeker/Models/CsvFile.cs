using System.Collections.Generic;
using System.IO;

namespace mayapeeker.Models
{
    public class CsvFile
    {
        public List<string[]> RowList { get; set; }


        public CsvFile(string filename = null)
        {
            RowList = new List<string[]>();
            if (filename != null)
            {
                Open(filename);
            }
        }


        public void Open(string filename)
        {
            _filename = filename;
            if (!File.Exists(filename)) return;

            var rowList = RowList;
            rowList.Clear();

            using (var reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    rowList.Add(reader.ReadLine().Split(','));
                }
            }
        }


        public void Save()
        {
            using (var writer = new StreamWriter(_filename))
            {
                foreach (var row in RowList)
                {
                    writer.WriteLine(string.Join(",", row));
                }
            }
        }


        private string _filename;

    }
}
