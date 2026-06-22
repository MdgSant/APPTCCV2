using EstudeX.ViewModels;
using Estudex0._1a.Models;
using Estudex0._1a.Services.Professor;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace Estudex0._1a.ViewModels.ProfessorViewModel
{
    public class InserirPerguntaViewModel : BaseViewModel
    {
        private ProfessorService aService;
        private AtividadeRascunho rascunho;

        private const int maxQuestoes = 10;
        private const int maxOpcoes = 5;
        private int questaoAtual = 1;

        private string enunciado;
        public string Enunciado
        {
            get => enunciado;
            set { enunciado = value; OnPropertyChanged(); }
        }

        private string contadorQuestao;
        public string ContadorQuestao
        {
            get => contadorQuestao;
            set { contadorQuestao = value; OnPropertyChanged(); }
        }

        private string rascunhoJson;
        public string RascunhoJson
        {
            set
            {
                rascunhoJson = Uri.UnescapeDataString(value);
                rascunho = JsonSerializer.Deserialize<AtividadeRascunho>(rascunhoJson);
            }
        }

        public ObservableCollection<OpcaoItem> Opcoes { get; set; } = new();

        public Command AdicionarOpcaoCommand { get; set; }
        public Command<OpcaoItem> RemoverOpcaoCommand { get; set; }
        public Command<OpcaoItem> MarcarCorretaCommand { get; set; }
        public Command ProximaQuestaoCommand { get; set; }
        public Command FinalizarAtividadeCommand { get; set; }

        public InserirPerguntaViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            aService = new ProfessorService(token);

            AtualizarContador();
            AdicionarOpcaoInicial();

            AdicionarOpcaoCommand = new Command(
                AdicionarOpcao,
                () => Opcoes.Count < maxOpcoes);

            RemoverOpcaoCommand = new Command<OpcaoItem>(opcao =>
            {
                if (Opcoes.Count > 1)
                {
                    Opcoes.Remove(opcao);
                    AtualizarLetras();
                    ((Command)AdicionarOpcaoCommand).ChangeCanExecute();
                }
            });

            MarcarCorretaCommand = new Command<OpcaoItem>(opcao =>
            {
                foreach (var o in Opcoes) o.Correta = false;
                opcao.Correta = true;
                OnPropertyChanged(nameof(Opcoes));
            });

            ProximaQuestaoCommand = new Command(async () => await ProximaQuestao());
            FinalizarAtividadeCommand = new Command(async () => await FinalizarAtividade());
        }

        private void AdicionarOpcaoInicial()
        {
            Opcoes.Add(new OpcaoItem { Letra = "A", Descricao = "", Correta = false });
        }

        private void AdicionarOpcao()
        {
            if (Opcoes.Count < maxOpcoes)
            {
                string[] letras = { "A", "B", "C", "D", "E" };
                Opcoes.Add(new OpcaoItem
                {
                    Letra = letras[Opcoes.Count],
                    Descricao = "",
                    Correta = false
                });
                ((Command)AdicionarOpcaoCommand).ChangeCanExecute();
            }
        }

        private void AtualizarLetras()
        {
            string[] letras = { "A", "B", "C", "D", "E" };
            for (int i = 0; i < Opcoes.Count; i++)
                Opcoes[i].Letra = letras[i];
        }

        private void AtualizarContador()
        {
            ContadorQuestao = $"{questaoAtual} / {maxQuestoes}";
        }

        private bool Validar()
        {
            if (string.IsNullOrEmpty(Enunciado))
            {
                Application.Current.MainPage.DisplayAlert("Atenção", "Preencha o enunciado!", "Ok");
                return false;
            }
            if (!Opcoes.Any(o => o.Correta))
            {
                Application.Current.MainPage.DisplayAlert("Atenção", "Marque uma opção como correta!", "Ok");
                return false;
            }
            if (Opcoes.Any(o => string.IsNullOrEmpty(o.Descricao)))
            {
                Application.Current.MainPage.DisplayAlert("Atenção", "Preencha todas as alternativas!", "Ok");
                return false;
            }
            return true;
        }

        private AtividadePergunta MontarPerguntaAtual()
        {
            return new AtividadePergunta
            {
                Enunciado = this.enunciado,
                Opcoes = Opcoes.Select(o => new PerguntasOpcoes
                {
                    Descricao = o.Descricao,
                    Correta = o.Correta
                }).ToList()
            };
        }

        private void LimparFormulario()
        {
            Enunciado = string.Empty;
            Opcoes.Clear();
            AdicionarOpcaoInicial();
            ((Command)AdicionarOpcaoCommand).ChangeCanExecute();
        }

        private async Task ProximaQuestao()
        {
            if (!Validar()) return;

            rascunho.Perguntas.Add(MontarPerguntaAtual());

            if (questaoAtual >= maxQuestoes)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Aviso", "Limite de 10 questões atingido!", "Ok");
                await FinalizarAtividade();
                return;
            }

            questaoAtual++;
            AtualizarContador();
            LimparFormulario();
        }

        private async Task FinalizarAtividade()
        {
            if (!Validar()) return;

            if (questaoAtual <= rascunho.Perguntas.Count == false)
                rascunho.Perguntas.Add(MontarPerguntaAtual());

            try
            {
                var atividade = await aService.PostAtividadeAsync(new Atividade
                {
                    Titulo = rascunho.Titulo,
                    PontuacaoMaxima = rascunho.PontuacaoMaxima,
                    IdOrientador = rascunho.Orientador.IdUtilizador,
                    IdNivelDificuldade = rascunho.NivelDificuldade.IdNivelDificuldade,
                    IdDisciplina = rascunho.Disciplina.IdDisciplina
                });

                var perguntasParaSalvar = rascunho.Perguntas.ToList();

                foreach (var pergunta in perguntasParaSalvar)
                {
                    pergunta.Atividade = new Atividade { IdAtividade = atividade.IdAtividade };
                    await aService.PostPerguntaAsync(pergunta);
                }

                await Application.Current.MainPage
                    .DisplayAlert("Sucesso", $"Atividade criada com {perguntasParaSalvar.Count} questão(ões)!", "Ok");

                await Shell.Current.GoToAsync("../..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " | " + ex.InnerException, "Ok");
            }
        }
    }

    public class OpcaoItem : EstudeX.ViewModels.BaseViewModel
    {
        private string letra;
        private string descricao;
        private bool correta;

        public string Letra
        {
            get => letra;
            set { letra = value; OnPropertyChanged(); }
        }
        public string Descricao
        {
            get => descricao;
            set { descricao = value; OnPropertyChanged(); }
        }
        public bool Correta
        {
            get => correta;
            set { correta = value; OnPropertyChanged(); }
        }
    }
}