using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GreenRoofApi.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        [ForeignKey("Cliente")]
        public int? ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DataPedido { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Total { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Em Processamento";

        // Adicione esta propriedade para representar o relacionamento com produtos
        public virtual ICollection<ItemPedido> ItemPedido { get; set; }
        public virtual ICollection<Pagamento> Pagamento { get; set; }
    }
}
