
<template>
  <div style="display: flex; min-height: 100vh;">
    <Sidebar />
    <main style="flex: 1; margin-left: 220px; padding: 2rem;">
      <div class="dashboard-container">
        <h2>Bem-vindo ao sistema!</h2>
        <p>Você está autenticado.</p>

      </div>
    </main>
  </div>
</template>

<script setup lang="ts">
import Sidebar from '../../components/layout/Sidebar.vue'


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

</style>
