using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeMetricsCalculator.Parsers.CodeInfo
{
    public interface IMethodBodyInfo : ICodeInfo
    {
        IMethodInfo Method { get; }

        IReadOnlyCollection<IExpressionInfo> GetExpressions();
    }
}
