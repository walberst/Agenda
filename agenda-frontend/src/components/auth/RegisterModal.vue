<template>
  <Dialog
    :visible="visible"
    modal
    header="Criar Conta"
    :style="{ width: '380px' }"
    @update:visible="$emit('update:visible', $event)"
    class="login-box"
  >
    <div class="login-content">
      <div class="form-group">
        <InputText v-model="name" placeholder="Nome completo" class="input-field" />
      </div>

      <div class="form-group">
        <InputText v-model="email" placeholder="E-mail" class="input-field" />
      </div>

      <div class="form-group">
        <Password v-model="password" placeholder="Senha" toggleMask class="input-field" />
      </div>

      <Button
        label="Cadastrar"
        icon="pi pi-user-plus"
        class="login-button"
        @click="register"
        :loading="loading"
      />

      <div class="divider"></div>

      <p class="register-text">
        Já tem conta?
        <button class="register-link" @click="openLogin">Entrar</button>
      </p>
    </div>
  </Dialog>
</template>

<script setup>
import { ref, computed, watch } from 'vue'
import { useAuthStore } from '@/store/authStore'
import Dialog from 'primevue/dialog'
import InputText from 'primevue/inputtext'
import Password from 'primevue/password'
import Button from 'primevue/button'

const props = defineProps({ visible: Boolean })
const emit = defineEmits(['update:visible', 'show-login'])

const auth = useAuthStore()
const name = ref('')
const email = ref('')
const password = ref('')

const loading = computed(() => auth.loading)

watch(() => props.visible, v => {
  if (!v) {
    name.value = ''
    email.value = ''
    password.value = ''
  }
})

async function register() {
  const ok = await auth.register({
    name: name.value,
    email: email.value,
    password: password.value
  })

  if (ok) {
    alert('Cadastro realizado com sucesso!')
    emit('update:visible', false)
    emit('show-login')
  } else {
    alert(auth.error || 'Erro ao registrar')
  }
}

function openLogin() {
  emit('update:visible', false)
  emit('show-login')
}
</script>

<style scoped>
@import './css/LoginModalStyle.css';
</style>
