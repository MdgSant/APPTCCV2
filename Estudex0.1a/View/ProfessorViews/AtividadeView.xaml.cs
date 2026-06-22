using Estudex0._1a.Models;
using Estudex0._1a.ViewModels.ProfessorViewModel;

namespace Estudex0._1a.View.ProfessorViews;

public partial class AtividadeView : ContentPage
{
    public AtividadeView()
    {
        InitializeComponent();
        BindingContext = new ListagemAtividadeViewModel();
    }

    private async void OnNovaAtividadeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(InserirAtividadeView));
    }

    private async void OnAtividadeSelecionada(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Atividade atividade)
        {
            // Limpa seleção visual
            ((CollectionView)sender).SelectedItem = null;

            await Shell.Current.GoToAsync($"DetalhesAtividadeView?idAtividade={atividade.IdAtividade}");
        }
    }
}