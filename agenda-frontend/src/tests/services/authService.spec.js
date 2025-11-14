import { describe, it, expect, vi, beforeEach } from 'vitest'


import api from '@/services/api'
import { loginUser, registerUser, getLoggedUser } from '@/services/authService'

describe('authService', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  it('loginUser retorna dados', async () => {
    api.post.mockResolvedValue({ data: { token: 'tk' } })
    const res = await loginUser({ email: 'a', password: 'b' })
    expect(api.post).toHaveBeenCalledWith('/v1/users/login', { email: 'a', password: 'b' })
    expect(res).toEqual({ token: 'tk' })
  })

  it('registerUser chama api.post', async () => {
    api.post.mockResolvedValue({ data: { id: 1 } })
    const res = await registerUser({ name: 'x' })
    expect(api.post).toHaveBeenCalledWith('/v1/users/register', { name: 'x' })
    expect(res).toEqual({ id: 1 })
  })

  it('getLoggedUser chama api.get', async () => {
    api.get.mockResolvedValue({ data: { id: 2 } })
    const res = await getLoggedUser()
    expect(api.get).toHaveBeenCalledWith('/v1/users')
    expect(res).toEqual({ id: 2 })
  })
})
