namespace GreenRoofApi.DTOs
{
    public class InformacaoNutricionalDTO
    {
        public int Id { get; set; }
        public decimal Calorias { get; set; }
        public decimal Proteinas { get; set; }
        public decimal Carboidratos { get; set; }
        public decimal Gorduras { get; set; }
        public decimal Fibras { get; set; }
        public int ProdutoId { get; set; }
    }

    public class InformacaoNutricionalRequestDTO
    {
        public decimal Calorias { get; set; }
        public decimal Proteinas { get; set; }
        public decimal Carboidratos { get; set; }
        public decimal Gorduras { get; set; }
        public decimal Fibras { get; set; }
        public int ProdutoId { get; set; }
    }
}
