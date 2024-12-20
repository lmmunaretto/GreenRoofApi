﻿using GreenRoofApi.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenRoofApi.Models
{
    public class Produto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        public int LimiteMinimoEstoque { get; set; }

        [StringLength(255)]
        public string Descricao { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantidade { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Preco { get; set; }

        [Required]
        [StringLength(50)]
        public string Tipo { get; set; }

        [ForeignKey("Usuario")]
        public int AdminId { get; set; }
        public Usuario Admin { get; set; }

        public virtual ICollection<ItemPedido> ItemPedido { get; set; }
        public virtual InformacaoNutricional InformacoesNutricionais { get; set; }

    }
}
