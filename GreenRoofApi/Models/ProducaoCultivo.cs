using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenRoofApi.Models
{
    public class ProducaoCultivo
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Produto")]
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int AdminId { get; set; }
        public Usuario Admin { get; set; }

        [Required]
        public DateTime DataProducao { get; set; } = DateTime.UtcNow;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "A quantidade produzida deve ser positiva.")]
        public int QuantidadeProduzida { get; set; }
    }
}
