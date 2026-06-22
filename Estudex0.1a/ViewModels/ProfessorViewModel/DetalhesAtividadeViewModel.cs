using EstudeX.ViewModels;
using Estudex0._1a.Models;
using Estudex0._1a.Services.Professor;

namespace Estudex0._1a.ViewModels.ProfessorViewModel
{
    public class DetalhesAtividadeViewModel : BaseViewModel
    {
        private ProfessorService aService;

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
                idAtividadeStr = value;
                if (int.TryParse(value, out int id))
                    _ = CarregarAtividade(id);
            }
        }

        public DetalhesAtividadeViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            aService = new ProfessorService(token);
        }

        private async Task CarregarAtividade(int id)
        {
            try
            {
                Atividade = await aService.GetAtividadeAsync(id);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Ops", ex.Message + " | " + ex.InnerException, "Ok");
            }
        }
    }
}