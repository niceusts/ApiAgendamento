using ApiAgendamento.Domain.Entities;

namespace ApiAgendamento.Domain.Repositories;

public interface IMedicoRepository
{
    Task<Medico?> ObterPorIdAsync(int id);
    Task<IEnumerable<Medico>> ObterTodosAsync();
    Task AdicionarAsync(Medico medico);
    Task AtualizarAsync(Medico medico);
}