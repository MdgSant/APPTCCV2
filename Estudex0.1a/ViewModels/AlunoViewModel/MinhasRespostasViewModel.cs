using EstudeX.ViewModels;
using Estudex0._1a.Models;
using Estudex0._1a.Services.Aluno;
using System.Collections.ObjectModel;

namespace Estudex0._1a.ViewModels.AlunoViewModel
{
    public class MinhasRespostasViewModel : BaseViewModel
    {
        private AlunoAtividadeService aService;

        public ObservableCollection<AtividadeResposta> Respostas { get; set; } = new();

        private int totalRespondidas;
        public int TotalRespondidas
        {
            get => totalRespondidas;
            set { totalRespondidas = value; OnPropertyChanged(); }
        }

        private float mediaPontuacao;
        public float MediaPontuacao
        {
            get => mediaPontuacao;
            set { mediaPontuacao = value; OnPropertyChanged(); }
        }

        public MinhasRespostasViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            aService = new AlunoAtividadeService(token);
        }

        public async Task InicializarAsync()
        {
            await CarregarRespostas();
        }

        private async Task CarregarRespostas()
        {
            try
            {
                Respostas.Clear();
                int idAluno = Preferences.Get("UsuarioId", 0);

                var lista = await aService.GetMinhasRespostasAsync(idAluno);
                foreach (var r in lista) Respostas.Add(r);

                TotalRespondidas = Respostas.Count;
                MediaPontuacao = Respostas.Count > 0
                    ? (float)Math.Round(Respostas.Average(r => r.Pontuacao), 1)
                    : 0;

                OnPropertyChanged(nameof(Respostas));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message, "Ok");
            }
        }
    }
}