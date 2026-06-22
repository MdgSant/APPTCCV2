namespace Estudex0._1a;
public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        #region Rotas Menu
        Routing.RegisterRoute("HomeView", typeof(Estudex0._1a.View.HomeView));
        #endregion
        #region Rotas Atividade
        Routing.RegisterRoute("ListarAtividadeView",
            typeof(Estudex0._1a.View.ProfessorViews.AtividadeView));
        Routing.RegisterRoute("InserirAtividadeView",
            typeof(Estudex0._1a.View.ProfessorViews.InserirAtividadeView));
        Routing.RegisterRoute("InserirPerguntasView",
            typeof(Estudex0._1a.View.ProfessorViews.InserirPerguntaView));
        Routing.RegisterRoute("DetalhesAtividadeView",
            typeof(Estudex0._1a.View.ProfessorViews.DetalhesAtividadeView));
        Routing.RegisterRoute("ListagemAtividadeView",
           typeof(Estudex0._1a.View.AlunoViews.ListagemAtividadeAlunoView));
        Routing.RegisterRoute("ResponderAtividadeView",
            typeof(Estudex0._1a.View.AlunoViews.ListagemAtividadeView));
        Routing.RegisterRoute("RespostasAtividadeView",
            typeof(Estudex0._1a.View.ProfessorViews.RespostasAtividadeView));
        Routing.RegisterRoute("MinhasRespostasView",
            typeof(Estudex0._1a.View.AlunoViews.MinhasRespostasView));
        Routing.RegisterRoute("ResultadoAtividadeView",
            typeof(Estudex0._1a.View.AlunoViews.ResultadoAtividadeView));
        #endregion

        #region Rotas Duvidas
        Routing.RegisterRoute("ListagemDuvidasView",
            typeof(Estudex0._1a.View.ProfessorViews.DuvidaView));
        Routing.RegisterRoute("InserirDuvidaView",
            typeof(Estudex0._1a.View.AlunoViews.InserirDuvidaView));
        Routing.RegisterRoute("ListagemDuvidasAlunoView",
            typeof(Estudex0._1a.View.AlunoViews.ListagemDuvidaAlunoView));
        Routing.RegisterRoute("DetalheDuvidaAlunoView",
            typeof(Estudex0._1a.View.AlunoViews.DetalheDuvidaAlunoView));
        Routing.RegisterRoute("DetalheDuvidaProfessorView",
            typeof(Estudex0._1a.View.ProfessorViews.DetalheDuvidaProfessorView));
        #endregion
    }

    public void AplicarPerfil()
    {
        int tipo = Preferences.Get("UtilizadorTipo", 0);
        string nome = Preferences.Get("UtilizadorNome", string.Empty);

        bool isProfessor = tipo == 2;
        bool isAluno = tipo == 1;

        LabelPerfil.Text = isProfessor ? $"Professor | {nome}" : $"Aluno | {nome}";

        FlyoutMenu.IsVisible = true;
        FlyoutProfAtividades.IsVisible = isProfessor;
        FlyoutProfDuvidas.IsVisible = isProfessor;
        FlyoutAlunoAtividades.IsVisible = isAluno;
        FlyoutAlunoDuvidas.IsVisible = isAluno;
        FlyoutAlunoRespostas.IsVisible = isAluno;
    }
}