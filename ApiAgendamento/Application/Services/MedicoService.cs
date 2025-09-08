using ApiAgendamento.Domain.Entities;

namespace ApiAgendamento.Application.Services;


/// <summary>
/// Serviço de regras de negócio para manipulação de horários disponíveis do médico.
/// </summary>
public class MedicoService
{
    /// <summary>
    /// Adiciona um novo horário disponível ao médico, validando conflitos e datas.
    /// </summary>
    /// <param name="medico">Entidade médico a ser alterada.</param>
    /// <param name="inicio">Data/hora de início do horário.</param>
    /// <param name="fim">Data/hora de fim do horário.</param>
    /// <exception cref="ArgumentException">Se houver conflito ou datas inválidas.</exception>
    public void AdicionarHorarioDisponivel(Medico medico, DateTime inicio, DateTime fim)
    {
        if (fim <= inicio)
            throw new ArgumentException("A data/hora de início deve ser menor que a data/hora de fim.");

        // Verifica se já existe um horário que conflita com o novo horário
        bool conflito = medico.HorariosDisponiveis.Any(h =>
            inicio < h.Fim && fim > h.Inicio
        );

        if (conflito)
            throw new ArgumentException("Já existe um horário disponível nesse intervalo.");

        medico.HorariosDisponiveis.Add(new HorarioDisponivel(inicio, fim, medico.Id));
    }

    /// <summary>
    /// Remove um horário disponível do médico, validando existência e se há agendamento vinculado.
    /// </summary>
    /// <param name="medico">Entidade médico a ser alterada.</param>
    /// <param name="horarioId">Id do horário a ser removido.</param>
    /// <param name="existeAgendamento">Função que retorna true se existe agendamento vinculado ao horário.</param>
    /// <exception cref="ArgumentException">Se não encontrar o horário ou houver agendamento vinculado.</exception>
    public void RemoverHorarioDisponivel(Medico medico, int horarioId, Func<int, bool> existeAgendamento)
    {
        var horario = medico.HorariosDisponiveis.FirstOrDefault(h => h.Id == horarioId && h.MedicoId == medico.Id);
        if (horario == null)
            throw new ArgumentException("Horário não encontrado.");
        if (existeAgendamento(horarioId))
            throw new ArgumentException("Não é possível remover um horário que já possui agendamento vinculado.");
        medico.HorariosDisponiveis.Remove(horario);
    }

    /// <summary>
    /// Atualiza um horário disponível do médico, validando conflitos e datas.
    /// </summary>
    /// <param name="medico">Entidade médico a ser alterada.</param>
    /// <param name="horarioId">Id do horário a ser atualizado.</param>
    /// <param name="novoInicio">Nova data/hora de início.</param>
    /// <param name="novoFim">Nova data/hora de fim.</param>
    /// <exception cref="ArgumentException">Se não encontrar o horário, houver conflito ou datas inválidas.</exception>
    public void AtualizarHorario(Medico medico, int horarioId, DateTime novoInicio, DateTime novoFim)
    {
        var horario = medico.HorariosDisponiveis.FirstOrDefault(h => h.Id == horarioId && h.MedicoId == medico.Id);
        if (horario == null)
            throw new ArgumentException("Horário não encontrado.");

        if (novoFim <= novoInicio)
            throw new ArgumentException("A data/hora de início deve ser menor que a data/hora de fim.");

        // Verifica se já existe um horário (exceto o atual) que conflita com o novo horário
        bool conflito = medico.HorariosDisponiveis.Any(h =>
            h.Id != horarioId && novoInicio < h.Fim && novoFim > h.Inicio
        );
        if (conflito)
            throw new ArgumentException("Já existe um horário disponível nesse intervalo.");

        // Supondo que Inicio e Fim tenham setters internos ou públicos
        horario.GetType().GetProperty("Inicio")!.SetValue(horario, novoInicio);
        horario.GetType().GetProperty("Fim")!.SetValue(horario, novoFim);
    }

    internal void RemoverHorarioDisponivel(Medico medico, int horarioId)
    {
        throw new NotImplementedException();
    }
}
