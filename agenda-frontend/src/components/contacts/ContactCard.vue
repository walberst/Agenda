<template>
  <div class="contact-card">
    <div><strong>Nome:</strong> {{ contact?.name || '' }}</div>
    <div><strong>Email:</strong> {{ contact?.email || '' }}</div>
    <div><strong>Telefone:</strong> {{ contact?.phone || '' }}</div>
    <div><strong>Endereço:</strong> {{ contact?.address || '' }}</div>
    <div><strong>Nascimento:</strong> {{ formatDate(contact?.birthDate) }}</div>

    <!-- Botões apenas se o contato tiver conteúdo -->
    <div class="card-buttons" v-if="contact?.id">
      <Button icon="pi pi-pencil" class="p-button-sm p-button-rounded p-button-info" @click="openEditModal" />
      <Button icon="pi pi-trash" class="p-button-sm p-button-rounded p-button-danger" @click="confirmDelete" />
    </div>

    <!-- Modal de edição -->
    <ContactForm :visible="showEditModal" :contact="contact" @update:visible="showEditModal = $event"
      @saved="handleSaved" />
  </div>
</template>

<script setup>
import Button from 'primevue/button'
import ContactForm from '@/components/contacts/ContactForm.vue'
import { reactive, ref } from 'vue'
import { useContactStore } from '@/store/contactStore'

const store = useContactStore()

const props = defineProps({
  contact: Object
})
const emit = defineEmits(['delete'])

const showEditModal = ref(false)
const editableContact = reactive({ ...props.contact })

function formatDate(dateStr) {
  if (!dateStr) return ''
  const date = new Date(dateStr)
  return date.toLocaleDateString('pt-BR', { day: '2-digit', month: '2-digit', year: 'numeric' })
}

function openEditModal() {
  // Atualiza os dados do formulário
  showEditModal.value = true
}

function handleSaved(updatedContact) {
  // Atualiza o contato localmente se necessário
  Object.assign(editableContact, updatedContact)
  showEditModal.value = false
}

async function confirmDelete() {
  if (confirm(`Deseja realmente deletar o contato "${props.contact.name}"?`)) {
    await store.removeContact(props.contact.id)
  }
}
</script>

<style scoped>
.contact-card {
  background: #fff8e7;
  color: #333;
  padding: 10px;
  border-radius: 8px;
  box-shadow: 0 3px 6px rgba(0, 0, 0, 0.2);
  font-size: 0.85rem;
  text-align: left;
  min-height: 100px;
  position: relative;
  box-sizing: border-box;
  height: 170px;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  align-items: flex-start;
}

.contact-card div {
  margin-bottom: 3px;
}

.contact-card strong {
  color: #5d4037;
}

.card-buttons {
  position: absolute;
  display: flex;
  flex-direction: column;
  top: 50%;
  transform: translateY(-50%);
  right: 4px;
  gap: 6px;
}
</style>
