namespace GreenRoofApi.DTOs
{
    public class PedidoDTO
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public ClienteDTO Cliente { get; set; }
        public DateTime DataPedido { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public List<ItemPedidoDTO> ItensPedido { get; set; }
    }
}
