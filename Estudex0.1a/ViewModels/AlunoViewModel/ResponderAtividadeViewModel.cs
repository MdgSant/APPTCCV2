
using EstudeX.ViewModels;
using Estudex0._1a.Models;
using Estudex0._1a.Services.Aluno;
using System.Collections.ObjectModel;

namespace Estudex0._1a.ViewModels.AlunoViewModel
{
    public class ResponderAtividadeViewModel : BaseViewModel
    {
        private AlunoAtividadeService aService;
        private DateTime momentoInicio;

        private Atividade atividade;
        public Atividade Atividade
        {
            get => atividade;
            set { atividade = value; OnPropertyChanged(); }
        }

        private string idAtividadeStr;
        public string IdAtividadeStr
        {
            set
            {
                if (int.TryParse(value, out int id))
                    _ = CarregarAtividade(id);
            }
        }

        // Guarda a opção selecionada por pergunta: chave = idPergunta, valor = idOpcao
        public Dictionary<int, int> RespostasSelecionadas { get; set; } = new();

        public ObservableCollection<PerguntaComResposta> PerguntasComResposta { get; set; } = new();

        public Command EnviarRespostaCommand { get; set; }

        public ResponderAtividadeViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            aService = new AlunoAtividadeService(token);
            momentoInicio = DateTime.Now;
            EnviarRespostaCommand = new Command(async () => await EnviarResposta());
        }

        private async Task CarregarAtividade(int id)
        {
            try
            {
                Atividade = await aService.GetAtividadeAsync(id);
                PerguntasComResposta.Clear();

                if (Atividade?.Perguntas != null)
                {
                    foreach (var p in Atividade.Perguntas)
                        PerguntasComResposta.Add(new PerguntaComResposta(p));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message, "Ok");
            }
        }

        private float CalcularPontuacao()
        {
            if (Atividade?.Perguntas == null || Atividade.Perguntas.Count == 0)
                return 0;

            int totalPerguntas = Atividade.Perguntas.Count;
            int acertos = PerguntasComResposta.Count(p => p.OpcaoSelecionada != null && p.OpcaoSelecionada.Correta);

            // Cada pergunta vale PontuacaoMaxima / totalPerguntas
            float valorPorPergunta = (float)Atividade.PontuacaoMaxima / totalPerguntas;
            float pontuacao = acertos * valorPorPergunta;

            // Arredonda para 1 casa decimal
            return (float)Math.Round(pontuacao, 1);
        }

        private async Task EnviarResposta()
        {
            if (PerguntasComResposta.Any(p => p.OpcaoSelecionada == null))
            {
                await Application.Current.MainPage
                    .DisplayAlert("Atenção", "Responda todas as questões antes de enviar!", "Ok");
                return;
            }

            try
            {
                float pontuacao = CalcularPontuacao();

                // Salva no banco
                AtividadeResposta resposta = new AtividadeResposta
                {
                    Aluno = new Utilizador { IdUtilizador = Preferences.Get("UsuarioId", 1) },
                    Atividade = new Atividade { IdAtividade = Atividade.IdAtividade },
                    MomentoInicio = momentoInicio.ToString("yyyy-MM-ddTHH:mm:ss"),
                    MomentoFim = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                    Pontuacao = pontuacao
                };

                await aService.PostRespostaAsync(resposta);

                // Monta o resultado em memória
                var resultado = new ResultadoAtividade
                {
                    TituloAtividade = Atividade.Titulo,
                    TotalPerguntas = PerguntasComResposta.Count,
                    TotalAcertos = PerguntasComResposta.Count(p => p.OpcaoSelecionada.Correta),
                    Pontuacao = pontuacao,
                    PontuacaoMaxima = Atividade.PontuacaoMaxima,
                    Perguntas = PerguntasComResposta.Select(p => new ResultadoPergunta
                    {
                        Enunciado = p.Pergunta.Enunciado,
                        OpcaoEscolhida = p.OpcaoSelecionada?.Descricao ?? "",
                        OpcaoCorreta = p.Pergunta.Opcoes.FirstOrDefault(o => o.Correta)?.Descricao ?? "",
                        Acertou = p.OpcaoSelecionada?.Correta ?? false
                    }).ToList()
                };

                // Passa via navegação serializado
                var json = Uri.EscapeDataString(
                    System.Text.Json.JsonSerializer.Serialize(resultado));

                await Shell.Current.GoToAsync($"ResultadoAtividadeView?resultado={json}");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " | " + ex.InnerException, "Ok");
            }
        }
    }

    // Classe auxiliar para manter a opção selecionada por pergunta
    public class PerguntaComResposta : EstudeX.ViewModels.BaseViewModel
    {
        public AtividadePergunta Pergunta { get; set; }

        private PerguntasOpcoes opcaoSelecionada;
        public PerguntasOpcoes OpcaoSelecionada
        {
            get => opcaoSelecionada;
            set { opcaoSelecionada = value; OnPropertyChanged(); AtualizarOpcoes(); }
        }

        public ObservableCollection<OpcaoComSelecao> Opcoes { get; set; } = new();

        public PerguntaComResposta(AtividadePergunta pergunta)
        {
            Pergunta = pergunta;
            if (pergunta.Opcoes != null)
                foreach (var o in pergunta.Opcoes)
                    Opcoes.Add(new OpcaoComSelecao(o, this));
        }

        private void AtualizarOpcoes()
        {
            foreach (var o in Opcoes)
                o.AtualizarSelecao();
        }
    }

    public class OpcaoComSelecao : EstudeX.ViewModels.BaseViewModel
    {
        private PerguntaComResposta perguntaPai;
        public PerguntasOpcoes Opcao { get; set; }

        private bool selecionada;
        public bool Selecionada
        {
            get => selecionada;
            set { selecionada = value; OnPropertyChanged(); }
        }

        public Command SelecionarCommand { get; set; }

        public OpcaoComSelecao(PerguntasOpcoes opcao, PerguntaComResposta pai)
        {
            Opcao = opcao;
            perguntaPai = pai;
            SelecionarCommand = new Command(() =>
            {
                perguntaPai.OpcaoSelecionada = opcao;
            });
        }

        public void AtualizarSelecao()
        {
            Selecionada = perguntaPai.OpcaoSelecionada == Opcao;
        }
    }
}