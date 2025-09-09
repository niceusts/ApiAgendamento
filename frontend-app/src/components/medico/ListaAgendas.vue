<template>
  <div class="agendas-container">
    <h2>Agendas do Médico</h2>
    <div class="table-responsive" v-if="agendas.length">
      <table class="table-agendas">
        <thead>
          <tr>
            <th>ID</th>
            <th>Data/Hora</th>
            <th>Paciente</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="agenda in agendas" :key="agenda.id">
            <td>{{ agenda.id }}</td>
            <td>{{ formatarData(agenda.dataHora) }}</td>
            <td>{{ agenda.pacienteId }} - {{ agenda.pacienteNome }} </td>
          </tr>
        </tbody>
      </table>
    </div>
    <p v-else>Nenhuma agenda encontrada.</p>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { type AgendaMedico } from '@/models/Medico'
import { getAgendasByMedicoId } from '@/services/medicoService'

const agendas = ref<AgendaMedico[]>([])

function formatarData(data: string) {
  if (!data) return ''
  const d = new Date(data)
  return d.toLocaleString('pt-BR', { dateStyle: 'short', timeStyle: 'short' })
}

async function carregarAgendas() {
  const medicoId = localStorage.getItem('medicoId')
  if (!medicoId) return
  agendas.value = await getAgendasByMedicoId(Number(medicoId))
}

onMounted(carregarAgendas)
</script>

<style scoped>
.agendas-container {
  padding: 2rem;
  background: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 8px #0001;
}
.table-responsive {
  width: 100%;
  overflow-x: auto;
}
.table-agendas {
  width: 100%;
  border-collapse: collapse;
  min-width: 400px;
}
.table-agendas th, .table-agendas td {
  padding: 0.75rem 0.5rem;
  border-bottom: 1px solid #e0e0e0;
  text-align: left;
}
.table-agendas th {
  background: #f5f5f5;
}
</style>
