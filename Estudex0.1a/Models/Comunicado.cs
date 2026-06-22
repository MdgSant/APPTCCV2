using Estudex0._1a.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Estudex0._1a.Models
{
     public class Comunicado
    {
        public int IdComunicado { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateOnly DataEnvio { get; set; }
        public string UtilizadorResponsavel { get; set; } = string.Empty;
        public DateTime DataPublicacao { get; set; }
        public int? IdSerie { get; set; }    
        public Serie? Serie { get; set; }
    }
}
