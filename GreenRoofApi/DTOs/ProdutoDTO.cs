﻿using GreenRoofApi.Models;

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
        public int LimiteMinimoEstoque { get; set; }
        public InformacaoNutricionalDTO? InformacoesNutricionais { get; set; }
    }

    public class ProdutosRequestDTO
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
        public string Tipo { get; set; }
        public int LimiteMinimoEstoque { get; set; }
    }

}
