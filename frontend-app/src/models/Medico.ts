// Modelo (entidade) para Medico
export interface Medico {
  id: number;
  nome: string;
  especialidade: string;
}

export interface AgendaMedico {
  id: number;
  dataHora: string;
  pacienteId: number;
  pacienteNome: string;
  pacienteEmail: string;
}
