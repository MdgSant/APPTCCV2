using Estudex0._1a.Models;
using Estudex0._1a.ViewModels.AlunoViewModel;

namespace Estudex0._1a.View.AlunoViews;

public partial class ListagemAtividadeAlunoView : ContentPage
{
    public ListagemAtividadeAlunoView()
    {
        InitializeComponent();
        BindingContext = new ListagemAtividadeAlunoViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var vm = BindingContext as ListagemAtividadeAlunoViewModel;
        if (vm != null)
            await vm.InicializarAsync();
    }

    private async void OnAtividadeSelecionada(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is AtividadeStatus item)
        {
            ((CollectionView)sender).SelectedItem = null;

            if (item.JaRespondeu)
            {
                await DisplayAlert("Aviso", "Você já respondeu esta atividade!", "Ok");
                return;
            }

            await Shell.Current.GoToAsync(
                $"ResponderAtividadeView?idAtividade={item.Atividade.IdAtividade}");
        }
    }
}