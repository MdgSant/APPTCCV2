using EstudeX.ViewModels;
using Estudex0._1a.Models;
using Estudex0._1a.Services.Professor;
using System.Collections.ObjectModel;

namespace Estudex0._1a.ViewModels.ProfessorViewModel
{
    public class RespostasAtividadeViewModel : BaseViewModel
    {
        private ProfessorService aService;

        private Atividade atividade;
        public Atividade Atividade
        {
            get => atividade;
            set { atividade = value; OnPropertyChanged(); }
        }

        public ObservableCollection<AtividadeResposta> Respostas { get; set; } = new();

        private string idAtividadeStr;
        public string IdAtividadeStr
        {
            set
            {
                if (int.TryParse(value, out int id))
                    _ = CarregarDados(id);
            }
        }

        private int totalRespostas;
        public int TotalRespostas
        {
            get => totalRespostas;
            set { totalRespostas = value; OnPropertyChanged(); }
        }

        private float mediaPontuacao;
        public float MediaPontuacao
        {
            get => mediaPontuacao;
            set { mediaPontuacao = value; OnPropertyChanged(); }
        }

        public RespostasAtividadeViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            aService = new ProfessorService(token);
        }

        private async Task CarregarDados(int id)
        {
            try
            {
                Atividade = await aService.GetAtividadeAsync(id);

                Respostas.Clear();
                var lista = await aService.GetRespostasPorAtividadeAsync(id);
                foreach (var r in lista) Respostas.Add(r);

                TotalRespostas = Respostas.Count;
                MediaPontuacao = Respostas.Count > 0
                    ? Respostas.Average(r => r.Pontuacao)
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