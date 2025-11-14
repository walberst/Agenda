<template>
  <Dialog :visible="visible" modal header="Entrar na Agenda" :style="{ width: '380px' }"
    @update:visible="$emit('update:visible', $event)" class="login-box">
    <div class="login-content">
      <div class="form-group">
        <InputText v-model="email" placeholder="E-mail" class="input-field" />
      </div>
      <div class="form-group">
        <Password v-model="password" placeholder="Senha" toggleMask class="input-field" />
      </div>
      <Button label="Entrar" icon="pi pi-sign-in" class="login-button" @click="login" :loading="loading" />

      <div class="divider"></div>
      <p class="register-text">
        Ainda não tem uma conta?
        <button class="register-link" @click="openRegister">Criar conta</button>
      </p>
    </div>
  </Dialog>
</template>

<script setup>
import { ref, computed } from 'vue'
import Dialog from 'primevue/dialog'
import InputText from 'primevue/inputtext'
import Password from 'primevue/password'
import Button from 'primevue/button'
import { useAuthStore } from '@/store/authStore'

const props = defineProps({ visible: Boolean })
const emit = defineEmits(['update:visible', 'open-register', 'login-success'])

const auth = useAuthStore()
const email = ref('')
const password = ref('')
const loading = computed(() => auth.loading)

async function login() {
  const ok = await auth.login({ email: email.value, password: password.value })
  if (ok) {
    emit('login-success')
  } else {
    alert(auth.error || 'Falha no login')
  }
}

function openRegister() {
  emit('update:visible', false)
  emit('open-register')
}
</script>

<style scoped>
@import './css/LoginModalStyle.css';
</style>
