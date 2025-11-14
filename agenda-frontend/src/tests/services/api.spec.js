import { describe, it, expect, beforeEach, vi } from 'vitest'

describe('api instance', () => {
  let api

  beforeEach(async () => {
    // ⚠ Remove QUALQUER mock existente do módulo api
    vi.unmock('@/services/api')
    vi.resetModules()
    localStorage.clear()

    // importa o módulo real
    api = (await import('@/services/api')).default
  })

  it('tem baseURL configurada corretamente', () => {
    expect(api.defaults.baseURL).toBe('https://localhost:5001/api')
  })

  it('tem timeout configurado', () => {
    expect(api.defaults.timeout).toBe(8000)
  })

  it('usa interceptors de request', () => {
    expect(api.interceptors.request.handlers.length).toBeGreaterThan(0)
  })

  it('adiciona Authorization quando existir token', async () => {
    localStorage.setItem('token', 'abc123')

    const config = await api.interceptors.request.handlers[0].fulfilled({
      headers: {}
    })

    expect(config.headers.Authorization).toBe('Bearer abc123')
  })

  it('não adiciona Authorization quando NÃO existir token', async () => {
    localStorage.removeItem('token')

    const config = await api.interceptors.request.handlers[0].fulfilled({
      headers: {}
    })

    expect(config.headers.Authorization).toBeUndefined()
  })
})
