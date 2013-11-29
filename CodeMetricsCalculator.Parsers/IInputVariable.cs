using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers
{
    public interface IInputVariable : IMemberInfo
    {
        string Name { get; }

        bool IsUsed { get; }

        bool IsModified { get; }

        bool IsControl { get; }
    }
}
