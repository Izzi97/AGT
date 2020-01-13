using System.Collections.Generic;
using System.Linq;

namespace AGT
{
    public static class IEnumberableExtentions
    {
        public static IEnumerable<T> Except<T>(this IEnumerable<T> iEnumerable, T element)
        {
            return iEnumerable.Except(new T[] { element });
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> iEnumerable, T element)
        {
            return iEnumerable.Concat(new T[] { element });
        }
    }
}