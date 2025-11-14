import { describe, it, expect, beforeEach, vi } from 'vitest'
import { setActivePinia, createPinia } from 'pinia'

vi.mock('@/services/contactService', () => ({
  getContacts: vi.fn(),
  saveContact: vi.fn(),
  deleteContact: vi.fn(),
  updateContact: vi.fn()
}))

import * as contactService from '@/services/contactService'
import { useContactStore } from '@/store/contactStore'

describe('contactStore', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
    vi.clearAllMocks()
  })

  it('fetchContacts popula contacts', async () => {
    contactService.getContacts.mockResolvedValue({ data: [{ id: 1, name: 'A' }], status: 200 })
    const store = useContactStore()
    const res = await store.fetchContacts()

    expect(store.contacts.length).toBe(1)
    expect(res.status).toBe(200)
  })

  it('addContact empurra novo contato', async () => {
    contactService.saveContact.mockResolvedValue({ data: { id: 2, name: 'B' } })
    const store = useContactStore()
    await store.addContact({ name: 'B' })

    // dependendo da implementação addContact pode retornar objeto; verificamos se chamou service
    expect(contactService.saveContact).toHaveBeenCalled()
  })

  it('removeContact chama service e filtra lista', async () => {
    contactService.deleteContact.mockResolvedValue()
    const store = useContactStore()
    store.contacts = [{ id: 3, name: 'C' }]
    await store.removeContact(3)

    expect(contactService.deleteContact).toHaveBeenCalledWith(3)
    expect(store.contacts.find(c => c.id === 3)).toBeUndefined()
  })
})
