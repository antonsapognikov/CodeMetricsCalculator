using System.Collections.Generic;

namespace CodeMetricsCalculator.Parsers.CodeInfo
{
    public interface IMethodInfo : IIdentifierInfo
    {
        IClassInfo Class { get; }

        IReadOnlyCollection<IMethodParameterInfo> Parameters { get; }

        ITypeInfo ReturnType { get; }

        IMethodBodyInfo GetBody();

        IReadOnlyCollection<IInputVariable> GetInputVariables();

        IReadOnlyDictionary<IVariableInfo, int> GetVariables(); 
        
        CodeDictionary GetMethodDictionary();
    }
}
