using System.Text.Json.Serialization;

namespace GreenRoofApi.DTOs
{
    public class ItemPedidoDTO
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int ProdutoId { get; set; }
        public string? ProdutoNome { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }


    public class ItensPedidosRequestDTO
    {
        public int PedidoId { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }
}
