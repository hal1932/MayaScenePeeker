using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mayapeeker.Models
{
    class CompareSelector<TSource, TCompareKey> : IEqualityComparer<TSource>
    {
        public CompareSelector(Func<TSource, TCompareKey> selector)
        {
            _selector = selector;
        }

        public bool Equals(TSource x, TSource y)
        {
            return _selector(x).Equals(_selector(y));
        }

        public int GetHashCode(TSource obj)
        {
            return _selector(obj).GetHashCode();
        }

        private Func<TSource, TCompareKey> _selector;
    }


    static class IEnumerableExtensions
    {
        public static IEnumerable<TSource> Distinct<TSource, TCompareKey>(
            this IEnumerable<TSource> source, Func<TSource, TCompareKey> selector)
        {
            return source.Distinct(new CompareSelector<TSource, TCompareKey>(selector));
        }
    }
}
