using ApiAgendamento.Domain.Entities;
using ApiAgendamento.Domain.Repositories;

namespace ApiAgendamento.Application.Services;

public class AgendamentoService
{
    private readonly IAgendamentoRepository _agendamentoRepo;
    private readonly IMedicoRepository _medicoRepo;

    public AgendamentoService(IAgendamentoRepository agendamentoRepo, IMedicoRepository medicoRepo)
    {
        _agendamentoRepo = agendamentoRepo;
        _medicoRepo = medicoRepo;
    }

    public async Task CriarAgendamento(int medicoId, int pacienteId, DateTime dataHora)
    {
        var medico = await _medicoRepo.ObterPorIdAsync(medicoId);
        if (medico == null)
            throw new Exception("M�dico n�o encontrado.");

        var disponivel = medico.HorariosDisponiveis
            .Any(h => dataHora >= h.Inicio && dataHora < h.Fim);

        if (!disponivel)
            throw new Exception("M�dico n�o est� dispon�vel nesse hor�rio.");

        var conflito = await _agendamentoRepo.ExisteConflito(medicoId, dataHora);
        if (conflito)
            throw new Exception("J� existe um agendamento nesse hor�rio.");

        var agendamento = new Agendamento(medicoId, pacienteId, dataHora);
        await _agendamentoRepo.AdicionarAsync(agendamento);
    }
}