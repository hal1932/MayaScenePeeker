
namespace mayapeeker.Models
{
    public class AppCache //: ModelBase
    {
        public static string Get(string key)
        {
            string value;
            if (_db.TryGetValue(key, out value)) return value;
            return null;
        }


        public static void Set(string key, string value)
        {
            _db[key] = value;
        }


        public static void Load(string filename)
        {
            _db.Open(filename);
        }


        public static void Save()
        {
            _db.Save();
        }


        private static HashDb _db = new HashDb();

    }
}
