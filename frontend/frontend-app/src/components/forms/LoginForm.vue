<template>
  <div class="login-container">
    <h2>Login</h2>
    <form @submit.prevent="handleLogin">
      <div>
        <label for="email">Email</label>
        <input v-model="email" type="email" id="email" required />
      </div>
      <div>
        <label for="password">Senha</label>
        <input v-model="password" type="password" id="password" required />
      </div>
      <button type="submit">Entrar</button>
      <p v-if="error" class="error">{{ error }}</p>
    </form>
    <div style="margin-top: 10px; text-align: center;">
      <p>Não tem uma conta?</p><a href="/register"> Cadastre-se</a>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import api from '../../services/api'

const email = ref('')
const password = ref('')
const error = ref('')

const handleLogin = async () => {
  error.value = ''
  try {
    const response = await api.post('/auth/login', {
      email: email.value,
      senha: password.value,
    })
    // Armazena o token JWT
    localStorage.setItem('token', response.data.token)
    // Armazena o tipo de usuário
    localStorage.setItem('tipo', response.data.tipo)
    // Armazena o nome do usuário (médico ou paciente)
    localStorage.setItem('nome', response.data.nome)
    console.log('Nome do usuário armazenado:', response.data.nome)
    // Redireciona para a dashboard
    window.location.href = '/dashboard'


  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  } catch (err: any) {
    error.value = err.response?.data?.message
  }
}
</script>

<style scoped>
.login-container {
  max-width: 350px;
  margin: 60px auto;
  padding: 2rem;
  border-radius: 8px;
  background: #fff;
  box-shadow: 0 2px 8px rgba(0,0,0,0.08);
}
.login-container h2 {
  text-align: center;
  margin-bottom: 1.5rem;
}
.login-container label {
  display: block;
  margin-bottom: 0.3rem;
}
.login-container input {
  width: 100%;
  padding: 0.5rem;
  margin-bottom: 1rem;
  border: 1px solid #ccc;
  border-radius: 4px;
}
.login-container button {
  width: 100%;
  padding: 0.7rem;
  background: #1976d2;
  color: #fff;
  border: none;
  border-radius: 4px;
  font-weight: bold;
  cursor: pointer;
}
.login-container .error {
  color: #d32f2f;
  text-align: center;
  margin-top: 1rem;
}
</style>
