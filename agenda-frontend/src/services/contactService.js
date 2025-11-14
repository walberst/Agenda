import api from './api'

export async function getContacts() {
  const { data, status } = await api.get('/v1/contacts')
  console.log('DEBUG: getContacts response data:', data)
  return { data, status }
}

export async function saveContact(contact) {
  const { data, status } = await api.post('/v1/contacts', contact)
  return { data, status }
}

export async function deleteContact(id) {
  await api.delete(`/v1/contacts/${id}`)
  window.location.reload()
}

export async function updateContact(id, contact) {
  const { data } = await api.put(`/v1/contacts/${id}`, contact)
  window.location.reload()
}