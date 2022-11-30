using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Propeller.Models.Requests
{
    public class AuthRequest
    {
        [JsonPropertyName("uid")]
        public string UserId { get; set; }

        [JsonPropertyName("pwd")]
        public string Password { get; set; }
    }
}
