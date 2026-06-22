using Estudex0._1a.Models;
using Estudex0._1a.Services.Aluno;
using Estudex0._1a.Services.Professor;

namespace Estudex0._1a.View;

public partial class HomeView : ContentPage
{
    public HomeView()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CarregarHome();
    }

    private async Task CarregarHome()
    {
        int tipo = Preferences.Get("UtilizadorTipo", 0);
        string nome = Preferences.Get("UtilizadorNome", string.Empty);
        string token = Preferences.Get("UsuarioToken", string.Empty);
        int idUtilizador = Preferences.Get("UtilizadorId", 0);

        LabelSaudacao.Text = $"Olá, {nome}! 👋";

        if (tipo == 2)
            await CarregarHomeProfessor(token);
        else
            await CarregarHomeAluno(token, idUtilizador);
    }

    private async Task CarregarHomeAluno(string token, int idAluno)
    {
        try
        {
            LabelDuvidasTitulo.Text = "Minhas Dúvidas";

            var atividadeService = new AlunoAtividadeService(token);
            var duvidaService = new AlunoDuvidaService(token);

            var respostas = await atividadeService.GetMinhasRespostasAsync(idAluno);
            var duvidas = await duvidaService.GetMinhasDuvidasAsync(idAluno);

            // --- Atividades ---
            ListaAtividades.Children.Clear();

            if (respostas != null && respostas.Count > 0)
            {
                int pendentes = 0; // já respondidas aparecem aqui
                LabelAtividadesSub.Text = $"{respostas.Count} respondida(s)";

                foreach (var r in respostas.Take(3))
                {
                    string titulo = r.Atividade?.Titulo ?? "Atividade";
                    float pontuacao = r.Pontuacao;
                    int max = r.Atividade?.PontuacaoMaxima ?? 1;
                    int pct = max > 0 ? (int)((pontuacao / max) * 100) : 0;
                    AdicionarItem(ListaAtividades, titulo, $"{pct}%", "#5DCAA5");
                }
            }
            else
            {
                LabelAtividadesSub.Text = "Nenhuma respondida ainda";
                AdicionarItem(ListaAtividades, "Nenhuma atividade respondida", "—", "#7c6aaa");
            }

            // --- Dúvidas ---
            ListaDuvidas.Children.Clear();

            if (duvidas != null && duvidas.Count > 0)
            {
                int respondidas = duvidas.Count(d =>
                    string.Equals(d.StatusDuvida, "Respondida", StringComparison.OrdinalIgnoreCase));
                int abertas = duvidas.Count - respondidas;

                LabelDuvidasSub.Text = abertas > 0
                    ? $"{abertas} aguardando resposta"
                    : "Todas respondidas";

                foreach (var d in duvidas.Take(3))
                {
                    bool respondida = string.Equals(d.StatusDuvida, "Respondida",
                        StringComparison.OrdinalIgnoreCase);
                    AdicionarItem(ListaDuvidas, d.Titulo ?? "Dúvida",
                        d.StatusDuvida ?? "Aberta",
                        respondida ? "#5DCAA5" : "#EF9F27");
                }
            }
            else
            {
                LabelDuvidasSub.Text = "Nenhuma dúvida enviada";
                AdicionarItem(ListaDuvidas, "Nenhuma dúvida enviada ainda", "—", "#7c6aaa");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro HomeAluno: {ex.Message}");
            LabelAtividadesSub.Text = "Erro ao carregar";
            LabelDuvidasSub.Text = "Erro ao carregar";
        }
    }

    private async Task CarregarHomeProfessor(string token)
    {
        try
        {
            LabelDuvidasTitulo.Text = "Dúvidas dos Alunos";

            var atividadeService = new AlunoAtividadeService(token);
            var duvidaService = new ProfessorDuvidasService(token);

            var atividades = await atividadeService.GetAtividadesAsync();
            var duvidas = await duvidaService.GetDuvidasAsync();

            // --- Atividades ---
            ListaAtividades.Children.Clear();

            if (atividades != null && atividades.Count > 0)
            {
                LabelAtividadesSub.Text = $"{atividades.Count} cadastrada(s)";

                foreach (var a in atividades.Take(3))
                    AdicionarItem(ListaAtividades, a.Titulo ?? "Atividade", "Ativa", "#c084fc");
            }
            else
            {
                LabelAtividadesSub.Text = "Nenhuma atividade";
                AdicionarItem(ListaAtividades, "Nenhuma atividade cadastrada", "—", "#7c6aaa");
            }

            // --- Dúvidas ---
            ListaDuvidas.Children.Clear();

            if (duvidas != null && duvidas.Count > 0)
            {
                int pendentes = duvidas.Count(d =>
                    !string.Equals(d.StatusDuvida, "Respondida", StringComparison.OrdinalIgnoreCase));

                LabelDuvidasSub.Text = pendentes > 0
                    ? $"{pendentes} aguardando resposta"
                    : "Todas respondidas";

                foreach (var d in duvidas.Take(3))
                {
                    bool respondida = string.Equals(d.StatusDuvida, "Respondida",
                        StringComparison.OrdinalIgnoreCase);
                    AdicionarItem(ListaDuvidas, d.Titulo ?? "Dúvida",
                        d.StatusDuvida ?? "Aberta",
                        respondida ? "#5DCAA5" : "#EF9F27");
                }
            }
            else
            {
                LabelDuvidasSub.Text = "Nenhuma dúvida";
                AdicionarItem(ListaDuvidas, "Nenhuma dúvida pendente", "—", "#7c6aaa");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro HomeProfessor: {ex.Message}");
            LabelAtividadesSub.Text = "Erro ao carregar";
            LabelDuvidasSub.Text = "Erro ao carregar";
        }
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

        var divisor = new BoxView
        {
            HeightRequest = 1,
            Color = Color.FromArgb("#3d3b6e"),
            VerticalOptions = LayoutOptions.Start
        };
        grid.Add(divisor, 0, 0);
        Grid.SetColumnSpan(divisor, 2);

        grid.Add(new Label
        {
            Text = titulo,
            FontSize = 12,
            TextColor = Color.FromArgb("#c0c0e0"),
            VerticalOptions = LayoutOptions.Center
        }, 0, 1);

        grid.Add(new Frame
        {
            BackgroundColor = Color.FromArgb(badgeColor + "22"),
            BorderColor = Color.FromArgb(badgeColor + "44"),
            CornerRadius = 10,
            Padding = new Thickness(0),
            HasShadow = false,
            Content = new Label
            {
                Text = badge,
                FontSize = 10,
                TextColor = Color.FromArgb(badgeColor),
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(8, 2)
            }
        }, 1, 1);

        lista.Add(grid);
    }

    private async void OnAtividadesClicked(object sender, EventArgs e)
    {
        int tipo = Preferences.Get("UtilizadorTipo", 0);
        await Shell.Current.GoToAsync(tipo == 2 ? "ListarAtividadeView" : "ListagemAtividadeView");
    }

    private async void OnDuvidasClicked(object sender, EventArgs e)
    {
        int tipo = Preferences.Get("UtilizadorTipo", 0);
        await Shell.Current.GoToAsync(tipo == 2 ? "ListagemDuvidasView" : "ListagemDuvidasAlunoView");
    }
}