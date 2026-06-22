using EstudeX.ViewModels;
using Estudex0._1a.Models;
using System.Text.Json;

namespace Estudex0._1a.ViewModels.AlunoViewModel
{
    public class ResultadoAtividadeViewModel : BaseViewModel
    {
        private ResultadoAtividade resultado;
        public ResultadoAtividade Resultado
        {
            get => resultado;
            set { resultado = value; OnPropertyChanged(); }
        }

        public string ResultadoJson
        {
            set
            {
                var json = Uri.UnescapeDataString(value);
                Resultado = JsonSerializer.Deserialize<ResultadoAtividade>(json);
            }
        }
    }
}