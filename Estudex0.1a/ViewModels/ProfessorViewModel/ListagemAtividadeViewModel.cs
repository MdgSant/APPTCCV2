using EstudeX.ViewModels;
using Estudex0._1a.Models;
using Estudex0._1a.Services.Professor;
using System.Collections.ObjectModel;

namespace Estudex0._1a.ViewModels.ProfessorViewModel
{
    public class ListagemAtividadeViewModel : BaseViewModel
    {
        private ProfessorService aService;
        private const int tamanhoPagina = 10;
        private int paginaAtual = 0;
        private bool carregando = false;

        public ObservableCollection<Atividade> Atividades { get; set; } = new();

        private int totalAtividades;
        public int TotalAtividades
        {
            get => totalAtividades;
            set { totalAtividades = value; OnPropertyChanged(); }
        }

        private bool temMaisPaginas = true;
        public bool TemMaisPaginas
        {
            get => temMaisPaginas;
            set { temMaisPaginas = value; OnPropertyChanged(); }
        }

        private bool carregandoMais;
        public bool CarregandoMais
        {
            get => carregandoMais;
            set { carregandoMais = value; OnPropertyChanged(); }
        }

        public Command CarregarMaisCommand { get; set; }

        public ListagemAtividadeViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            aService = new ProfessorService(token);

            CarregarMaisCommand = new Command(
                async () => await CarregarPagina(),
                () => TemMaisPaginas && !carregando);
        }

        public async Task InicializarAsync()
        {
            paginaAtual = 0;
            TemMaisPaginas = true;
            Atividades.Clear();
            await CarregarPagina();
        }

        private async Task CarregarPagina()
        {
            if (carregando || !TemMaisPaginas) return;

            try
            {
                carregando = true;
                CarregandoMais = true;
                ((Command)CarregarMaisCommand).ChangeCanExecute();

                var resultado = await aService.GetAtividadesPaginadasAsync(paginaAtual, tamanhoPagina);

                foreach (var a in resultado.Conteudo)
                    Atividades.Add(a);

                TotalAtividades = resultado.TotalElementos;
                TemMaisPaginas = !resultado.Ultima;
                paginaAtual++;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " | " + ex.InnerException, "Ok");
            }
            finally
            {
                carregando = false;
                CarregandoMais = false;
                ((Command)CarregarMaisCommand).ChangeCanExecute();
            }
        }
    }
}