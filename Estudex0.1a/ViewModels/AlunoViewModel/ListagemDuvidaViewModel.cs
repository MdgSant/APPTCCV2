using Estudex0._1a.Models;
using Estudex0._1a.Services.Aluno;
using EstudeX.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Estudex0._1a.ViewModels.AlunoViewModel
{
    public class ListagemDuvidaAlunoViewModel : BaseViewModel
    {
        private AlunoDuvidaService dService;

        // Lista completa (sem filtro) e lista exibida (filtrada)
        private List<Duvida> todasDuvidas = new();
        public ObservableCollection<Duvida> Duvidas { get; set; } = new();

        public ObservableCollection<string> ListaDisciplinas { get; set; } = new();
        public ObservableCollection<string> ListaStatus { get; set; } = new() { "Todos os status", "Aberta", "Respondida" };

        private string serie = "Carregando...";
        public string Serie
        {
            get => serie;
            set { serie = value; OnPropertyChanged(); }
        }

        private string textoBusca = string.Empty;
        public string TextoBusca
        {
            get => textoBusca;
            set { textoBusca = value; OnPropertyChanged(); AplicarFiltros(); }
        }

        private string disciplinaSelecionada = "Todas as disciplinas";
        public string DisciplinaSelecionada
        {
            get => disciplinaSelecionada;
            set { disciplinaSelecionada = value; OnPropertyChanged(); AplicarFiltros(); }
        }

        private string statusSelecionado = "Todos os status";
        public string StatusSelecionado
        {
            get => statusSelecionado;
            set { statusSelecionado = value; OnPropertyChanged(); AplicarFiltros(); }
        }

        private int total;
        public int Total
        {
            get => total;
            set { total = value; OnPropertyChanged(); }
        }

        private bool isLoading;
        public bool IsLoading
        {
            get => isLoading;
            set { isLoading = value; OnPropertyChanged(); OnPropertyChanged(nameof(MostrarErro)); OnPropertyChanged(nameof(MostrarVazio)); }
        }

        private bool houveErro;
        public bool HouveErro
        {
            get => houveErro;
            set { houveErro = value; OnPropertyChanged(); OnPropertyChanged(nameof(MostrarErro)); OnPropertyChanged(nameof(MostrarVazio)); }
        }

        public bool MostrarErro => !IsLoading && HouveErro;
        public bool MostrarVazio => !IsLoading && !HouveErro && Duvidas.Count == 0;

        public ICommand RecarregarCommand { get; set; }

        public ListagemDuvidaAlunoViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            dService = new AlunoDuvidaService(token);

            RecarregarCommand = new Command(async () => await ObterDuvidas());
        }

        public async Task InicializarAsync()
        {
            await ObterDuvidas();
        }

        public async Task ObterDuvidas()
        {
            try
            {
                IsLoading = true;
                HouveErro = false;

                int idUtilizador = Preferences.Get("UtilizadorId", 0);

                var lista = await dService.GetMinhasDuvidasAsync(idUtilizador);
                todasDuvidas = lista?.ToList() ?? new List<Duvida>();

                // Monta a lista de disciplinas disponíveis a partir dos dados
                ListaDisciplinas.Clear();
                ListaDisciplinas.Add("Todas as disciplinas");
                foreach (var nomeDisciplina in todasDuvidas
                             .Where(d => d.Disciplina != null && !string.IsNullOrWhiteSpace(d.Disciplina.Nome))
                             .Select(d => d.Disciplina.Nome)
                             .Distinct()
                             .OrderBy(n => n))
                {
                    ListaDisciplinas.Add(nomeDisciplina);
                }

                AplicarFiltros();
            }
            catch (Exception ex)
            {
                HouveErro = true;
                System.Diagnostics.Debug.WriteLine($"Erro ao carregar dúvidas: {ex.Message} | {ex.InnerException}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void AplicarFiltros()
        {
            IEnumerable<Duvida> filtradas = todasDuvidas;

            if (!string.IsNullOrWhiteSpace(TextoBusca))
            {
                string busca = TextoBusca.Trim().ToLower();
                filtradas = filtradas.Where(d =>
                    (d.Titulo?.ToLower().Contains(busca) ?? false) ||
                    (d.Descricao?.ToLower().Contains(busca) ?? false));
            }

            if (!string.IsNullOrEmpty(DisciplinaSelecionada) && DisciplinaSelecionada != "Todas as disciplinas")
            {
                filtradas = filtradas.Where(d => d.Disciplina?.Nome == DisciplinaSelecionada);
            }

            if (!string.IsNullOrEmpty(StatusSelecionado) && StatusSelecionado != "Todos os status")
            {
                filtradas = filtradas.Where(d =>
                    string.Equals(d.StatusDuvida, StatusSelecionado, StringComparison.OrdinalIgnoreCase));
            }

            Duvidas.Clear();
            foreach (var d in filtradas) Duvidas.Add(d);

            Total = todasDuvidas.Count;
            OnPropertyChanged(nameof(MostrarVazio));
        }
    }
}