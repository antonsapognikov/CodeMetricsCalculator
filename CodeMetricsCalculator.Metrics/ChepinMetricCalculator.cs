using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Metrics
{
    public static class ChepinMetricCalculator
    {
        public static double Calculate(IClassInfo classInfo)
        {
            Contract.Requires(classInfo != null);

            var inputVariables = classInfo.GetMethods().SelectMany(info => info.GetInputVariables()).ToList();
            //Q = P + 2M + 3C + 0.5T
            var p = inputVariables.Count(variable => variable.IsCalculationOrOutput && variable.IsUsed);
            var m = inputVariables.Count(variable => variable.IsModified && variable.IsUsed);
            var c = inputVariables.Count(variable => variable.IsControl && variable.IsUsed);
            var t = inputVariables.Count(variable => !variable.IsUsed);
            return p + 2*m + 3*c + 0.5*t;
        }
    }
}
