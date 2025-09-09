namespace ApiAgendamento.Api.Dtos;

public class HorarioDisponivelDto
{
    /// <summary>
    /// Data/hora de início do horário disponível.
    /// </summary>
    public DateTime Inicio { get; set; }
    /// <summary>
    /// Data/hora de fim do horário disponível.
    /// </summary>
    public DateTime Fim { get; set; }
}
