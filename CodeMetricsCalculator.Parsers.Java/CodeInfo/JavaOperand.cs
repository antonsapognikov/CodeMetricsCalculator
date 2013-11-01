using System;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.CodeInfo
{
    public class JavaOperand : JavaCode, IOperandInfo , IEquatable<JavaOperand>
    {
        private readonly string _name;

        public JavaOperand(string name, string originalSource) : base(originalSource)
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
            return Equals(obj as JavaOperand);
        }

        public bool Equals(JavaOperand other)
        {
            return other != null && string.Equals(other.Name, Name);
        }

        public override int GetHashCode()
        {
            return (_name != null ? _name.GetHashCode() : 0);
        }
    }
}
