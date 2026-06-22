using System;
using System.Collections.Generic;
using System.Text;
using Estudex0._1a.Models;
using System.Windows.Input;
using EstudeX.ViewModels;
using EstudeX.Services;
using Estudex0._1a.Models.Login;
using Estudex0._1a.Models.Utilizadores.Enum;
using Estudex0._1a.View.Utilizador;
using System.Diagnostics;

namespace Estudex0._1a.ViewModels.UtilizadoresViewModel
{
    public class UtilizadorViewModel : BaseViewModel
    {
        private UtilizadorService uService;

        // Commands
        public ICommand AutenticarCommand { get; set; }
        public ICommand RegistrarCommand { get; set; }
        public ICommand DirecionarCadastroCommand { get; set; }
        public ICommand EsqueceuSenhaCommand { get; set; }

        public UtilizadorViewModel()
        {
            uService = new UtilizadorService();
            InicializarCommands();
            CarregarCredenciaisSalvas();
        }

        public void InicializarCommands()
        {
            AutenticarCommand = new Command(async () => await AutenticarUtilizador());
            RegistrarCommand = new Command(async () => await RegistrarUtilizador());
            DirecionarCadastroCommand = new Command(async () => await DirecionarParaCadastro());
            EsqueceuSenhaCommand = new Command(async () => await EsqueceuSenha());
        }

        // ===== PROPRIEDADES DE LOADING =====
        private bool carregando;
        public bool Carregando
        {
            get => carregando;
            set { carregando = value; OnPropertyChanged(); }
        }

        // ===== PROPRIEDADES DE LOGIN =====
        private string email = string.Empty;
        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }

        private string senha = string.Empty;
        public string Senha
        {
            get { return senha; }
            set { senha = value; OnPropertyChanged(); }
        }

        private bool lembrarMe;
        public bool LembrarMe
        {
            get => lembrarMe;
            set { lembrarMe = value; OnPropertyChanged(); }
        }

        // ===== PROPRIEDADES DE CADASTRO =====
        private string nome = string.Empty;
        public string Nome
        {
            get { return nome; }
            set { nome = value; OnPropertyChanged(); }
        }

        private string cpf = string.Empty;
        public string Cpf
        {
            get { return cpf; }
            set { cpf = value; OnPropertyChanged(); }
        }

        private string cidade = string.Empty;
        public string Cidade
        {
            get { return cidade; }
            set { cidade = value; OnPropertyChanged(); }
        }

        private string uf = string.Empty;
        public string Uf
        {
            get { return uf; }
            set { uf = value; OnPropertyChanged(); }
        }

        private string tipoSelecionado = string.Empty;
        public string TipoSelecionado
        {
            get { return tipoSelecionado; }
            set { tipoSelecionado = value; OnPropertyChanged(); }
        }

