using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Aluno> Alunos => Set<Aluno>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Aluno>(entity =>
        {
            entity.ToTable("alunos");
            entity.HasKey(aluno => aluno.Id);

            entity.Property(aluno => aluno.Nome).HasMaxLength(120).IsRequired();
            entity.Property(aluno => aluno.Email).HasMaxLength(120).IsRequired();
            entity.Property(aluno => aluno.Matricula).HasMaxLength(20).IsRequired();
            entity.Property(aluno => aluno.Curso).HasMaxLength(80).IsRequired();
            entity.Property(aluno => aluno.Periodo).IsRequired();
            entity.Property(aluno => aluno.DataNascimento).IsRequired();
            entity.Property(aluno => aluno.Ativo).IsRequired();
            entity.Property(aluno => aluno.CriadoEm).IsRequired();

            entity.HasIndex(aluno => aluno.Email).IsUnique();
            entity.HasIndex(aluno => aluno.Matricula).IsUnique();
        });
    }
}
