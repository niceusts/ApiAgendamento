namespace ApiAgendamento.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty; // "medico" ou "paciente"
    public int? MedicoId { get; set; }
    public int? PacienteId { get; set; }
}