        // ===== MÉTODOS DE LOGIN =====
        public async Task AutenticarUtilizador()
        {
            // Validações
            if (string.IsNullOrWhiteSpace(Email))
            {
                await Application.Current.MainPage
                    .DisplayAlert("Aviso", "Por favor, insira seu e-mail", "Ok");
                return;
            }

            if (string.IsNullOrWhiteSpace(Senha))
            {
                await Application.Current.MainPage
                    .DisplayAlert("Aviso", "Por favor, insira sua senha", "Ok");
                return;
            }

            try
            {
                Carregando = true;

                LoginRequest u = new LoginRequest();
                u.Email = Email;
                u.Senha = Senha;

                LoginResponse uAutenticado = await uService.PostAutenticarUtilizadorAsync(u);

                if (!string.IsNullOrEmpty(uAutenticado.Token))
                {
                    string mensagem = $"Bem-vindo(a) {uAutenticado.Nome}.";

                    // ✅ Salva credenciais
                    Preferences.Set("UtilizadorId", uAutenticado.IdUtilizador);
                    Preferences.Set("UtilizadorNome", uAutenticado.Nome);
                    Preferences.Set("UtilizadorToken", uAutenticado.Token);
                    Preferences.Set("UtilizadorTipo", uAutenticado.TipoUtilizador);

                    // ✅ Salva email se "lembrar-me" está marcado
                    if (LembrarMe)
                    {
                        SalvarCredenciais();
                    }
                    else
                    {
                        LimparCredenciais();
                    }

                    await Application.Current.MainPage
                        .DisplayAlert("Sucesso", mensagem, "Ok");

                    // ✅ Redireciona para o Shell com perfil correto
                    var shell = new AppShell();
                    shell.AplicarPerfil();
                    Application.Current.MainPage = shell;
                }
                else
                {
                    await Application.Current.MainPage
                        .DisplayAlert("Erro", "E-mail ou senha incorretos", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Erro de Autenticação",
                        $"{ex.Message}", "Ok");

                Debug.WriteLine($"Erro: {ex.InnerException}");
            }
            finally
            {
                Carregando = false;
            }
        }

        // ===== MÉTODOS DE CADASTRO =====
        public async Task RegistrarUtilizador()
        {
            try
            {
                // Validações
                if (string.IsNullOrEmpty(Nome) || string.IsNullOrEmpty(Cpf) ||
                    string.IsNullOrEmpty(Cidade) || string.IsNullOrEmpty(Uf) ||
                    string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Senha) ||
                    string.IsNullOrEmpty(TipoSelecionado))
                {
                    await Application.Current.MainPage
                        .DisplayAlert("Atenção", "Preencha todos os campos!", "Ok");
                    return;
                }

                // Validação de email
                if (!IsValidEmail(Email))
                {
                    await Application.Current.MainPage
                        .DisplayAlert("Atenção", "E-mail inválido!", "Ok");
                    return;
                }

                // Validação de CPF
                if (Cpf.Length < 11)
                {
                    await Application.Current.MainPage
                        .DisplayAlert("Atenção", "CPF inválido!", "Ok");
                    return;
                }

                Carregando = true;

                Utilizador u = new Utilizador();
                u.Nome = Nome;
                u.CPF = Cpf;
                u.Cidade = Cidade;
                u.UF = Uf;
                u.Email = Email;
                u.SenhaHash = Senha;
                u.IdTipoUtilizador = TipoSelecionado == "Professor" ? 2 : 1;

                Utilizador uRegistrado = await uService.PostRegistrarUtilizadorAsync(u);

                if (uRegistrado.IdUtilizador != 0)
                {
                    string mensagem = $"Utilizador '{uRegistrado.Nome}' registrado com sucesso!";
                    await Application.Current.MainPage
                        .DisplayAlert("Sucesso", mensagem, "Ok");

                    // Limpa os campos
                    LimparCadastro();

                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage
                        .DisplayAlert("Erro", "Não foi possível registrar o utilizador", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Erro", $"{ex.Message}", "Ok");

                Debug.WriteLine($"Erro: {ex.InnerException}");
            }
            finally
            {
                Carregando = false;
            }
        }

        // ===== MÉTODOS DE NAVEGAÇÃO =====
        public async Task DirecionarParaCadastro()
        {
            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new CadastroView());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Erro", $"{ex.Message}", "Ok");
            }
        }

        public async Task EsqueceuSenha()
        {
            try
            {
                // TODO: Implementar view de recuperação de senha
                // await Shell.Current.GoToAsync("recuperar-senha");

                await Application.Current.MainPage
                    .DisplayAlert("Recuperação de Senha",
                        "Entre em contato com o suporte para recuperar sua senha.", "Ok");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Erro", ex.Message, "Ok");
            }
        }

        // ===== MÉTODOS AUXILIARES =====
        private void SalvarCredenciais()
        {
            Preferences.Set("EmailSalvo", Email);
            Preferences.Set("LembrarMe", true);
            // IMPORTANTE: NUNCA salve a senha!
        }

        private void CarregarCredenciaisSalvas()
        {
            bool lembrarMeSalvo = Preferences.Get("LembrarMe", false);
            if (lembrarMeSalvo)
            {
                Email = Preferences.Get("EmailSalvo", string.Empty);
                LembrarMe = true;
            }
        }

        private void LimparCredenciais()
        {
            Preferences.Remove("EmailSalvo");
            Preferences.Set("LembrarMe", false);
        }

        private void LimparCadastro()
        {
            Nome = string.Empty;
            Cpf = string.Empty;
            Cidade = string.Empty;
            Uf = string.Empty;
            Email = string.Empty;
            Senha = string.Empty;
            TipoSelecionado = string.Empty;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}