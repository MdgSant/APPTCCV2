using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Estudex0._1a.Models
{
    public class PerguntasOpcoes
    {
        [JsonProperty("idOpcao")]
        public int? IdOpcao { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("correta")]
        public bool Correta { get; set; }

        [JsonProperty("atividadePergunta")]
        public AtividadePergunta AtividadePergunta { get; set; }
    }
}