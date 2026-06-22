using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Estudex0._1a.Models
{
    public class Disciplina
    {
        [JsonProperty("idDisciplina")]
        public int IdDisciplina { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }
    }
}
