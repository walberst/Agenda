import { describe, it, expect, beforeEach, vi } from 'vitest'
import { setActivePinia, createPinia } from 'pinia'
import { useAuthStore } from '@/store/authStore'

// Mocks dos serviços
vi.mock('@/services/authService', () => ({
  loginUser: vi.fn(),
  logoutUser: vi.fn(),
  getLoggedUser: vi.fn(),
  registerUser: vi.fn(),
}))

// Mock do router
vi.mock('@/router', () => ({
  default: {
    push: vi.fn(),
  },
}))

import { loginUser, getLoggedUser } from '@/services/authService'
import router from '@/router'

describe('authStore', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
    vi.clearAllMocks()
    localStorage.clear()
  })

  it('faz login e salva token + usuário', async () => {
    loginUser.mockResolvedValue({ token: '123' })
    getLoggedUser.mockResolvedValue({ id: 1, name: 'Walber' })

    const store = useAuthStore()

    await store.login({ email: 'a@a.com', password: '123' })

    expect(store.token).toBe('123')
    expect(store.user.name).toBe('Walber')
    expect(localStorage.getItem('token')).toBe('123')
    expect(localStorage.getItem('user')).toBe(JSON.stringify({ id: 1, name: 'Walber' }))
  })

  it('faz logout limpando store e localStorage', () => {
    const store = useAuthStore()
    store.token = '123'
    store.user = { id: 1, name: 'Walber' }

    store.logout()

    expect(store.token).toBe(null)
    expect(store.user).toBe(null)
    expect(localStorage.getItem('token')).toBe(null)
    expect(localStorage.getItem('user')).toBe(null)
    expect(router.push).toHaveBeenCalledWith('/')
  })

  it('carrega o usuário logado do localStorage', () => {
    const store = useAuthStore()
    localStorage.setItem('user', JSON.stringify({ id: 10, name: 'Antonio' }))

    const user = store.getLoggedUser()

    expect(user.id).toBe(10)
    expect(user.name).toBe('Antonio')
  })

  it('retorna null se não houver usuário no localStorage', () => {
    const store = useAuthStore()
    localStorage.removeItem('user')

    const user = store.getLoggedUser()

    expect(user).toBe(null)
  })
})
