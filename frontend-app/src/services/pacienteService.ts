// Serviços para consumir a API de paciente
import type { HorarioDisponivel } from '@/models/HorarioDisponivel'

export interface MedicoComHorarios {
  medicoId: number
  nome: string
  especialidade: string
  horariosDisponiveis: HorarioDisponivel[]
}
import api from './api'

// Buscar agendamentos do paciente
export async function getMeusAgendamentos(pacienteId: number) {
  const response = await api.get('/paciente/meus-agendamentos', {
    params: { pacienteId }
  })
  return response.data
}

// Listar todos os horários disponíveis de todos os médicos
export async function getTodosHorariosDisponiveis() {
  const response = await api.get<MedicoComHorarios[]>('/paciente/horarios-disponiveis')
  return response.data
}
