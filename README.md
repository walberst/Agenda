# 📅 Agenda App

<p align="center">
  <img src="https://img.shields.io/badge/Status-Em%20Desenvolvimento-yellow?style=for-the-badge" alt="Status do Projeto">
  <img src="https://img.shields.io/badge/Backend-.NET%208-512BD4?style=for-the-badge&logo=dotnet" alt="Tecnologia Backend">
  <img src="https://img-br.vercel.app/badges/database/MySQL-00758F?style=for-the-badge&logo=mysql" alt="Banco de Dados">
  <img src="https://img.shields.io/badge/Frontend-Vue.js%203-4FC08D?style=for-the-badge&logo=vue.js" alt="Tecnologia Frontend">
  <img src="https://img-br.vercel.app/badges/UI%20Library/PrimeVue-4286f4?style=for-the-badge&logo=primefaces" alt="UI Library">
  <img src="https://img.shields.io/badge/Containerization-Docker%20Compose-2496ED?style=for-the-badge&logo=docker" alt="Containerization">
</p>

## 📝 Descrição do Projeto

Este projeto é uma aplicação de **Gerenciamento de Contatos/Agenda** desenvolvida em arquitetura Full-Stack. O objetivo é fornecer uma API robusta e um frontend moderno para a gestão de informações de contato e usuários.

O **Backend** é construído com **ASP.NET Core Web API** e segue os princípios da **Arquitetura Limpa (Clean Architecture)**, dividindo as responsabilidades em camadas: `Domain`, `Application`, `Infrastructure` e `Api`. O **Frontend** é uma **Single Page Application (SPA)** desenvolvida com **Vue.js (Vite)** e utiliza a biblioteca **PrimeVue** para componentes de interface.

---

## 🛠️ Tecnologias Utilizadas

### Backend (Agenda.Api)

* **Linguagem:** C# (.NET 8)
* **Framework:** ASP.NET Core Web API
* **Arquitetura:** Clean Architecture / Domain-Driven Design (DDD)
* **Padrões:** CQRS (Commands/Queries), Repositório
* **Banco de Dados:** **MySQL** (Gerenciado via Entity Framework Core)
* **Autenticação:** JWT (JSON Web Tokens)

### Frontend (agenda-frontend)

* **Framework:** Vue.js 3 (Composition API)
* **UI Library:** **PrimeVue** (Para componentes de interface)
* **Build Tool:** Vite
* **Roteamento:** Vue Router
* **Gerenciamento de Estado:** Vuex/Pinia (Configurado na pasta `store`)
* **Linguagem:** JavaScript

### Infraestrutura/DevOps

* **Containerização:** Docker e Docker Compose

---

## 🚀 Como Rodar o Projeto

### Pré-requisitos

Certifique-se de ter instalado em sua máquina:

* **.NET SDK 8.0** ou superior
* **Node.js 20.x** ou superior
* **Docker Desktop** (para rodar com Docker Compose)

### Opção 1: Usando Docker Compose (Recomendado)

O Docker Compose é a maneira mais fácil de iniciar todos os serviços (**MySQL**, API e Frontend) simultaneamente.

1.  **Navegue até o diretório raiz** do projeto (onde está o `docker-compose.yml`):
    ```bash
    cd /caminho/para/Agenda.Net.sln
    ```
2.  **Suba os contêineres:**
    ```bash
    docker-compose up --build -d
    ```
    * A flag `--build` garante que as imagens mais recentes sejam construídas. O `-d` roda em background.

3.  **Aguarde e Verifique:** Espere alguns segundos para o contêiner do **MySQL** iniciar completamente. O serviço **`agenda-api`** está configurado para **aplicar automaticamente as Migrações do Entity Framework Core** no MySQL durante sua inicialização (usando o `entrypoint.sh`).

4.  **Acesse a Aplicação:**
    * **Frontend:** `http://localhost:80`
    * **Backend API (Swagger):** `http://localhost:8080/swagger`

### Opção 2: Localmente (Sem Docker)

#### 1. Backend (API)

1.  **Configure o MySQL:** Certifique-se de que um servidor MySQL esteja rodando e que as *connection strings* no `appSettings.json` apontem para ele.
2.  **Navegue até o diretório da API:**
    ```bash
    cd Agenda.Api
    ```
3.  **Restaure as dependências e aplique as Migrações:**
    ```bash
    dotnet restore
    # O comando aplica as migrações usando o projeto de Infraestrutura
    dotnet ef database update --project ../Agenda.Infrastructure
    ```
4.  **Rode o projeto:**
    ```bash
    dotnet run
    ```
    * A API estará acessível em `https://localhost:70XX` ou `http://localhost:5001`.

#### 2. Frontend (Vue.js)

1.  **Navegue até o diretório do Frontend:**
    ```bash
    cd agenda-frontend
    ```
2.  **Instale as dependências:**
    ```bash
    npm install
    ```
3.  **Rode o servidor de desenvolvimento:**
    ```bash
    npm run dev
    ```
    * O frontend estará acessível em `http://localhost:5173` (porta padrão do Vite).

> **Atenção:** Ao rodar localmente, configure a URL da API no arquivo de ambiente do Vue.js (`agenda-frontend/.env` ou similar) para apontar para a porta correta da API.

---

## 📂 Estrutura do Projeto

O projeto segue uma estrutura de múltiplas soluções, aderindo à **Clean Architecture**:

### Backend (.NET Core)
* **`Agenda.Api`** (Presentation Layer): Contém os Controllers, configurações de inicialização e o ponto de entrada da API.
* **`Agenda.Application`** (Application Layer): Contém a lógica de negócio orquestrada (`Commands`, `Queries`, `Validators`, `DTOs`). Implementa o padrão CQRS.
* **`Agenda.Domain`** (Domain Layer): O coração do sistema. Contém as Entidades (`Contact.cs`, `User.cs`) e Interfaces de repositório e serviços que definem os contratos.
* **`Agenda.Infrastructure`** (Infrastructure Layer): Implementações concretas (p. ex., `ContactRepository.cs`, `JwtService.cs`). Responsável por acesso a dados, Entity Framework Core e MySQL.

### Frontend (Vue.js)
* **`agenda-frontend`**: Contém os componentes **Vue.js** (incluindo PrimeVue), `router` para roteamento, `services` para comunicação com a API e `store` para gerenciamento de estado.

---

## 🤝 Contribuições

Sinta-se à vontade para contribuir! Para propor melhorias ou correções:

1.  Faça um Fork do projeto.
2.  Crie uma branch para sua feature: `git checkout -b feature/minha-feature`
3.  Commit suas mudanças: `git commit -m 'feat: Adiciona nova funcionalidade'`
4.  Faça o push para a branch: `git push origin feature/minha-feature`
5.  Abra um Pull Request.

---

## ✒️ Autor

* **Antonio Walber** - [https://github.com/walberst](https://github.com/walberst)

---

## 📄 Licença

Este projeto está licenciado sob a **Licença MIT** - veja o arquivo `LICENSE` para detalhes.