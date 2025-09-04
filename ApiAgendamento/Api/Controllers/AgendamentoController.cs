using ApiAgendamento.Application.Services;
using ApiAgendamento.Domain.Entities;
using ApiAgendamento.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ApiAgendamento.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AgendamentoController : ControllerBase
{
    private readonly AgendamentoService _service;
    private readonly IMedicoRepository _medicoRepository;
    private readonly IAgendamentoRepository _agendamentoRepository;

    public AgendamentoController(AgendamentoService service, IMedicoRepository medicoRepository, IAgendamentoRepository agendamentoRepository)
    {
        _service = service;
        _medicoRepository = medicoRepository;
        _agendamentoRepository = agendamentoRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CriarAgendamento(int pacienteId, int horarioId)
    {
        try
        {
            await _service.CriarAgendamento(pacienteId, horarioId);
            return Ok("Agendamento criado com sucesso.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> ListarAgendamentos()
    {
        var agendamentos = await _agendamentoRepository.ObterTodosAsync();
        var result = agendamentos.Select(a => new
        {
            a.Id,
            a.MedicoId,
            a.PacienteId,
            a.DataHora
        });
        return Ok(result);
    }

    [HttpPatch]
    public async Task<IActionResult> AtualizarAgendamento(int agendamentoId, int novoHorarioId)
    {
        try
        {
            await _service.AtualizarAgendamento(agendamentoId, novoHorarioId);
            return Ok("Agendamento atualizado com sucesso.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> CancelarAgendamento(int agendamentoId)
    {
        try
        {
            await _service.CancelarAgendamento(agendamentoId);
            return Ok("Agendamento cancelado com sucesso.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}