using Estudex0._1a.Models;
using Estudex0._1a.ViewModels.ProfessorViewModel;

namespace Estudex0._1a.View.ProfessorViews;

public partial class DuvidaView : ContentPage
{
    public DuvidaView()
    {
        InitializeComponent();
        BindingContext = new ListagemDuvidaViewModel();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var vm = BindingContext as ListagemDuvidaViewModel;
        if (vm != null)
            await vm.InicializarAsync();
    }

    private async void OnBuscarPorIdClicked(object sender, EventArgs e)
    {
        var vm = BindingContext as ListagemDuvidaViewModel;
        if (vm == null) return;

        if (int.TryParse(EntryIdDuvida.Text, out int idDuvida))
        {
            await vm.ObterDuvidaPorId(idDuvida);
        }
        else
        {
            await DisplayAlert("Ops", "Informe um ID válido.", "Ok");
        }
    }

    private async void OnDuvidaSelecionada(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Duvida duvida)
        {
            ((CollectionView)sender).SelectedItem = null;
            await Shell.Current.GoToAsync(
                $"DetalheDuvidaProfessorView?idDuvida={duvida.IdDuvida}");
        }
    }
}