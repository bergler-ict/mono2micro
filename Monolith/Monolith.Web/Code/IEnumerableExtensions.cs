using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Monolith.Web
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> source, int partitionSize)
        {
            return source?
                .Select((e, i) => new { Element = e, Index = i })
                .GroupBy(o => o.Index / partitionSize)
                .Select(g => g.Select(o => o.Element));
        }
    }
}