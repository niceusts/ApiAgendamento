import { createRouter, createWebHistory } from 'vue-router'
import LoginView from '../views/auth/LoginView.vue'
import DashboardView from '../views/dashboard/DashboardView.vue'
import RegisterView from '../views/auth/RegisterView.vue'
import AdicionarHorarioView from '@/components/medico/AdicionarHorario.vue'
import ListaHorariosView from '@/components/medico/ListaHorarios.vue'
import ListaAgendaView from '@/components/medico/ListaAgendas.vue'
import AtualizarHorarioView from '@/components/medico/AtualizarHorario.vue'
import ListaHorariosDisponiveisPacienteView from '@/components/paciente/ListaHorariosDisponiveisPaciente.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      redirect: '/login',
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView,
    },
    {
      path: '/register',
      name: 'register',
      component: RegisterView,
    },
    {
      path: '/dashboard',
      name: 'dashboard',
      component: DashboardView,
      meta: { requiresAuth: true },
    },
    {
      path: '/AdicionarHorario',
      name: 'adicionarHorario',
      component: AdicionarHorarioView,
      meta: { requiresAuth: true },
    },
    {
      path: '/listaHorarios',
      name: 'listaHorarios',
      component: ListaHorariosView,
      meta: { requiresAuth: true },
    },
    {
      path: '/listaAgenda',
      name: 'listaAgenda',
      component: ListaAgendaView,
      meta: { requiresAuth: true },
    },
    {
      path: '/atualizarHorario/:horarioId',
      name: 'atualizarHorario',
      component: AtualizarHorarioView,
      meta: { requiresAuth: true },
    },
    {
      path: '/listaHorariosDisponiveis',
      name: 'listaHorariosDisponiveis',
      component: ListaHorariosDisponiveisPacienteView,
      meta: { requiresAuth: true },
    }
  ],
})

// Navigation guard para proteger rotas autenticadas
router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token')
  if (to.meta.requiresAuth && !token) {
    next({ name: 'login' })
  } else if (to.name === 'login' && token) {
    next({ name: 'dashboard' })
  } else {
    next()
  }
})

export default router
