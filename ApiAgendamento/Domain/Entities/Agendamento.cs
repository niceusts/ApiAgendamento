namespace ApiAgendamento.Domain.Entities;

public class Agendamento
{
    public int Id { get; private set; }
    public int MedicoId { get; private set; }
    public int PacienteId { get; private set; }
    public DateTime DataHora { get; private set; }
    public int HorarioId { get; private set; }

    protected Agendamento() { }

    internal Agendamento(int medicoId, int pacienteId, DateTime dataHora, int horarioId)
    {
        MedicoId = medicoId;
        PacienteId = pacienteId;
        DataHora = dataHora;
        HorarioId = horarioId;
    }

    public void AtualizarMedicoEHorario(int novoMedicoId, DateTime novoHorario, int novoHorarioId)
    {
        MedicoId = novoMedicoId;
        DataHora = novoHorario;
        HorarioId = novoHorarioId;
    }

    public static Agendamento Criar(Medico medico, int pacienteId, int horarioId)
    {
        var horario = medico.HorariosDisponiveis.FirstOrDefault(h => h.Id == horarioId);
        if (horario == null)
            throw new ArgumentException("Horario não disponível para este médico.");

        return new Agendamento(medico.Id, pacienteId, horario.Inicio, horarioId);
    }
}