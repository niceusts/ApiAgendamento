namespace ApiAgendamento.Application.Dtos;

public class RegisterDto
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty; // "medico" ou "paciente"
}
