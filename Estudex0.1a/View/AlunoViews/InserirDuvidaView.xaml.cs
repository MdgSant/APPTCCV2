using Estudex0._1a.ViewModels.AlunoViewModel;

namespace Estudex0._1a.View.AlunoViews;

public partial class InserirDuvidaView : ContentPage
{
    public InserirDuvidaView()
    {
        InitializeComponent();
        BindingContext = new InserirDuvidaViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var vm = BindingContext as InserirDuvidaViewModel;
        if (vm != null)
            await vm.InicializarAsync();
    }
}