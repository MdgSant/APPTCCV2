using Estudex0._1a.Models.Utilizadores.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Estudex0._1a.Models
{
    public class Utilizador
    {
        [JsonProperty("idUtilizador")]
        public int IdUtilizador { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; } = string.Empty;
        
        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;
        
        [JsonProperty("cpf")]
        public string CPF { get; set; } = string.Empty;

        [JsonProperty("cidade")]
        public string Cidade { get; set; } = string.Empty;

        [JsonProperty("uf")]
        public string UF { get; set; } = string.Empty;

        [JsonProperty("foto")]
        public bool? Foto { get; set; }

        [JsonProperty("senhaHash")]
        public string SenhaHash { get; set; } = string.Empty;
        public TipoUltilizador TipoUltilizador { get; set;}
        
        [JsonProperty("idTipoUtilizador")]
        public int? IdTipoUtilizador { get; set; }
    }
}
