using ApiAgendamento.Domain.Entities;
using ApiAgendamento.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiAgendamento.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Exige autenticação JWT
public class MedicoController : ControllerBase
{
    private readonly IMedicoRepository _medicoRepository;

    public MedicoController(IMedicoRepository medicoRepository)
    {
        _medicoRepository = medicoRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CadastrarMedico([FromBody] MedicoDto dto)
    {
        var medico = new Medico(dto.Nome, dto.Especialidade);
        await _medicoRepository.AdicionarAsync(medico);
        return Ok(medico.Id);
    }

    [HttpPost("{medicoId}/horarios")]
    public async Task<IActionResult> AdicionarHorarioDisponivel(int medicoId, [FromBody] HorarioDisponivelDto dto)
    {
        var medico = await _medicoRepository.ObterPorIdAsync(medicoId);
        if (medico == null)
            return NotFound("Médico não encontrado.");
        try
        {
            medico.AdicionarHorarioDisponivel(dto.Inicio, dto.Fim);
            await _medicoRepository.AtualizarAsync(medico);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{medicoId}/horarios")]
    public async Task<IActionResult> ListarHorariosDisponiveis(int medicoId)
    {
        var medico = await _medicoRepository.ObterPorIdAsync(medicoId);
        if (medico == null)
            return NotFound("Médico não encontrado.");
        return Ok(medico.HorariosDisponiveis);
    }
}

public class MedicoDto
{
    public string Nome { get; set; } = string.Empty;
    public string Especialidade { get; set; } = string.Empty;
}

public class HorarioDisponivelDto
{
    public DateTime Inicio { get; set; }
    public DateTime Fim { get; set; }
}
