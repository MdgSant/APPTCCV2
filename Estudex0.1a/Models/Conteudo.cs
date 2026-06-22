using System;
using System.Collections.Generic;
using System.Text;

namespace Estudex0._1a.Models
{
    public class Conteudo
    {
        public int IdConteúdo {  get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Texto { get; set; } = string.Empty;
        public int? IdDisciplina { get; set; }
        public Disciplina? Disciplina { get; set; }
    }
}
