using ApiAgendamento.Domain.Entities;

namespace ApiAgendamento.Domain.Repositories;

public interface IAgendamentoRepository
{
    Task<bool> ExisteConflito(int medicoId, DateTime dataHora);
    Task AdicionarAsync(Agendamento agendamento);
}