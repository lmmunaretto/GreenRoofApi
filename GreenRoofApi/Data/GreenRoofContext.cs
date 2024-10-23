using GreenRoofApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenRoofApi.Data
{
    public class GreenRoofContext : DbContext
    {
        public GreenRoofContext(DbContextOptions<GreenRoofContext> options) : base(options) { }

        // Definição das tabelas no contexto
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedidos { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        // Configurações adicionais
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tabela Clientes: Configuração do CPF para ser fixo com 11 caracteres
            modelBuilder.Entity<Cliente>()
                .Property(c => c.Cpf)
                .HasMaxLength(11)
                .IsFixedLength()
                .IsRequired();

            // Tabela Fornecedores: Configuração do CNPJ para ser fixo com 14 caracteres
            modelBuilder.Entity<Fornecedor>()
                .Property(f => f.Cnpj)
                .HasMaxLength(14)
                .IsFixedLength()
                .IsRequired();

            // Tabela Produtos: Configura a relação com Fornecedores
            modelBuilder.Entity<Produto>()
                .HasOne(p => p.Fornecedor)
                .WithMany(f => f.Produtos)
                .HasForeignKey(p => p.FornecedorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Tabela Pedidos: Relacionamento com Clientes
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Tabela ItensPedido: Relacionamento com Pedidos e Produtos
            modelBuilder.Entity<ItemPedido>()
                .HasOne(ip => ip.Pedido)
                .WithMany(p => p.ItemPedido)
                .HasForeignKey(ip => ip.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemPedido>()
                .HasOne(ip => ip.Produto)
                .WithMany(p => p.ItemPedido)
                .HasForeignKey(ip => ip.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Tabela Pagamentos: Relacionamento com Pedidos
            modelBuilder.Entity<Pagamento>()
                .HasOne(pg => pg.Pedido)
                .WithMany(p => p.Pagamento)
                .HasForeignKey(pg => pg.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Tabela Usuarios: Definir tamanho máximo para senha e email
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Email)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Senha)
                .HasMaxLength(100)
                .IsRequired();

            // Configurações gerais de mapeamento de tabelas
            modelBuilder.Entity<Cliente>().ToTable("clientes");
            modelBuilder.Entity<Fornecedor>().ToTable("fornecedores");
            modelBuilder.Entity<Produto>().ToTable("produtos");
            modelBuilder.Entity<Pedido>().ToTable("pedidos");
            modelBuilder.Entity<ItemPedido>().ToTable("itens_pedido");
            modelBuilder.Entity<Pagamento>().ToTable("pagamentos");
            modelBuilder.Entity<Usuario>().ToTable("usuarios");
        }
    }
}
