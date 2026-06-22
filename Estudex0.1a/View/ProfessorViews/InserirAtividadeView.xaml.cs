using Estudex0._1a.ViewModels.ProfessorViewModel;

namespace Estudex0._1a.View.ProfessorViews;

public partial class InserirAtividadeView : ContentPage
{
    public InserirAtividadeView()
    {
        InitializeComponent();
        BindingContext = new InserirAtividadeViewModel(); // ← faltava isso
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var vm = BindingContext as InserirAtividadeViewModel;
        if (vm != null)
            await vm.InicializarAsync();
    }
}