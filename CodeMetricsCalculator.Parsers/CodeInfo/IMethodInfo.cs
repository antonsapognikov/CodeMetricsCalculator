using System.Collections.Generic;

namespace CodeMetricsCalculator.Parsers.CodeInfo
{
    public interface IMethodInfo : IMemberInfo
    {
        IClassInfo Class { get; }

        string Name { get; }

        IMethodBodyInfo GetBody();
    }
}
