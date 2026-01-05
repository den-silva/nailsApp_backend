using Microsoft.EntityFrameworkCore;
using nailsApp_Backend.Models;

namespace nailsApp_Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
        public DbSet<Cliente> Clientes { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configurações do modelo Cliente
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(100);
                    
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);
                    
                entity.Property(e => e.CPF)
                    .IsRequired()
                    .HasMaxLength(11);
                    
                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValue("Ativo");
                    
                entity.Property(e => e.DataInclusao)
                    .HasDefaultValueSql("GETDATE()");
                    
                entity.Property(e => e.DataAtualizacao)
                    .HasDefaultValueSql("GETDATE()");
                    
                // Índices para melhor performance
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.CPF).IsUnique();
            });
        }
    }
}