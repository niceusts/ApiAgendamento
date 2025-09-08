

using ApiAgendamento.Domain.Entities;
using ApiAgendamento.Domain.Repositories;
using ApiAgendamento.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiAgendamento.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize]

/// <summary>
/// Controller responsável pelas operações de horários disponíveis do médico.
/// </summary>
public class MedicoController : ControllerBase
{
    private readonly IMedicoRepository _medicoRepository;
    private readonly IAgendamentoRepository _agendamentoRepo;
    private readonly MedicoService _medicoService;


    public MedicoController(IMedicoRepository medicoRepository, IAgendamentoRepository agendamentoRepository)
    {
        _medicoRepository = medicoRepository;
        _agendamentoRepo = agendamentoRepository;
        _medicoService = new MedicoService();
    }

    /// <summary>
    /// Adiciona um novo horário disponível para o médico informado.
    /// </summary>
    /// <param name="medicoId">Id do médico</param>
    /// <param name="dto">Dados do horário (início e fim)</param>
    /// <returns>Mensagem de sucesso ou erro</returns>
    [HttpPost("{medicoId}/horarios")]
    public async Task<IActionResult> AdicionarHorarioDisponivel(int medicoId, [FromBody] HorarioDisponivelDto dto)
    {
        var medico = await _medicoRepository.ObterPorIdAsync(medicoId);
        if (medico == null)
            return NotFound("Médico não encontrado.");
        try
        {
            _medicoService.AdicionarHorarioDisponivel(medico, dto.Inicio, dto.Fim);
            await _medicoRepository.AtualizarAsync(medico);
            return Ok("Horário adicionado com sucesso");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Remove um horário disponível do médico, se não houver agendamento vinculado.
    /// </summary>
    /// <param name="medicoId">Id do médico</param>
    /// <param name="horarioId">Id do horário a remover</param>
    /// <returns>Mensagem de sucesso ou erro</returns>
    [HttpDelete("{medicoId}/horarios/{horarioId}/remover")]
    public async Task<IActionResult> RemoverHorarioDisponivel(int medicoId, int horarioId)
    {
        var medico = await _medicoRepository.ObterPorIdAsync(medicoId);
        if (medico == null)
            return NotFound("Médico não encontrado.");

        try
        {
            // Passa função que verifica se existe agendamento vinculado ao horário usando ObterHorarioDisponivelPorIdAsync
            _medicoService.RemoverHorarioDisponivel(
                medico,
                horarioId,
                (hid) => _agendamentoRepo.ObterHorarioDisponivelPorIdAsync(hid).Result != null
            );
            await _medicoRepository.AtualizarAsync(medico);
            return Ok("Horário removido com sucesso.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Lista todos os horários disponíveis do médico.
    /// </summary>
    /// <param name="medicoId">Id do médico</param>
    /// <returns>Lista de horários disponíveis</returns>
    [HttpGet("{medicoId}/horarios")]
    public async Task<IActionResult> ListarHorariosDisponiveis(int medicoId)
    {
        var medico = await _medicoRepository.ObterPorIdAsync(medicoId);
        if (medico == null)
            return NotFound("Médico não encontrado.");
        return Ok(medico.HorariosDisponiveis);
    }

    /// <summary>
    /// Atualiza um horário disponível do médico.
    /// </summary>
    /// <param name="medicoId">Id do médico</param>
    /// <param name="horarioId">Id do horário</param>
    /// <param name="dto">Novos dados do horário</param>
    /// <returns>Mensagem de sucesso ou erro</returns>
    [HttpPatch("{medicoId}/horarios/{horarioId}")]
    public async Task<IActionResult> AtualizarHorario(int medicoId, int horarioId, [FromBody] HorarioDisponivelDto dto)
    {
        var medico = await _medicoRepository.ObterPorIdAsync(medicoId);
        if (medico == null)
            return NotFound("Médico não encontrado.");
        try
        {
            _medicoService.AtualizarHorario(medico, horarioId, dto.Inicio, dto.Fim);
            await _medicoRepository.AtualizarAsync(medico);
            return Ok("Horário atualizado com sucesso.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
/// <summary>
/// DTO para receber dados de horário disponível (início e fim).
/// </summary>
public class HorarioDisponivelDto
{
    /// <summary>
    /// Data/hora de início do horário disponível.
    /// </summary>
    public DateTime Inicio { get; set; }
    /// <summary>
    /// Data/hora de fim do horário disponível.
    /// </summary>
    public DateTime Fim { get; set; }
}