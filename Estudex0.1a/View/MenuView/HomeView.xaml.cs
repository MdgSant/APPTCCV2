namespace Estudex0._1a.View;

public partial class HomeView : ContentPage
{
    public HomeView()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CarregarHome();
    }

    private void CarregarHome()
    {
        int tipo = Preferences.Get("UtilizadorTipo", 0);
        string nome = Preferences.Get("UtilizadorNome", string.Empty);

        LabelSaudacao.Text = $"Olá, {nome}! 👋";

        if (tipo == 2)
            CarregarHomeProfessor();
        else
            CarregarHomeAluno();
    }

    private void CarregarHomeAluno()
    {
        LabelAtividadesSub.Text = "3 pendentes";
        LabelDuvidasTitulo.Text = "Minhas Dúvidas";
        LabelDuvidasSub.Text = "2 respondidas";

        AdicionarItem(ListaAtividades, "Matemática — Equações", "Prazo hoje", "#85B7EB");
        AdicionarItem(ListaAtividades, "Português — Redação", "Nova", "#c084fc");
        AdicionarItem(ListaAtividades, "Ciências — Células", "Nova", "#c084fc");

        AdicionarItem(ListaDuvidas, "Como resolver equação de 2° grau?", "Respondida", "#5DCAA5");
        AdicionarItem(ListaDuvidas, "Diferença entre célula animal e vegetal", "Respondida", "#5DCAA5");
    }

    private void CarregarHomeProfessor()
    {
        LabelAtividadesSub.Text = "3 ativas no momento";
        LabelDuvidasTitulo.Text = "Dúvidas dos Alunos";
        LabelDuvidasSub.Text = "2 aguardando resposta";

        AdicionarItem(ListaAtividades, "Matemática — Equações", "Ativa", "#c084fc");
        AdicionarItem(ListaAtividades, "Português — Redação", "Ativa", "#c084fc");
        AdicionarItem(ListaAtividades, "Ciências — Células", "Ativa", "#c084fc");

        AdicionarItem(ListaDuvidas, "Como resolver equação de 2° grau?", "Pendente", "#EF9F27");
        AdicionarItem(ListaDuvidas, "Diferença entre célula animal e vegetal", "Nova", "#85B7EB");
    }

    private void AdicionarItem(VerticalStackLayout lista, string titulo, string badge, string badgeColor)
    {
        var grid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto }
            },
            Padding = new Thickness(0, 8),
        };

        grid.Add(new BoxView
        {
            HeightRequest = 1,
            Color = Color.FromArgb("#3d3b6e"),
            VerticalOptions = LayoutOptions.Start
        }, 0, 0);
        Grid.SetColumnSpan(grid.Children[0] as BoxView, 2);

        var labelTitulo = new Label
        {
            Text = titulo,
            FontSize = 12,
            TextColor = Color.FromArgb("#c0c0e0"),
            VerticalOptions = LayoutOptions.Center
        };

        var labelBadge = new Label
        {
            Text = badge,
            FontSize = 10,
            TextColor = Color.FromArgb(badgeColor),
            VerticalOptions = LayoutOptions.Center,
            Padding = new Thickness(8, 2)
        };

        var badgeFrame = new Frame
        {
            BackgroundColor = Color.FromArgb(badgeColor + "22"),
            BorderColor = Color.FromArgb(badgeColor + "44"),
            CornerRadius = 10,
            Padding = new Thickness(0),
            HasShadow = false,
            Content = labelBadge
        };

        grid.Add(labelTitulo, 0, 1);
        grid.Add(badgeFrame, 1, 1);

        lista.Add(grid);
    }

    private async void OnAtividadesClicked(object sender, EventArgs e)
    {
        int tipo = Preferences.Get("UtilizadorTipo", 0);
        if (tipo == 2)
            await Shell.Current.GoToAsync("ListarAtividadeView");
        else
            await Shell.Current.GoToAsync("ListagemAtividadeView");
    }

    private async void OnDuvidasClicked(object sender, EventArgs e)
    {
        int tipo = Preferences.Get("UtilizadorTipo", 0);
        if (tipo == 2)
            await Shell.Current.GoToAsync("ListagemDuvidasView");
        else
            await Shell.Current.GoToAsync("ListagemDuvidasAlunoView");
    }
}