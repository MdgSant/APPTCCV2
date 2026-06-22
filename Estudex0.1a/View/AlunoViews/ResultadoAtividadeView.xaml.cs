using Estudex0._1a.ViewModels.AlunoViewModel;

namespace Estudex0._1a.View.AlunoViews;

[QueryProperty(nameof(ResultadoJson), "resultado")]
public partial class ResultadoAtividadeView : ContentPage
{
    public string ResultadoJson
    {
        set
        {
            if (BindingContext is ResultadoAtividadeViewModel vm)
                vm.ResultadoJson = value;
        }
    }

    public ResultadoAtividadeView()
    {
        InitializeComponent();
        BindingContext = new ResultadoAtividadeViewModel();
    }

    private async void OnVoltarClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("../..");
    }
}