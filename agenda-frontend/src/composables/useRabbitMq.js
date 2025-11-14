import { ref, onUnmounted } from 'vue'

export function useRabbitMq(queueName) {
  const messages = ref([])

  const connection = new WebSocket('ws://localhost:15675/ws')
  const client = Stomp.over(connection)

  client.connect('guest', 'guest', () => {
    client.subscribe(`/queue/${queueName}`, (msg) => {
      messages.value.push(JSON.parse(msg.body))
    })
  })

  onUnmounted(() => client.disconnect())

  return { messages }
}
