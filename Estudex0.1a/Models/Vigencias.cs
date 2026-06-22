using System;
using System.Collections.Generic;
using System.Text;

namespace Estudex0._1a.Models
{
    public class Vigencias
    {
        public int IdVigencia { get; set; }
        public int? NroContrato { get; set; }
        public Contrato? Contrato { get; set; }
        public int? IdUtilizador { get; set; }
        public Utilizador? Utilizador { get; set; }
        public DateTime InVigencia { get; set; }
        public DateTime FnVigencia { get; set; }
        public DateTime DcProrrogacao { get; set; }
    }
}
