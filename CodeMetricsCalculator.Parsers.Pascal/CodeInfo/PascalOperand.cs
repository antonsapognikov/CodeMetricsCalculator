using System;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Pascal.CodeInfo
{
    public class PascalOperand : PascalCode, IOperandInfo , IEquatable<PascalOperand>
    {
        private readonly string _name;

        public PascalOperand(string name, string originalSource) : base(originalSource)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Operand name cannot be null or WhiteSpace.");

            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PascalOperand);
        }

        public bool Equals(PascalOperand other)
        {
            return other != null && string.Equals(other.Name, Name);
        }

        public override int GetHashCode()
        {
            return (_name != null ? _name.GetHashCode() : 0);
        }
    }
}
