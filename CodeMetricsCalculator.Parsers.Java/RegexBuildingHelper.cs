using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;
using CodeMetricsCalculator.Parsers.Java.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java
{
    internal static class RegexBuildingHelper
    {
        public static Regex BuildForOperator(JavaOperator operatorInfo)
        {
            //слева от оператор что угодно, кроме него самого; справа от него что угодно, кроме него самого и знака '=',
            //чтобы не спутать с бинарными операторами -= и т.п.

            //эскейпим символы слешами
            var operatorString = Regex.Escape(operatorInfo.Name);//operatorInfo.Name.Aggregate(string.Empty, (current, ch) => current + ("\\" + ch));

            //todo: тут надо серьёзно подумать
            if (operatorInfo.OperationType == OperationType.Ternary) //тернарный оператор в java только один
                return new Regex(@"\?.*:", RegexOptions.Compiled);

            //todo: надо как-то различать префиксные и постфиксные, унарные и бинарные операторы
            return new Regex(string.Format(@"[^{0}]{0}[^{0}=]", operatorString), RegexOptions.Compiled);
        }
    }
}
