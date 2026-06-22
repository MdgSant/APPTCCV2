using Newtonsoft.Json;

namespace Estudex0._1a.Models;

public class RespostaDuvida
{
    [JsonProperty("idRespostaDuvida")]
    public int? IdRespostaDuvida { get; set; }

    [JsonProperty("conteudoResposta")]
    public string ConteudoResposta { get; set; } = string.Empty;

    [JsonProperty("momento")]
    public string Momento { get; set; } = string.Empty;

    [JsonProperty("idUtilizador")]
    public int? IdUtilizador { get; set; }

    [JsonProperty("utilizador")]
    public Utilizador? Utilizador { get; set; }

    [JsonProperty("idDuvida")]
    public int? IdDuvida { get; set; }

    [JsonProperty("duvida")]
    public Duvida? Duvida { get; set; }
}