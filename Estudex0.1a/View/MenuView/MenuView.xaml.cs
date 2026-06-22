using Estudex0._1a.Models;
using Estudex0._1a.Services.Aluno;
using Estudex0._1a.View.Utilizador;

namespace Estudex0._1a.View;

public partial class MenuView : ContentPage
{
    public MenuView()
    {
        InitializeComponent();
        AplicarPerfil();
    }

    private async void AplicarPerfil()
    {
        int tipo = Preferences.Get("UtilizadorTipo", 0);
        string nome = Preferences.Get("UtilizadorNome", string.Empty);

        LabelNome.Text = nome;
        LabelPerfil.Text = tipo == 2 ? "👨‍🏫 Professor" : "🎓 Aluno";

        bool isAluno = tipo == 1;
        MenuProfessor.IsVisible = tipo == 2;
        MenuAluno.IsVisible = isAluno;

        if (isAluno)
        {
            string bio = Preferences.Get("AlunoBio", "Estudante focado no ENEM 2025! 🚀");
            LabelBio.Text = bio;

            await CarregarDesempenhoAluno();
        }
        else
        {
            GridBio.IsVisible = false;
            GridStats.IsVisible = false;
            DividerSelos.IsVisible = false;
            LayoutSelos.IsVisible = false;
        }
    }

    private async Task CarregarDesempenhoAluno()
    {
        try
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            int idAluno = Preferences.Get("UtilizadorId", 0);

            var service = new AlunoAtividadeService(token);
            var respostas = await service.GetMinhasRespostasAsync(idAluno);

            int totalAtividades = respostas?.Count ?? 0;
            LabelAtividades.Text = totalAtividades.ToString();

            // Calcula média de acertos: pontuacao / PontuacaoMaxima de cada atividade
            if (totalAtividades > 0)
            {
                var percentuais = respostas
                    .Where(r => r.Atividade != null
                    && r.Atividade.PontuacaoMaxima > 0)
                    .Select(r => (r.Pontuacao / r.Atividade.PontuacaoMaxima) * 100)
                    .ToList();

                double mediaAcertos = percentuais.Any() ? percentuais.Average() : 0;
                LabelAcertos.Text = $"{mediaAcertos:0}%";
            }
            else
            {
                LabelAcertos.Text = "0%";
            }

            // Conquistas: 1 selo a cada 5 atividades respondidas
            int conquistas = totalAtividades / 5;
            LabelConquistas.Text = conquistas.ToString();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erro ao carregar desempenho: {ex.Message}");
            LabelAtividades.Text = "0";
            LabelAcertos.Text = "0%";
            LabelConquistas.Text = "0";
        }
    }

    private async void OnEditarBioClicked(object sender, EventArgs e)
    {
        string bioAtual = Preferences.Get("AlunoBio", "Estudante focado no ENEM 2025! 🚀");

        string novaBio = await DisplayPromptAsync(
            "Editar Bio",
            "Fale um pouco sobre você:",
            accept: "Salvar",
            cancel: "Cancelar",
            initialValue: bioAtual,
            maxLength: 120,
            keyboard: Keyboard.Text);

        if (novaBio != null)
        {
            Preferences.Set("AlunoBio", novaBio);
            LabelBio.Text = novaBio;
        }
    }

    // Professor
    private async void OnAtividadesClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("ListarAtividadeView");

    private async void OnDuvidasClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("ListagemDuvidasView");

    // Aluno
    private async void OnAtividadesAlunoClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("ListagemAtividadeView");

    private async void OnDuvidasAlunoClicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync("ListagemDuvidasAlunoView");

    // Logout
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        Preferences.Clear();
        Application.Current.MainPage = new NavigationPage(new LoginView());
    }
}