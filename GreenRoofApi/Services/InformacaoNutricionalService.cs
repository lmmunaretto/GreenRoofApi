using GreenRoofApi.Data;
using GreenRoofApi.DTOs;
using GreenRoofApi.Models;
using Microsoft.EntityFrameworkCore;


namespace GreenRoofApi.Services
{
    public class InformacaoNutricionalService
    {
        private readonly GreenRoofContext _context;

        public InformacaoNutricionalService(GreenRoofContext context)
        {
            _context = context;
        }

        public async Task<List<InformacaoNutricionalDTO>> GetAllAsync()
        {
            var informacoes = await _context.InformacoesNutricionais.ToListAsync();
            return informacoes.Select(inf => new InformacaoNutricionalDTO
            {
                Id = inf.Id,
                NomeProduto = inf.NomeProduto,
                Calorias = inf.Calorias,
                Proteinas = inf.Proteinas,
                Carboidratos = inf.Carboidratos,
                GordurasTotais = inf.GordurasTotais,
                ProdutoId = inf.ProdutoId
            }).ToList();
        }

        public async Task<InformacaoNutricionalDTO> GetByIdAsync(int id)
        {
            var informacao = await _context.InformacoesNutricionais.FindAsync(id);
            if (informacao == null) return null;

            return new InformacaoNutricionalDTO
            {
                Id = informacao.Id,
                NomeProduto = informacao.NomeProduto,
                Calorias = informacao.Calorias,
                Proteinas = informacao.Proteinas,
                Carboidratos = informacao.Carboidratos,
                GordurasTotais = informacao.GordurasTotais,
                ProdutoId = informacao.ProdutoId
            };
        }

        public async Task<InformacaoNutricional> CreateAsync(InformacaoNutricionalRequestDTO informacaoDTO)
        {
            var informacao = new InformacaoNutricional
            {
                NomeProduto = informacaoDTO.NomeProduto,
                Calorias = informacaoDTO.Calorias,
                Proteinas = informacaoDTO.Proteinas,
                Carboidratos = informacaoDTO.Carboidratos,
                GordurasTotais = informacaoDTO.GordurasTotais,
                ProdutoId = informacaoDTO.ProdutoId
            };

            _context.InformacoesNutricionais.Add(informacao);
            await _context.SaveChangesAsync();

            return informacao;
        }

        public async Task<InformacaoNutricional> UpdateAsync(int id, InformacaoNutricionalDTO informacaoDTO)
        {
            var informacao = await _context.InformacoesNutricionais.FindAsync(id);
            if (informacao == null)
            {
                return null;
            }

            informacao.NomeProduto = informacaoDTO.NomeProduto;
            informacao.Calorias = informacaoDTO.Calorias;
            informacao.Proteinas = informacaoDTO.Proteinas;
            informacao.Carboidratos = informacaoDTO.Carboidratos;
            informacao.GordurasTotais = informacaoDTO.GordurasTotais;
            informacao.ProdutoId = informacaoDTO.ProdutoId;

            _context.InformacoesNutricionais.Update(informacao);
            await _context.SaveChangesAsync();

            return informacao;
        }

        public async Task DeleteAsync(int id)
        {
            var informacao = await _context.InformacoesNutricionais.FindAsync(id);
            if (informacao == null) return;

            _context.InformacoesNutricionais.Remove(informacao);
            await _context.SaveChangesAsync();
        }
    }
}
