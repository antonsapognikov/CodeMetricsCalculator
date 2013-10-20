using System.Collections.Generic;

namespace CodeMetricsCalculator.Parsers.CodeInfo
{
    public interface IMethodInfo : ICodeInfo
    {
        IClassInfo Class { get; }

        string Name { get; }

        IMethodBodyInfo GetBody();
    }
}
