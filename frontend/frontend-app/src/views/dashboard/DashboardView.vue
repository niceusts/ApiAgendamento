<template>
  <div class="dashboard-container">
    <h2>Bem-vindo ao sistema!</h2>
    <p>Você está autenticado.</p>
    <button @click="logout">Sair</button>
  </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'

const router = useRouter()

function logout() {
  localStorage.removeItem('token')
  router.push({ name: 'login' })
}
</script>

<style scoped>
.dashboard-container {
  max-width: 500px;
  margin: 60px auto;
  padding: 2rem;
  border-radius: 8px;
  background: #fff;
  box-shadow: 0 2px 8px rgba(0,0,0,0.08);
  text-align: center;
}
.dashboard-container h2 {
  margin-bottom: 1rem;
}
.dashboard-container button {
  margin-top: 2rem;
  padding: 0.7rem 2rem;
  background: #1976d2;
  color: #fff;
  border: none;
  border-radius: 4px;
  font-weight: bold;
  cursor: pointer;
}
</style>
