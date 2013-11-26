using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeMetricsCalculator.Parsers
{
    public class CodeDictionary
    {
        private readonly IReadOnlyDictionary<string, int> _operators;
        private readonly IReadOnlyDictionary<string, int> _operands;

        public CodeDictionary(IReadOnlyDictionary<string, int> operators, IReadOnlyDictionary<string, int> operands)
        {
            Contract.Requires(operators != null);
            Contract.Requires(operands != null);

            _operators = operators;
            _operands = operands;
        }

        public IReadOnlyDictionary<string, int> Operators
        {
            get { return _operators; }
        }

        public IReadOnlyDictionary<string, int> Operands
        {
            get { return _operands; }
        }
    }
}
