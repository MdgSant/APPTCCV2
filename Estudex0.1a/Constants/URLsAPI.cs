using System;
using System.Collections.Generic;
using System.Text;

namespace Estudex0._1a.Constants
{
    public class URLsAPI
    {
        private const string BaseUrl = "https://estudex-appapi.azurewebsites.net";
        //private const string BaseUrl = "http://10.0.2.2:8080";
        // const string BaseUrl = "http://localhost:8080";

        public const string Autenticar = $"{Utilizadores}/autenticar";
        public const string Registrar = $"{Utilizadores}/registrar";
        public const string Atividades = $"{BaseUrl}/atividades";
        public const string NivelDificuldade = $"{BaseUrl}/niveldificuldade";
        public const string Utilizadores = $"{BaseUrl}/utilizadores";
        public const string Disciplinas = $"{BaseUrl}/disciplinas";
        public const string AtividadesPergunta = $"{BaseUrl}/atividadesPergunta";
        public const string Duvidas = $"{BaseUrl}/duvidas";
        public const string RespostasDuvidas = $"{BaseUrl}/respostasDuvidas";
        public const string AtividadesRespostas = $"{BaseUrl}/atividadesrespostas";
        public const string VerificarResposta = $"{BaseUrl}/atividadesrespostas/verificar";
    }
}
