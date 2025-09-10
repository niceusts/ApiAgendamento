using ApiAgendamento.Domain.Entities;
using ApiAgendamento.Domain.Repositories;
using ApiAgendamento.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiAgendamento.Api.Dtos;

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
    private readonly IPacienteRepository _pacienteRepository;

    public MedicoController(IMedicoRepository medicoRepository, IAgendamentoRepository agendamentoRepository, IPacienteRepository pacienteRepository)
    {
        _medicoRepository = medicoRepository;
        _agendamentoRepo = agendamentoRepository;
        _medicoService = new MedicoService();
        _pacienteRepository = pacienteRepository;
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
            _medicoService.RemoverHorarioDisponivel(
                medico,
                horarioId,
                (hid) => _agendamentoRepo.ExisteAgendamentoParaHorario(hid).Result
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
    [HttpGet("{medicoId}/agendas")]
    public async Task<IActionResult> ListarAgendasDoMedico(int medicoId)
    {
        var medico = await _medicoRepository.ObterPorIdAsync(medicoId);
        if (medico == null)
            return NotFound("Médico não encontrado.");

        var agendamentos = await _agendamentoRepo.ObterTodosAsync();
        var agendasDoMedico = agendamentos.Where(a => a.MedicoId == medicoId).ToList();

        // Buscar todos os pacientes envolvidos nesses agendamentos
        var pacienteIds = agendasDoMedico.Select(a => a.PacienteId).Distinct().ToList();

        var pacientes = await _pacienteRepository.ObterPorIdsAsyncList(pacienteIds);

        var result = agendasDoMedico.Select(a => new
        {
            a.Id,
            a.DataHora,
            PacienteId = a.PacienteId,
            PacienteNome = pacientes.FirstOrDefault(p => p.Id == a.PacienteId)?.Nome
        });

        return Ok(result);
    }
}
