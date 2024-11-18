namespace GreenRoofApi.DTOs
{
    public class ProducaoCultivoDTO
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public int AdminId { get; set; }
        public DateTime DataProducao { get; set; }
        public int QuantidadeProduzida { get; set; }
    }

    public class ProducaoCultivoRequestDTO
    {
        public int ProdutoId { get; set; }
        public DateTime DataProducao { get; set; }
        public int QuantidadeProduzida { get; set; }
    }
}
