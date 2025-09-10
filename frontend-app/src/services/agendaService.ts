// Serviços para consumir a API de agendamento
import type { Agendamento } from '@/models/Agendamento'
import api from './api'

// Criar novo agendamento
export async function criarAgendamento(pacienteId: number, horarioId: number): Promise<Agendamento> {
  const response = await api.post('/agendamento', null, {
    params: { pacienteId, horarioId }
  })
  return response.data
}

// Listar todos os agendamentos
export async function listarAgendamentos() {
  const response = await api.get('/agendamento')
  return response.data
}

// Atualizar agendamento existente
export async function atualizarAgendamento(agendamentoId: number, novoHorarioId: number): Promise<string> {
  const response = await api.patch('/agendamento', null, {
    params: { agendamentoId, novoHorarioId }
  })
  return response.data
}

// Cancelar agendamento
export async function cancelarAgendamento(agendamentoId: number): Promise<string> {
  const response = await api.delete('/agendamento', {
    params: { agendamentoId }
  })
  return response.data
}
