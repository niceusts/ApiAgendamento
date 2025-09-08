using ApiAgendamento.Domain.Entities;

namespace ApiAgendamento.Domain.Repositories;

public interface IAgendamentoRepository
{
    Task<Agendamento?> ObterAgendaId(int id);
    Task<bool> ExisteConflito(int medicoId, DateTime dataHora);
    Task AdicionarAsync(Agendamento agendamento);
    Task<List<Agendamento>> ObterTodosAsync();
    Task AtualizarAsync(Agendamento agendamento);
    Task<HorarioDisponivel?> ObterHorarioDisponivelPorIdAsync(int horarioId);
    Task<bool> ExisteAgendamentoParaHorario(int horarioId);
    Task DeleteAsync(Agendamento agendamento);
}