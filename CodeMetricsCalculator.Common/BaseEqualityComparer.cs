using System;
using System.Collections.Generic;

namespace CodeMetricsCalculator.Common
{
    public class BaseEqualityComparer<T> : EqualityComparer<T>
    {
        private readonly Func<T, T, bool> _equals;
        private readonly Func<T, int> _hashCode;
        
        public BaseEqualityComparer(Func<T, T, bool> equals, Func<T, int> hashCode)
        {
            if (equals == null)
                throw new ArgumentNullException("equals");
            if (hashCode == null)
                throw new ArgumentNullException("hashCode");
            _equals = equals;
            _hashCode = hashCode;
        }
        
        public override bool Equals(T x, T y)
        {
            return _equals(x, y);
        }

        public override int GetHashCode(T obj)
        {
            return _hashCode(obj);
        }
    }
}
