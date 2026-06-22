using Estudex0._1a.ViewModels.AlunoViewModel;

namespace Estudex0._1a.View.AlunoViews;

[QueryProperty(nameof(IdAtividade), "idAtividade")]
public partial class ListagemAtividadeView : ContentPage
{
    private string _idAtividade;
    public string IdAtividade
    {
        get => _idAtividade;
        set
        {
            _idAtividade = value;
            if (BindingContext is ResponderAtividadeViewModel vm)
                vm.IdAtividadeStr = value;
        }
    }

    public ListagemAtividadeView()
    {
        InitializeComponent();
        BindingContext = new ResponderAtividadeViewModel();
    }
}