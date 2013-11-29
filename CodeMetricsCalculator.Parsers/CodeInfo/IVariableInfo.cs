using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeMetricsCalculator.Parsers.CodeInfo
{
    public interface IVariableInfo : IIdentifierInfo
    {
        ITypeInfo Type { get; }
    }
}
