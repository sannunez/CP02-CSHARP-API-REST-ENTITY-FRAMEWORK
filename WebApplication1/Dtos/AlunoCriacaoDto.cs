using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos;

public class AlunoCriacaoDto
{
    [Required]
    [StringLength(120, MinimumLength = 3)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(120)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(20, MinimumLength = 3)]
    public string Matricula { get; set; } = string.Empty;

    [Required]
    [StringLength(80, MinimumLength = 2)]
    public string Curso { get; set; } = string.Empty;

    [Range(1, 12)]
    public int Periodo { get; set; }

    [Required]
    public DateOnly? DataNascimento { get; set; }
}
