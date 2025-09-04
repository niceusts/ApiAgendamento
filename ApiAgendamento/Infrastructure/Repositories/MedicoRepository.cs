using ApiAgendamento.Domain.Entities;
using ApiAgendamento.Domain.Repositories;
using ApiAgendamento.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiAgendamento.Infrastructure.Repositories;

public class MedicoRepository : IMedicoRepository
{
    private readonly AppDbContext _context;
    public MedicoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Medico?> ObterPorIdAsync(int id)
    {
        return await _context.Medicos
            .Include(m => m.HorariosDisponiveis)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Medico>> ObterTodosAsync()
    {
        return await _context.Medicos.Include(m => m.HorariosDisponiveis).ToListAsync();
    }

    public async Task AdicionarAsync(Medico medico)
    {
        await _context.Medicos.AddAsync(medico);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Medico medico)
    {
        _context.Medicos.Update(medico);
        await _context.SaveChangesAsync();
    }
}