using System.Collections.Generic;

namespace CodeMetricsCalculator.Parsers.CodeInfo
{
    public interface IMethodInfo : IMemberInfo
    {
        IClassInfo Class { get; }

        string Name { get; }

        IReadOnlyCollection<IMethodParameterInfo> Parameters { get; }

        IMethodBodyInfo GetBody();

        IReadOnlyCollection<IInputVariable> GetInputVariables();
        
        CodeDictionary GetMethodDictionary();
    }
}
