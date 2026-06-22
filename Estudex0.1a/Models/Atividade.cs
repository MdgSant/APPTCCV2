using Estudex0._1a.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Estudex0._1a.Models
{
    public class Atividade
    {
        [JsonProperty("idAtividade")]
        public int? IdAtividade { get; set; }

        [JsonProperty("titulo")]
        public string Titulo { get; set; }

        [JsonProperty("pontuacaoMaxima")]
        public int PontuacaoMaxima { get; set; }

        [JsonProperty("idOrientador")]
        public int? IdOrientador { get; set; }

        [JsonProperty("idNivelDificuldade")]
        public int? IdNivelDificuldade { get; set; }

        [JsonProperty("idDisciplina")]
        public int? IdDisciplina { get; set; }

        [JsonProperty("nivelDificuldade")]
        public NivelDificuldade NivelDificuldade { get; set; }

        [JsonProperty("perguntas")]
        public List<AtividadePergunta> Perguntas { get; set; }
    }
}
