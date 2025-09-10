<template>
  <div class="atualizar-horario">
    <h2>Atualizar Horário Disponível</h2>
    <form @submit.prevent="atualizarHorario">
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
      <button class="btn btn-secondary mx-2" @click="$router.push('/ListaHorarios')">Voltar</button>
      <button class="btn btn-primary" type="submit">Atualizar</button>
    </form>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import Swal from 'sweetalert2'
import { atualizarHorarioDisponivel } from '@/services/medicoService'
import { getHorariosByMedicoId } from '@/services/medicoService'

const route = useRoute()
const router = useRouter()
const inicio = ref('')
const fim = ref('')

const horarioId = Number(route.params.horarioId)
const medicoId = Number(localStorage.getItem('medicoId'))

// Busca o horário pelo ID e preenche os campos ao montar
onMounted(async () => {
  if (!medicoId || !horarioId) return
  try {
    const horarios = await getHorariosByMedicoId(Number(medicoId))
    const horario = horarios.find(h => h.id === horarioId)
    if (horario) {
      // Converter ISO para local no formato YYYY-MM-DDTHH:mm
      inicio.value = formatToDatetimeLocal(horario.inicio)
      fim.value = formatToDatetimeLocal(horario.fim)
    } else {
      Swal.fire('Erro', 'Horário não encontrado.', 'error')
    }
  } catch {
    Swal.fire('Erro', 'Erro ao buscar horário.', 'error')
  }
})

function formatToDatetimeLocal(iso: string) {
  if (!iso) return ''
  const date = new Date(iso)
  // Ajusta para o fuso local do navegador
  const off = date.getTimezoneOffset()
  const local = new Date(date.getTime() - off * 60 * 1000)
  return local.toISOString().slice(0, 16)
}
function toUtcIsoString(local: string) {
  if (!local) return ''
  const date = new Date(local)
  return date.toISOString()
}

async function atualizarHorario() {
  try {
    if (!medicoId || !horarioId) {
      Swal.fire('Erro', 'Médico ou horário não identificado.', 'error')
      return
    }
    const msg = await atualizarHorarioDisponivel(
      medicoId,
      horarioId,
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
      'Erro ao atualizar horário.'
    Swal.fire('Erro', msg, 'error')
  }
}
</script>

<style scoped>
.atualizar-horario {
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
</style>
