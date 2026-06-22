using Estudex0._1a.ViewModels.ProfessorViewModel;

namespace Estudex0._1a.View.ProfessorViews;

[QueryProperty(nameof(IdAtividade), "idAtividade")]
public partial class RespostasAtividadeView : ContentPage
{
    private string _idAtividade;
    public string IdAtividade
    {
        get => _idAtividade;
        set
        {
            _idAtividade = value;
            if (BindingContext is RespostasAtividadeViewModel vm)
                vm.IdAtividadeStr = value;
        }
    }

    public RespostasAtividadeView()
    {
        InitializeComponent();
        BindingContext = new RespostasAtividadeViewModel();
    }
}