using System.Collections.Generic;

namespace CodeMetricsCalculator.Parsers.CodeInfo
{
    public interface IClassInfo : ICodeInfo
    {
        string Name { get; }

        IReadOnlyCollection<IFieldInfo> GetFields();

        IReadOnlyCollection<IMethodInfo> GetMethods();
    }
}
