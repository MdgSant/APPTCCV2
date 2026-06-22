using Estudex0._1a.ViewModels.UtilizadoresViewModel;

namespace Estudex0._1a.View.Utilizador;

public partial class LoginView : ContentPage
{
    public LoginView()
    {
        InitializeComponent();
        BindingContext = new UtilizadorViewModel();
    }
}