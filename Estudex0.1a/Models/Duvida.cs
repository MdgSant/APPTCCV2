using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Estudex0._1a.Models
{
    public class Duvida
    {
        [JsonProperty("idDuvida")]
        public int IdDuvida { get; set; }

        [JsonProperty("titulo")]
        public string Titulo { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("momento")]
        public string Momento { get; set; }

        [JsonProperty("statusDuvida")]
        public string StatusDuvida { get; set; }

        [JsonProperty("utilizador")]
        public Utilizador Utilizador { get; set; }

        [JsonProperty("disciplina")]
        public Disciplina Disciplina { get; set; }
    }
}
