using ApiAgendamento.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiAgendamento.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AgendamentoController : ControllerBase
{
    private readonly AgendamentoService _service;

    public AgendamentoController(AgendamentoService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CriarAgendamento(int medicoId, int pacienteId, DateTime dataHora)
    {
        try
        {
            await _service.CriarAgendamento(medicoId, pacienteId, dataHora);
            return Ok("Agendamento criado com sucesso.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}