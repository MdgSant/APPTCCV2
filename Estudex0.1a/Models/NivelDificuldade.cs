using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;


namespace Estudex0._1a.Models
{
    public class NivelDificuldade
    {
        [JsonProperty("idNivelDificuldade")]
        public int IdNivelDificuldade { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }
    }
}
