using System;
using System.Collections.Generic;
using System.Text;

namespace Estudex0._1a.Models
{
    public class Contrato
    {
        public int nroContrato { get; set; }
        public DateTime dcContrato { get; set; }
        public DateTime daContrato { get; set; }
        public string StatusContrato { get; set; }
        public string UtilizadorResponsavel { get; set; }
    }
}
