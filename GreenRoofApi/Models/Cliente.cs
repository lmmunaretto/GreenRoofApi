using System.ComponentModel.DataAnnotations;

namespace GreenRoofApi.Models
{
    public class Cliente
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
        [Phone]
        [StringLength(15)]
        public string Telefone { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11)]
        public string Cpf { get; set; }

        [Required]
        [StringLength(255)]
        public string Endereco { get; set; }

        public virtual ICollection<Pedido> Pedidos { get; set; }

    }
}
