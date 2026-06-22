using Newtonsoft.Json;
using System.Collections.Generic;

namespace Estudex0._1a.Models
{
    public class PaginaResposta<T>
    {
        [JsonProperty("content")]
        public List<T> Conteudo { get; set; } = new();

        [JsonProperty("totalElements")]
        public int TotalElementos { get; set; }

        [JsonProperty("totalPages")]
        public int TotalPaginas { get; set; }

        [JsonProperty("number")]
        public int PaginaAtual { get; set; }

        [JsonProperty("last")]
        public bool Ultima { get; set; }
    }
}