// Modelo (entidade) para Paciente
export interface Paciente {
  id: number;
  nome: string;
  email: string;
}

export interface AgendamentoPaciente {
  id: number
  dataHora: string
  medico: {
    id: number
    nome: string
    especialidade: string
  }
}
