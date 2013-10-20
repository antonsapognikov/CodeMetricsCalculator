using System;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.CodeInfo
{
    public class JavaOperand : JavaCode, IOperandInfo 
    {
        private readonly string _name;

        public JavaOperand(string originalSource, string name) : base(originalSource)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Operand name cannot be null or WhiteSpace.");

            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }
    }
}
