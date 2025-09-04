using ApiAgendamento.Domain.Repositories;
using ApiAgendamento.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAgendamento.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize] // Exige autenticação JWT
public class PacienteController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMedicoRepository _medicoRepository;

    public PacienteController(AppDbContext context, IMedicoRepository medicoRepository)
    {
        _context = context;
        _medicoRepository = medicoRepository;
    }

    [HttpGet("meus-agendamentos")]
    public async Task<IActionResult> MeusAgendamentos([FromQuery] int pacienteId)
    {
        var agendamentos = await _context.Agendamentos
            .Where(a => a.PacienteId == pacienteId)
            .Join(_context.Medicos,
                  a => a.MedicoId,
                  m => m.Id,
                  (a, m) => new {
                      a.Id,
                      a.DataHora,
                      Medico = new { m.Id, m.Nome, m.Especialidade }
                  })
            .ToListAsync();

        return Ok(agendamentos);
    }

    [HttpGet("horarios-disponiveis")]
    public async Task<IActionResult> ListarTodosHorariosDisponiveis()
    {
        var medicos = await _medicoRepository.ObterTodosAsync();
        var result = medicos.Select(m => new {
            MedicoId = m.Id,
            Nome = m.Nome,
            Especialidade = m.Especialidade,
            HorariosDisponiveis = m.HorariosDisponiveis
                .Select(h => new { h.Id, h.Inicio, h.Fim })
        });
        return Ok(result);
    }


}
