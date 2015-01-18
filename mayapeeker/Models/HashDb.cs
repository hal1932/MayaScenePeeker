using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mayapeeker.Models
{
    class HashDb : Disposable
    {
        public string this[string key]
        {
            get
            {
                string value;
                if(TryGetValue(key, out value)) return value;
                throw new KeyNotFoundException();
            }

            set
            {
                SetValue(key, value);
            }
        }


        public void Open(string filename)
        {
            using (var reader = new StreamReader(filename))
            {
                while(!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Trim();
                    var items = line.Split(',');

                    var keyHash = CalcHash(items[0]);
                    _dic[keyHash] = items[1];
                }
            }
            _filename = filename;
        }


        public void Save()
        {
            using (var writer = new StreamWriter(_filename))
            {
                foreach (var item in _dic)
                {
                    writer.WriteLine("{0},{1}", item.Key, item.Value);
                }
            }
        }


        public bool TryGetValue(string key, out string value)
        {
            var keyHash = CalcHash(key);
            return _dic.TryGetValue(keyHash, out value);
        }


        private void SetValue(string key, string value)
        {
            var keyHash = CalcHash(key);
            _dic[keyHash] = value;
        }


        private int CalcHash(string key)
        {
            return Crc32.OptimizedCRC.Compute(Encoding.UTF8.GetBytes(key));
        }


        private string _filename;
        private Dictionary<int, string> _dic = new Dictionary<int, string>();

    }
}
