using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CodeMetricsCalculator.Parsers.CodeInfo
{
    public interface IVariableInfo : IMemberInfo
    {
        string Name { get; }
    }
}
