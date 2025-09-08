// Service para consumir a API de médicos
import api from './api';

// Buscar horários de um médico
import { type HorarioDisponivel } from '../models/HorarioDisponivel';

export async function getHorariosByMedicoId(medicoId: number): Promise<HorarioDisponivel[]> {
  const response = await api.get<HorarioDisponivel[]>(`/medico/${medicoId}/horarios`);
  return response.data;
}

// Adicionar novo horário disponível
export async function adicionarHorarioDisponivel(medicoId: number, inicio: string, fim: string): Promise<string> {
  const response = await api.post(`/medico/${medicoId}/horarios`, { inicio, fim });
  return response.data;
}

// Remover horário disponível
export async function removerHorarioDisponivel(medicoId: number, horarioId: number): Promise<string> {
  const response = await api.delete(`/medico/${medicoId}/horarios/${horarioId}/remover`);
  return response.data;
}

// Atualizar horário disponível
export async function atualizarHorarioDisponivel(medicoId: number, horarioId: number, inicio: string, fim: string): Promise<string> {
  const response = await api.patch(`/medico/${medicoId}/horarios/${horarioId}`, { inicio, fim });
  return response.data;
}
