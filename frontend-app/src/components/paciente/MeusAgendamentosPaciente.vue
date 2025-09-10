<template>
  <div class="meus-agendamentos container-fluid py-4">
    <div class="row justify-content-center mb-4">
      <div class="">
        <div class="card shadow-sm border-0 p-4">
          <h2 class="mb-4 text-primary fw-bold text-center my-3">Meus Agendamentos</h2>
          <div v-if="agendamentos.length">
            <div class="row g-3 my-3">
              <div v-for="ag in agendamentos" :key="ag.id" class="col-12 col-md-6 col-lg-3">
                <div class="card h-100 border-0 shadow-sm agendamento-card">
                  <div class="card-body d-flex flex-column justify-content-between">
                    <div>
                      <h5 class="card-title mb-1 text-primary text-center">{{ ag.medico.nome }}</h5>
                      <p class="mb-1"><span class="fw-semibold">Especialidade:</span> {{ ag.medico.especialidade }}</p>
                      <p class="mb-1"><span class="fw-semibold">Data:</span> {{ formatarData(ag.dataHora) }}</p>
                    </div>
                    <button class="btn btn-outline-danger w-100 mt-3" @click="abrirModalRemover(ag)">
                      <i class="fa-solid fa-trash me-2"></i>Cancelar
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <p v-else class="text-center text-muted mt-4">Nenhum agendamento encontrado.</p>
          <p v-if="mensagem" class="erro text-center">{{ mensagem }}</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { getMeusAgendamentos } from '@/services/pacienteService'
import { cancelarAgendamento } from '@/services/agendaService'
import type { AgendamentoPaciente } from '@/models/Paciente'
import Swal from 'sweetalert2'

const agendamentos = ref<AgendamentoPaciente[]>([])
const mensagem = ref('')

function formatarData(data: string) {
  if (!data) return ''
  const d = new Date(data)
  return d.toLocaleString('pt-BR', { dateStyle: 'short', timeStyle: 'short' })
}

async function carregarAgendamentos() {
  mensagem.value = ''
  const pacienteId = Number(localStorage.getItem('pacienteId'))
  if (!pacienteId) {
    mensagem.value = 'Paciente não identificado.'
    return
  }
  try {
    agendamentos.value = await getMeusAgendamentos(pacienteId)
  } catch (e: unknown) {
    if (e && typeof e === 'object' && 'response' in e) {
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      mensagem.value = (e as any).response?.data?.message || 'Erro ao buscar agendamentos.'
    } else {
      mensagem.value = 'Erro ao buscar agendamentos.'
    }
  }
}

async function abrirModalRemover(ag: AgendamentoPaciente) {
  const confirm = await Swal.fire({
    title: 'Cancelar agendamento?',
    html: `<b>Médico:</b> ${ag.medico.nome}<br><b>Especialidade:</b> ${ag.medico.especialidade}<br><b>Data:</b> ${formatarData(ag.dataHora)}`,
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Sim, cancelar',
    cancelButtonText: 'Fechar'
  })
  if (confirm.isConfirmed) {
    try {
      await cancelarAgendamento(ag.id)
      Swal.fire('Cancelado', 'Agendamento removido com sucesso.', 'success')
      await carregarAgendamentos()
    } catch (e: unknown) {
      if (e && typeof e === 'object' && 'response' in e) {
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        Swal.fire('Erro', (e as any).response?.data || 'Erro ao cancelar.', 'error')
      } else {
        Swal.fire('Erro', 'Erro ao cancelar.', 'error')
      }
    }
  }
}

onMounted(carregarAgendamentos)
</script>

<style scoped>
.meus-agendamentos {
  min-height: 100vh;
  background: linear-gradient(135deg, #f8fafc 0%, #e0e7ef 100%);
}
.card {
  border-radius: 1.25rem;
  background: #fff;
}
.agendamento-card {
  transition: box-shadow 0.2s;
}
.agendamento-card:hover {
  box-shadow: 0 6px 24px #0002;
  transform: translateY(-2px) scale(1.01);
}
.btn-outline-danger {
  font-weight: 600;
  letter-spacing: 0.5px;
}
.erro {
  color: #c00;
  margin-top: 1rem;
}
</style>
