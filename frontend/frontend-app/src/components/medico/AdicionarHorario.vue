<template>
  <div class="adicionar-horario">
    <h2>Adicionar Horário Disponível</h2>
    <form @submit.prevent="adicionarHorario">
      <label for="inicio">Início:</label>
      <input
        id="inicio"
        type="datetime-local"
        v-model="inicio"
        required
      />
      <label for="fim">Fim:</label>
      <input
        id="fim"
        type="datetime-local"
        v-model="fim"
        required
      />
      <button type="submit">Adicionar</button>
    </form>
    <p v-if="mensagem" :class="{ erro: erro }">{{ mensagem }}</p>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import api from '@/services/api'

const inicio = ref('')
const fim = ref('')
const mensagem = ref('')
const erro = ref(false)

function toUtcIsoString(local: string) {
  if (!local) return ''
  const date = new Date(local)
  return date.toISOString()
}

async function adicionarHorario() {
  mensagem.value = ''
  erro.value = false
  try {
    const medicoId = localStorage.getItem('medicoId')
    if (!medicoId) {
      mensagem.value = 'Médico não identificado.'
      erro.value = true
      return
    }
    await api.post(`/medico/${medicoId}/horarios`, {
      inicio: toUtcIsoString(inicio.value),
      fim: toUtcIsoString(fim.value)
    })
    mensagem.value = 'Horário adicionado com sucesso!'
    inicio.value = ''
    fim.value = ''
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  } catch (e: any) {
    mensagem.value =
      e.response?.data?.message ||
      e.response?.data?.error ||
      (typeof e.response?.data === 'string' ? e.response.data : '') ||
      'Erro ao adicionar horário.'
    erro.value = true
  }
}
</script>

<style scoped>
.adicionar-horario {
  max-width: 400px;
  margin: 2rem auto;
  padding: 2rem;
  background: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 8px #0001;
}
label {
  display: block;
  margin-bottom: 0.5rem;
}
input[type="datetime-local"] {
  width: 100%;
  padding: 0.5rem;
  margin-bottom: 1rem;
}
button {
  padding: 0.5rem 1.5rem;
  background: #1976d2;
  color: #fff;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}
.erro {
  color: #c00;
  margin-top: 1rem;
}
</style>
