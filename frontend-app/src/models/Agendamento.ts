// Modelo (entidade) para Agendamento
export interface Agendamento {
  id: number;
  medicoId: number;
  pacienteId: number;
  dataHora: string;
}
