namespace ApiAgendamento.Domain.Entities;

public class Paciente
{
    public int Id { get; private set; }
    public string Nome { get; private set; }
    public string Email { get; private set; }

    protected Paciente() { }

    public Paciente(string nome, string email)
    {
        Nome = nome;
        Email = email;
    }
}