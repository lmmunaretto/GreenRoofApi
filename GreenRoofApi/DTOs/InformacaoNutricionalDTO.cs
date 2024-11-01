namespace GreenRoofApi.DTOs
{
    public class InformacaoNutricionalDTO
    {
        public int Id { get; set; }
        public string NomeProduto { get; set; }
        public decimal Calorias { get; set; }
        public decimal Proteinas { get; set; }
        public decimal Carboidratos { get; set; }
        public decimal GordurasTotais { get; set; }
        public int ProdutoId { get; set; }
    }

    public class InformacaoNutricionalRequestDTO
    {
        public string NomeProduto { get; set; }
        public decimal Calorias { get; set; }
        public decimal Proteinas { get; set; }
        public decimal Carboidratos { get; set; }
        public decimal GordurasTotais { get; set; }
        public int ProdutoId { get; set; }
    }
}
