using ApiAgendamento.Domain.Entities;
using ApiAgendamento.Domain.Repositories;
using ApiAgendamento.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiAgendamento.Infrastructure.Repositories;

public class AgendamentoRepository : IAgendamentoRepository
{
    private readonly AppDbContext _context;
    public AgendamentoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExisteConflito(int medicoId, DateTime dataHora)
    {
        return await _context.Agendamentos.AnyAsync(a => a.MedicoId == medicoId && a.DataHora == dataHora);
    }

    public async Task AdicionarAsync(Agendamento agendamento)
    {
        await _context.Agendamentos.AddAsync(agendamento);
        await _context.SaveChangesAsync();
    }
}