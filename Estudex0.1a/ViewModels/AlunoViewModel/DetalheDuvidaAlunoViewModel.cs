using Estudex0._1a.Models;
using Estudex0._1a.Services.Aluno;
using EstudeX.ViewModels;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Estudex0._1a.ViewModels.AlunoViewModel;

public class DetalheDuvidaAlunoViewModel : BaseViewModel
{
    private AlunoDuvidaService _service;

    private Duvida duvida;
    public Duvida Duvida
    {
        get => duvida;
        set { duvida = value; OnPropertyChanged(); }
    }

    public ObservableCollection<RespostaDuvida> Respostas { get; set; } = new();

    public bool SemRespostas => Respostas.Count == 0;

    private string idDuvidaStr;
    public string IdDuvidaStr
    {
        get => idDuvidaStr;
        set
        {
            idDuvidaStr = value;
            if (int.TryParse(value, out int id))
                _ = CarregarAsync(id);
        }
    }

    public DetalheDuvidaAlunoViewModel()
    {
        string token = Preferences.Get("UsuarioToken", string.Empty);
        _service = new AlunoDuvidaService(token);

        // Atualiza SemRespostas sempre que a lista mudar
        Respostas.CollectionChanged += (s, e) => OnPropertyChanged(nameof(SemRespostas));
    }

    private async Task CarregarAsync(int idDuvida)
    {
        try
        {
            Duvida = await _service.GetDuvidaAsync(idDuvida);
            var respostas = await _service.GetRespostasDuvidaAsync(idDuvida);
            Respostas.Clear();
            foreach (var r in respostas) Respostas.Add(r);
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage
                .DisplayAlert("Erro", ex.Message, "Ok");
        }
    }
}