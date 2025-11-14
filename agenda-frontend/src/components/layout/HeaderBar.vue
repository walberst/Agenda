<template>
  <header class="header-bar">
    <h1 class="title">Agenda .Net + Vue.js</h1>

    <!-- Mensagem de boas-vindas -->
    <p v-if="logged" class="welcome-text">
      Bem-vindo(a), {{ userName }}!
    </p>

    <div class="actions" v-if="logged">
      <Button 
        class="action-btn add" 
        @click="openNewContact"
      >
        <i class="pi pi-plus mr-2"></i>
        Novo Contato
      </Button>

      <Button 
        class="action-btn logout" 
        @click="handleLogout"
      >
        <i class="pi pi-sign-out mr-2"></i>
        Sair
      </Button>
    </div>
  </header>

  <!-- Modal de Contato -->
  <ContactForm 
    v-model:visible="showModal"
    @close="showModal = false"
  />
</template>

<script setup>
import { ref, computed } from 'vue'
import { useAuthStore } from '../../store/authStore'
import ContactForm from '../contacts/ContactForm.vue'

const authStore = useAuthStore()

const logged = computed(() => authStore.isLoggedIn())
const showModal = ref(false)

// Nome do usuário logado
const userName = computed(() => {
  const user = authStore.getLoggedUser() // { name: 'Antonio', ... }
  return user?.name || 'Usuário'
})

function handleLogout() {
  authStore.logout()
}

function openNewContact() {
  showModal.value = true
}
</script>


<style scoped>
.header-bar {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  align-items: center;

  background: linear-gradient(90deg, #002f47, #001a27);
  color: #00eaff;

  padding: 1rem 2rem;
  border-bottom: 1px solid rgba(0, 255, 255, 0.25);
  box-shadow: 0 4px 18px rgba(0, 255, 255, 0.12);
  margin-bottom: 20px;
}

.title {
  font-size: 1.7rem;
  font-weight: 700;
  letter-spacing: 1.5px;
  text-shadow: 0 0 8px rgba(0,255,255,0.3);
}

.actions {
  display: flex;
  gap: 0.75rem;
}

/* BOTÕES */

.action-btn {
  display: flex;
  align-items: center;
  gap: 0.4rem;

  padding: 0.45rem 1rem;
  border-radius: 8px;
  border: none;
  cursor: pointer;

  font-size: 0.9rem;
  font-weight: 600;
  letter-spacing: 0.5px;

  transition: all 0.25s ease;

  box-shadow: 0 0 6px rgba(0, 255, 255, 0.3);
}

.action-btn i {
  font-size: 1rem;
}

/* Botão adicionar */
.add {
  background: #00ffff;
  color: #003d4d;
}
.add:hover {
  background: #00d6e6;
  box-shadow: 0 0 10px rgba(0, 255, 255, 0.5);
}

/* Botão logout */
.logout {
  background: #ff4d4d;
  color: #fff;
}
.logout:hover {
  background: #ff3333;
  box-shadow: 0 0 10px rgba(255, 77, 77, 0.5);
}
</style>
