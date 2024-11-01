using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenRoofApi.Models
{
    public class Pagamento
    {
        public int Id { get; set; }

        [ForeignKey("Pedido")]
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }

        [Required]
        [StringLength(50)]
        public string MetodoPagamento { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal ValorPagamento { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DataPagamento { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string StatusPagamento { get; set; } = "Pendente";
    }
}
