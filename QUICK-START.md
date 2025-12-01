# 🚀 Quick Start - 5 Minutos para Rodar

## Pré-requisitos Rápidos

```powershell
# 1. Verificar .NET instalado
dotnet --version

# 2. Instalar RabbitMQ (se não tiver)
choco install rabbitmq
# OU baixar em: https://www.rabbitmq.com/download.html

# 3. Iniciar RabbitMQ
rabbitmq-server
```

## 🎯 Execução em 3 Passos

### Passo 1: Banco de Dados (30 segundos)

```powershell
# EstoqueService
cd EstoqueService
dotnet ef database update

# VendasService
cd ..\VendasService
dotnet ef database update

cd ..
```

### Passo 2: Iniciar Serviços (abrir 3 terminais)

**Terminal 1:**
```powershell
cd EstoqueService
dotnet run
# Porta: 5002
```

**Terminal 2:**
```powershell
cd VendasService
dotnet run
# Porta: 5000
```

**Terminal 3:**
```powershell
cd APIGateway
dotnet run
# Porta: 5001
```

### Passo 3: Testar (2 minutos)

**3.1 - Login:**
```bash
POST http://localhost:5002/api/auth/login
{
  "username": "admin",
  "password": "admin123"
}
```
**→ Copie o token retornado**

**3.2 - Cadastrar Produto:**
```bash
POST http://localhost:5002/api/produtos
Authorization: Bearer SEU_TOKEN
{
  "nome": "Notebook",
  "descricao": "Notebook Dell",
  "preco": 3000,
  "quantidade": 5
}
```

**3.3 - Login Cliente:**
```bash
POST http://localhost:5000/api/auth/login
{
  "username": "cliente",
  "password": "cliente123"
}
```
**→ Copie o novo token**

**3.4 - Criar Pedido:**
```bash
POST http://localhost:5000/api/pedidos
Authorization: Bearer SEU_TOKEN_CLIENTE
{
  "cliente": "João",
  "status": "Pendente",
  "itens": [
    {
      "produtoId": 1,
      "quantidade": 2
    }
  ]
}
```

**3.5 - Verificar Estoque Atualizado:**
```bash
GET http://localhost:5002/api/produtos/1
Authorization: Bearer SEU_TOKEN_CLIENTE
```
**→ Quantidade deve ter diminuído de 5 para 3!**

## ✅ Funcionou se...

1. ✅ Todos os 3 serviços iniciaram sem erro
2. ✅ Login retornou um token JWT
3. ✅ Produto foi cadastrado com sucesso
4. ✅ Pedido foi criado
5. ✅ **Estoque foi reduzido automaticamente** (via RabbitMQ)
6. ✅ Logs mostram "RabbitMQ Consumer iniciado" no EstoqueService

## 🐛 Troubleshooting Rápido

### Erro: "Connection refused" (RabbitMQ)
```powershell
# Iniciar RabbitMQ
rabbitmq-server
```

### Erro: "Database does not exist"
```powershell
cd EstoqueService
dotnet ef database update
cd ..\VendasService
dotnet ef database update
```

### Erro: "401 Unauthorized"
- Verifique se copiou o token completo
- Token expira em 2 horas, faça login novamente

### Porta já em uso
- Altere a porta no `launchSettings.json` de cada serviço

## 📱 Interfaces Úteis

- **Swagger EstoqueService:** http://localhost:5002/swagger
- **Swagger VendasService:** http://localhost:5000/swagger
- **Swagger API Gateway:** http://localhost:5001/swagger
- **RabbitMQ Management:** http://localhost:15672 (guest/guest)

## 🎯 Fluxo Completo Validado

```
1. Cliente faz login → Recebe token
2. Admin cadastra produto com estoque 5
3. Cliente cria pedido de 2 unidades
4. VendasService valida estoque (chamada HTTP)
5. Pedido é salvo no banco
6. Mensagem enviada ao RabbitMQ
7. EstoqueService recebe mensagem
8. Estoque atualizado para 3 unidades ✅
```

## 📝 Usuários de Teste

| Serviço | Usuário | Senha | Role |
|---------|---------|-------|------|
| Estoque | admin | admin123 | Administrador |
| Vendas | cliente | cliente123 | Cliente |
| Vendas | admin | admin123 | Administrador |

## 📚 Arquivos Importantes

- `README.md` - Documentação completa
- `TESTES.http` - Todos os exemplos de requisições
- `CORRECOES.md` - Detalhes de tudo que foi corrigido

## 🎓 Próximos Passos

1. Explorar o arquivo `TESTES.http` com todas as requisições
2. Ver logs do RabbitMQ Consumer no terminal do EstoqueService
3. Testar cenários de erro (estoque insuficiente, etc)
4. Usar o API Gateway (porta 5001) como ponto único de entrada

**Divirta-se testando! 🎉**