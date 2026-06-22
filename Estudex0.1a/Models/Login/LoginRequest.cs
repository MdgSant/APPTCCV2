using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Estudex0._1a.Models.Login
{
    public class LoginRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;

        [JsonProperty("senha")]
        public string Senha { get; set; } = string.Empty;
    }
}