# 🛒 E-Commerce Microservices - Desafio DIO

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?logo=docker)
![Tests](https://img.shields.io/badge/Tests-15%20Passed-success?logo=checkmarx)
![License](https://img.shields.io/badge/License-Educational-blue)

Sistema completo de e-commerce construído com **arquitetura de microserviços** em .NET Core, implementando padrões modernos de desenvolvimento como API Gateway, mensageria assíncrona, autenticação JWT centralizada e containerização Docker.

---

## 🎬 Demonstrações do Sistema

### 💻 Interface Web - Gestão Completa de Estoque
![Demo Interface Web](https://raw.githubusercontent.com/Creynaix/dio-desafio-ecommerce/main/Gif_DIO.gif)

*Interface HTML completa: Login (Admin/Cliente), Cadastro de produtos, Edição de estoque em tempo real, e controle de acesso baseado em roles (JWT)*

### 🐰 RabbitMQ + Microservices - Comunicação Assíncrona
![Demo RabbitMQ Script](https://raw.githubusercontent.com/Creynaix/dio-desafio-ecommerce/main/Gif_DIO_script.gif)

*Script automatizado criando 5 pedidos simultaneamente, mostrando dashboard RabbitMQ em tempo real: métricas de Publish, Deliver, Consumer Ack, e logs do consumer processando mensagens*

---

## 🚀 Início Rápido (2 Minutos)

```powershell
# 1. Clone o repositório
git clone https://github.com/Creynaix/dio-desafio-ecommerce.git
cd dio-desafio-ecommerce

# 2. Execute tudo com Docker (requer Docker Desktop)
docker-compose up --build

# 3. Acesse o Swagger
# http://localhost:5004/swagger
```

**✅ Pronto!** SQL Server, RabbitMQ e 3 microserviços rodando!

---

## 📖 O Que É Este Projeto?

Um sistema de e-commerce **production-ready** que demonstra:

- ✅ **Microserviços independentes** com responsabilidades bem definidas
- ✅ **API Gateway** como ponto de entrada único com autenticação centralizada
- ✅ **Comunicação assíncrona** via RabbitMQ para operações não-bloqueantes
- ✅ **Database per Service** (cada microserviço tem seu próprio banco)
- ✅ **Testes unitários** completos (15 testes, 100% aprovados)
- ✅ **Docker & Docker Compose** para deploy em qualquer máquina
- ✅ **Documentação técnica completa** com fluxos detalhados

### 🎯 Casos de Uso

1. **Gestão de Estoque**: Administradores cadastram e gerenciam produtos
2. **Criação de Pedidos**: Clientes criam pedidos com validação de estoque em tempo real
3. **Atualização Assíncrona**: Estoque é atualizado automaticamente após confirmação do pedido
4. **Controle de Acesso**: JWT com policies diferenciadas (Administrador vs Cliente)

## 🏗️ Arquitetura do Sistema

```
┌─────────────────────────────────────────────────────────────┐
│                      API GATEWAY (5004)                     │
│        • Autenticação JWT Centralizada                      │
│        • Roteamento Inteligente                             │
│        • Policy-Based Authorization                         │
└──────────────────┬──────────────────┬───────────────────────┘
                   │                  │
        ┌──────────┴────────┐  ┌─────┴──────────┐
        │  ESTOQUE (5000)   │  │  VENDAS (5002) │
        │  • CRUD Produtos  │  │  • Pedidos     │
        │  • RabbitMQ       │  │  • Validação   │
        │    Consumer       │  │  • RabbitMQ    │
        │  • EstoqueDB      │  │    Producer    │
        └─────────┬─────────┘  │  • VendasDB    │
                  │            └────────┬───────┘
                  │                     │
              ┌───┴─────────────────────┴───┐
              │   RabbitMQ Message Broker   │
              │      Queue: vendasQueue     │
              └─────────────────────────────┘

┌─────────────────┐  ┌──────────────────┐
│  SQL Server     │  │   RabbitMQ       │
│  • EstoqueDB    │  │   • Port 5672    │
│  • VendasDB     │  │   • Management   │
│  • Port 1433    │  │     Port 15672   │
└─────────────────┘  └──────────────────┘
```

### Componentes:

- **API Gateway** (Porta 5004) - Ponto de entrada único, autenticação JWT, autorização
- **Microserviço de Estoque** (Porta 5000) - Gestão de produtos, consumidor RabbitMQ
- **Microserviço de Vendas** (Porta 5002) - Gestão de pedidos, produtor RabbitMQ
- **RabbitMQ** (Portas 5672/15672) - Mensageria assíncrona entre serviços
- **SQL Server** (Porta 1433) - Bancos de dados isolados por serviço

## ⚡ Quick Start - Escolha Seu Método

### 🐳 Método 1: Docker Compose (RECOMENDADO - Mais Fácil)

**Execute o sistema completo com 1 comando. Funciona em qualquer máquina!**

```powershell
# Pré-requisito: Docker Desktop instalado
# Windows: https://docs.docker.com/desktop/install/windows-install/

# 1. Clone o repositório
git clone https://github.com/Creynaix/dio-desafio-ecommerce.git
cd dio-desafio-ecommerce

# 2. Execute tudo (SQL Server + RabbitMQ + 3 microserviços)
docker-compose up --build

# 3. Aguarde ~60 segundos e acesse:
# http://localhost:5004/swagger (API Gateway)
```

**Pronto! O sistema está rodando com:**
- ✅ SQL Server 2019 (bancos EstoqueDB e VendasDB criados)
- ✅ RabbitMQ 3-management (fila vendasQueue configurada)
- ✅ 3 microserviços (.NET) rodando e conectados
- ✅ Swagger disponível para testar APIs

---

### 💻 Método 2: Execução Manual (Desenvolvimento Local)

**Para desenvolvimento ou se não quiser usar Docker:**

#### Pré-requisitos

1. **.NET 10.0 SDK** ([download](https://dotnet.microsoft.com/download))
2. **SQL Server** ([LocalDB](https://learn.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb) ou [Express](https://www.microsoft.com/sql-server/sql-server-downloads))
3. **RabbitMQ** instalado e rodando em `localhost:5672`

**Instalando RabbitMQ:**
```powershell
# Windows (com Chocolatey)
choco install rabbitmq

# Ou baixe em: https://www.rabbitmq.com/download.html
# Após instalar, execute:
rabbitmq-plugins enable rabbitmq_management
```

## 🚀 Como Executar (Método Manual)

### 1. Configurar Banco de Dados

```powershell
# Estoque Service
cd EstoqueService
dotnet ef database update

# Vendas Service
cd ../VendasService
dotnet ef database update
```

### 2. Iniciar RabbitMQ

```powershell
# Verificar se está rodando
rabbitmq-plugins enable rabbitmq_management

# Acessar interface: http://localhost:15672
# Usuário: guest / Senha: guest
```

### 3. Executar os Serviços

**Você precisa de 3 terminais simultâneos:**

```powershell
# Terminal 1 - Estoque Service
cd EstoqueService
dotnet run
# Aguarde mensagem: "Now listening on: http://localhost:5000"

# Terminal 2 - Vendas Service (em outro terminal)
cd VendasService
dotnet run
# Aguarde mensagem: "Now listening on: http://localhost:5002"

# Terminal 3 - API Gateway (em outro terminal)
cd APIGateway
dotnet run
# Aguarde mensagem: "Now listening on: http://localhost:5004"
```

**✅ Sistema rodando! Acesse:**
- Swagger API Gateway: http://localhost:5004/swagger
- Swagger Estoque: http://localhost:5000/swagger
- Swagger Vendas: http://localhost:5002/swagger
- RabbitMQ Management: http://localhost:15672 (guest/guest)

---

## 🧪 Testando o Sistema (Passo a Passo)

### Fluxo Completo: Cadastrar Produto → Criar Pedido → Ver Estoque Atualizado

#### 1. Autenticação (Obter Token JWT)

**Fazer login como Administrador via API Gateway:**
```bash
POST http://localhost:5004/api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "admin123"
}

# Resposta:
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**⚠️ IMPORTANTE:** Copie o token retornado! Você precisará dele nos próximos passos.

#### 2. Cadastrar Produto (Como Administrador)

```bash
POST http://localhost:5004/api/gateway/estoque/api/produtos
Authorization: Bearer {cole_seu_token_aqui}
Content-Type: application/json

{
  "nome": "Notebook Dell Inspiron",
  "descricao": "Notebook i7 16GB RAM 512GB SSD",
  "preco": 3500.00,
  "quantidade": 10
}

# Resposta: HTTP 201 Created
{
  "id": 1,
  "nome": "Notebook Dell Inspiron",
  "descricao": "Notebook i7 16GB RAM 512GB SSD",
  "preco": 3500.00,
  "quantidade": 10
}
```

#### 3. Consultar Produtos Cadastrados

```bash
GET http://localhost:5004/api/gateway/estoque/api/produtos
Authorization: Bearer {seu_token}

# Resposta: Lista de todos os produtos
[
  {
    "id": 1,
    "nome": "Notebook Dell Inspiron",
    "preco": 3500.00,
    "quantidade": 10
  }
]
```

#### 4. Fazer Login como Cliente

```bash
POST http://localhost:5004/api/auth/login
Content-Type: application/json

{
  "username": "cliente",
  "password": "cliente123"
}

# Resposta: Token diferente (role: Cliente)
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

#### 5. Criar Pedido (Como Cliente)

```bash
POST http://localhost:5004/api/gateway/vendas/api/pedidos
Authorization: Bearer {token_do_cliente}
Content-Type: application/json

{
  "cliente": "Maria Silva",
  "itens": [
    {
      "produtoId": 1,
      "quantidade": 2
    }
  ]
}

# Resposta: HTTP 201 Created
{
  "id": 1,
  "cliente": "Maria Silva",
  "data": "2025-11-30T14:30:00",
  "status": "Confirmado",
  "itens": [
    {
      "produtoId": 1,
      "quantidade": 2
    }
  ]
}
```

#### 6. Verificar Estoque Atualizado (Processamento Assíncrono)

```bash
GET http://localhost:5004/api/gateway/estoque/api/produtos/1
Authorization: Bearer {qualquer_token}

# Resposta: Quantidade atualizada automaticamente via RabbitMQ!
{
  "id": 1,
  "nome": "Notebook Dell Inspiron",
  "quantidade": 8  // Era 10, agora é 8 (10 - 2 do pedido)
}
```

### 🎯 O Que Aconteceu nos Bastidores?

1. ✅ **Validação Síncrona**: VendasService consultou EstoqueService via HTTP antes de criar o pedido
2. ✅ **Pedido Criado**: Salvo no banco VendasDB
3. ✅ **Mensagem Publicada**: VendasService enviou mensagem para RabbitMQ (fila `vendasQueue`)
4. ✅ **Consumo Assíncrono**: EstoqueService (BackgroundService) consumiu a mensagem
5. ✅ **Estoque Atualizado**: EstoqueDB foi atualizado (Quantidade -= 2)
6. ✅ **ACK Enviado**: RabbitMQ removeu mensagem da fila

---

### 📱 Testando via Swagger (Mais Fácil)

1. Acesse http://localhost:5004/swagger
2. Clique em `/api/auth/login` → Try it out
3. Execute com credenciais (admin/admin123)
4. Copie o token da resposta
5. Clique no botão **"Authorize"** no topo
6. Cole o token: `Bearer {seu_token}`
7. Agora todos os endpoints estão autorizados!
8. Teste os endpoints de produtos e pedidos

---

### 🔐 Credenciais de Teste

| Usuário | Senha | Role | Permissões |
|---------|-------|------|------------|
| admin | admin123 | Administrador | POST/PUT/DELETE produtos, consultas |
| cliente | cliente123 | Cliente | POST pedidos, consultas |

## 📋 Funcionalidades Implementadas

### ✅ Requisitos Principais

- [x] Microserviço de Gestão de Estoque
  - [x] Cadastro de produtos
  - [x] Consulta de produtos
  - [x] Atualização automática via RabbitMQ
- [x] Microserviço de Gestão de Vendas
  - [x] Criação de pedidos
  - [x] Validação real de estoque
  - [x] Consulta de pedidos
  - [x] Notificação via RabbitMQ
- [x] API Gateway
  - [x] Roteamento para todos os métodos HTTP
  - [x] Repasse de tokens JWT
- [x] Autenticação JWT em ambos os serviços
- [x] Comunicação assíncrona via RabbitMQ
- [x] RabbitMQ Consumer como BackgroundService

### 🔐 Usuários de Teste

**Estoque Service:**
- Admin: `admin` / `admin123`

**Vendas Service:**
- Cliente: `cliente` / `cliente123`
- Admin: `admin` / `admin123`

## 🏗️ Estrutura do Projeto

```
dio-desafio-ecommerce/
├── APIGateway/              # Gateway de roteamento
├── EstoqueService/          # Microserviço de estoque
│   ├── Controllers/
│   ├── Models/
│   ├── Data/
│   ├── Services/
│   │   └── RabbitMQConsumer.cs  # Background Service
│   └── Migrations/
├── VendasService/           # Microserviço de vendas
│   ├── Controllers/
│   ├── Models/
│   ├── Data/
│   ├── Services/
│   │   └── RabbitMQProducer.cs
│   └── Migrations/
└── README.md
```

## 🗄️ Modelo de Banco de Dados

### Database per Service Pattern

Cada microserviço possui seu próprio banco de dados isolado, garantindo baixo acoplamento e escalabilidade independente.

#### EstoqueDB - Produtos

| Coluna | Tipo | Descrição |
|--------|------|-----------|
| **Id** | INT (PK) | Identificador único |
| **Nome** | NVARCHAR | Nome do produto |
| **Descricao** | NVARCHAR | Descrição detalhada |
| **Preco** | DECIMAL(18,2) | Preço unitário |
| **Quantidade** | INT | Estoque disponível |

```csharp
public class Produto
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public required string Descricao { get; set; }
    public decimal Preco { get; set; }
    public int Quantidade { get; set; }
}
```

#### VendasDB - Pedidos e Itens

**Tabela: Pedidos**

| Coluna | Tipo | Descrição |
|--------|------|-----------|
| **Id** | INT (PK) | Identificador único |
| **Cliente** | NVARCHAR | Nome do cliente |
| **Data** | DATETIME2 | Data/hora do pedido |
| **Status** | NVARCHAR | Status (Pendente/Confirmado) |

**Tabela: ItensPedido**

| Coluna | Tipo | Descrição |
|--------|------|-----------|
| **Id** | INT (PK) | Identificador único |
| **PedidoId** | INT (FK) | Referência ao pedido |
| **ProdutoId** | INT | ID do produto (não é FK - Database per Service) |
| **Quantidade** | INT | Quantidade do produto |

```csharp
public class Pedido
{
    public int Id { get; set; }
    public required string Cliente { get; set; }
    public DateTime Data { get; set; }
    public string Status { get; set; }
    public List<ItemPedido> Itens { get; set; }
}

public class ItemPedido
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
}
```

### 👤 Como Seria o Cadastro de Clientes?

**Implementação Atual:** Usuários hardcoded no `APIGateway/Controllers/AuthController.cs` para simplificação educacional.

**Implementação Proposta para Produção:**

Criação de um **UsuariosService** dedicado com banco próprio:

#### UsuariosDB - Modelo Proposto

| Coluna | Tipo | Descrição |
|--------|------|-----------|
| **Id** | INT (PK) | Identificador único |
| **Username** | NVARCHAR (UNIQUE) | Nome de usuário |
| **Email** | NVARCHAR (UNIQUE) | Email do usuário |
| **PasswordHash** | NVARCHAR | Hash bcrypt da senha |
| **Role** | NVARCHAR | "Administrador" ou "Cliente" |
| **DataCriacao** | DATETIME2 | Data de cadastro |
| **Ativo** | BIT | Flag ativo/inativo |

**Fluxo de Registro:**
1. Cliente preenche formulário (username, email, senha)
2. Frontend → `POST /api/gateway/usuarios/api/usuarios/registrar`
3. Gateway roteia para UsuariosService (endpoint público, sem JWT)
4. UsuariosService valida dados e cria hash bcrypt da senha
5. Salva no banco UsuariosDB com Role="Cliente"
6. Retorna HTTP 201 Created
7. Usuário já pode fazer login

**Fluxo de Login:**
1. Cliente envia credenciais → `POST /api/auth/login`
2. Gateway → UsuariosService valida credenciais
3. UsuariosService verifica senha com bcrypt
4. Gateway gera JWT com claims do usuário
5. Retorna token para cliente

## 🔄 Fluxo de Criação de Pedido

1. Cliente faz POST em `/api/pedidos`
2. VendasService valida estoque chamando EstoqueService
3. Se OK, cria pedido no banco VendasDB
4. Envia mensagem JSON estruturada para RabbitMQ
5. EstoqueService (Consumer) recebe mensagem
6. Atualiza quantidade no banco EstoqueDB
7. Log de confirmação

## 🐰 Monitorando RabbitMQ - Como Interpretar o Dashboard

### Acessando o Dashboard
- **URL:** http://localhost:15672
- **Credenciais:** guest/guest
- **Navegue até:** Aba "Queues" → Clique em "vendasQueue"

### 📊 Entendendo as Métricas

#### 1. Totals (Estado da Fila)

| Métrica | O Que Significa | Valor Ideal |
|---------|-----------------|-------------|
| **Ready** 🟡 | Mensagens aguardando processamento | 0 (tudo processado) |
| **Unacked** 🔵 | Mensagens em processamento (sem ACK) | 0-5 (depende do volume) |
| **Total** 🔴 | Total de mensagens na fila | 0 (sistema em dia) |

**Cenários:**
- ✅ `Ready: 0, Unacked: 0` → Sistema ocioso (normal)
- ⚠️ `Ready: 50+` → Consumer lento, considere escalar
- 🚨 `Unacked: 20+` → Possível erro no consumer, verificar logs

#### 2. Message Rates (Taxa de Mensagens/segundo)

| Taxa | O Que Significa | Quando Aparece |
|------|-----------------|----------------|
| **Publish** | VendasService enviando mensagens | Ao criar pedido |
| **Deliver (manual ack)** | EstoqueService recebendo mensagens | Durante processamento |
| **Consumer ack** | EstoqueService confirmando processamento | Após atualizar estoque |
| **Unroutable** | Mensagens sem destino (erro!) | Nunca (se tudo estiver OK) |

### 🎯 Fluxo de uma Mensagem no Dashboard

```
1. PEDIDO CRIADO
   Publish: 1.00/s     ← VendasService publica
   Ready: 1            ← Mensagem na fila

2. PROCESSANDO (50-200ms depois)
   Deliver: 1.00/s     ← EstoqueService recebe
   Unacked: 1          ← Processando (sem ACK)
   Ready: 0            ← Fila vazia

3. COMPLETO (100-300ms depois)
   Consumer ack: 1.00/s ← EstoqueService confirma
   Unacked: 0          ← Processamento completo
```

### 🧪 Teste Prático - Ver Movimento no Dashboard

```powershell
# 1. Abrir dashboard: http://localhost:15672 (aba Queues → vendasQueue)

# 2. Em outro terminal, executar:

# Login Admin
$loginAdmin = Invoke-RestMethod -Uri 'http://localhost:5004/api/auth/login' `
  -Method POST -ContentType 'application/json' `
  -Body '{"username":"admin","password":"admin123"}'
$tokenAdmin = $loginAdmin.token

# Criar produto
$produto = @{
    nome = "Mouse Gamer RGB"
    descricao = "16000 DPI"
    preco = 250.00
    quantidade = 100
} | ConvertTo-Json

Invoke-RestMethod -Uri 'http://localhost:5004/api/gateway/estoque/api/produtos' `
  -Method POST -Headers @{Authorization="Bearer $tokenAdmin"} `
  -ContentType 'application/json' -Body $produto

# Login Cliente
$loginCliente = Invoke-RestMethod -Uri 'http://localhost:5004/api/auth/login' `
  -Method POST -ContentType 'application/json' `
  -Body '{"username":"cliente","password":"cliente123"}'
$tokenCliente = $loginCliente.token

# Criar 10 pedidos rapidamente (observe o dashboard!)
1..10 | ForEach-Object {
    $pedido = @{
        cliente = "Cliente $_"
        itens = @(@{ produtoId = 1; quantidade = 2 })
    } | ConvertTo-Json
    
    Invoke-RestMethod -Uri 'http://localhost:5004/api/gateway/vendas/api/pedidos' `
      -Method POST -Headers @{Authorization="Bearer $tokenCliente"} `
      -ContentType 'application/json' -Body $pedido
    
    Write-Host "Pedido $_ criado" -ForegroundColor Green
    Start-Sleep -Milliseconds 500
}
```

**O que você verá no dashboard:**
1. **Publish rate** aumenta (10 mensagens em 5 segundos = ~2/s)
2. **Ready** pode aparecer brevemente (1-3 mensagens)
3. **Deliver rate** aumenta (consumer processando)
4. **Consumer ack** aumenta (confirmações)
5. Após 10-20 segundos: tudo volta a 0 (sistema processou tudo)

### 🔍 Verificando os Logs

```powershell
# Ver processamento em tempo real
docker-compose logs -f estoque-service

# Você verá logs como:
# [INFO] RabbitMQ Consumer: Processando pedido ID 1
# [INFO] RabbitMQ Consumer: Atualizando estoque - Produto 1, Quantidade -2
# [INFO] RabbitMQ Consumer: Estoque atualizado com sucesso
```

### 📈 Padrões de Uso

| Cenário | Ready | Unacked | Publish/s | Ack/s |
|---------|-------|---------|-----------|-------|
| Sistema Ocioso | 0 | 0 | 0 | 0 |
| Tráfego Normal | 0-2 | 1-3 | 0.5-2 | 0.5-2 |
| Pico de Vendas | 5-20 | 3-5 | 5-10 | 5-10 |
| Consumer Lento 🚨 | 50+ | 1-2 | 10 | 2 |
| Consumer Travado 🚨 | 100+ | 0 | 10 | 0 |

### 🛠️ Troubleshooting

**Problema: Ready aumentando constantemente**
```
Causa: Consumer não consegue processar rápido suficiente
Solução: Escalar EstoqueService
docker-compose up --scale estoque-service=3
```

**Problema: Unacked alto e não diminui**
```
Causa: Erro no consumer (não está enviando ACK)
Solução: Verificar logs e reiniciar
docker-compose restart estoque-service
docker-compose logs estoque-service
```

**Problema: Unroutable > 0**
```
Causa: Fila não existe ou routing key incorreto
Solução: Verificar configuração do RabbitMQ
- Queue name: vendasQueue
- Durabilidade: true
- Exchange: (default)
```

## 🐛 Troubleshooting

### RabbitMQ não conecta
- Verifique se RabbitMQ está rodando: `rabbitmqctl status`
- Porta padrão: 5672

### Erro de migração do banco
```powershell
dotnet ef database drop --force
dotnet ef database update
```

### Token JWT inválido
- Verifique se a chave JWT é a mesma em todos os serviços
- Token expira em 2 horas

## 📝 Notas Importantes

- O RabbitMQConsumer roda automaticamente como BackgroundService
- Mensagens são persistentes (durable: true)
- Validação de estoque é feita ANTES de criar o pedido
- Estoque é atualizado de forma assíncrona via mensageria

## 🎯 Critérios de Aceitação Atendidos

### ✅ Requisitos Obrigatórios (100%)

- ✅ **Cadastro de produtos no microserviço de estoque**  
  → CRUD completo em `EstoqueService/Controllers/ProdutosController.cs`
  
- ✅ **Criação de pedidos com validação de estoque**  
  → Validação síncrona via HTTP antes de confirmar pedido  
  → Ver `VendasService/Controllers/PedidosController.cs` linha 48-76
  
- ✅ **Comunicação eficiente via RabbitMQ**  
  → Producer: `VendasService/Services/RabbitMQProducer.cs`  
  → Consumer: `EstoqueService/Services/RabbitMQConsumer.cs` (BackgroundService)  
  → Mensagens persistentes com Ack/Nack manual
  
- ✅ **API Gateway funcional**  
  → Roteamento para todos métodos HTTP (GET, POST, PUT, DELETE)  
  → Autenticação JWT centralizada  
  → Ver `APIGateway/Controllers/GatewayController.cs`
  
- ✅ **Segurança com JWT e permissões**  
  → JWT centralizado no API Gateway  
  → Policy-based authorization (Administrador, Cliente)  
  → Role-based access control
  
- ✅ **Código bem estruturado com POO**  
  → Separação em camadas (Controllers, Services, Data, Models)  
  → Dependency Injection  
  → Single Responsibility Principle

### ⭐ Extras Implementados

- ✅ **Testes Unitários**  
  → `EstoqueService.Tests` com xUnit, Moq, FluentAssertions  
  → `VendasService.Tests` com cobertura completa de Controllers
  
- ⚠️ **Monitoramento e Logs**  
  → ✅ ILogger em todos Controllers  
  → ✅ Logs de validação, erros e sucessos  
  → ❌ Prometheus/Grafana (não implementado)
  
- ✅ **Escalabilidade**  
  → Database per Service (EstoqueDB, VendasDB)  
  → Stateless (JWT permite horizontal scaling)  
  → RabbitMQ suporta múltiplos consumers  
  → **Docker Compose para deploy fácil**  
  → Microservices independentes

### 📊 Score Final

**Obrigatórios:** 6/6 (100%) ✅  
**Extras:** 2.5/3 (83%) ⭐

**Status:** ✅ **PROJETO COMPLETO E PRONTO PARA ENTREGA**

## 🐳 Execução com Docker (Deploy em Qualquer Máquina)

### Pré-requisitos
- **Docker Desktop** instalado e rodando
  - Windows: https://docs.docker.com/desktop/install/windows-install/
  - Linux: https://docs.docker.com/engine/install/
  - Mac: https://docs.docker.com/desktop/install/mac-install/

### 🚀 Opção 1: Docker Compose (Recomendado)

**Execução com um único comando:**

```powershell
# Construir e iniciar TODA a aplicação (SQL Server + RabbitMQ + 3 microserviços)
docker-compose up --build

# Ou executar em background (detached mode)
docker-compose up --build -d
```

**O que acontece:**
1. ✅ SQL Server inicia e cria bancos EstoqueDB e VendasDB
2. ✅ RabbitMQ inicia com fila `vendasQueue` configurada
3. ✅ EstoqueService aguarda SQL Server estar saudável (health check)
4. ✅ VendasService aguarda SQL Server e RabbitMQ estarem prontos
5. ✅ API Gateway aguarda ambos microserviços estarem online
6. ✅ Migrations são aplicadas automaticamente
7. ✅ Sistema completo pronto em ~30-60 segundos

**Serviços disponíveis:**
- 🌐 **API Gateway:** http://localhost:5004/swagger
- 📦 **Estoque Service:** http://localhost:5000/swagger
- 🛒 **Vendas Service:** http://localhost:5002/swagger
- 🐰 **RabbitMQ Management:** http://localhost:15672 (guest/guest)
- 🗄️ **SQL Server:** localhost:1433 (sa/YourStrong@Passw0rd)

**Comandos úteis:**

```powershell
# Ver logs de todos os serviços
docker-compose logs -f

# Ver logs de um serviço específico
docker-compose logs -f estoque-service
docker-compose logs -f vendas-service
docker-compose logs -f api-gateway

# Verificar status dos containers
docker-compose ps

# Parar todos os serviços (mantém volumes)
docker-compose stop

# Parar e remover containers (mantém volumes)
docker-compose down

# Parar e remover TUDO (incluindo volumes - limpa bancos de dados)
docker-compose down -v

# Rebuild sem cache (útil após mudanças no código)
docker-compose build --no-cache
docker-compose up

# Escalar serviços (múltiplas instâncias)
docker-compose up --scale estoque-service=3 --scale vendas-service=2
```

**Troubleshooting Docker Compose:**

```powershell
# Se portas estiverem em uso
# Parar processos locais em 5000, 5002, 5004, 1433, 5672, 15672

# Se SQL Server não iniciar
docker-compose down -v  # Remove volumes
docker-compose up --build

# Se RabbitMQ não conectar
docker-compose restart rabbitmq
docker-compose logs rabbitmq

# Acessar shell de um container
docker-compose exec estoque-service bash
docker-compose exec sqlserver bash

# Verificar health checks
docker inspect ecommerce-sqlserver | grep -A 10 Health
docker inspect ecommerce-rabbitmq | grep -A 10 Health
```

### 🔧 Opção 2: Docker Individual (Avançado)

**Se preferir controle manual:**

```powershell
# 1. Criar network compartilhada
docker network create ecommerce-network

# 2. Iniciar SQL Server
docker run -d \
  --name sqlserver \
  --network ecommerce-network \
  -e "ACCEPT_EULA=Y" \
  -e "SA_PASSWORD=YourStrong@Passw0rd" \
  -e "MSSQL_PID=Express" \
  -p 1433:1433 \
  -v sqlserver-data:/var/opt/mssql \
  mcr.microsoft.com/mssql/server:2019-latest

# 3. Iniciar RabbitMQ
docker run -d \
  --name rabbitmq \
  --network ecommerce-network \
  -e "RABBITMQ_DEFAULT_USER=guest" \
  -e "RABBITMQ_DEFAULT_PASS=guest" \
  -p 5672:5672 \
  -p 15672:15672 \
  -v rabbitmq-data:/var/lib/rabbitmq \
  rabbitmq:3-management

# 4. Aguardar SQL Server e RabbitMQ iniciarem (~20s)
Start-Sleep -Seconds 20

# 5. Construir imagens dos microserviços
docker build -f EstoqueService/Dockerfile -t ecommerce-estoque .
docker build -f VendasService/Dockerfile -t ecommerce-vendas .
docker build -f APIGateway/Dockerfile -t ecommerce-gateway .

# 6. Iniciar EstoqueService
docker run -d \
  --name estoque-service \
  --network ecommerce-network \
  -e "ASPNETCORE_URLS=http://+:5000" \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver;Database=EstoqueDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True" \
  -e "RabbitMQ__HostName=rabbitmq" \
  -p 5000:5000 \
  ecommerce-estoque

# 7. Iniciar VendasService
docker run -d \
  --name vendas-service \
  --network ecommerce-network \
  -e "ASPNETCORE_URLS=http://+:5002" \
  -e "ConnectionStrings__DefaultConnection=Server=sqlserver;Database=VendasDB;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True" \
  -e "RabbitMQ__HostName=rabbitmq" \
  -e "EstoqueServiceUrl=http://estoque-service:5000" \
  -p 5002:5002 \
  ecommerce-vendas

# 8. Iniciar API Gateway
docker run -d \
  --name api-gateway \
  --network ecommerce-network \
  -e "ASPNETCORE_URLS=http://+:5004" \
  -e "Services__EstoqueService=http://estoque-service:5000" \
  -e "Services__VendasService=http://vendas-service:5002" \
  -p 5004:5004 \
  ecommerce-gateway
```

### 📦 Estrutura dos Dockerfiles

Todos os serviços usam **multi-stage builds** para imagens otimizadas:

```dockerfile
# Estágio 1: Base runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

# Estágio 2: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EstoqueService/EstoqueService.csproj", "EstoqueService/"]
RUN dotnet restore "EstoqueService/EstoqueService.csproj"
COPY . .
WORKDIR "/src/EstoqueService"
RUN dotnet build -c Release -o /app/build

# Estágio 3: Publish
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Estágio 4: Runtime final (imagem menor)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EstoqueService.dll"]
```

**Benefícios:**
- ✅ Imagem final ~300MB (sem SDK, apenas runtime)
- ✅ Build em cache (restauração de pacotes separada)
- ✅ Seguro (sem código-fonte na imagem final)

### 🌐 Deploy em Produção

**Para Azure, AWS ou servidor Linux:**

```bash
# 1. Copiar projeto para servidor
scp -r dio-desafio-ecommerce user@servidor:/home/user/

# 2. SSH no servidor
ssh user@servidor

# 3. Instalar Docker (se necessário)
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh

# 4. Executar aplicação
cd /home/user/dio-desafio-ecommerce
docker-compose up -d

# 5. Verificar logs
docker-compose logs -f

# 6. Configurar firewall
sudo ufw allow 5004/tcp  # API Gateway
sudo ufw allow 15672/tcp # RabbitMQ Management
```

### 🔒 Variáveis de Ambiente para Produção

**Criar arquivo `.env` (não commitar no git):**

```env
# SQL Server
SA_PASSWORD=SuaSenhaForteMuitoSegura@2024!
MSSQL_PID=Express

# RabbitMQ
RABBITMQ_DEFAULT_USER=admin
RABBITMQ_DEFAULT_PASS=SuaSenhaRabbitMQ@2024!

# Connection Strings
ESTOQUE_CONNECTION_STRING=Server=sqlserver;Database=EstoqueDB;User Id=sa;Password=SuaSenhaForteMuitoSegura@2024!;TrustServerCertificate=True
VENDAS_CONNECTION_STRING=Server=sqlserver;Database=VendasDB;User Id=sa;Password=SuaSenhaForteMuitoSegura@2024!;TrustServerCertificate=True

# JWT Secret (mínimo 32 caracteres)
JWT_SECRET=sua-chave-super-secreta-de-producao-com-64-caracteres-aqui-2024
```

**Atualizar `docker-compose.yml` para usar `.env`:**

```yaml
services:
  sqlserver:
    environment:
      SA_PASSWORD: ${SA_PASSWORD}
      
  estoque-service:
    environment:
      ConnectionStrings__DefaultConnection: ${ESTOQUE_CONNECTION_STRING}
      JWT__Secret: ${JWT_SECRET}
```

### 📊 Monitoramento

**Ver uso de recursos:**

```powershell
# CPU, Memória, Network, I/O de cada container
docker stats

# Uso de disco
docker system df

# Limpar recursos não utilizados
docker system prune -a --volumes
```

## 🧪 Testes Unitários - Guia Completo

### 📚 O Que São Testes Unitários?

Testes unitários são **pequenos programas que testam seu código automaticamente**. Em vez de você testar manualmente cada endpoint no Swagger, os testes fazem isso por você em milissegundos.

**Benefícios:**
- ✅ **Confiança:** Garante que mudanças futuras não quebrem funcionalidades existentes
- ✅ **Documentação Viva:** Testes mostram como usar o código corretamente
- ✅ **Detecção Precoce:** Bugs são encontrados antes de ir para produção
- ✅ **Refatoração Segura:** Permite melhorar código sem medo
- ✅ **Rapidez:** 15 testes rodam em ~1.4 segundos

---

### 🎯 Estrutura dos Testes

**Projetos de Teste:**
```
dio-desafio-ecommerce/
├── EstoqueService.Tests/    # 8 testes
│   ├── ProdutosControllerTests.cs
│   └── EstoqueService.Tests.csproj
└── VendasService.Tests/     # 7 testes
    ├── PedidosControllerTests.cs
    └── VendasService.Tests.csproj
```

**Tecnologias Utilizadas:**
- **xUnit 3.1.4** - Framework de testes (recomendado pela Microsoft)
- **Moq 4.20.72** - Cria mocks (objetos falsos) de dependências
- **FluentAssertions 8.8.0** - Validações legíveis e expressivas
- **EF Core InMemory 10.0.0** - Banco de dados em memória para testes isolados

---

### 🔨 Como Criar Testes do Zero

#### **Passo 1: Criar Projeto de Testes**

```powershell
# Na raiz do projeto
dotnet new xunit -n EstoqueService.Tests

# Adicionar referência ao projeto principal
cd EstoqueService.Tests
dotnet add reference ../EstoqueService/EstoqueService.csproj

# Instalar dependências de teste
dotnet add package Moq --version 4.20.72
dotnet add package FluentAssertions --version 8.8.0
dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 10.0.0

# Adicionar ao solution
cd ..
dotnet sln add EstoqueService.Tests/EstoqueService.Tests.csproj
```

#### **Passo 2: Entender o Padrão AAA**

Todo teste segue o padrão **Arrange-Act-Assert**:

```csharp
[Fact]  // Atributo xUnit indicando que é um teste
public void NomeDoTeste_DeveRetornarAlgo_QuandoCondicao()
{
    // ===== ARRANGE (Preparação) =====
    // Configura tudo que o teste precisa
    var produto = new Produto 
    { 
        Nome = "Notebook", 
        Descricao = "i7 16GB",
        Preco = 3500, 
        Quantidade = 10 
    };
    
    // ===== ACT (Ação) =====
    // Executa o método sendo testado
    var result = _controller.CadastrarProduto(produto);
    
    // ===== ASSERT (Verificação) =====
    // Verifica se o resultado é o esperado
    result.Should().BeOfType<CreatedAtActionResult>();
    var createdResult = result as CreatedAtActionResult;
    createdResult?.Value.Should().BeEquivalentTo(produto);
}
```

#### **Passo 3: Criar Classe de Teste Completa**

**Arquivo:** `EstoqueService.Tests/ProdutosControllerTests.cs`

```csharp
using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EstoqueService.Controllers;
using EstoqueService.Data;
using EstoqueService.Models;
using Microsoft.Extensions.Logging;

namespace EstoqueService.Tests
{
    public class ProdutosControllerTests
    {
        private readonly Mock<ILogger<ProdutosController>> _loggerMock;
        private readonly EstoqueContext _context;
        private readonly ProdutosController _controller;

        // CONSTRUCTOR - Roda ANTES de cada teste
        public ProdutosControllerTests()
        {
            // 1. Criar banco em memória (isolado, cada teste tem o seu)
            var options = new DbContextOptionsBuilder<EstoqueContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_" + Guid.NewGuid())
                .Options;
            
            _context = new EstoqueContext(options);

            // 2. Criar mock do ILogger (não queremos logs nos testes)
            _loggerMock = new Mock<ILogger<ProdutosController>>();

            // 3. Criar instância do controller
            _controller = new ProdutosController(_context, _loggerMock.Object);

            // 4. Simular headers HTTP (X-User-Role, X-User-Name)
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["X-User-Role"] = "Administrador";
            httpContext.Request.Headers["X-User-Name"] = "TestUser";
            
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public void CadastrarProduto_DeveRetornarCreated_QuandoProdutoValido()
        {
            // ARRANGE
            var produto = new Produto
            {
                Nome = "Notebook Dell",
                Descricao = "i7 16GB RAM",
                Preco = 3500.00m,
                Quantidade = 10
            };

            // ACT
            var result = _controller.CadastrarProduto(produto);

            // ASSERT
            result.Should().BeOfType<CreatedAtActionResult>();
            
            var createdResult = result as CreatedAtActionResult;
            var produtoRetornado = createdResult?.Value as Produto;
            
            produtoRetornado.Should().NotBeNull();
            produtoRetornado!.Nome.Should().Be("Notebook Dell");
            
            // Verificar se salvou no banco
            _context.Produtos.Should().HaveCount(1);
        }

        [Fact]
        public void ConsultarProduto_DeveRetornarNotFound_QuandoProdutoNaoExiste()
        {
            // ACT
            var result = _controller.ConsultarProduto(999);

            // ASSERT
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void ConsultarProdutos_DeveRetornarListaVazia_QuandoNaoHaProdutos()
        {
            // ACT
            var result = _controller.ConsultarProdutos();

            // ASSERT
            result.Should().BeOfType<OkObjectResult>();
            
            var okResult = result as OkObjectResult;
            var produtos = okResult?.Value as List<Produto>;
            
            produtos.Should().BeEmpty();
        }

        [Fact]
        public void AtualizarProduto_DeveAtualizarQuantidade_QuandoProdutoExiste()
        {
            // ARRANGE - Criar produto no banco
            var produto = new Produto 
            { 
                Nome = "Mouse", 
                Descricao = "Gamer",
                Preco = 150,
                Quantidade = 5 
            };
            _context.Produtos.Add(produto);
            _context.SaveChanges();

            var request = new AtualizarProdutoRequest { Quantidade = 20 };

            // ACT
            var result = _controller.AtualizarProduto(produto.Id, request);

            // ASSERT
            result.Should().BeOfType<OkObjectResult>();
            
            var produtoDb = _context.Produtos.Find(produto.Id);
            produtoDb!.Quantidade.Should().Be(20);
        }
    }
}
```

---

### 🚀 Como Rodar os Testes

#### **Opção 1: Rodar Todos os Testes**

```powershell
# Na raiz do projeto
dotnet test

# Resultado esperado:
# Resumo do teste: total: 15; falhou: 0; bem-sucedido: 15
# Tempo de execução: ~1.4s
```

#### **Opção 2: Rodar com Detalhes**

```powershell
dotnet test --verbosity detailed

# Mostra cada teste rodando:
# [xUnit.net 00:00:00.52] EstoqueService.Tests.ProdutosControllerTests.CadastrarProduto_DeveRetornarCreated ✅ PASSED
# [xUnit.net 00:00:00.71] EstoqueService.Tests.ProdutosControllerTests.ConsultarProduto_NotFound ✅ PASSED
```

#### **Opção 3: Rodar Apenas Um Projeto**

```powershell
# Navegar até a pasta do teste
cd EstoqueService.Tests
dotnet test

# Ou especificar o caminho
dotnet test EstoqueService.Tests/EstoqueService.Tests.csproj

# Rodar teste específico com filtro
dotnet test --filter "FullyQualifiedName~CadastrarProduto"
```

#### **Opção 4: Rodar com Cobertura de Código**

```powershell
# Instalar coverlet (ferramenta de cobertura)
dotnet add package coverlet.collector

# Rodar com cobertura
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# Ver relatório
# Instalar ferramenta de relatório
dotnet tool install -g dotnet-reportgenerator-globaltool

# Gerar relatório HTML
reportgenerator -reports:coverage.opencover.xml -targetdir:coveragereport
```

---

### 🎓 Conceitos Importantes

#### **1. Mock - O Que É?**

**Mock** é um "objeto falso" que simula dependências externas.

```csharp
// ❌ SEM MOCK: Precisaria de RabbitMQ rodando de verdade
var producer = new RabbitMQProducer(); 
producer.SendMessage("teste"); // Erro se RabbitMQ não estiver rodando

// ✅ COM MOCK: Simula RabbitMQ sem precisar dele
var producerMock = new Mock<IRabbitMQProducer>();
producerMock.Setup(p => p.SendMessage(It.IsAny<string>())).Verifiable();
```

**Por que usar:**
- Testes ficam **rápidos** (não precisa subir serviços externos)
- Testes ficam **isolados** (não dependem de RabbitMQ/SQL Server)
- Você pode simular **erros** facilmente

#### **2. InMemory Database**

```csharp
// Banco em memória - cada teste tem seu próprio banco
var options = new DbContextOptionsBuilder<EstoqueContext>()
    .UseInMemoryDatabase(databaseName: "TestDB_" + Guid.NewGuid())
    .Options;
```

**Vantagens:**
- ✅ Rápido (tudo em RAM)
- ✅ Isolado (cada teste tem banco próprio com GUID único)
- ✅ Limpo (destruído após teste)
- ✅ Não precisa SQL Server rodando

#### **3. FluentAssertions - Validações Legíveis**

```csharp
// ❌ RUIM (xUnit tradicional):
Assert.IsType<OkObjectResult>(result);
Assert.Equal(10, produto.Quantidade);

// ✅ BOM (FluentAssertions):
result.Should().BeOfType<OkObjectResult>();
produto.Quantidade.Should().Be(10);
```

**Mais legível!** Lê como inglês natural.

---

### 🐛 Problemas Comuns e Soluções

#### **Problema 1: HttpContext null**

```csharp
// ❌ ERRO: Request.Headers["X-User-Name"] dá NullReferenceException

// ✅ SOLUÇÃO: Criar HttpContext fake
var httpContext = new DefaultHttpContext();
httpContext.Request.Headers["X-User-Name"] = "TestUser";
httpContext.Request.Headers["X-User-Role"] = "Administrador";
_controller.ControllerContext = new ControllerContext() 
{ 
    HttpContext = httpContext 
};
```

#### **Problema 2: RabbitMQProducer não mockável**

```csharp
// ❌ ERRO: Cannot instantiate proxy of class: RabbitMQProducer

// ✅ SOLUÇÃO: Criar interface
public interface IRabbitMQProducer 
{
    void SendMessage<T>(T message);
}

// Implementar interface
public class RabbitMQProducer : IRabbitMQProducer
{
    public void SendMessage<T>(T message) { /* ... */ }
}

// Registrar no Program.cs
builder.Services.AddScoped<IRabbitMQProducer, RabbitMQProducer>();

// Agora o mock funciona:
var mock = new Mock<IRabbitMQProducer>();
```

#### **Problema 3: Required properties não inicializadas**

```csharp
// ❌ ERRO: CS9035: Required member 'Produto.Nome' must be set

// ✅ SOLUÇÃO: Inicializar todas propriedades required
var produto = new Produto 
{ 
    Nome = "Obrigatório",      // ✅
    Descricao = "Obrigatório", // ✅
    Preco = 100,
    Quantidade = 20 
};
```

---

### 📊 O Que Testar?

#### **✅ DEVE Testar:**
- **Controllers:** Endpoints retornam status HTTP correto?
- **Validações:** Rejeita dados inválidos (quantidade negativa, pedido sem itens)?
- **Regras de Negócio:** Estoque é atualizado corretamente?
- **Casos Extremos:** Lista vazia, ID inexistente, estoque insuficiente

#### **❌ NÃO Testar:**
- Entity Framework (Microsoft já testou)
- ASP.NET Core (Microsoft já testou)
- Bibliotecas de terceiros (RabbitMQ, SQL Server)

---

### 📈 Cobertura de Testes do Projeto

#### **EstoqueService.Tests (8 testes):**
- ✅ Cadastro de produtos com sucesso
- ✅ Consulta de produtos (lista completa)
- ✅ Consulta de produto individual por ID
- ✅ Consulta de produtos quando lista está vazia
- ✅ Atualização de estoque com sucesso
- ✅ Validação de produto não encontrado (404)
- ✅ Validação de dados obrigatórios
- ✅ Verificação de persistência no banco

#### **VendasService.Tests (7 testes):**
- ✅ Criação de pedidos com sucesso
- ✅ Consulta de pedidos (lista completa)
- ✅ Consulta de pedido individual por ID
- ✅ Validação de pedido sem itens (400)
- ✅ Validação de quantidade inválida (negativa/zero)
- ✅ Verificação de envio de mensagem ao RabbitMQ
- ✅ Validação de pedido não encontrado (404)

---

### 🔍 Exemplo Completo: Teste do VendasService

```csharp
public class PedidosControllerTests
{
    private readonly Mock<IRabbitMQProducer> _rabbitMQProducerMock;
    private readonly Mock<ILogger<PedidosController>> _loggerMock;
    private readonly VendasContext _context;
    private readonly PedidosController _controller;

    public PedidosControllerTests()
    {
        // Configurar banco InMemory
        var options = new DbContextOptionsBuilder<VendasContext>()
            .UseInMemoryDatabase(databaseName: "TestVendasDB_" + Guid.NewGuid())
            .Options;
        
        _context = new VendasContext(options);

        // Criar mocks
        _rabbitMQProducerMock = new Mock<IRabbitMQProducer>();
        _loggerMock = new Mock<ILogger<PedidosController>>();

        // Configurar mock do RabbitMQ para verificar chamadas
        _rabbitMQProducerMock.Setup(r => r.SendMessage(It.IsAny<VendaMessage>()))
            .Verifiable();

        // Criar controller com dependências mockadas
        _controller = new PedidosController(
            _context, 
            _rabbitMQProducerMock.Object,
            new HttpClient(),
            Mock.Of<IConfiguration>(),
            _loggerMock.Object
        );

        // Simular headers HTTP
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["X-User-Name"] = "TestUser";
        httpContext.Request.Headers["X-User-Role"] = "Cliente";
        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };
    }

    [Fact]
    public async Task CriarPedido_DeveRetornarBadRequest_QuandoPedidoSemItens()
    {
        // ARRANGE
        var pedido = new Pedido
        {
            Cliente = "Maria Silva",
            Itens = new List<ItemPedido>()  // Lista vazia!
        };

        // ACT
        var result = await _controller.CriarPedido(pedido);

        // ASSERT
        result.Should().BeOfType<BadRequestObjectResult>();
        
        var badRequest = result as BadRequestObjectResult;
        badRequest?.Value.Should().Be("Pedido deve conter pelo menos um item");
        
        // Verificar que RabbitMQ NÃO foi chamado
        _rabbitMQProducerMock.Verify(
            r => r.SendMessage(It.IsAny<VendaMessage>()), 
            Times.Never
        );
    }

    [Fact]
    public async Task CriarPedido_DeveEnviarMensagemRabbitMQ_QuandoPedidoValido()
    {
        // ARRANGE
        var pedido = new Pedido
        {
            Cliente = "João Silva",
            Itens = new List<ItemPedido>
            {
                new ItemPedido { ProdutoId = 1, Quantidade = 2 }
            }
        };

        // ACT
        var result = await _controller.CriarPedido(pedido);

        // ASSERT
        result.Should().BeOfType<CreatedAtActionResult>();
        
        // Verificar que RabbitMQ FOI chamado exatamente 1 vez
        _rabbitMQProducerMock.Verify(
            r => r.SendMessage(It.IsAny<VendaMessage>()), 
            Times.Once
        );
        
        // Verificar que pedido foi salvo no banco
        _context.Pedidos.Should().HaveCount(1);
    }
}
```

---

### 🎯 Resumo - Checklist para Criar Testes

**1. Criar projeto de testes:**
```powershell
dotnet new xunit -n SeuProjeto.Tests
dotnet add reference ../SeuProjeto/SeuProjeto.csproj
```

**2. Instalar dependências:**
```powershell
dotnet add package Moq
dotnet add package FluentAssertions
dotnet add package Microsoft.EntityFrameworkCore.InMemory
```

**3. Seguir padrão AAA:**
- **Arrange:** Preparar dados
- **Act:** Executar método
- **Assert:** Verificar resultado

**4. Usar mocks para dependências:**
```csharp
var mock = new Mock<IInterface>();
mock.Setup(m => m.Method()).Returns(value);
```

**5. Usar InMemory para banco:**
```csharp
.UseInMemoryDatabase("TestDB_" + Guid.NewGuid())
```

**6. Nomear testes claramente:**
```csharp
public void MetodoTestado_DeveRetornarAlgo_QuandoCondicao()
```

**7. Rodar testes:**
```powershell
dotnet test
```

---

### 📚 Documentação Adicional

Para mais detalhes sobre a implementação dos testes, consulte:
- **`DOCUMENTACAO.html`** - Seção 11: Explicação completa com exemplos
- **`EstoqueService.Tests/`** - Código-fonte completo dos testes
- **`VendasService.Tests/`** - Mais exemplos de testes com mocks

**Status Atual:** ✅ **15 testes, 100% aprovados, rodando em ~1.4 segundos**

## 📚 Tecnologias Utilizadas

### Backend & Framework
- **.NET 10.0 Preview RC2** - Framework principal
- **ASP.NET Core Web API** - Construção de APIs RESTful
- **Entity Framework Core 10.0** - ORM para acesso a dados
- **C# 12** - Linguagem de programação

### Banco de Dados & Mensageria
- **SQL Server 2019** - Banco de dados relacional (2 bancos isolados)
- **RabbitMQ 3-management** - Message broker para comunicação assíncrona

### Segurança & Autenticação
- **JWT Bearer Authentication** - Tokens de autenticação
- **Policy-Based Authorization** - Controle de acesso granular
- **Role-Based Access Control (RBAC)** - Permissões por perfil

### DevOps & Containerização
- **Docker** - Containerização de aplicações
- **Docker Compose** - Orquestração multi-container
- **Multi-stage Builds** - Otimização de imagens Docker

### Testes & Qualidade
- **xUnit 3.1.4** - Framework de testes unitários
- **Moq 4.20.72** - Library de mocking/stubbing
- **FluentAssertions 8.8.0** - Assertions expressivas e legíveis

### Documentação & API
- **Swagger/OpenAPI** - Documentação interativa de APIs
- **Swashbuckle** - Geração automática de docs Swagger

### Padrões & Arquitetura
- **Microservices Architecture** - Serviços independentes e desacoplados
- **API Gateway Pattern** - Ponto de entrada único
- **Database per Service** - Isolamento de dados
- **Event-Driven Architecture** - Comunicação via eventos (RabbitMQ)
- **CQRS (parcial)** - Separação de leitura/escrita
- **Repository Pattern** - Abstração de acesso a dados
- **Dependency Injection** - Inversão de controle
- **Health Checks** - Monitoramento de saúde dos serviços

---

## 📊 Estatísticas do Projeto

- **Linhas de Código:** ~3.500 linhas (sem contar testes)
- **Microserviços:** 3 (Gateway, Estoque, Vendas)
- **Endpoints REST:** 15+ endpoints
- **Testes Unitários:** 15 testes (100% aprovados)
- **Cobertura de Testes:** Controllers e Services
- **Bancos de Dados:** 2 bancos isolados
- **Filas RabbitMQ:** 1 fila (vendasQueue)
- **Containers Docker:** 5 containers (SQL, RabbitMQ, 3 apps)
- **Documentação:** 2.500+ linhas (HTML + README)

---

## 🎓 Conceitos Demonstrados

### Arquitetura de Software
- ✅ Microservices com responsabilidades únicas
- ✅ API Gateway como ponto de entrada
- ✅ Database per Service (isolamento de dados)
- ✅ Event-Driven Architecture (mensageria)
- ✅ Trusted Subsystem Pattern (segurança)

### Desenvolvimento
- ✅ RESTful APIs com boas práticas
- ✅ Dependency Injection (DI)
- ✅ Async/Await para operações I/O
- ✅ Entity Framework com Code-First Migrations
- ✅ Separation of Concerns (Controllers, Services, Data)

### Segurança
- ✅ JWT Authentication centralizada
- ✅ Policy-Based Authorization
- ✅ Role-Based Access Control
- ✅ Trusted Subsystem Pattern
- ✅ Secrets em variáveis de ambiente

### Mensageria & Integração
- ✅ RabbitMQ Producer/Consumer
- ✅ BackgroundService para processamento contínuo
- ✅ Mensagens persistentes (durable)
- ✅ Ack/Nack manual para confiabilidade
- ✅ Comunicação síncrona (HTTP) + assíncrona (RabbitMQ)

### DevOps & Deploy
- ✅ Docker multi-stage builds
- ✅ Docker Compose para orquestração
- ✅ Health checks para garantir disponibilidade
- ✅ Volumes para persistência de dados
- ✅ Networks isoladas para comunicação interna

### Qualidade de Código
- ✅ Testes unitários com xUnit
- ✅ Mocking de dependências com Moq
- ✅ Banco InMemory para testes isolados
- ✅ Padrão AAA (Arrange-Act-Assert)
- ✅ FluentAssertions para legibilidade

---

## 📖 Documentação Adicional

- **`DOCUMENTACAO.html`** - Documentação técnica completa (2.500+ linhas)
  - Diagramas de arquitetura ASCII
  - Fluxos detalhados passo a passo
  - Explicação de refatorações realizadas
  - Decisões arquiteturais justificadas
  - Guia de testes unitários
  - Lições aprendidas

- **Interfaces HTML:**
  - `estoque.html` - Interface administrativa
  - `vendas.html` - Interface do cliente

---

## 🤝 Contribuindo

Este é um projeto educacional do Desafio DIO. Contribuições são bem-vindas!

1. Fork o projeto
2. Crie uma branch (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

---

## 📄 Licença

Este projeto foi desenvolvido para fins educacionais como parte do Desafio DIO.

---

## 👤 Autor

Desenvolvido como parte do **Desafio DIO - Arquitetura de Microserviços**

---

## 🙏 Agradecimentos

- [Digital Innovation One (DIO)](https://www.dio.me/) - Plataforma de ensino
- Comunidade .NET - Documentação e suporte
- Microsoft - Framework .NET e ferramentas

---

## 📞 Suporte

Se encontrar algum problema:

1. Verifique a seção **Troubleshooting** acima
2. Verifique os logs: `docker-compose logs -f`
3. Abra uma issue no GitHub