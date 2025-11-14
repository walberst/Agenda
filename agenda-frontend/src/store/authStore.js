import { defineStore } from 'pinia'
import { loginUser, registerUser, getLoggedUser } from '@/services/authService'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: null,
    token: null,
    loading: false,
    error: null
  }),

  actions: {
    async login(credentials) {
      this.loading = true
      this.error = null
      try {
        const response = await loginUser(credentials)
        this.token = response.token

        // Salva no localStorage
        localStorage.setItem('token', this.token)

        const userData = await getLoggedUser()
        this.user = userData
        localStorage.setItem('user', JSON.stringify(userData))

        return true
      } catch (err) {
        this.error = err.message
        return false
      } finally {
        this.loading = false
      }
    },
    async fetchLoggedUser() {
      try {
        const userData = await getLoggedUser()
        this.user = userData
      } catch (err) {
        this.user = null
      }
    },

    async register(data) {
      this.loading = true
      this.error = null
      try {
        await registerUser(data)
        return true
      } catch (err) {
        this.error = err.message
        return false
      } finally {
        this.loading = false
      }
    },

    logout() {
      this.user = null
      this.token = null
      localStorage.removeItem('token')
      localStorage.removeItem('user')
       router.push('/')
    },

    getLoggedUser() {
      if (!this.user) {
        const userData = localStorage.getItem('user')
        this.user = userData ? JSON.parse(userData) : null
      }
      return this.user
    },

    isLoggedIn() {
      return !!this.token || !!localStorage.getItem('token')
    }
  }
})
