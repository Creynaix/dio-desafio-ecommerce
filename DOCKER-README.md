# 🐳 Executando o Projeto com Docker

Este guia explica como executar todo o sistema de microservices usando Docker e Docker Compose.

## Pré-requisitos

- **Docker Desktop** instalado e rodando
- **Docker Compose** (já incluído no Docker Desktop)
- Portas disponíveis: 5000, 5002, 5004, 1433, 5672, 15672

## Início Rápido

### Windows (PowerShell)

```powershell
# 1. Executar script de inicialização automática
.\start-docker.ps1
```

### Linux/Mac ou Manual

```bash
# 1. Construir e iniciar todos os containers
docker-compose up --build -d

# 2. Aguardar serviços iniciarem (~15 segundos)
sleep 15

# 3. Aplicar migrations
docker-compose exec estoque-service dotnet ef database update
docker-compose exec vendas-service dotnet ef database update

# 4. Verificar status
docker-compose ps
```

## Estrutura dos Containers

| Container | Porta | Descrição |
|-----------|-------|-----------|
| `api-gateway` | 5004 | API Gateway com autenticação JWT |
| `estoque-service` | 5000 | Microserviço de gestão de produtos |
| `vendas-service` | 5002 | Microserviço de gestão de pedidos |
| `sqlserver` | 1433 | SQL Server 2019 Express |
| `rabbitmq` | 5672, 15672 | RabbitMQ com interface de gerenciamento |

## Endpoints Disponíveis

### API Gateway (Ponto de Entrada)
- **Base URL:** http://localhost:5004
- **Swagger:** http://localhost:5004/swagger
- **Login:** POST http://localhost:5004/api/auth/login
- **Estoque:** http://localhost:5004/api/gateway/estoque/api/produtos
- **Vendas:** http://localhost:5004/api/gateway/vendas/api/pedidos

### Estoque Service (Interno)
- **Base URL:** http://localhost:5000
- **Swagger:** http://localhost:5000/swagger

### Vendas Service (Interno)
- **Base URL:** http://localhost:5002
- **Swagger:** http://localhost:5002/swagger

### RabbitMQ Management
- **URL:** http://localhost:15672
- **Credenciais:** guest / guest

## Credenciais do Sistema

### Usuários da Aplicação
- **Administrador:** `admin` / `admin123`
- **Cliente:** `cliente` / `cliente123`

### Banco de Dados
- **Server:** localhost,1433
- **Username:** sa
- **Password:** SqlServer2019!
- **Databases:** EstoqueDB, VendasDB

## Comandos Úteis

### Ver logs de todos os serviços
```bash
docker-compose logs -f
```

### Ver logs de um serviço específico
```bash
docker-compose logs -f api-gateway
docker-compose logs -f estoque-service
docker-compose logs -f vendas-service
```

### Parar todos os containers
```bash
docker-compose down
```

### Parar e remover volumes (limpa banco de dados)
```bash
docker-compose down -v
```

### Restart de um serviço específico
```bash
docker-compose restart api-gateway
```

### Acessar shell de um container
```bash
docker-compose exec estoque-service /bin/bash
```

### Rebuild sem cache
```bash
docker-compose build --no-cache
docker-compose up -d
```

## Testando o Sistema

### 1. Via Swagger (Recomendado)
1. Acesse http://localhost:5004/swagger
2. Use POST /api/auth/login com:
   ```json
   {
     "username": "admin",
     "password": "admin123"
   }
   ```
3. Copie o token retornado
4. Clique em "Authorize" no Swagger e cole: `Bearer {token}`
5. Teste os endpoints

### 2. Via HTML Pages
1. Abra `estoque.html` no navegador
2. Faça login com admin/admin123
3. Cadastre produtos

4. Abra `vendas.html` no navegador
5. Faça login com cliente/cliente123
6. Crie pedidos

### 3. Via cURL
```bash
# Login
curl -X POST http://localhost:5004/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"admin123"}'

# Listar produtos (substitua {TOKEN})
curl http://localhost:5004/api/gateway/estoque/api/produtos \
  -H "Authorization: Bearer {TOKEN}"
```

## Troubleshooting

