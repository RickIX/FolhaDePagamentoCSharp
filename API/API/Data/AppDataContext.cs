using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
        {

        }

        // Tabelas no banco de dados
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<FolhaPagamento> FolhasPagamento { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Defina configurações específicas, como chaves primárias, índices, relações, etc., aqui.

            // Exemplo de configuração de chave primária para Funcionario
            modelBuilder.Entity<Funcionario>()
                .HasKey(f => f.Id);

            // Exemplo de configuração de relação entre Funcionario e FolhaPagamento
            modelBuilder.Entity<FolhaPagamento>()
                .HasOne(fp => fp.Funcionario)
                .WithMany()
                .HasForeignKey(fp => fp.FuncionarioId);

            // Outras configurações específicas podem ser adicionadas conforme necessário.

            base.OnModelCreating(modelBuilder);
        }
    }
}
