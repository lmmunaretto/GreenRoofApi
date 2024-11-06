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
                Fibras = inf.Fibras,
                Calorias = inf.Calorias,
                Proteinas = inf.Proteinas,
                Carboidratos = inf.Carboidratos,
                Gorduras = inf.Gorduras,
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
                Fibras = informacao.Fibras,
                Calorias = informacao.Calorias,
                Proteinas = informacao.Proteinas,
                Carboidratos = informacao.Carboidratos,
                Gorduras = informacao.Gorduras,
                ProdutoId = informacao.ProdutoId
            };
        }

        public async Task<InformacaoNutricionalDTO> GetByProdutoIdAsync(int produtoId)
        {
            var informacao = await _context.InformacoesNutricionais.Where(x => x.ProdutoId == produtoId).FirstOrDefaultAsync();
            if (informacao == null) return null;

            return new InformacaoNutricionalDTO
            {
                Id = informacao.Id,
                Fibras = informacao.Fibras,
                Calorias = informacao.Calorias,
                Proteinas = informacao.Proteinas,
                Carboidratos = informacao.Carboidratos,
                Gorduras = informacao.Gorduras,
                ProdutoId = informacao.ProdutoId
            };
        }

        public async Task<InformacaoNutricional> CreateAsync(InformacaoNutricionalRequestDTO informacaoDTO)
        {
            var informacao = new InformacaoNutricional
            {
                Fibras = informacaoDTO.Fibras,
                Calorias = informacaoDTO.Calorias,
                Proteinas = informacaoDTO.Proteinas,
                Carboidratos = informacaoDTO.Carboidratos,
                Gorduras = informacaoDTO.Gorduras,
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

            informacao.Fibras = informacaoDTO.Fibras;
            informacao.Calorias = informacaoDTO.Calorias;
            informacao.Proteinas = informacaoDTO.Proteinas;
            informacao.Carboidratos = informacaoDTO.Carboidratos;
            informacao.Gorduras = informacaoDTO.Gorduras;
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
