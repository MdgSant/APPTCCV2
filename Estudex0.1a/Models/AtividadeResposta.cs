using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Estudex0._1a.Models
{
    public class AtividadeResposta
    {
        [JsonProperty("idAtividadeAluno")]
        public int IdAtividadeAluno { get; set; }

        [JsonProperty("aluno")]
        public Utilizador Aluno { get; set; }

        [JsonProperty("atividade")]
        public Atividade Atividade { get; set; }

        [JsonProperty("momentoInicio")]
        public string MomentoInicio { get; set; }

        [JsonProperty("momentoFim")]
        public string MomentoFim { get; set; }

        [JsonProperty("pontuacao")]
        public float Pontuacao { get; set; }
    }
}