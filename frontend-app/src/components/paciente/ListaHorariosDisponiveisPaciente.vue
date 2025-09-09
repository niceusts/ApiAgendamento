<template>
  <div class="lista-horarios-disponiveis">
    <h2>Horários Disponíveis para Agendamento</h2>
    <div class="table-responsive" v-if="horarios.length">
      <table class="table table-bordered">
        <thead>
          <tr>
            <th>ID</th>
            <th>Médico</th>
            <th>Especialidade</th>
            <th>Início</th>
            <th>Fim</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="item in horariosFlat" :key="item.horarioId">
            <td>{{ item.horarioId }}</td>
            <td>{{ item.nome }}</td>
            <td>{{ item.especialidade }}</td>
            <td>{{ formatarData(item.inicio) }}</td>
            <td>{{ formatarData(item.fim) }}</td>
          </tr>
        </tbody>
      </table>
    </div>
    <p v-else>Nenhum horário disponível para agendamento.</p>
    <p v-if="mensagem" class="erro">{{ mensagem }}</p>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { getTodosHorariosDisponiveis } from '@/services/pacienteService'
import type { MedicoComHorarios } from '@/models/Medico'

const horarios = ref<MedicoComHorarios[]>([])
const mensagem = ref('')

const horariosFlat = computed(() => {
  // Retorna um array "achatado" com cada linha da tabela
  return horarios.value.flatMap(medico =>
    medico.horariosDisponiveis.map(horario => ({
      horarioId: `${horario.id}`,
      nome: medico.nome,
      especialidade: medico.especialidade,
      inicio: horario.inicio,
      fim: horario.fim
    }))
  )
})

function formatarData(data: string) {
  if (!data) return ''
  const d = new Date(data)
  return d.toLocaleString('pt-BR', { dateStyle: 'short', timeStyle: 'short' })
}

async function carregarHorarios() {
  mensagem.value = ''
  try {
    horarios.value = await getTodosHorariosDisponiveis()
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  } catch (e: any) {
    mensagem.value = e.response?.data?.message || 'Erro ao buscar horários.'
  }
}

onMounted(carregarHorarios)
</script>

<style scoped>
.lista-horarios-disponiveis {
  padding: 2rem;
  background: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 8px #0001;
}
.table {
  width: 100%;
  margin-bottom: 1rem;
  background-color: transparent;
}
.erro {
  color: #c00;
  margin-top: 1rem;
}
</style>
