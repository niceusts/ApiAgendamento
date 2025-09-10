// Modelo para Usuario
export interface Usuario {
  id: number;
  nome: string;
  email: string;
  senha: string;
  tipo: 'paciente' | 'medico';
  medicoId?: number; // Opcional, se for paciente
  pacienteId?: number; // Opcional, se for médico
}
