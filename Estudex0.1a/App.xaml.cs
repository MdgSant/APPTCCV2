using Microsoft.Extensions.DependencyInjection;

namespace Estudex0._1a
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new View.Utilizador.LoginView());
        }
    }
}