<template>
  <div class="lista-horarios">
    <div class="headerOptions">
      <h2>Meus Horários</h2>
      <button class="btn btn-success" @click="$router.push('/AdicionarHorario')">Adicionar Horário</button>
    </div>

    <ul v-if="horarios.length">
      <li v-for="h in horarios" :key="h.id">
        <span>
          {{ formatarData(h.inicio) }} - {{ formatarData(h.fim) }}
        </span>
      </li>
    </ul>
    <p v-else>Nenhum horário disponível.</p>
    <p v-if="mensagem" class="erro">{{ mensagem }}</p>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import api from '@/services/api'

interface Horario {
  id: number
  inicio: string
  fim: string
}

const horarios = ref<Horario[]>([])
const mensagem = ref('')

function formatarData(data: string) {
  if (!data) return ''
  const d = new Date(data)
  return d.toLocaleString('pt-BR', { dateStyle: 'short', timeStyle: 'short' })
}

async function carregarHorarios() {
  mensagem.value = ''
  try {
    const medicoId = localStorage.getItem('medicoId')
    if (!medicoId) {
      mensagem.value = 'Médico não identificado.'
      return
    }
    const { data } = await api.get(`/medico/${medicoId}/horarios`)
    horarios.value = data
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  } catch (e: any) {
    mensagem.value = e.response?.data?.message || 'Erro ao buscar horários.'
  }
}

onMounted(carregarHorarios)
</script>

<style scoped>
.lista-horarios {
  padding: 2rem;
  background: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 8px #0001;
}
ul {
  list-style: none;
  padding: 0;
}
li {
  margin-bottom: 1rem;
  padding: 0.5rem 1rem;
  background: #f5f5f5;
  border-radius: 4px;
}
.erro {
  color: #c00;
  margin-top: 1rem;
}
</style>
