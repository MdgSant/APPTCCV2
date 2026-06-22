using Estudex0._1a.Models;
using Estudex0._1a.Services.Professor;
using EstudeX.ViewModels;
using System.Collections.ObjectModel;

namespace Estudex0._1a.ViewModels.ProfessorViewModel
{
    public class ListagemDuvidaViewModel : BaseViewModel
    {
        private ProfessorDuvidasService dService;

        public ObservableCollection<Duvida> Duvidas { get; set; }

        private int totalPendentes;
        private int totalRespondidas;
        private int total;

        public int TotalPendentes
        {
            get => totalPendentes;
            set { totalPendentes = value; OnPropertyChanged(); }
        }

        public int TotalRespondidas
        {
            get => totalRespondidas;
            set { totalRespondidas = value; OnPropertyChanged(); }
        }

        public int Total
        {
            get => total;
            set { total = value; OnPropertyChanged(); }
        }

        public ListagemDuvidaViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            dService = new ProfessorDuvidasService(token);
            Duvidas = new ObservableCollection<Duvida>();
        }

        public async Task InicializarAsync()
        {
            await ObterDuvidas();
        }

        public async Task ObterDuvidas()
        {
            try
            {
                Duvidas.Clear();
                var lista = await dService.GetDuvidasAsync();
                foreach (var d in lista) Duvidas.Add(d);

                TotalPendentes = Duvidas.Count(d =>
                    d.StatusDuvida?.ToLower() == "aberta" ||
                    d.StatusDuvida?.ToLower() == "pendente");

                TotalRespondidas = Duvidas.Count(d =>
                    d.StatusDuvida?.ToLower() == "respondida");

                Total = Duvidas.Count;

                OnPropertyChanged(nameof(Duvidas));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }

        public async Task ObterDuvidaPorId(int idDuvida)
        {
            try
            {
                var duvida = await dService.GetDuvidaProfessorAsync(idDuvida);
                Duvidas.Clear();
                Duvidas.Add(duvida);
                Total = Duvidas.Count;
                OnPropertyChanged(nameof(Duvidas));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " Detalhes: " + ex.InnerException, "Ok");
            }
        }
    }
}