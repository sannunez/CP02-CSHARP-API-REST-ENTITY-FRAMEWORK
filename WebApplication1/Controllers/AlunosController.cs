using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/alunos")]
public class AlunosController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AlunoRespostaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AlunoRespostaDto>>> Listar()
    {
        var alunos = await context.Alunos.AsNoTracking().OrderBy(aluno => aluno.Nome).Select(aluno => ToRespostaDto(aluno)).ToListAsync();

        return Ok(alunos);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AlunoRespostaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AlunoRespostaDto>> BuscarPorId(int id)
    {
        var aluno = await context.Alunos.AsNoTracking().FirstOrDefaultAsync(aluno => aluno.Id == id);

        if (aluno is null)
        {
            return NotFound();
        }

        return Ok(ToRespostaDto(aluno));
    }

    [HttpPost]
    [ProducesResponseType(typeof(AlunoRespostaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AlunoRespostaDto>> Criar(AlunoCriacaoDto dto)
    {
        if (await ExisteEmailOuMatricula(dto.Email, dto.Matricula))
        {
            return BadRequest("Ja existe aluno cadastrado com este email ou matricula.");
        }

        var aluno = new Aluno
        {
            Nome = dto.Nome.Trim(),
            Email = dto.Email.Trim(),
            Matricula = dto.Matricula.Trim(),
            Curso = dto.Curso.Trim(),
            Periodo = dto.Periodo,
            DataNascimento = dto.DataNascimento!.Value
        };

        context.Alunos.Add(aluno);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(BuscarPorId), new { id = aluno.Id, version = "1" }, ToRespostaDto(aluno));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, AlunoAtualizacaoDto dto)
    {
        var aluno = await context.Alunos.FirstOrDefaultAsync(aluno => aluno.Id == id);

        if (aluno is null)
        {
            return NotFound();
        }

        if (await ExisteEmailOuMatricula(dto.Email, dto.Matricula, id))
        {
            return BadRequest("Ja existe outro aluno cadastrado com este email ou matricula.");
        }

        aluno.Nome = dto.Nome.Trim();
        aluno.Email = dto.Email.Trim();
        aluno.Matricula = dto.Matricula.Trim();
        aluno.Curso = dto.Curso.Trim();
        aluno.Periodo = dto.Periodo;
        aluno.DataNascimento = dto.DataNascimento!.Value;
        aluno.Ativo = dto.Ativo;

        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(int id)
    {
        var aluno = await context.Alunos.FirstOrDefaultAsync(aluno => aluno.Id == id);

        if (aluno is null)
        {
            return NotFound();
        }

        context.Alunos.Remove(aluno);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> ExisteEmailOuMatricula(string email, string matricula, int? ignorarId = null)
    {
        var emailNormalizado = email.Trim();
        var matriculaNormalizada = matricula.Trim();

        return await context.Alunos.AnyAsync(aluno =>
            (!ignorarId.HasValue || aluno.Id != ignorarId.Value)
            && (aluno.Email == emailNormalizado || aluno.Matricula == matriculaNormalizada));
    }

    private static AlunoRespostaDto ToRespostaDto(Aluno aluno)
    {
        return new AlunoRespostaDto
        {
            Id = aluno.Id,
            Nome = aluno.Nome,
            Email = aluno.Email,
            Matricula = aluno.Matricula,
            Curso = aluno.Curso,
            Periodo = aluno.Periodo,
            DataNascimento = aluno.DataNascimento,
            Ativo = aluno.Ativo,
            CriadoEm = aluno.CriadoEm
        };
    }
}
