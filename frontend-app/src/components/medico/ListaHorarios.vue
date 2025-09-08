
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
          <div style="display: inline-block; position: relative;">
            <button @click="alternarDropdown(h.id)" class="btn btn-sm btn-secondary" style="margin-left: 10px;">
              Ações ▼
            </button>
            <ul v-if="dropdownAberto === h.id" style="position: absolute; z-index: 10; background: #fff; border: 1px solid #ccc; padding: 0.5rem; list-style: none; right: 0; min-width: 120px;">
              <li>
                <button class="btn btn-link" @click="$router.push({ path: '/EditarHorario', query: { id: h.id } })">Editar</button>
              </li>
              <li>
                <button class="btn btn-link text-danger" @click="removerHorario(h.id)">Excluir</button>
              </li>
            </ul>
          </div>
        </span>
      </li>
    </ul>
    <p v-else>Nenhum horário disponível.</p>
    <p v-if="mensagem" class="erro">{{ mensagem }}</p>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { type HorarioDisponivel } from '@/models/HorarioDisponivel'
import { getHorariosByMedicoId } from '@/services/medicoService'
import { removerHorarioDisponivel } from '@/services/medicoService'
import Swal from 'sweetalert2'

const horarios = ref<HorarioDisponivel[]>([])
const mensagem = ref('')
const dropdownAberto = ref<number | null>(null)


function formatarData(data: string) {
  if (!data) return ''
  const d = new Date(data)
  return d.toLocaleString('pt-BR', { dateStyle: 'short', timeStyle: 'short' })
}

function alternarDropdown(id: number) {
  dropdownAberto.value = dropdownAberto.value === id ? null : id
}

async function carregarHorarios() {
  mensagem.value = ''
  try {
    const medicoId = localStorage.getItem('medicoId')
    if (!medicoId) {
      mensagem.value = 'Médico não identificado.'
      return
    }
    horarios.value = await getHorariosByMedicoId(Number(medicoId))
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  } catch (e: any) {
    mensagem.value = e.response?.data?.message || 'Erro ao buscar horários.'
  }
}

async function removerHorario(horarioId: number) {
  const confirm = await Swal.fire({
    title: 'Tem certeza?',
    text: 'Deseja realmente remover este horário?',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Sim, remover',
    cancelButtonText: 'Cancelar',
  })
  if (!confirm.isConfirmed) return

  const medicoId = localStorage.getItem('medicoId')
  if (!medicoId) {
    Swal.fire('Erro', 'Médico não identificado.', 'error')
    return
  }
  try {
    await removerHorarioDisponivel(Number(medicoId), horarioId)
    Swal.fire('Sucesso', 'Horário excluído com sucesso!', 'success')
    await carregarHorarios()
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  } catch (e: any) {
    const msg = e.response?.data?.message || 'Erro ao excluir horário.'
    Swal.fire('Erro', msg, 'error')
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
