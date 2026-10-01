using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Aluno
{
    public int Id { get; set; }

    [Required]
    [MaxLength(120)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Matricula { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string Curso { get; set; } = string.Empty;

    [Range(1, 12)]
    public int Periodo { get; set; }

    public DateOnly DataNascimento { get; set; }

    public bool Ativo { get; set; } = true;

    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}
