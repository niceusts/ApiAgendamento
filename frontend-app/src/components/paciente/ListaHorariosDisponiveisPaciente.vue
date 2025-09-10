<template>
  <div class="lista-horarios-disponiveis container-fluid py-4">
    <div class="row justify-content-center mb-4">
      <div class="">
        <div class="card shadow-sm border-0 p-4">
          <h2 class="mb-4 text-primary fw-bold text-center">Horários Disponíveis para Agendamento</h2>
          <div class="row g-2 g-md-3 mb-3 filtros align-items-end">
            <div class="col-12 col-sm-6 col-md-4 col-lg-3">
              <label class="form-label">Especialidade</label>
              <select v-model="especialidadeSelecionada" class="form-select" @change="filtrarHorarios">
                <option value="">Todas</option>
                <option v-for="esp in especialidades" :key="esp">{{ esp }}</option>
              </select>
            </div>
            <div class="col-12 col-sm-6 col-md-4 col-lg-3">
              <label class="form-label">Nome do Médico</label>
              <input type="text" v-model="nomeMedicoFiltro" class="form-control" placeholder="Digite o nome" @input="filtrarHorarios" />
            </div>
            <div class="col-6 col-sm-6 col-md-2 col-lg-2">
              <label class="form-label">Dia</label>
              <input type="date" v-model="dataSelecionada" class="form-control" @change="filtrarHorarios" />
            </div>
            <div class="col-6 col-sm-6 col-md-2 col-lg-2 d-flex align-items-end">
              <button class="btn btn-outline-danger limpar-filtros-btn d-flex align-items-center justify-content-center py-2 mb-3" @click="limparFiltros" title="Limpar filtros">
                <i class="fa-solid fa-filter-circle-xmark me-2" style="font-size:1.1em;"></i>
                <span class="limpar-filtros-text"></span>
              </button>
            </div>
          </div>
          <div v-if="horariosFiltrados.length">
            <div class="row g-3">
              <div v-for="item in horariosFiltrados" :key="item.horarioId" class="col-12 col-md-6 col-lg-3">
                <div class="card h-100 border-0 shadow-sm horario-card">
                  <div class="card-body d-flex flex-column justify-content-between">
                    <div>
                      <h5 class="card-title mb-1 text-primary">{{ item.nome }}</h5>
                      <p class="mb-1"><span class="fw-semibold">Especialidade:</span> {{ item.especialidade }}</p>
                      <p class="mb-1"><span class="fw-semibold">Início:</span> {{ formatarData(item.inicio) }}</p>
                      <p class="mb-3"><span class="fw-semibold">Fim:</span> {{ formatarData(item.fim) }}</p>
                    </div>
                    <button class="btn btn-success w-100 mt-auto" @click="() => abrirModalAgendar(item)">
                      <i class="bi bi-calendar-plus me-2"></i>Agendar
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <p v-else class="text-center text-muted mt-4">Nenhum horário disponível para agendamento.</p>
          <p v-if="mensagem" class="erro text-center">{{ mensagem }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">

import { ref, onMounted } from 'vue'
import { getTodosHorariosDisponiveis } from '@/services/pacienteService'
import type { MedicoComHorarios } from '@/models/Medico'
import { criarAgendamento } from '@/services/agendaService'
import Swal from 'sweetalert2'

const horarios = ref<MedicoComHorarios[]>([])
const mensagem = ref('')
const especialidades = ref<string[]>([])
const especialidadeSelecionada = ref('')
const dataSelecionada = ref('')
const horariosFiltrados = ref<Array<{horarioId: string, nome: string, especialidade: string, inicio: string, fim: string}>>([])
const nomeMedicoFiltro = ref('')

function formatarData(data: string) {
  if (!data) return ''
  const d = new Date(data)
  return d.toLocaleString('pt-BR', { dateStyle: 'short', timeStyle: 'short' })
}

function filtrarHorarios() {
  let lista = horarios.value.flatMap(medico =>
    medico.horariosDisponiveis.map(horario => ({
      horarioId: `${horario.id}`,
      nome: medico.nome,
      especialidade: medico.especialidade,
      inicio: horario.inicio,
      fim: horario.fim
    }))
  )
  if (especialidadeSelecionada.value) {
    lista = lista.filter(h => h.especialidade === especialidadeSelecionada.value)
  }
  if (nomeMedicoFiltro.value) {
    const termo = nomeMedicoFiltro.value.trim().toLowerCase()
    lista = lista.filter(h => h.nome.toLowerCase().includes(termo))
  }
  if (dataSelecionada.value) {
    lista = lista.filter(h => h.inicio.startsWith(dataSelecionada.value))
  }
  horariosFiltrados.value = lista
}

function limparFiltros() {
  especialidadeSelecionada.value = ''
  nomeMedicoFiltro.value = ''
  dataSelecionada.value = ''
  filtrarHorarios()
}

async function carregarHorarios() {
  mensagem.value = ''
  try {
    horarios.value = await getTodosHorariosDisponiveis()
    especialidades.value = [...new Set(horarios.value.map(m => m.especialidade).filter(e => e))]
    filtrarHorarios()
  } catch (e: unknown) {
    if (e && typeof e === 'object' && 'response' in e) {
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      mensagem.value = (e as any).response?.data?.message || 'Erro ao buscar horários.'
    } else {
      mensagem.value = 'Erro ao buscar horários.'
    }
  }
}

const abrirModalAgendar = async (item: {horarioId: string, nome: string, especialidade: string, inicio: string, fim: string}) => {
  const pacienteId = Number(localStorage.getItem('pacienteId'))
  if (!pacienteId) {
    Swal.fire('Erro', 'Paciente não identificado.', 'error')
    return
  }
  const confirm = await Swal.fire({
    title: 'Confirmar agendamento?',
    html: `<b>Médico:</b> ${item.nome}<br><b>Especialidade:</b> ${item.especialidade}<br><b>Início:</b> ${formatarData(item.inicio)}<br><b>Fim:</b> ${formatarData(item.fim)}`,
    icon: 'question',
    showCancelButton: true,
    confirmButtonText: 'Agendar',
    cancelButtonText: 'Cancelar'
  })
  if (confirm.isConfirmed) {
    try {
      await criarAgendamento(pacienteId, Number(item.horarioId))
      Swal.fire('Sucesso', 'Agendamento realizado!', 'success')
      await carregarHorarios()
    } catch (e: unknown) {
      if (e && typeof e === 'object' && 'response' in e) {
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        Swal.fire('Erro', (e as any).response?.data || 'Erro ao agendar.', 'error')
      } else {
        Swal.fire('Erro', 'Erro ao agendar.', 'error')
      }
    }
  }
}

onMounted(carregarHorarios)
</script>

<style scoped>
.lista-horarios-disponiveis {
  min-height: 100vh;
  background: linear-gradient(135deg, #f8fafc 0%, #e0e7ef 100%);
}
.card {
  border-radius: 1.25rem;
  background: #fff;
}
.horario-card {
  transition: box-shadow 0.2s;
}
.horario-card:hover {
  box-shadow: 0 6px 24px #0002;
  transform: translateY(-2px) scale(1.01);
}
.btn-success {
  font-weight: 600;
  letter-spacing: 0.5px;
}
.erro {
  color: #c00;
  margin-top: 1rem;
}
.filtros .form-label {
  font-weight: 500;
}
.limpar-filtros-btn {
  min-width: 140px;
  white-space: nowrap;
  height: 38px;
  padding-top: 0 !important;
  padding-bottom: 0 !important;
  font-size: 1rem;
}
.limpar-filtros-text {
  display: inline-block;
  line-height: 1;
}
</style>

