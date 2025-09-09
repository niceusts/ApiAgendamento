// Serviços para consumir a API de agendamento
import api from './api'

// Criar novo agendamento
export async function criarAgendamento(pacienteId: number, horarioId: number): Promise<string> {
  const response = await api.post('/agenda', null, {
    params: { pacienteId, horarioId }
  })
  return response.data
}

// Listar todos os agendamentos
export async function listarAgendamentos() {
  const response = await api.get('/agenda')
  return response.data
}

// Atualizar agendamento existente
export async function atualizarAgendamento(agendamentoId: number, novoHorarioId: number): Promise<string> {
  const response = await api.patch('/agenda', null, {
    params: { agendamentoId, novoHorarioId }
  })
  return response.data
}

// Cancelar agendamento
export async function cancelarAgendamento(agendamentoId: number): Promise<string> {
  const response = await api.delete('/agenda', {
    params: { agendamentoId }
  })
  return response.data
}
