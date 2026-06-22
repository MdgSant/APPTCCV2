using Estudex0._1a.Models;
using Estudex0._1a.ViewModels.AlunoViewModel;

namespace Estudex0._1a.View.AlunoViews;

public partial class ListagemDuvidaAlunoView : ContentPage
{
    public ListagemDuvidaAlunoView()
    {
        InitializeComponent();
        BindingContext = new ListagemDuvidaAlunoViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var vm = BindingContext as ListagemDuvidaAlunoViewModel;
        if (vm != null)
            await vm.InicializarAsync();
    }

    private async void OnNovaDuvidaClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("InserirDuvidaView");
    }

    private async void OnDuvidaSelecionada(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Duvida duvida)
        {
            ((CollectionView)sender).SelectedItem = null;
            await Shell.Current.GoToAsync(
                $"DetalheDuvidaAlunoView?idDuvida={duvida.IdDuvida}");
        }
    }
}