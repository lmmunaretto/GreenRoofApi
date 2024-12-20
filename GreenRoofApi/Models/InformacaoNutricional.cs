﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace GreenRoofApi.Models
{
    public class InformacaoNutricional
    {
        public int Id { get; set; }

        [Required]
        public decimal Calorias { get; set; }

        [Required]
        public decimal Proteinas { get; set; }

        [Required]
        public decimal Carboidratos { get; set; }

        [Required]
        public decimal Fibras { get; set; }

        [Required]
        public decimal Gorduras { get; set; }

        [ForeignKey("Produto")]
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }
    }
}
