# 🐳 Guia Rápido - Docker

Execute o sistema E-Commerce completo com Docker em 3 passos simples.

## ⚡ Método 1: Script Automatizado (Windows)

### Passo 1: Pré-requisito
- Docker Desktop instalado e rodando
- Download: https://docs.docker.com/desktop/install/windows-install/

### Passo 2: Executar Script

``powershell
# Navegar para o diretório do projeto
cd dio-desafio-ecommerce

# Executar script PowerShell
.\start-docker.ps1
``

### Passo 3: Aguardar

O script irá:
1. ✅ Verificar se Docker está rodando
2. ✅ Parar containers antigos (se existirem)
3. ✅ Construir todas as imagens Docker
4. ✅ Iniciar SQL Server + RabbitMQ + 3 microserviços
5. ✅ Aguardar 30 segundos para serviços ficarem prontos
6. ✅ Exibir URLs dos serviços
7. ✅ Oferecer abrir Swagger automaticamente

**Tempo estimado:** ~2-3 minutos (primeira vez)

---

## 🔧 Método 2: Comando Manual

``powershell
# Um único comando
docker-compose up --build

# Ou em background (detached)
docker-compose up --build -d
``

---

## 🌐 Serviços Disponíveis

Após inicialização bem-sucedida:

| Serviço | URL | Credenciais |
|---------|-----|-------------|
| **API Gateway** | http://localhost:5004/swagger | - |
| **Estoque Service** | http://localhost:5000/swagger | - |
| **Vendas Service** | http://localhost:5002/swagger | - |
| **RabbitMQ Management** | http://localhost:15672 | guest/guest |
| **SQL Server** | localhost:1433 | sa/YourStrong@Passw0rd |

---

## 📋 Comandos Úteis

``powershell
# Ver logs em tempo real
docker-compose logs -f

# Ver logs de um serviço específico
docker-compose logs -f estoque-service
docker-compose logs -f vendas-service
docker-compose logs -f api-gateway

# Verificar status dos containers
docker-compose ps

# Parar serviços (mantém dados)
docker-compose stop

# Reiniciar serviços
docker-compose restart

# Parar e remover containers
docker-compose down

# Parar e remover TUDO (incluindo volumes/bancos)
docker-compose down -v

# Rebuild sem cache
docker-compose build --no-cache
docker-compose up
``

---

## 🧪 Testando o Sistema

### 1. Abrir Swagger
- Acesse: http://localhost:5004/swagger

### 2. Fazer Login
- Endpoint: ``POST /api/auth/login``
- Body:
  ``json
  {
    \"username\": \"admin\",
    \"password\": \"admin123\"
  }
  ``
- Copie o **token** retornado

### 3. Autorizar no Swagger
- Clique no botão **"Authorize"** no topo
- Cole: ``Bearer {seu_token}``
- Clique "Authorize"

### 4. Cadastrar Produto
- Endpoint: ``POST /api/gateway/estoque/api/produtos``
- Body:
  ``json
  {
    \"nome\": \"Notebook Dell\",
    \"descricao\": \"i7 16GB RAM\",
    \"preco\": 3500.00,
    \"quantidade\": 10
  }
  ``

### 5. Criar Pedido
- Faça login como cliente (cliente/cliente123)
- Endpoint: ``POST /api/gateway/vendas/api/pedidos``
- Body:
  ``json
  {
    \"cliente\": \"Maria Silva\",
    \"itens\": [
      {
        \"produtoId\": 1,
        \"quantidade\": 2
      }
    ]
  }
  ``

### 6. Verificar Estoque Atualizado
- Endpoint: ``GET /api/gateway/estoque/api/produtos/1``
- **Quantidade foi reduzida automaticamente via RabbitMQ!**

---

## 🐛 Troubleshooting

### Docker não está rodando
``powershell
# Verificar se Docker Desktop está aberto
# Reiniciar Docker Desktop se necessário
``

### Portas em uso
``powershell
# Parar processos que estão usando as portas:
# 5000, 5002, 5004, 1433, 5672, 15672

# Verificar portas em uso:
netstat -ano | findstr :5004
``

### SQL Server não inicia
``powershell
# Limpar volumes e reiniciar
docker-compose down -v
docker-compose up --build
``

### RabbitMQ não conecta
``powershell
# Verificar logs do RabbitMQ
docker-compose logs rabbitmq

# Reiniciar apenas o RabbitMQ
docker-compose restart rabbitmq
``

### Containers não sobem
``powershell
# Ver erros detalhados
docker-compose up --build

# Verificar logs de um container específico
docker logs ecommerce-sqlserver
docker logs ecommerce-rabbitmq
docker logs ecommerce-estoque
``

---

## 🔍 Verificar Health Checks

``powershell
# SQL Server
docker inspect ecommerce-sqlserver | Select-String -Pattern \"Health\"

# RabbitMQ
docker inspect ecommerce-rabbitmq | Select-String -Pattern \"Health\"
``

---

## 💾 Persistência de Dados

Os dados são persistidos em volumes Docker:
- ``sqlserver-data`` - Bancos EstoqueDB e VendasDB
- ``rabbitmq-data`` - Filas e mensagens RabbitMQ

Para **limpar todos os dados**:
``powershell
docker-compose down -v
``

---

## 📊 Monitoramento

``powershell
# Ver uso de CPU, Memória, Network de cada container
docker stats

# Ver uso de disco
docker system df

# Limpar recursos não utilizados
docker system prune -a --volumes
``

---

## 🎯 Próximos Passos

1. ✅ Sistema rodando com Docker
2. 📖 Leia ``DOCUMENTACAO.html`` para entender a arquitetura
3. 🧪 Execute ``dotnet test`` para rodar testes unitários
4. 🚀 Experimente criar produtos e pedidos via Swagger
5. 📊 Monitore o RabbitMQ em http://localhost:15672

---

## 📚 Recursos Adicionais

- **README.md** - Documentação completa do projeto
- **DOCUMENTACAO.html** - Documentação técnica detalhada (2.500+ linhas)
- **docker-compose.yml** - Configuração de orquestração
- **.dockerignore** - Otimizações de build

---

**🎉 Sucesso!** Você tem um sistema de microserviços completo rodando!
