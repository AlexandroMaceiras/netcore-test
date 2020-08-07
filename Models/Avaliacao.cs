using System;
using System.ComponentModel.DataAnnotations;

namespace AlbertEinstein.Models
{
    public class Avaliacao
    {
        public int Id { get; set; }
        [Required]
        public int PacienteId { get; set; }
        [Required]
        public int NotaTempoDemora{ get; set; }
        [Required]
        public int NotaAtendimentoOnLine{ get; set; }
        [Required]
        public int NotaAtendimentoFone{ get; set; }
        [Required]
        public int NotaPorEspera{ get; set; }
        [Required]
        public int NotaConcorrencia{ get; set; }
    }
}