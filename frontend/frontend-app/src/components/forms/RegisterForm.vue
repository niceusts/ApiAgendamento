<template>
  <div class="register-container">
    <h2>Cadastro</h2>
    <form @submit.prevent="handleRegister">
      <div>
        <label for="nome">Nome</label>
        <input v-model="nome" type="text" id="nome" required />
      </div>
      <div>
        <label for="email">Email</label>
        <input v-model="email" type="email" id="email" required />
      </div>
      <div>
        <label for="senha">Senha</label>
        <input v-model="senha" type="password" id="senha" required />
      </div>
      <div>
        <label for="tipo">Tipo de usuário</label>
        <select v-model="tipo" id="tipo" required>
          <option value="paciente">Paciente</option>
          <option value="medico">Médico</option>
        </select>
      </div>
      <div v-if="tipo === 'medico'">
        <label for="especialidade">Especialidade</label>
        <select v-model="especialidade" id="especialidade" required>
          <option value="" disabled>Selecione a especialidade</option>
          <option v-for="esp in especialidades" :key="esp" :value="esp">{{ esp }}</option>
        </select>
      </div>
      <button type="submit">Registrar</button>
      <p v-if="error" class="error">{{ error }}</p>
      <p v-if="success" class="success">Cadastro realizado com sucesso!</p>
    </form>
    <div style="margin-top: 10px; text-align: center;">
      <p>Tem uma conta?</p><a href="/login"> Faça login</a>
    </div>
  </div>
</template>

<script setup lang="ts">

import { ref } from 'vue'
import api from '../../services/api'

const nome = ref('')
const email = ref('')
const senha = ref('')
const tipo = ref('paciente')
const especialidade = ref('')
const error = ref('')
const success = ref(false)

const especialidades = [
  'Cardiologia',
  'Dermatologia',
  'Endocrinologia',
  'Ginecologia',
  'Neurologia',
  'Oftalmologia',
  'Ortopedia',
  'Pediatria',
  'Psiquiatria',
  'Urologia',
]

const handleRegister = async () => {
  error.value = ''
  success.value = false
  try {
    const payload: {
      nome: string
      email: string
      senha: string
      tipo: string
      especialidade?: string
    } = {
      nome: nome.value,
      email: email.value,
      senha: senha.value,
      tipo: tipo.value,
    }
    if (tipo.value === 'medico') {
      payload.especialidade = especialidade.value
    }
    await api.post('/auth/register', payload)
    success.value = true
    nome.value = ''
    email.value = ''
    senha.value = ''
    tipo.value = 'paciente'
    especialidade.value = ''
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  } catch (err: any) {
    error.value = err.response?.data?.message
  }
}
</script>

<style scoped>
.register-container {
  max-width: 400px;
  margin: 60px auto;
  padding: 2rem;
  border-radius: 8px;
  background: #fff;
  box-shadow: 0 2px 8px rgba(0,0,0,0.08);
}
.register-container h2 {
  text-align: center;
  margin-bottom: 1.5rem;
}
.register-container label {
  display: block;
  margin-bottom: 0.3rem;
}
.register-container input,
.register-container select {
  width: 100%;
  padding: 0.5rem;
  margin-bottom: 1rem;
  border: 1px solid #ccc;
  border-radius: 4px;
}
.register-container button {
  width: 100%;
  padding: 0.7rem;
  background: #1976d2;
  color: #fff;
  border: none;
  border-radius: 4px;
  font-weight: bold;
  cursor: pointer;
}
.register-container .error {
  color: #d32f2f;
  text-align: center;
  margin-top: 1rem;
}
.register-container .success {
  color: #388e3c;
  text-align: center;
  margin-top: 1rem;
}
</style>
