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

    public async Task CriarAgendamento(int pacienteId, int horarioId)
    {
        var horario = await _agendamentoRepo.ObterHorarioDisponivelPorIdAsync(horarioId);
        if (horario == null)
            throw new Exception("Horário não encontrado.");

        var conflito = await _agendamentoRepo.ExisteConflito(horario.MedicoId, horario.Inicio);
        if (conflito)
            throw new Exception("Já existe um agendamento nesse horário.");

        // regra de domínio
        var agendamento = new Agendamento(horario.MedicoId, pacienteId, horario.Inicio);
        await _agendamentoRepo.AdicionarAsync(agendamento);
    }

    public async Task AtualizarAgendamento(int agendamentoId, int novoHorarioId)
    {
        var agendamento = await _agendamentoRepo.ObterAgendaId(agendamentoId);
        if (agendamento == null)
            throw new Exception("Agendamento não encontrado.");

        var novoHorario = await _agendamentoRepo.ObterHorarioDisponivelPorIdAsync(novoHorarioId);
        if (novoHorario == null)
            throw new Exception("Novo horário não encontrado.");

        var conflito = await _agendamentoRepo.ExisteConflito(novoHorario.MedicoId, novoHorario.Inicio);
        if (conflito)
            throw new Exception("Já existe um agendamento nesse novo horário.");

        // Atualiza os dados do agendamento via método de domínio
        agendamento.AtualizarMedicoEHorario(novoHorario.MedicoId, novoHorario.Inicio);

        await _agendamentoRepo.AtualizarAsync(agendamento);
    }

    public async Task CancelarAgendamento(int agendamentoId)
    {
        var agendamento = await _agendamentoRepo.ObterAgendaId(agendamentoId);
        if (agendamento == null)
            throw new Exception("Agendamento não encontrado.");
        await _agendamentoRepo.DeleteAsync(agendamento);
    }
}