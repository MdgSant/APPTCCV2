// ResultadoAtividade.cs
namespace Estudex0._1a.Models
{
    public class ResultadoAtividade
    {
        public string TituloAtividade { get; set; }
        public int TotalPerguntas { get; set; }
        public int TotalAcertos { get; set; }
        public float Pontuacao { get; set; }
        public float PontuacaoMaxima { get; set; }
        public List<ResultadoPergunta> Perguntas { get; set; } = new();
    }

    public class ResultadoPergunta
    {
        public string Enunciado { get; set; }
        public string OpcaoEscolhida { get; set; }
        public string OpcaoCorreta { get; set; }
        public bool Acertou { get; set; }
    }
}