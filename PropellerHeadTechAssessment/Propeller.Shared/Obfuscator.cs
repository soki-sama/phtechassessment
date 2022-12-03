using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.Shared
{
    public static class Obfuscator
    {
        /// <summary>
        /// Obfuscate will always be used for ID's, an empty string is safe to be used since it'll never happen
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ObfuscateId(int value)
        {
            return Obfuscator.ObfuscateId(value);
        }

        /// <summary>
        /// Deobfuscation is used for ID's, so a -1 value will never be valid, safe to use
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int DeobfuscateId(string value)
        {
            return Obfuscator.DeobfuscateId(value);
        }

    }
}
