﻿namespace GreenRoofApi.DTOs
{
    public class ItemPedidoDTO
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public PedidoDTO Pedido { get; set; }
        public int ProdutoId { get; set; }
        public ProdutoDTO Produto { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }
}
