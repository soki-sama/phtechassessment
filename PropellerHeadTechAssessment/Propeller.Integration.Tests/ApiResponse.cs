using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Propeller.Integration.Tests
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ResponseBody { get; set; } = string.Empty;
    }
}
