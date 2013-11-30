using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Metrics
{
    public class HolstedMetricCalculator
    {
        private readonly int _uniqueOperators, _uniqueOperands, _operators, _operands;

        public HolstedMetricCalculator(IClassInfo classInfo)
        {
            Contract.Requires(classInfo != null);

            var classDictionaries = classInfo.GetClassDictionary();
            _uniqueOperators = classDictionaries.Operators.Count;
            _uniqueOperands = classDictionaries.Operands.Count;
            _operators = classDictionaries.Operators.Values.Sum();
            _operands = classDictionaries.Operands.Values.Sum();
        }

        /// <summary>
        /// n=n1+n2
        /// </summary>
        /// <returns></returns>
        public int CalculateProgramDictionary()
        {
            return _uniqueOperands + _uniqueOperators;
        }

        /// <summary>
        /// N=N1+N2        
        /// </summary>
        /// <returns></returns>
        public int CalculateProgramLength()
        {
            return _operands + _operators;
        }

        /// <summary>
        /// V=N*log2(n) (бит)
        /// </summary>
        /// <returns></returns>
        public double CalculateProgramVolume()
        {
            return CalculateProgramLength() * Math.Log(CalculateProgramDictionary(), 2);
        }
        
        /// <summary>
        /// N^ = n1*log2(n1) + n2*log2(n2)
        /// </summary>
        /// <returns></returns>
        public double CalculateTheoreticalProgramLength()
        {
            return _uniqueOperators*Math.Log(_uniqueOperators, 2) + _uniqueOperands*Math.Log(_uniqueOperands, 2);
        }

        /// <summary>
        /// V^=N^log2(N^) 
        /// </summary>
        /// <returns></returns>
        public double CalculateTheoreticalProgramVolume()
        {
            return CalculateTheoreticalProgramLength() * Math.Log(CalculateTheoreticalProgramLength(), 2);
        }

        /// <summary>
        /// L=V^/V
        /// </summary>
        /// <returns></returns>
        public double CalculateProgramLevel()
        {
            return CalculateTheoreticalProgramVolume()/CalculateProgramVolume();
        }

        /// <summary>
        /// L^ = 2*n2 / (n1*N2)
        /// </summary>
        /// <returns></returns>
        public double CalculateRealProgramParameters()
        {
            return 2*((double)_uniqueOperands)/(_uniqueOperators*_operands);
        }

        /// <summary>
        /// E = N^ * log2(n/L)
        /// </summary>
        /// <returns></returns>
        public double CalculateRequiredElementarySolutions()
        {
            return CalculateTheoreticalProgramLength()*Math.Log(CalculateProgramDictionary()/CalculateProgramLevel(), 2);
        }
    }
}
