using GreenRoofApi.DTOs;
using System.ComponentModel.DataAnnotations;

namespace GreenRoofApi.Models
{
    public class Fornecedor
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [Phone]
        [StringLength(15)]
        public string Telefone { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(14, MinimumLength = 14)]
        public string Cnpj { get; set; }

        [Required]
        [StringLength(255)]
        public string Endereco { get; set; }

        // Adicione esta propriedade para representar o relacionamento com produtos

        public int AdminId { get; set; }
        public Usuario Admin { get; set; }
        public virtual ICollection<Produto> Produtos { get; set; }

        public static explicit operator Fornecedor(FornecedorDTO v)
        {
            throw new NotImplementedException();
        }
    }
}
