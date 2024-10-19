using GreenRoofApi.Data;
using GreenRoofApi.DTOs;
using GreenRoofApi.Models;

namespace GreenRoofApi.Services
{
    public class FornecedorService
    {
        private readonly GreenRoofContext _context;

        public FornecedorService(GreenRoofContext context)
        {
            _context = context;
        }

        public async Task<List<FornecedorDTO>> GetAllAsync()
        {
            var fornecedores = await _context.Fornecedores.ToListAsync();
            return fornecedores.Select(f => new FornecedorDTO
            {
                Id = f.Id,
                Nome = f.Nome,
                Telefone = f.Telefone,
                Email = f.Email,
                Cnpj = f.Cnpj,
                Endereco = f.Endereco
            }).ToList();
        }

        public async Task<FornecedorDTO> GetByIdAsync(int id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor == null) return null;

            return new FornecedorDTO
            {
                Id = fornecedor.Id,
                Nome = fornecedor.Nome,
                Telefone = fornecedor.Telefone,
                Email = fornecedor.Email,
                Cnpj = fornecedor.Cnpj,
                Endereco = fornecedor.Endereco
            };
        }

        public async Task CreateAsync(FornecedorDTO fornecedorDTO)
        {
            var fornecedor = new Fornecedor
            {
                Nome = fornecedorDTO.Nome,
                Telefone = fornecedorDTO.Telefone,
                Email = fornecedorDTO.Email,
                Cnpj = fornecedorDTO.Cnpj,
                Endereco = fornecedorDTO.Endereco
            };

            _context.Fornecedores.Add(fornecedor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, FornecedorDTO fornecedorDTO)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor == null) return;

            fornecedor.Nome = fornecedorDTO.Nome;
            fornecedor.Telefone = fornecedorDTO.Telefone;
            fornecedor.Email = fornecedorDTO.Email;
            fornecedor.Cnpj = fornecedorDTO.Cnpj;
            fornecedor.Endereco = fornecedorDTO.Endereco;

            _context.Fornecedores.Update(fornecedor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor == null) return;

            _context.Fornecedores.Remove(fornecedor);
            await _context.SaveChangesAsync();
        }
    }
}
