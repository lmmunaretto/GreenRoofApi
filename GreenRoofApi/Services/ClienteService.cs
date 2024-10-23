using GreenRoofApi.Data;
using GreenRoofApi.DTOs;
using GreenRoofApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenRoofApi.Services
{
    public class ClienteService
    {
        private readonly GreenRoofContext _context;

        public ClienteService(GreenRoofContext context)
        {
            _context = context;
        }

        public async Task<List<ClienteDTO>> GetAllAsync()
        {
            var clientes = await _context.Clientes.ToListAsync();
            return clientes.Select(c => new ClienteDTO
            {
                Id = c.Id,
                Nome = c.Nome,
                Email = c.Email,
                Telefone = c.Telefone,
                Cpf = c.Cpf,
                Endereco = c.Endereco
            }).ToList();
        }

        public async Task<ClienteDTO> GetByIdAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return null;

            return new ClienteDTO
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Email = cliente.Email,
                Telefone = cliente.Telefone,
                Cpf = cliente.Cpf,
                Endereco = cliente.Endereco
            };
        }

        public async Task CreateAsync(ClienteDTO clienteDTO)
        {
            var cliente = new Cliente
            {
                Nome = clienteDTO.Nome,
                Email = clienteDTO.Email,
                Telefone = clienteDTO.Telefone,
                Cpf = clienteDTO.Cpf,
                Endereco = clienteDTO.Endereco
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, ClienteDTO clienteDTO)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return;

            cliente.Nome = clienteDTO.Nome;
            cliente.Email = clienteDTO.Email;
            cliente.Telefone = clienteDTO.Telefone;
            cliente.Cpf = clienteDTO.Cpf;
            cliente.Endereco = clienteDTO.Endereco;

            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return;

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
        }
    }
}
