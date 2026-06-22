using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Estudex0._1a.Models.Login
{
        public class LoginResponse
        {
            [JsonProperty("token")]
            public string Token { get; set; } = string.Empty;

            [JsonProperty("tipoUtilizador")]
            public int TipoUtilizador { get; set; }

            [JsonProperty("idUtilizador")]
            public int IdUtilizador { get; set; }

            [JsonProperty("nome")]
            public string Nome { get; set; } = string.Empty;
            

        }
}

