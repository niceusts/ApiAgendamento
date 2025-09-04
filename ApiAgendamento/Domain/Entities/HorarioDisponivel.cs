namespace ApiAgendamento.Domain.Entities;

public class HorarioDisponivel
{
    public int Id { get; private set; }
    public DateTime Inicio { get; private set; }
    public DateTime Fim { get; private set; }
    public int MedicoId { get; private set; }

    protected HorarioDisponivel() { }

    public HorarioDisponivel(DateTime inicio, DateTime fim, int medicoId)
    {
        Inicio = inicio;
        Fim = fim;
        MedicoId = medicoId;
    }
}