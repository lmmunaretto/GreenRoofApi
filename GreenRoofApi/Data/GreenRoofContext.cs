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
        public DbSet<InformacaoNutricional> InformacoesNutricionais { get; set; }

        // Configurações adicionais
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações para a tabela Clientes
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("clientes");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id).HasColumnName("id");
                entity.Property(c => c.Nome).HasColumnName("nome").IsRequired().HasMaxLength(100);
                entity.Property(c => c.Email).HasColumnName("email").IsRequired().HasMaxLength(100);
                entity.Property(c => c.Telefone).HasColumnName("telefone").IsRequired().HasMaxLength(15);
                entity.Property(c => c.Cpf).HasColumnName("cpf").IsRequired().HasMaxLength(11).IsFixedLength();
                entity.Property(c => c.Endereco).HasColumnName("endereco").IsRequired().HasMaxLength(255);
                entity.Property(c => c.AdminId).HasColumnName("admin_id");
            });

            // Configurações para a tabela Fornecedores
            modelBuilder.Entity<Fornecedor>(entity =>
            {
                entity.ToTable("fornecedores");
                entity.HasKey(f => f.Id);
                entity.Property(f => f.Id).HasColumnName("id");
                entity.Property(f => f.Nome).HasColumnName("nome").IsRequired().HasMaxLength(100);
                entity.Property(f => f.Telefone).HasColumnName("telefone").IsRequired().HasMaxLength(15);
                entity.Property(f => f.Email).HasColumnName("email").HasMaxLength(100);
                entity.Property(f => f.Cnpj).HasColumnName("cnpj").IsRequired().HasMaxLength(14).IsFixedLength();
                entity.Property(f => f.Endereco).HasColumnName("endereco").IsRequired().HasMaxLength(255);
                entity.Property(c => c.AdminId).HasColumnName("admin_id");
            });

            // Configurações para a tabela Produtos
            modelBuilder.Entity<Produto>(entity =>
            {
                entity.ToTable("produtos");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).HasColumnName("id");
                entity.Property(p => p.Nome).HasColumnName("nome").IsRequired().HasMaxLength(100);
                entity.Property(p => p.Descricao).HasColumnName("descricao").HasMaxLength(255);
                entity.Property(p => p.Quantidade).HasColumnName("quantidade").IsRequired().HasDefaultValue(0);
                entity.Property(p => p.Preco).HasColumnName("preco").IsRequired().HasColumnType("numeric(10,2)");
                entity.Property(p => p.Tipo).HasColumnName("tipo").IsRequired().HasMaxLength(50);
                entity.Property(p => p.LimiteMinimoEstoque).HasColumnName("limite_min_etq");
                entity.Property(p => p.FornecedorId).HasColumnName("fornecedor_id");
                entity.HasOne(p => p.Fornecedor)
                      .WithMany(f => f.Produtos)
                      .HasForeignKey(p => p.FornecedorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configurações para a tabela Pedidos
            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.ToTable("pedidos");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id).HasColumnName("id");
                entity.Property(p => p.ClienteId).HasColumnName("cliente_id");
                entity.Property(p => p.DataPedido).HasColumnName("data_pedido").IsRequired().HasDefaultValueSql("CURRENT_DATE");
                entity.Property(p => p.TotalPedido).HasColumnName("total").IsRequired().HasColumnType("numeric(10,2)");
                entity.Property(p => p.Status).HasColumnName("status").IsRequired().HasMaxLength(50).HasDefaultValue("Em Processamento");
                entity.HasOne(p => p.Cliente)
                      .WithMany(c => c.Pedidos)
                      .HasForeignKey(p => p.ClienteId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(p => p.ItemPedido)
                      .WithOne(i => i.Pedido)
                      .HasForeignKey(i => i.PedidoId);
            });

            // Configurações para a tabela ItensPedido
            modelBuilder.Entity<ItemPedido>(entity =>
            {
                entity.ToTable("itens_pedido");
                entity.HasKey(ip => ip.Id);
                entity.Property(ip => ip.Id).HasColumnName("id");
                entity.Property(ip => ip.PedidoId).HasColumnName("pedido_id");
                entity.Property(ip => ip.ProdutoId).HasColumnName("produto_id");
                entity.Property(ip => ip.Quantidade).HasColumnName("quantidade").IsRequired().HasDefaultValue(1);
                entity.Property(ip => ip.PrecoUnitario).HasColumnName("preco_unitario").IsRequired().HasColumnType("numeric(10,2)");
                entity.HasOne(ip => ip.Pedido)
                      .WithMany(p => p.ItemPedido)
                      .HasForeignKey(ip => ip.PedidoId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(ip => ip.Produto)
                      .WithMany(p => p.ItemPedido)
                      .HasForeignKey(ip => ip.ProdutoId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configurações para a tabela Pagamentos
            modelBuilder.Entity<Pagamento>(entity =>
            {
                entity.ToTable("pagamentos");
                entity.HasKey(pg => pg.Id);
                entity.Property(pg => pg.Id).HasColumnName("id");
                entity.Property(pg => pg.PedidoId).HasColumnName("pedido_id");
                entity.Property(pg => pg.MetodoPagamento).HasColumnName("metodo_pagamento").IsRequired().HasMaxLength(50);
                entity.Property(pg => pg.ValorPagamento).HasColumnName("valor_pagamento").IsRequired().HasColumnType("numeric(10,2)");
                entity.Property(pg => pg.DataPagamento).HasColumnName("data_pagamento").IsRequired().HasDefaultValueSql("CURRENT_DATE");
                entity.Property(pg => pg.StatusPagamento).HasColumnName("status_pagamento").IsRequired().HasMaxLength(50).HasDefaultValue("Pendente");
                entity.HasOne(pg => pg.Pedido)
                      .WithMany(p => p.Pagamento)
                      .HasForeignKey(pg => pg.PedidoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configurações para a tabela Usuarios
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuarios");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).HasColumnName("id");
                entity.Property(u => u.Nome).HasColumnName("nome").IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).HasColumnName("email").IsRequired().HasMaxLength(100);
                entity.Property(u => u.Senha).HasColumnName("senha").IsRequired().HasMaxLength(255);
                entity.Property(u => u.Role).HasColumnName("role").IsRequired().HasMaxLength(50);
                entity.Property(u => u.DeveTrocarSenha).HasColumnName("deve_trocar_senha");
                entity.HasMany(u => u.Clientes)
                      .WithOne(c => c.Admin)
                      .HasForeignKey(c => c.AdminId);
                entity.HasMany(u => u.Fornecedores)
                      .WithOne(f => f.Admin)
                      .HasForeignKey(f => f.AdminId);
            });

            modelBuilder.Entity<InformacaoNutricional>(entity =>
            {
                entity.ToTable("informacoes_nutricionais");
                entity.HasKey(inf => inf.Id);
                entity.Property(inf => inf.Id).HasColumnName("id");
                entity.Property(inf => inf.Fibras).HasColumnName("fibras").IsRequired().HasColumnType("numeric(10,2)");
                entity.Property(inf => inf.Calorias).HasColumnName("calorias").IsRequired().HasColumnType("numeric(10,2)");
                entity.Property(inf => inf.Proteinas).HasColumnName("proteinas").IsRequired().HasColumnType("numeric(10,2)");
                entity.Property(inf => inf.Carboidratos).HasColumnName("carboidratos").IsRequired().HasColumnType("numeric(10,2)");
                entity.Property(inf => inf.Gorduras).HasColumnName("gorduras").IsRequired().HasColumnType("numeric(10,2)");
                entity.Property(inf => inf.ProdutoId).HasColumnName("produto_id");
            });


        }
    }
}
