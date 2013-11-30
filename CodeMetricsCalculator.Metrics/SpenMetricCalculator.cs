using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Metrics
{
    public static class SpenMetricCalculator
    {

        public static double CalculateAverage(IClassInfo classInfo)
        {
            Contract.Requires(classInfo != null);

            var identifiers = classInfo.GetIdentifiers();
            return identifiers.Select(pair => pair.Value - 1).Average();
        }

        public static IReadOnlyDictionary<IIdentifierInfo, int> Calculate(IClassInfo classInfo)
        {
            Contract.Requires(classInfo != null);

            var identifiers = classInfo.GetIdentifiers();
            return identifiers.ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value - 1);
        } 
    }
}
