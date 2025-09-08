namespace ApiAgendamento.Domain.Entities;

public class Agendamento
{
    public int Id { get; private set; }
    public int MedicoId { get; private set; }
    public int PacienteId { get; private set; }
    public DateTime DataHora { get; private set; }

    protected Agendamento() { }

    internal Agendamento(int medicoId, int pacienteId, DateTime dataHora)
    {
        MedicoId = medicoId;
        PacienteId = pacienteId;
        DataHora = dataHora;
    }

    public void AtualizarMedicoEHorario(int novoMedicoId, DateTime novoHorario)
    {
        MedicoId = novoMedicoId;
        DataHora = novoHorario;
    }

    public static Agendamento Criar(Medico medico, int pacienteId, int horarioId)
    {
        var horario = medico.HorariosDisponiveis.FirstOrDefault(h => h.Id == horarioId);
        if (horario == null)
            throw new ArgumentException("Horario não disponível para este médico.");

        return new Agendamento(medico.Id, pacienteId, horario.Inicio);
    }
}