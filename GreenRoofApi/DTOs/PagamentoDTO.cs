namespace GreenRoofApi.DTOs
{
    public class PagamentoDTO
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public PedidoDTO Pedido { get; set; }
        public string MetodoPagamento { get; set; }
        public decimal ValorPagamento { get; set; }
        public DateTime DataPagamento { get; set; }
        public string StatusPagamento { get; set; }
    }
}
