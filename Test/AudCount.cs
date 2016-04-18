using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Test
{
    public class AuthModel
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccesToken { get; set; }

        [JsonProperty(PropertyName =  "token_type")]
        public string TokenType { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }
    }
}
