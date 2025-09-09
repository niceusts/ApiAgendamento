
<template>
  <div class="lista-horarios">
    <div class="headerOptions">
      <h2>Meus Horários</h2>
      <button class="btn btn-success" @click="$router.push('/AdicionarHorario')">Adicionar Horário</button>
    </div>

    <div class="table-responsive" v-if="horarios.length">
      <table class="table-horarios">
        <thead>
          <tr>
            <th>ID</th>
            <th>Início</th>
            <th>Fim</th>
            <th style="width: 120px;">Ações</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="h in horarios" :key="h.id">
            <td>{{ h.id }}</td>
            <td>{{ formatarData(h.inicio) }}</td>
            <td>{{ formatarData(h.fim) }}</td>
            <td>
              <div class="dropdown">
                <button class="btn btn-sm btn-secondary dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false" data-bs-auto-close="outside">
                  <i class="fas fa-ellipsis-v"></i> Ações
                </button>
                <ul class="dropdown-menu dropdown-menu-end">
                  <li>
                    <a class="dropdown-item" href="#" @click.prevent="$router.push({ name: 'atualizarHorario', params: { horarioId: h.id } })">
                      <i class="fas fa-edit"></i> Editar
                    </a>
                  </li>
                  <li>
                    <a class="dropdown-item text-danger" href="#" @click.prevent="removerHorario(h.id)">
                      <i class="fas fa-trash-alt"></i> Excluir
                    </a>
                  </li>
                </ul>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
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
.dropdown-menu {
  z-index: 1050 !important;
}
.lista-horarios {
  padding: 2rem;
  background: #fff;
  border-radius: 8px;
  box-shadow: 0 2px 8px #0001;
}
.table-responsive {
  width: 100%;
  overflow: visible !important;
}
.dropdown-menu {
  z-index: 2000 !important;
}
@media (max-width: 768px) {
  .table-responsive {
    overflow-x: auto !important;
  }
}
.table-horarios {
  width: 100%;
  border-collapse: collapse;
  min-width: 400px;
}
.table-horarios th, .table-horarios td {
  padding: 0.75rem 0.5rem;
  border-bottom: 1px solid #e0e0e0;
  text-align: left;
}
.table-horarios th {
  background: #f5f5f5;
}

.erro {
  color: #c00;
  margin-top: 1rem;
}
</style>
