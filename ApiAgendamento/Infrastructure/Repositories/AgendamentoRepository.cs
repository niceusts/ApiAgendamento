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
    public async Task<List<Agendamento>> ObterTodosAsync()
    {
        return await _context.Agendamentos.ToListAsync();
    }

    public async Task<Agendamento?> ObterAgendaId(int id)
    {
        return await _context.Agendamentos.FirstOrDefaultAsync(a => a.Id == id);
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

    public async Task AtualizarAsync(Agendamento agendamento)
    {
        _context.Agendamentos.Update(agendamento);
        await _context.SaveChangesAsync();
    }

    // buscar HorarioDisponivel por Id
    public async Task<HorarioDisponivel?> ObterHorarioDisponivelPorIdAsync(int horarioId)
    {
        return await _context.HorarioDisponiveis.FirstOrDefaultAsync(h => h.Id == horarioId);
    }
   
    public async Task DeleteAsync(Agendamento agendamento)
    {
        _context.Agendamentos.Remove(agendamento);
        await _context.SaveChangesAsync();
    }
}