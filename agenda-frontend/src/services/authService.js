import api from './api'

export async function loginUser(credentials) {
  const { data } = await api.post('/v1/users/login', credentials)
  return data
}

export async function registerUser(data) {
  const res = await api.post('/v1/users/register', data)
  return res.data
}

export async function getLoggedUser() {
  const res = await api.get('/v1/users')
  return res.data
}