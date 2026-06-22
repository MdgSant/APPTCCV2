using Estudex0._1a.ViewModels.ProfessorViewModel;

namespace Estudex0._1a.View.ProfessorViews;

[QueryProperty(nameof(IdAtividade), "idAtividade")]
public partial class DetalhesAtividadeView : ContentPage
{
    private string _idAtividade;
    public string IdAtividade
    {
        get => _idAtividade;
        set
        {
            _idAtividade = value;
            if (BindingContext is DetalhesAtividadeViewModel vm)
                vm.IdAtividadeStr = value;
        }
    }

    private async void OnVerRespostasClicked(object sender, EventArgs e)
    {
        var vm = BindingContext as DetalhesAtividadeViewModel;
        if (vm?.Atividade != null)
            await Shell.Current.GoToAsync(
                $"RespostasAtividadeView?idAtividade={vm.Atividade.IdAtividade}");
    }

    public DetalhesAtividadeView()
    {
        InitializeComponent();
        BindingContext = new DetalhesAtividadeViewModel();
    }
}