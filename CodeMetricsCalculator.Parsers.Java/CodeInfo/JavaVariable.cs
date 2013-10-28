using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMetricsCalculator.Parsers.CodeInfo;

namespace CodeMetricsCalculator.Parsers.Java.CodeInfo
{
    internal abstract class JavaVariable : JavaCode, IVariableInfo
    {
        private readonly string _name;

        protected JavaVariable(string name, string originalSource) : base(originalSource)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }
    }
}
