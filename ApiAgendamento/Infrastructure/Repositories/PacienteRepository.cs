using ApiAgendamento.Domain.Entities;
using ApiAgendamento.Domain.Repositories;
using ApiAgendamento.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiAgendamento.Infrastructure.Repositories;

public class PacienteRepository : IPacienteRepository
{
    private readonly AppDbContext _context;
    public PacienteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Paciente?> ObterPorIdAsync(int id)
    {
        return await _context.Pacientes.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Paciente>> ObterTodosAsync()
    {
        return await _context.Pacientes.ToListAsync();
    }

    public async Task AdicionarAsync(Paciente paciente)
    {
        await _context.Pacientes.AddAsync(paciente);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Paciente paciente)
    {
        _context.Pacientes.Update(paciente);
        await _context.SaveChangesAsync();
    }
}