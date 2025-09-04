namespace ApiAgendamento.Domain.Entities;

public class Medico
{
    public int Id { get; private set; }
    public string Nome { get; private set; }
    public string Especialidade { get; private set; }

    public List<HorarioDisponivel> HorariosDisponiveis { get; private set; } = new();

    protected Medico() { }

    public Medico(string nome, string especialidade)
    {
        Nome = nome;
        Especialidade = especialidade;
    }

    public void AdicionarHorarioDisponivel(DateTime inicio, DateTime fim)
    {
        if (fim <= inicio)
            throw new ArgumentException("Horário inválido.");

        HorariosDisponiveis.Add(new HorarioDisponivel(inicio, fim, Id));
    }
}