using System;
using System.Collections.Generic;
using System.Text;

namespace Estudex0._1a.Models.Utilizadores
{
    public class Aluno : Utilizador
    {
        public int IdSerie { get; set; }
        public int? Xp {  get; set; }
        public Serie? Serie { get; set; }
    }
}
