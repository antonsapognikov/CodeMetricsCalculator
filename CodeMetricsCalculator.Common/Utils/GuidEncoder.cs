using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeMetricsCalculator.Common.Utils
{
    public static class GuidEncoder
    {
        public static String Encode(Guid guid)
        {
            string enc = Convert.ToBase64String(guid.ToByteArray());
            enc = enc.Replace("/", "_");
            enc = enc.Replace("+", "$");
            return enc.Substring(0, 22);
        }

        public static Guid Decode(String encoded)
        {
            Contract.Requires<ArgumentNullException>(encoded != null, "encoded");
            encoded = encoded.Replace("_", "/");
            encoded = encoded.Replace("$", "+");
            byte[] buffer = Convert.FromBase64String(encoded + "==");
            return new Guid(buffer);
        }

        public static Boolean TryDecode(String encoded, out Guid guid)
        {
            Contract.Requires<ArgumentNullException>(encoded != null, "encoded");
            encoded = encoded.Replace("_", "/");
            encoded = encoded.Replace("$", "+");
            try
            {
                byte[] buffer = Convert.FromBase64String(encoded + "==");
                guid = new Guid(buffer);
                return true;
            }
            catch (Exception)
            {
                guid = Guid.Empty;
                return false;
            }
        }
    }
}
