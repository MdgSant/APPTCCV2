using Estudex0._1a.ViewModels.UtilizadoresViewModel;

namespace Estudex0._1a.View.Utilizador;

public partial class CadastroView : ContentPage
{
	UtilizadorViewModel viewModel;
	public CadastroView()
	{
		InitializeComponent();

		viewModel = new UtilizadorViewModel();
		BindingContext = viewModel;
	}
		private void OnRegistrarClicked(object sender, EventArgs e)
	{
		this.Focus();
	}
}