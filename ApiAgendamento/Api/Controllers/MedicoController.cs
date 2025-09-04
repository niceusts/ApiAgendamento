using ApiAgendamento.Domain.Entities;
using ApiAgendamento.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiAgendamento.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize] // Exige autenticação JWT
public class MedicoController : ControllerBase
{
    private readonly IMedicoRepository _medicoRepository;
    private readonly IAgendamentoRepository _agendamentoRepo;

    public MedicoController(IMedicoRepository medicoRepository, IAgendamentoRepository agendamentoRepository)
    {
        _medicoRepository = medicoRepository;
        _agendamentoRepo = agendamentoRepository;
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
            return Ok("Horário adicionado com sucesso");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{medicoId}/horarios/{horarioId}/remover")]
    public async Task<IActionResult> RemoverHorarioDisponivel(int medicoId, int horarioId)
    {
        var medico = await _medicoRepository.ObterPorIdAsync(medicoId);
        if (medico == null)
            return NotFound("Médico não encontrado.");

        var agenda = await _agendamentoRepo.ObterAgendaId(horarioId);
        if (agenda != null)
            return BadRequest("Não é possível remover um horário que já possui agendamentos.");

        try
        {
            medico.RemoverHorarioDisponivel(medicoId, horarioId);
            await _medicoRepository.AtualizarAsync(medico);
            return Ok("Horário removido com sucesso.");
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

    [HttpPatch("{medicoId}/horarios/{horarioId}")]
    public async Task<IActionResult> AtualizarHorario(int medicoId, int horarioId, [FromBody] HorarioDisponivelDto dto)
    {
        var medico = await _medicoRepository.ObterPorIdAsync(medicoId);
        if (medico == null)
            return NotFound("Médico não encontrado.");
        try
        {
            medico.AtualizarHorario(medicoId, horarioId, dto.Inicio, dto.Fim);
            await _medicoRepository.AtualizarAsync(medico);
            return Ok("Horário atualizado com sucesso.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
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
