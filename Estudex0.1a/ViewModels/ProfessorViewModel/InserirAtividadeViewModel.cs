using EstudeX.ViewModels;
using Estudex0._1a.Models;
using Estudex0._1a.Services.Professor;
using System.Collections.ObjectModel;

namespace Estudex0._1a.ViewModels.ProfessorViewModel
{
    public class InserirAtividadeViewModel : BaseViewModel
    {
        private string titulo;
        private int pontuacaoMaxima;
        private NivelDificuldade nivelDificuldadeSelecionado;
        private Utilizador orientadorSelecionado;
        private Disciplina disciplinaSelecionada;

        public ObservableCollection<NivelDificuldade> ListaNiveisDificuldade { get; set; } = new();
        public ObservableCollection<Utilizador> ListaOrientadores { get; set; } = new();
        public ObservableCollection<Disciplina> ListaDisciplinas { get; set; } = new();

        public string Titulo
        {
            get => titulo;
            set { titulo = value; OnPropertyChanged(); }
        }
        public int PontuacaoMaxima
        {
            get => pontuacaoMaxima;
            set { pontuacaoMaxima = value; OnPropertyChanged(); }
        }
        public NivelDificuldade NivelDificuldadeSelecionado
        {
            get => nivelDificuldadeSelecionado;
            set { nivelDificuldadeSelecionado = value; OnPropertyChanged(); }
        }
        public Utilizador OrientadorSelecionado
        {
            get => orientadorSelecionado;
            set { orientadorSelecionado = value; OnPropertyChanged(); }
        }
        public Disciplina DisciplinaSelecionada
        {
            get => disciplinaSelecionada;
            set { disciplinaSelecionada = value; OnPropertyChanged(); }
        }

        public Command MaterialApoioCommand { get; set; }
        public Command ProximoCommand { get; set; }

        private ProfessorService aService;

        public InserirAtividadeViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            aService = new ProfessorService(token);

            MaterialApoioCommand = new Command(() =>
            {
                Application.Current.MainPage
                    .DisplayAlert("Em breve", "Funcionalidade de material de apoio em desenvolvimento!", "Ok");
            });

            ProximoCommand = new Command(async () => await Proximo());
        }

        public async Task InicializarAsync()
        {
            await CarregarListas();
        }

        private async Task CarregarListas()
        {
            try
            {
                var niveis = await aService.GetNiveisDificuldadeAsync();
                foreach (var n in niveis) ListaNiveisDificuldade.Add(n);

                var orientadores = await aService.GetOrientadoresAsync();
                foreach (var o in orientadores) ListaOrientadores.Add(o);

                var disciplinas = await aService.GetDisciplinasAsync();
                foreach (var d in disciplinas) ListaDisciplinas.Add(d);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Erro", ex.Message + " | " + ex.InnerException, "Ok");
            }
        }

        private async Task Proximo()
        {
            if (string.IsNullOrEmpty(Titulo) || NivelDificuldadeSelecionado == null
                || OrientadorSelecionado == null || DisciplinaSelecionada == null)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Atenção", "Preencha todos os campos!", "Ok");
                return;
            }

            var rascunho = new AtividadeRascunho
            {
                Titulo = this.titulo,
                PontuacaoMaxima = this.pontuacaoMaxima,
                Orientador = OrientadorSelecionado,
                NivelDificuldade = NivelDificuldadeSelecionado,
                Disciplina = DisciplinaSelecionada
            };

            var json = System.Text.Json.JsonSerializer.Serialize(rascunho);
            var encoded = Uri.EscapeDataString(json);

            await Shell.Current.GoToAsync($"InserirPerguntasView?rascunho={encoded}");
        }
    }
}