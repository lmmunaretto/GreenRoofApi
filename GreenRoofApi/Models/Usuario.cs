using System.ComponentModel.DataAnnotations;

namespace GreenRoofApi.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; }

    }
}
