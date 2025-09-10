// Serviço de autenticação (login e registro)
import api from './api'

export interface LoginPayload {
  email: string
  senha: string
}

export interface LoginResponse {
  token: string
  tipo: string
  nome: string
  medicoId?: number
  pacienteId?: number
}

export async function login(payload: LoginPayload): Promise<LoginResponse> {
  const response = await api.post<LoginResponse>('/auth/login', payload)
  return response.data
}

export interface RegisterPayload {
  nome: string
  email: string
  senha: string
  tipo: string
  especialidade?: string
}

export async function register(payload: RegisterPayload): Promise<void> {
  await api.post('/auth/register', payload)
}
