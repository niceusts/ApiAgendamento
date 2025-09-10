

<script setup lang="ts">
import { RouterView, useRoute } from 'vue-router'
import { computed } from 'vue'
import Sidebar from './components/layout/Sidebar.vue'

const route = useRoute()
const esconderRotas = ['login', 'register']
const mostrarSidebar = computed(() => !esconderRotas.includes(route.name as string))
</script>

<template>
  <div>
    <div v-if="mostrarSidebar" class="app-flex">
      <Sidebar />
      <main class="main-content">
        <RouterView />
      </main>
    </div>
    <div v-else class="login-center">
      <RouterView />
    </div>
  </div>
</template>


<style scoped>
.login-center {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: #f8f9fa;
  width: 100vw;
}
header {
  line-height: 1.5;
  max-height: 100vh;
}

.logo {
  display: block;
  margin: 0 auto 2rem;
}

nav {
  width: 100%;
  font-size: 12px;
  text-align: center;
  margin-top: 2rem;
}

nav a.router-link-exact-active:hover {
  background-color: transparent;
}

nav a {
  padding: 0 1rem;
  border-left: 1px solid var(--color-border);
}

nav a:first-of-type {
  border: 0;
}

@media (min-width: 1024px) {
  header {
    display: flex;
    place-items: center;
    padding-right: calc(var(--section-gap) / 2);
  }

  .logo {
    margin: 0 2rem 0 0;
  }

  header .wrapper {
    display: flex;
    place-items: flex-start;
    flex-wrap: wrap;
  }

  nav {
    text-align: left;
    margin-left: -1rem;
    font-size: 1rem;

    padding: 1rem 0;
    margin-top: 1rem;
  }
}

.app-flex {
  display: flex;
  min-height: 100vh;
  flex-direction: row;
}

.main-content {
  flex: 1;
  min-width: 0;
  padding: 0;
  display: flex;
  flex-direction: column;
  background: #f8f9fa;
}

@media (max-width: 900px) {
  .app-flex {
    flex-direction: column;
  }
  .main-content {
    margin-left: 0 !important;
    width: 100vw !important;
    padding: 0 !important;
  }
}
</style>
