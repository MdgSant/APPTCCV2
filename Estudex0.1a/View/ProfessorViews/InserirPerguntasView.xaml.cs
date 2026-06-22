using Estudex0._1a.ViewModels.ProfessorViewModel;

namespace Estudex0._1a.View.ProfessorViews;

[QueryProperty(nameof(RascunhoJson), "rascunho")]
public partial class InserirPerguntaView : ContentPage
{
    private string _rascunhoJson;
    public string RascunhoJson
    {
        get => _rascunhoJson;
        set
        {
            _rascunhoJson = value;
            // Passa para o ViewModel após receber
            if (BindingContext is InserirPerguntaViewModel vm)
                vm.RascunhoJson = value;
        }
    }

    public InserirPerguntaView()
    {
        InitializeComponent();
        BindingContext = new InserirPerguntaViewModel();
    }

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("../..");
    }
}