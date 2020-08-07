using System.ComponentModel.DataAnnotations;

namespace AlbertEinstein.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }
        [Required]
        [MaxLength(11)]
        public string CPF { get; set; }

    }
}
