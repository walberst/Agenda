<template>
  <div class="home-container">
    <!-- Header quando agenda aberta -->
    <HeaderBar v-if="agendaOpen" @new-contact="openCreateContact" @logout="logout" />
    <!-- Agenda -->
    <div class="agenda" :class="{ open: agendaOpen }">
      <div class="spine"></div>

      <!-- Capa -->
      <div class="cover">
        <div class="label">Lista de Contatos</div>
        <div class="cover-shadow"></div>
      </div>

      <!-- Páginas dinâmicas -->
      <div v-for="(page, pageIndex) in paginatedContacts" :key="pageIndex" class="page" :class="[
        'page' + (pageIndex + 1),
        { visible: isPageVisible(pageIndex), flipped: isPageFlipped(pageIndex) }
      ]">
        <div class="page-content">
          <div v-for="(contact, index) in page" :key="'p' + pageIndex + '-' + index">
            <ContactCard :contact="contact" @edit="openEditContact" @delete="deleteContact" />
          </div>
        </div>
      </div>

      <!-- Áudios -->
      <audio id="flip-sound" src="@/assets/sounds/page-flip.mp3" preload="auto"></audio>
      <audio id="open-sound" src="@/assets/sounds/page-flip.mp3" preload="auto"></audio>

      <!-- Cadeado -->
      <div class="lock">
        <div class="lock-body"></div>
        <div class="lock-shackle"></div>
      </div>
    </div>

    <!-- Navegação de páginas -->
    <div v-if="agendaOpen" class="nav-buttons">
      <Button icon="pi pi-angle-left" class="p-button-rounded p-button-secondary" :disabled="currentPage <= 0"
        @click="prevPage" />
      <Button icon="pi pi-angle-right" class="p-button-rounded p-button-secondary"
        :disabled="currentPage >= paginatedContacts.length - 2" @click="nextPage" />
    </div>

    <!-- Caixa de aviso -->
    <div class="login-box" v-if="!agendaOpen">
      <h2>Agenda Protegida</h2>
      <Button label="Destrancar Agenda" class="p-button-danger" @click="prepareAudioAndShowLogin" />
    </div>

    <!-- Modal de Login -->
    <LoginModal :visible="showLogin" @update:visible="showLogin = $event" @login-success="handleLoginSuccess"
      @open-register="openRegisterModal" />

    <!-- Modal de Registro -->
    <RegisterModal :visible="showRegister" @update:visible="showRegister = $event" @show-login="openLoginModal" />
  </div>
</template>

<script setup>
import { ref, computed, onMounted,watch  } from 'vue'
import LoginModal from '@/components/auth/LoginModal.vue'
import RegisterModal from '@/components/auth/RegisterModal.vue'
import Button from 'primevue/button'
import HeaderBar from '@/components/layout/HeaderBar.vue'
import ContactCard from '@/components/contacts/ContactCard.vue'

// Estado
const showLogin = ref(false)
const showRegister = ref(false)
const agendaOpen = ref(false)
const currentPage = ref(0)

import flipSoundFile from '@/assets/sounds/page-flip.mp3'
import openSoundFile from '@/assets/sounds/page-flip.mp3'
import { useContactStore } from '@/store/contactStore'
import { useAuthStore } from '../store/authStore'

const authStore = useAuthStore()
const store = useContactStore()

const isLoggedIn = computed(() => authStore.isLoggedIn())
watch(isLoggedIn, (loggedIn) => {
  if (!loggedIn) {
    agendaOpen.value = false
    showLogin.value = true
    contacts.value = []
  }
})

let flipSound = null
let openSound = null
// Contatos
const contacts = ref([])

onMounted(async () => {
  flipSound = new Audio(flipSoundFile)
  openSound = new Audio(openSoundFile)
  flipSound.preload = 'auto'
  openSound.preload = 'auto'

  // Se já estiver logado, abre direto a agenda
  if (authStore.isLoggedIn()) {
    try {
      const response = await store.fetchContacts()
      console.log('Contatos carregados automaticamente:', response)
      contacts.value = response.data
      showLogin.value = false
      agendaOpen.value = true
      playOpenSound()
    } catch (e) {
      console.error('Erro ao carregar contatos automaticamente', e)
      showLogin.value = true
      agendaOpen.value = false
    }
  }
})



// Sons
function playFlipSound() {
  if (!flipSound) return
  flipSound.currentTime = 0
  flipSound.play().catch(err => console.warn('Som bloqueado:', err))
}

function playOpenSound() {
  if (!openSound) return
  openSound.currentTime = 0
  openSound.play().catch(err => console.warn('Som bloqueado:', err))
}

// Paginação
const paginatedContacts = computed(() => {
  const perPage = 6
  const pages = []
  for (let i = 0; i < contacts.value.length; i += perPage) {
    const page = contacts.value.slice(i, i + perPage)
    while (page.length < perPage) {
      page.push({ name: '', email: '', phone: '', address: '', birthday: '' })
    }
    pages.push(page)
  }

  if (pages.length === 0) {
    const blankPage = Array.from({ length: perPage }, () => ({
      name: '', email: '', phone: '', address: '', birthday: ''
    }))
    pages.push(blankPage)
  }

  if (pages.length % 2 !== 0 || pages.length === 0) {
    const blankPage = Array.from({ length: perPage }, () => ({
      name: '', email: '', phone: '', address: '', birthday: ''
    }))
    pages.push(blankPage)
  }
  return pages
})

// Navegação
const isPageVisible = (i) => i === currentPage.value || i === currentPage.value + 1
const isPageFlipped = (i) => i < currentPage.value

function nextPage() {
  if (currentPage.value < paginatedContacts.value.length - 2) {
    playFlipSound()
    currentPage.value += 2
  }
}

function prevPage() {
  if (currentPage.value > 0) {
    playFlipSound()
    currentPage.value -= 2
  }
}

// Pré-autorização do som 
function prepareAudioAndShowLogin() {
  flipSound?.play().then(() => flipSound.pause())
  openSound?.play().then(() => openSound.pause())
  showLogin.value = true
}

// Login com sucesso 
async function handleLoginSuccess() {
  try {
    const response = await store.fetchContacts()
    console.log('Contatos carregados após login:', response)
    contacts.value = response.data
    if (response.status === 200) {
      agendaOpen.value = true
      showLogin.value = false
      playOpenSound()
    }
  } catch (e) {
    agendaOpen.value = false
    showLogin.value = true
  }
}

// Alternar modais
function openRegisterModal() {
  showLogin.value = false
  showRegister.value = true
}

function openLoginModal() {
  showRegister.value = false
  showLogin.value = true
}
</script>

<style scoped>
@import './css/HomeViewStyle.css';
</style>
