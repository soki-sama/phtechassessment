using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.Integration.Tests.Support
{
    public static class Extensions
    {
        public static string Obfuscate(this int value)
        {
            // TODO: Add err handling for mapper
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value.ToString()));
        }

        public static int Deobfuscate(this string value)
        {
            // TODO: Add proper error handling

            try
            {
                var v = Encoding.UTF8.GetString(Convert.FromBase64String(value));

                if (!int.TryParse(v, out int result))
                {
                    throw new Exception("INvalid Value"); // TODO: Add proper exception here
                }

                return result;

            }
            catch (Exception ex)
            {

                throw;
            }

        }


      

    }
}
