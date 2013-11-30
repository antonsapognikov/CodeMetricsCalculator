using System.Collections.Generic;

namespace CodeMetricsCalculator.Parsers.CodeInfo
{
    public interface IClassInfo : IMemberInfo
    {
        string Name { get; }

        IReadOnlyCollection<IFieldInfo> GetFields();

        IReadOnlyCollection<IMethodInfo> GetMethods();

        CodeDictionary GetClassDictionary();

        IReadOnlyDictionary<IIdentifierInfo, int> GetIdentifiers();
    }
}
