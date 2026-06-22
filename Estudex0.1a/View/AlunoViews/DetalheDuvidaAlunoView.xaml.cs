using Estudex0._1a.ViewModels.AlunoViewModel;

namespace Estudex0._1a.View.AlunoViews;

[QueryProperty(nameof(IdDuvida), "idDuvida")]
public partial class DetalheDuvidaAlunoView : ContentPage
{
    private string _idDuvida;
    public string IdDuvida
    {
        get => _idDuvida;
        set
        {
            _idDuvida = value;
            if (BindingContext is DetalheDuvidaAlunoViewModel vm)
                vm.IdDuvidaStr = value;
        }
    }

    public DetalheDuvidaAlunoView()
    {
        InitializeComponent();
        BindingContext = new DetalheDuvidaAlunoViewModel();
    }
}