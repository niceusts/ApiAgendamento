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
      <button class="btn btn-secondary mx-2" @click="$router.push('/ListaHorarios')"> Voltar</button>
      <button class="btn btn-success" type="submit">Adicionar</button>
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { adicionarHorarioDisponivel } from '@/services/medicoService'
import Swal from 'sweetalert2'

const inicio = ref('')
const router = useRouter()
const fim = ref('')

function toUtcIsoString(local: string) {
  if (!local) return ''
  const date = new Date(local)
  return date.toISOString()
}

async function adicionarHorario() {
  try {
    const medicoId = localStorage.getItem('medicoId')
    if (!medicoId) {
      Swal.fire('Erro', 'Médico não identificado.', 'error')
      return
    }
    const msg = await adicionarHorarioDisponivel(
      Number(medicoId),
      toUtcIsoString(inicio.value),
      toUtcIsoString(fim.value)
    )
    Swal.fire('Sucesso', msg, 'success')
    inicio.value = ''
    fim.value = ''
    router.push('/ListaHorarios')
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  } catch (e: any) {
    const msg =
      e.response?.data?.message ||
      e.response?.data?.error ||
      (typeof e.response?.data === 'string' ? e.response.data : '') ||
      'Erro ao adicionar horário.'
    Swal.fire('Erro', msg, 'error')
  }
}
</script>

<style scoped>
.adicionar-horario {
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
.erro {
  color: #c00;
  margin-top: 1rem;
}
</style>
