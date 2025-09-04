using ApiAgendamento.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAgendamento.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Exige autenticação JWT
public class PacienteController : ControllerBase
{
    private readonly AppDbContext _context;

    public PacienteController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("meus-agendamentos")] // /api/paciente/meus-agendamentos?pacienteId=1
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
}