### Containers não iniciam
```bash
# Verificar logs
docker-compose logs

# Verificar se portas estão ocupadas
netstat -ano | findstr "5000 5002 5004 1433 5672"
```

### Erro de migração de banco
```bash
# Recriar banco do zero
docker-compose down -v
docker-compose up -d
sleep 15
docker-compose exec estoque-service dotnet ef database update
docker-compose exec vendas-service dotnet ef database update
```

### RabbitMQ não conecta
```bash
# Verificar se está saudável
docker-compose ps rabbitmq

# Restart
docker-compose restart rabbitmq
sleep 10
docker-compose restart estoque-service vendas-service
```

### SQL Server não aceita conexões
```bash
# Verificar logs
docker-compose logs sqlserver

# Aguardar inicialização completa (pode levar até 30s)
docker-compose logs -f sqlserver
# Aguarde mensagem: "SQL Server is now ready for client connections"
```

## Arquitetura Docker

```
┌─────────────────────────────────────────────────────┐
│                  ecommerce-network                  │
│                   (Docker Bridge)                   │
│                                                     │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────┐ │
│  │ api-gateway  │  │   estoque    │  │  vendas  │ │
│  │   :5004      │←→│   :5000      │←→│  :5002   │ │
│  └──────┬───────┘  └──────┬───────┘  └────┬─────┘ │
│         │                 │                │       │
│         │        ┌────────┴────────────────┘       │
│         │        │                                 │
│         │  ┌─────▼──────┐      ┌────────────────┐ │
│         │  │ sqlserver  │      │   rabbitmq     │ │
│         │  │   :1433    │      │ :5672, :15672  │ │
│         │  └────────────┘      └────────────────┘ │
│         │                                          │
└─────────┼──────────────────────────────────────────┘
          │
     ┌────▼─────┐
     │  Cliente │
     │   HTML   │
     └──────────┘
```

## Volumes Persistentes

Os dados são mantidos em volumes Docker:
- `sqlserver_data`: Dados do SQL Server
- `rabbitmq_data`: Dados do RabbitMQ

Para limpar completamente:
```bash
docker-compose down -v
docker volume rm dio-desafio-ecommerce_sqlserver_data
docker volume rm dio-desafio-ecommerce_rabbitmq_data
```

## Boas Práticas

1. **Sempre use o Gateway** para chamadas externas (porta 5004)
2. **Não acesse microservices diretamente** em produção (portas 5000, 5002)
3. **Monitore RabbitMQ** para verificar processamento de mensagens
4. **Verifique logs** em caso de erros: `docker-compose logs -f`
5. **Backup de dados** antes de `docker-compose down -v`

## Publicando Imagens

### Docker Hub
```bash
# Tag images
docker tag dio-desafio-ecommerce-api-gateway yourusername/dio-api-gateway:latest
docker tag dio-desafio-ecommerce-estoque-service yourusername/dio-estoque:latest
docker tag dio-desafio-ecommerce-vendas-service yourusername/dio-vendas:latest

# Push images
docker push yourusername/dio-api-gateway:latest
docker push yourusername/dio-estoque:latest
docker push yourusername/dio-vendas:latest
```

### GitHub Container Registry
```bash
# Login
echo $GITHUB_TOKEN | docker login ghcr.io -u USERNAME --password-stdin

# Tag e push
docker tag dio-desafio-ecommerce-api-gateway ghcr.io/username/dio-api-gateway:latest
docker push ghcr.io/username/dio-api-gateway:latest
```

## Próximos Passos

- [ ] Adicionar Kubernetes manifests (k8s/)
- [ ] Implementar CI/CD com GitHub Actions
- [ ] Adicionar health checks customizados
- [ ] Configurar Nginx como reverse proxy
- [ ] Implementar circuit breaker (Polly)
- [ ] Adicionar Redis para cache
- [ ] Configurar ELK Stack para logs centralizados
- [ ] Implementar OpenTelemetry para tracing

## Suporte

Para problemas ou dúvidas:
1. Verifique logs: `docker-compose logs -f`
2. Consulte DOCUMENTACAO.html
3. Abra issue no repositório GitHub
