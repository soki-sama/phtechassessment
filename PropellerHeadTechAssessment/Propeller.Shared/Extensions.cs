using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.Shared
{
    public static class Extensions
    {
        public static string Obfuscate(this int value)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value.ToString()));
        }

        public static int Deobfuscate(this string value)
        {
            // TODO: Add proper error handling
            return int.Parse(Encoding.UTF8.GetString(Convert.FromBase64String(value)));
        }
    }
}
