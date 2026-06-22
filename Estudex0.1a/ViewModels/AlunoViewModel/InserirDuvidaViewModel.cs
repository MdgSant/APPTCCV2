using EstudeX.ViewModels;
using Estudex0._1a.Models;
using Estudex0._1a.Services.Aluno;
using Estudex0._1a.Services.Professor;
using System.Collections.ObjectModel;

namespace Estudex0._1a.ViewModels.AlunoViewModel
{
    public class InserirDuvidaViewModel : BaseViewModel
    {
        private string titulo;
        private string descricao;
        private Disciplina disciplinaSelecionada;

        public ObservableCollection<Disciplina> ListaDisciplinas { get; set; } = new();

        public string Titulo
        {
            get => titulo;
            set { titulo = value; OnPropertyChanged(); }
        }
        public string Descricao
        {
            get => descricao;
            set { descricao = value; OnPropertyChanged(); }
        }
        public Disciplina DisciplinaSelecionada
        {
            get => disciplinaSelecionada;
            set { disciplinaSelecionada = value; OnPropertyChanged(); }
        }

        public Command SalvarDuvidaCommand { get; set; }

        private AlunoDuvidaService dService;
        private ProfessorService pService; // para buscar disciplinas

        public InserirDuvidaViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            dService = new AlunoDuvidaService(token);
            pService = new ProfessorService(token);
            SalvarDuvidaCommand = new Command(async () => await SalvarDuvida());
        }

        public async Task InicializarAsync()
        {
            await CarregarListas();
        }

        private async Task CarregarListas()
        {
            try
            {
                var disciplinas = await pService.GetDisciplinasAsync();
                foreach (var d in disciplinas) ListaDisciplinas.Add(d);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Erro", ex.Message + " | " + ex.InnerException, "Ok");
            }
        }

        public async Task SalvarDuvida()
        {
            try
            {
                if (string.IsNullOrEmpty(Titulo) || string.IsNullOrEmpty(Descricao)
                    || DisciplinaSelecionada == null)
                {
                    await Application.Current.MainPage
                        .DisplayAlert("Atenção", "Preencha todos os campos!", "Ok");
                    return;
                }

                int idAluno = Preferences.Get("UsuarioId", 0);

                Duvida model = new Duvida()
                {
                    Titulo = this.titulo,
                    Descricao = this.descricao,
                    Momento = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                    StatusDuvida = "Aberta",
                    Utilizador = new Utilizador { IdUtilizador = 1 }, //Quando implementar Login, alterar para idAluno
                    Disciplina = DisciplinaSelecionada
                };

                await dService.PostDuvidaAsync(model);

                await Application.Current.MainPage
                    .DisplayAlert("Mensagem", "Dúvida enviada com sucesso!", "Ok");

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }
    }
}