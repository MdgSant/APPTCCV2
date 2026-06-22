using Estudex0._1a.Models.Utilizadores;
using System;
using System.Collections.Generic;
using System.Text;


namespace Estudex0._1a.Models
{
    public class Serie
    {   
        public int idSerie {  get; set; }
        public DateOnly Inicio { get; set; } //vê depois
        public string NomeSerie { get; set; } = string.Empty;
        public List<Aluno> Alunos { get; set; } = new ();
        public List<Comunicado> Comunicados { get; set; } = new ();
    }
}
