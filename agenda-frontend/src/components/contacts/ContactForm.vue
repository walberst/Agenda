<template>
    <Dialog 
        :visible="visible"
        @update:visible="value => $emit('update:visible', value)"
        modal
        header="Contato"
        :style="{ width: '420px' }"
        class="contact-dialog"
    >
        <div class="form-wrapper">
            
            <InputText 
                v-model="form.name" 
                placeholder="Nome completo" 
                class="input"
            />

            <InputText 
                v-model="form.email"
                type="email"
                placeholder="E-mail"
                class="input"
            />

            <InputMask 
                v-model="form.phone"
                mask="(99) 99999-9999"
                slotChar="_"
                placeholder="Telefone"
                class="input"
            />

            <InputText
                v-model="form.birthDate"
                type="date"
                class="input"
            />

            <InputText 
                v-model="form.address" 
                placeholder="Endereço"
                class="input"
            />

            <Button 
                label="Salvar" 
                @click="save" 
                class="save-button"
            />
        </div>
    </Dialog>
</template>

<script setup>
import { ref, watch } from 'vue'
import { useContactStore } from '@/store/contactStore'
import InputMask from 'primevue/inputmask'
import InputText from 'primevue/inputtext'
import Button from 'primevue/button'

const props = defineProps({
    visible: Boolean,
    contact: { type: Object, default: null }
})

const emit = defineEmits(['update:visible'])
const store = useContactStore()

const form = ref({
    id: null,
    name: '',
    email: '',
    phone: '',
    birthDate: '',
    address: ''
})

watch(() => props.contact, (c) => {
  form.value = c
    ? { 
        id: c.id,
        name: c.name || '',
        email: c.email || '',
        phone: c.phone || '',
        birthDate: c.birthDate ? c.birthDate.substring(0, 10) : '',
        address: c.address || ''
      }
    : { id: null, name: '', email: '', phone: '', birthDate: '', address: '' }
})


async function save() {
  if (!form.value.name.trim()) return alert('Informe o nome do contato.')

  const contactDto = {
    name: form.value.name,
    email: form.value.email,
    phone: form.value.phone,
    address: form.value.address || null,
    birthDate: form.value.birthDate ? new Date(form.value.birthDate).toISOString() : null,
  }

  if (form.value.id) {
    // Editando
    await store.updateContact(form.value.id, contactDto)
  } else {
    // Criando
    await store.addContact(contactDto)
  }

  emit('update:visible', false)
}

</script>


<style scoped>
/* ===== MODAL ===== */
.contact-dialog .p-dialog-header {
    background: #00283b;
    color: #00eaff;
    border-bottom: 1px solid rgba(0,255,255,0.3);
    text-shadow: 0 0 6px rgba(0,255,255,0.4);
}

.contact-dialog .p-dialog-content {
    background: #001f2f;
    color: #e6ffff !important;
    padding-top: 1.5rem !important;
}

.contact-dialog .p-dialog-footer {
    background: #00283b;
    border-top: 1px solid rgba(0,255,255,0.3);
}

/* ===== FORM ===== */
.form-wrapper {
    display: flex;
    flex-direction: column;
    gap: 1rem;
}

/* ===== INPUTS ===== */
.input {
    background: #00293d !important;
    border: 1px solid rgba(0,255,255,0.2) !important;
    padding: 0.65rem !important;
    border-radius: 6px !important;
    color: #c8faff !important;
    transition: 0.25s ease;
    box-shadow: 0 0 6px rgba(0,255,255,0.1);
}

.input:focus {
    border-color: #00eaff !important;
    box-shadow: 0 0 10px rgba(0,255,255,0.35);
}

/* ===== BOTÃO ===== */
.save-button {
    background: #00ffff !important;
    color: #003d4d !important;
    border: none !important;
    font-weight: 600;
    padding: 0.7rem 1rem;
    border-radius: 8px;
    transition: 0.25s ease;
    box-shadow: 0 0 8px rgba(0,255,255,0.4);
}

.save-button:hover {
    background: #00d6e6 !important;
    box-shadow: 0 0 14px rgba(0,255,255,0.6);
}
</style>
