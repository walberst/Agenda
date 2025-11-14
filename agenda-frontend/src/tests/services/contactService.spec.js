import { describe, it, expect, vi, beforeEach } from 'vitest'

import api from '@/services/api'
import { getContacts, saveContact, deleteContact, updateContact } from '@/services/contactService'

describe('contactService', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  it('getContacts retorna data e status', async () => {
    api.get.mockResolvedValue({ data: [{ id: 1 }], status: 200 })
    const res = await getContacts()
    expect(api.get).toHaveBeenCalledWith('/v1/contacts')
    expect(res.data).toEqual([{ id: 1 }])
    expect(res.status).toBe(200)
  })

  it('saveContact chama post', async () => {
    api.post.mockResolvedValue({ data: { id: 2 }, status: 201 })
    const res = await saveContact({ name: 'X' })
    expect(api.post).toHaveBeenCalledWith('/v1/contacts', { name: 'X' })
    expect(res.data).toEqual({ id: 2 })
  })

  it('deleteContact e updateContact chamam endpoints corretos', async () => {
    api.delete.mockResolvedValue({})
    api.put.mockResolvedValue({ data: { id: 3 } })

    await deleteContact(3)
    expect(api.delete).toHaveBeenCalledWith('/v1/contacts/3')

    await updateContact(3, { name: 'Y' })
    expect(api.put).toHaveBeenCalledWith('/v1/contacts/3', { name: 'Y' })
  })
})
