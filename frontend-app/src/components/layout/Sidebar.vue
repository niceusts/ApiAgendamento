<!-- eslint-disable vue/multi-word-component-names -->
<template>
  <button class="sidebar-toggle" @click="toggleSidebar" aria-label="Abrir menu">
    <span v-if="!sidebarAberto">☰</span>
    <span v-else>×</span>
  </button>
  <aside class="sidebar" :class="{ aberto: sidebarAberto }">
    <nav>
      <ul>
        <template v-if="tipoUsuario === 'medico'">
          <div>
            <div class="sidebar-nome">{{ nomeUsuario }}</div>
            <div class="sidebar-tipo">{{ tipoUsuario }}</div>
            <li><router-link to="/dashboard">Dashboard</router-link></li>
            <li><router-link to="/listaHorarios">Meus Horários</router-link></li>
            <li><router-link to="/listaAgenda">Meus Pacientes</router-link></li>
          </div>
          <button @click="logout">Sair</button>
        </template>
        <template v-else-if="tipoUsuario === 'paciente'">
            <div>
            <div class="sidebar-nome">{{ nomeUsuario }}</div>
            <div class="sidebar-tipo">{{ tipoUsuario }}</div>
            <li><router-link to="/dashboard">Dashboard</router-link></li>
            <li><router-link to="/">Horários Disponiveis</router-link></li>
            <li><router-link to="/">Meus Agendamentos</router-link></li>
          </div>
          <button @click="logout">Sair</button>
        </template>
      </ul>
    </nav>
  </aside>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
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

const sidebarAberto = ref(window.innerWidth > 900)

function toggleSidebar() {
  sidebarAberto.value = !sidebarAberto.value
}

function handleResize() {
  sidebarAberto.value = window.innerWidth > 900
}

onMounted(() => {
  tipoUsuario.value = localStorage.getItem('tipo') || ''
  nomeUsuario.value = localStorage.getItem('nome') || ''
  window.addEventListener('resize', handleResize)
})
</script>

<style scoped>

.sidebar-toggle {
  display: none;
  position: fixed;
  top: 1rem;
  left: 1rem;
  z-index: 2001;
  background: #1976d2;
  color: #fff;
  border: none;
  border-radius: 4px;
  font-size: 2rem;
  padding: 0.3rem 0.8rem;
  cursor: pointer;
  box-shadow: 0 2px 8px rgba(0,0,0,0.08);
}

@media (max-width: 900px) {
  .sidebar-toggle {
    display: block;
  }
  .sidebar {
    left: -240px;
    transition: left 0.3s;
    z-index: 2000;
  }
  .sidebar.aberto {
    left: 0;
    box-shadow: 2px 0 8px rgba(0,0,0,0.18);
  }
}

.sidebar-nome {
  font-size: 1.1rem;
  font-weight: bold;
  color: #fff;
  text-align: center;
}
.sidebar-tipo {
  font-size: 0.8rem;
  color: #fff;
  text-align: center;
  margin-bottom: 1.5rem;
}
.sidebar {
  width: 220px;
  min-height: 100vh;
  background: #1976d2;
  color: #fff;
  padding: 2rem 1rem;
  box-shadow: 2px 0 8px rgba(0,0,0,0.04);
  position: fixed;
  top: 0;
  left: 0;
  display: flex;
  flex-direction: column;
  transition: left 0.3s;
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
}
.sidebar nav ul {
  min-height: 90vh;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}
.sidebar nav a.router-link-exact-active {
  color: #ffeb3b;
}
.sidebar nav a:hover {
  color: #bbdefb;
}
.sidebar button {
  margin-top: 2rem;
  padding: 0.7rem 2rem;
  background: red;
  color: #fff;
  border: none;
  border-radius: 4px;
  font-weight: bold;
  cursor: pointer;
}

.sidebar button:hover {
  background: darkred;
}

</style>
