using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Estudex0._1a.Models
{
    public class AtividadePergunta
    {
        [JsonProperty("idPergunta")]
        public int? IdPergunta { get; set; }

        [JsonProperty("enunciado")]
        public string Enunciado { get; set; }

        [JsonProperty("atividade")]
        public Atividade Atividade { get; set; }

        [JsonProperty("opcoes")]
        public List<PerguntasOpcoes> Opcoes { get; set; }
    }
}
