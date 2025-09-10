import type { HorarioDisponivel } from '@/models/HorarioDisponivel'
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

export interface MedicoComHorarios {
  medicoId: number;
  nome: string;
  especialidade: string;
  horariosDisponiveis: HorarioDisponivel[];
}
