using System;
using System.ComponentModel.DataAnnotations;

namespace AlbertEinstein.Models
{
    public class Consulta
    {
        public int Id { get; set; }
        [Required]
        public int PacienteId { get; set; }
        [Required]
        public int MedicoId { get; set; }
        [Required]
        public DateTime Data{ get; set; }
    }
}