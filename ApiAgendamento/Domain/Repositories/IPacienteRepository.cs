using ApiAgendamento.Domain.Entities;

namespace ApiAgendamento.Domain.Repositories;

public interface IPacienteRepository
{
    Task<Paciente?> ObterPorIdAsync(int id);
    Task<IEnumerable<Paciente>> ObterTodosAsync();
    Task AdicionarAsync(Paciente paciente);
    Task AtualizarAsync(Paciente paciente);
    Task<IEnumerable<Paciente>> ObterPorIdsAsyncList(List<int> ids);

}