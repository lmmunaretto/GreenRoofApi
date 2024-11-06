using System.Text.Json.Serialization;

namespace GreenRoofApi.DTOs
{
    public class PedidoDTO
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        [JsonIgnore]
        public ClienteDTO? Cliente { get; set; }
        public DateTime DataPedido { get; set; }
        public decimal TotalPedido { get; set; }
        public string Status { get; set; }
        public List<ItemPedidoDTO> ItensPedido { get; set; }
    }

    public class PedidosRequestDTO
    {
        public int ClienteId { get; set; }
        public DateTime DataPedido { get; set; }
        public decimal TotalPedido { get; set; }
        public string Status { get; set; }
        public List<ItemPedidoDTO>? ItensPedido { get; set; }
    }

}
