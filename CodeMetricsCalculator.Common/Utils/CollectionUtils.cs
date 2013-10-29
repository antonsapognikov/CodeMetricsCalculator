using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeMetricsCalculator.Common.Utils
{
    public static class CollectionUtils
    {
        public static TKey GetKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TValue value)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");
            if (!dictionary.ContainsValue(value))
                throw new KeyNotFoundException();
            return dictionary.First(pair => pair.Value.Equals(value)).Key;
        }
    }
}
