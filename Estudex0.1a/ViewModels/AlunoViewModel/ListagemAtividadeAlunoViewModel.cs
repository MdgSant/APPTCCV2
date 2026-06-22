using EstudeX.ViewModels;
using Estudex0._1a.Models;
using Estudex0._1a.Services.Aluno;
using System.Collections.ObjectModel;

namespace Estudex0._1a.ViewModels.AlunoViewModel
{
    public class ListagemAtividadeAlunoViewModel : BaseViewModel
    {
        private AlunoAtividadeService aService;
        private const int tamanhoPagina = 10;
        private int paginaAtual = 0;
        private bool carregando = false;
        private int idAluno;

        public ObservableCollection<AtividadeStatus> Atividades { get; set; } = new();

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

        public ListagemAtividadeAlunoViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            aService = new AlunoAtividadeService(token);
            idAluno = Preferences.Get("UtilizadorId", 0);

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
                {
                    int idAtividade = a.IdAtividade ?? 0;
                    bool jaRespondeu = await aService.VerificarJaRespondeuAsync(idAluno, idAtividade); // ✅ usar a variável
                    Atividades.Add(new AtividadeStatus
                    {
                        Atividade = a,
                        JaRespondeu = jaRespondeu
                    });
                }

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