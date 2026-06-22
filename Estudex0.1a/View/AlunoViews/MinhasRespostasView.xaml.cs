using Estudex0._1a.ViewModels.AlunoViewModel;

namespace Estudex0._1a.View.AlunoViews;

public partial class MinhasRespostasView : ContentPage
{
    public MinhasRespostasView()
    {
        InitializeComponent();
        BindingContext = new MinhasRespostasViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var vm = BindingContext as MinhasRespostasViewModel;
        if (vm != null)
            await vm.InicializarAsync();
    }
}