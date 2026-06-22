using Estudex0._1a.ViewModels.ProfessorViewModel;

namespace Estudex0._1a.View.ProfessorViews;

[QueryProperty(nameof(IdDuvida), "idDuvida")]
public partial class DetalheDuvidaProfessorView : ContentPage
{
    private string _idDuvida;
    public string IdDuvida
    {
        get => _idDuvida;
        set
        {
            _idDuvida = value;
            if (BindingContext is DetalheDuvidaProfessorViewModel vm)
                vm.IdDuvidaStr = value;
        }
    }

    public DetalheDuvidaProfessorView()
    {
        InitializeComponent();
        BindingContext = new DetalheDuvidaProfessorViewModel();
    }
}