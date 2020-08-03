using System.ComponentModel.DataAnnotations;

namespace AlbertEinstein.Models
{
    public class Medico
    {
        public int Id { get; set; } 
        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }
        [Required]
        [MaxLength(100)]
        public string Local{ get; set; }

    }
}