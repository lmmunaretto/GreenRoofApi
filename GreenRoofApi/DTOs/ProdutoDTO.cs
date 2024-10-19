namespace GreenRoofApi.DTOs
{
    public class ProdutoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
        public string Tipo { get; set; }
        public int? FornecedorId { get; set; }
        public FornecedorDTO Fornecedor { get; set; }
    }
}
