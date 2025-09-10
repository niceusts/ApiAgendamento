<!-- eslint-disable vue/multi-word-component-names -->
<template>
  <button class="sidebar-toggle" @click="toggleSidebar" aria-label="Abrir menu">
    <span v-if="!sidebarAberto">☰</span>
    <span v-else>×</span>
  </button>
  <!-- Overlay para mobile -->
  <div v-if="sidebarAberto && isMobile" class="sidebar-overlay" @click="closeSidebar"></div>
  <aside class="sidebar" :class="{ aberto: sidebarAberto, mobile: isMobile }" :style="sidebarStyle">
    <nav>
      <ul>
        <template v-if="tipoUsuario === 'medico'">
          <div>
            <div class="sidebar-nome">{{ nomeUsuario }}</div>
            <div class="sidebar-tipo">{{ tipoUsuario }}</div>
            <li><router-link to="/dashboard" @click="closeSidebarIfMobile">Dashboard</router-link></li>
            <li><router-link to="/listaHorarios" @click="closeSidebarIfMobile">Meus Horários</router-link></li>
            <li><router-link to="/listaAgenda" @click="closeSidebarIfMobile">Meus Pacientes</router-link></li>
          </div>
          <button @click="logout">Sair</button>
        </template>
        <template v-else-if="tipoUsuario === 'paciente'">
          <div>
            <div class="sidebar-nome">{{ nomeUsuario }}</div>
            <div class="sidebar-tipo">{{ tipoUsuario }}</div>
            <li><router-link to="/dashboard" @click="closeSidebarIfMobile">Dashboard</router-link></li>
            <li><router-link to="/ListaHorariosDisponiveis" @click="closeSidebarIfMobile">Horários Disponiveis</router-link></li>
            <li><router-link to="/MeusAgendamentos" @click="closeSidebarIfMobile">Meus Agendamentos</router-link></li>
          </div>
          <button @click="logout">Sair</button>
        </template>
      </ul>
    </nav>
    <!-- Botão de fechar no mobile
    <button v-if="isMobile && sidebarAberto" class="sidebar-close" @click="closeSidebar" aria-label="Fechar menu">×</button> -->
  </aside>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted, computed } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()

function logout() {
  localStorage.removeItem('token')
  localStorage.removeItem('nome')
  localStorage.removeItem('medicoId')
  router.push({ name: 'login' })
}
const tipoUsuario = ref('')
const nomeUsuario = ref('')

const isMobile = ref(window.innerWidth <= 900)
const sidebarAberto = ref(window.innerWidth > 900)

const sidebarStyle = computed(() => {
  if (isMobile.value) {
    return {
      width: '240px',
      left: sidebarAberto.value ? '0' : '-240px',
      transition: 'left 0.3s cubic-bezier(.4,0,.2,1)',
    }
  }
  return {}
})

function toggleSidebar() {
  sidebarAberto.value = !sidebarAberto.value
}
function closeSidebar() {
  sidebarAberto.value = false
}
function closeSidebarIfMobile() {
  if (isMobile.value) sidebarAberto.value = false
}

function handleResize() {
  isMobile.value = window.innerWidth <= 900
  if (!isMobile.value) {
    sidebarAberto.value = true // Sempre aberto no desktop
  }
  // No mobile, não altera sidebarAberto (deixa o usuário controlar)
}

onMounted(() => {
  tipoUsuario.value = localStorage.getItem('tipo') || ''
  nomeUsuario.value = localStorage.getItem('nome') || ''
  window.addEventListener('resize', handleResize)
})
onUnmounted(() => {
  window.removeEventListener('resize', handleResize)
})
</script>

<style scoped>

.sidebar-toggle {
  display: none;
  position: fixed;
  top: 1rem;
  left: 1rem;
  z-index: 2002;
  background: #1976d2;
  color: #fff;
  border: none;
  border-radius: 4px;
  font-size: 1rem;
  padding: 0.2rem 0.5rem;
  cursor: pointer;
  box-shadow: 0 2px 8px rgba(0,0,0,0.08);
  transition: background 0.2s;
}
.sidebar-toggle:hover {
  background: #1251a3;
}

@media (max-width: 900px) {
  .sidebar-toggle {
    display: block;
  }
  .sidebar {
    left: -260px;
    transition: left 0.3s cubic-bezier(.4,0,.2,1);
    z-index: 2003;
    box-shadow: none;
  }
  .sidebar.aberto {
    left: 0;
    box-shadow: 2px 0 16px rgba(0,0,0,0.25);
  }
  .sidebar.mobile {
    border-top-right-radius: 16px;
    border-bottom-right-radius: 16px;
    min-height: 100vh;
  }
  .sidebar-close {
    display: block;
    position: fixed;
    top: 1.2rem;
    left: 240px;
    background: #1976d2;
    color: #fff;
    border: none;
    font-size: 2.2rem;
    cursor: pointer;
    z-index: 2100;
    transition: color 0.2s;
    width: 40px;
    height: 40px;
    border-radius: 50%;
    box-shadow: 0 2px 8px rgba(0,0,0,0.10);
    display: flex;
    align-items: center;
    justify-content: center;
  }
  .sidebar-close:hover {
    color: #ffeb3b;
    background: #1251a3;
  }
  .sidebar-overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100vw;
    height: 100vh;
    background: rgba(0,0,0,0.45);
    transition: opacity 0.3s;
  }
}

.sidebar-nome {
  font-size: 1.1rem;
  font-weight: bold;
  color: #fff;
  text-align: center;
  margin-top: 0.5rem;
}
.sidebar-tipo {
  font-size: 0.8rem;
  color: #fff;
  text-align: center;
  margin-bottom: 1.5rem;
}
.sidebar {
  min-height: 100vh;
  min-width: 210px;
  z-index: 999;
  background: linear-gradient(135deg, #1976d2 80%, #2196f3 100%);
  color: #fff;
  padding: 2rem 1rem 1rem 1.2rem;
  box-shadow: 2px 0 8px rgba(0,0,0,0.04);
  position: fixed;
  top: 0;
  left: 0;
  display: flex;
  flex-direction: column;
  transition: left 0.3s cubic-bezier(.4,0,.2,1);
  border-top-right-radius: 12px;
  border-bottom-right-radius: 12px;
}
.sidebar nav ul {
  list-style: none;
  padding: 0;
}
.sidebar nav li {
  margin-bottom: 1.5rem;
}
.sidebar nav a {
  color: #fff;
  text-decoration: none;
  font-weight: 500;
  font-size: 1.1rem;
  transition: color 0.2s;
  border-radius: 6px;
  padding: 0.3rem 0.7rem;
  display: block;
}
.sidebar nav ul {
  min-height: 90vh;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}
.sidebar nav a.router-link-exact-active {
  color: #ffeb3b;
  background: rgba(255,255,255,0.08);
}
.sidebar nav a:hover {
  color: #bbdefb;
  background: rgba(255,255,255,0.05);
}
.sidebar button {
  margin-top: 2rem;
  padding: 0.7rem 2rem;
  background: rgb(218, 8, 8);
  color: #fff;
  border: none;
  border-radius: 4px;
  font-weight: bold;
  cursor: pointer;
  box-shadow: 0 2px 8px rgba(229,57,53,0.08);
  transition: background 0.2s;
}
.sidebar button:hover {
  background: #b71c1c;
}
</style>
