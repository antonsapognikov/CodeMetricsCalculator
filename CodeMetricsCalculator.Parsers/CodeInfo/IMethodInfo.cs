using System.Collections.Generic;

namespace CodeMetricsCalculator.Parsers.CodeInfo
{
    public interface IMethodInfo : IIdentifierInfo
    {
        IClassInfo Class { get; }

        IReadOnlyCollection<IMethodParameterInfo> Parameters { get; }

        IMethodBodyInfo GetBody();

        IReadOnlyCollection<IInputVariable> GetInputVariables();

        IReadOnlyDictionary<IIdentifierInfo, int> GetIdentifiers(); 
        
        CodeDictionary GetMethodDictionary();
    }
}
