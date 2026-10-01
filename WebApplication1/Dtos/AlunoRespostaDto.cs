namespace WebApplication1.Dtos;

public class AlunoRespostaDto
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Matricula { get; set; } = string.Empty;

    public string Curso { get; set; } = string.Empty;

    public int Periodo { get; set; }

    public DateOnly DataNascimento { get; set; }

    public bool Ativo { get; set; }

    public DateTime CriadoEm { get; set; }
}
