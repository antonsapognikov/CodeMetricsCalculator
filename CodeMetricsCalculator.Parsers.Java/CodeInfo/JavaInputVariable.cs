using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeMetricsCalculator.Parsers.Java.CodeInfo
{
    class JavaInputVariable : JavaCode, IInputVariable
    {
        private readonly string _name;

        protected JavaInputVariable(string name, string originalSource)
            : base(originalSource)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }

        public bool IsUsed
        {
            get; set;
        }

        public bool IsModified
        {
            get; set;
        }

        public bool IsControl
        {
            get; set;
        }

        public bool IsCalculationOrOutput
        {
            get; set;
        }
    }
}
