import { defineStore } from 'pinia'
import { getContacts, saveContact, deleteContact, updateContact } from '@/services/contactService'

export const useContactStore = defineStore('contacts', {
  state: () => ({
    contacts: [],
    loading: false,
    error: null
  }),

  actions: {
    async fetchContacts() {
      this.loading = true
      try {
      const { data, status } = await getContacts()
      this.contacts = data   // atualiza o estado do store
      return { data, status } // retorna para quem chamou
    } catch (err) {
      this.error = err.message
      return { data: [], status: 500 } // opcional, para garantir retorno
    } finally {
      this.loading = false
    }
    },

    async addContact(data) {
      const newContact = await saveContact(data)
      this.contacts.push(newContact)
    },

    async removeContact(id) {
      await deleteContact(id)
      this.contacts = this.contacts.filter(c => c.id !== id)
    },

    async updateContact(id, data) {
      const updatedContact = await updateContact(id, data)
      const index = this.contacts.findIndex(c => c.id === id)
      if (index !== -1) {
        this.contacts[index] = updatedContact
      }
    }
  }
})
