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

    public void RemoverHorarioDisponivel(int medicoId, int horarioId)
    {
        var horario = HorariosDisponiveis.FirstOrDefault(h => h.Id == horarioId && h.MedicoId == medicoId);
        if (horario == null)
            throw new ArgumentException("Horário não encontrado.");
        HorariosDisponiveis.Remove(horario);
    }

    public void AtualizarHorario(int medicoId, int horarioId, DateTime novoInicio, DateTime novoFim)
    {
        var horario = HorariosDisponiveis.FirstOrDefault(h => h.Id == horarioId && h.MedicoId == medicoId);
        if (horario == null)
            throw new ArgumentException("Horário não encontrado.");

        if (novoFim <= novoInicio)
            throw new ArgumentException("Horário inválido.");

        horario.GetType().GetProperty("Inicio")!.SetValue(horario, novoInicio);
        horario.GetType().GetProperty("Fim")!.SetValue(horario, novoFim);
    }
}