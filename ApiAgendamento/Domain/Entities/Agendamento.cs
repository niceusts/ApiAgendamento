namespace ApiAgendamento.Domain.Entities;

public class Agendamento
{
    public int Id { get; private set; }
    public int MedicoId { get; private set; }
    public int PacienteId { get; private set; }
    public DateTime DataHora { get; private set; }

    protected Agendamento() { }

    public Agendamento(int medicoId, int pacienteId, DateTime dataHora)
    {
        MedicoId = medicoId;
        PacienteId = pacienteId;
        DataHora = dataHora;
    }
}