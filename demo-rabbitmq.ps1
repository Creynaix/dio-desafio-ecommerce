# ========================================
#   DEMO RABBITMQ - E-Commerce DIO
# ========================================

# Configurar cores
$Host.UI.RawUI.BackgroundColor = "Black"
$Host.UI.RawUI.ForegroundColor = "White"
Clear-Host

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  DEMONSTRACAO RABBITMQ - MICROSERVICES" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

# URLs base
$gateway = "http://localhost:5004"
$headers = @{ "Content-Type" = "application/json" }

# ========================================
# PASSO 1: Login Admin
# ========================================
Write-Host "[PASSO 1] Fazendo login como ADMIN..." -ForegroundColor Yellow

$loginAdmin = @{
    username = "admin"
    password = "admin123"
} | ConvertTo-Json

try {
    $responseAdmin = Invoke-RestMethod -Uri "$gateway/api/auth/login" -Method POST -Body $loginAdmin -ContentType "application/json"
    $tokenAdmin = $responseAdmin.token
    Write-Host "✅ Login admin realizado com sucesso!" -ForegroundColor Green
    Write-Host "   Token: $($tokenAdmin.Substring(0,50))..." -ForegroundColor Gray
    Start-Sleep -Seconds 1
} catch {
    Write-Host "❌ Erro no login admin: $_" -ForegroundColor Red
    exit 1
}

# ========================================
# PASSO 2: Criar Produto
# ========================================
Write-Host "`n[PASSO 2] Criando produto..." -ForegroundColor Yellow

$produto = @{
    nome = "Teclado Mecanico RGB"
    descricao = "Teclado gamer com iluminacao RGB"
    preco = 350.00
    quantidade = 100
} | ConvertTo-Json

$headersAdmin = @{
    "Content-Type" = "application/json"
    "Authorization" = "Bearer $tokenAdmin"
}

try {
    $responseProduto = Invoke-RestMethod -Uri "$gateway/api/gateway/estoque/api/produtos" -Method POST -Body $produto -Headers $headersAdmin
    Write-Host "✅ Produto criado! ID: $($responseProduto.id)" -ForegroundColor Green
    Write-Host "   Nome: $($responseProduto.nome)" -ForegroundColor Gray
    Write-Host "   Estoque: $($responseProduto.quantidade) unidades" -ForegroundColor Gray
    $produtoId = $responseProduto.id
    Start-Sleep -Seconds 1
} catch {
    Write-Host "⚠️  Produto pode já existir, usando ID 1..." -ForegroundColor Yellow
    $produtoId = 1
}

# ========================================
# PASSO 3: Login Cliente
# ========================================
Write-Host "`n[PASSO 3] Fazendo login como CLIENTE..." -ForegroundColor Yellow

$loginCliente = @{
    username = "cliente"
    password = "cliente123"
} | ConvertTo-Json

try {
    $responseCliente = Invoke-RestMethod -Uri "$gateway/api/auth/login" -Method POST -Body $loginCliente -ContentType "application/json"
    $tokenCliente = $responseCliente.token
    Write-Host "✅ Login cliente realizado com sucesso!" -ForegroundColor Green
    Start-Sleep -Seconds 1
} catch {
    Write-Host "❌ Erro no login cliente: $_" -ForegroundColor Red
    exit 1
}

# ========================================
# PASSO 4: Criar 5 Pedidos
# ========================================
Write-Host "`n[PASSO 4] Criando 5 pedidos..." -ForegroundColor Yellow
Write-Host "`n╔════════════════════════════════════════════╗" -ForegroundColor Red
Write-Host "║  ABRA O DASHBOARD RABBITMQ AGORA!         ║" -ForegroundColor Yellow
Write-Host "║  http://localhost:15672                   ║" -ForegroundColor White
Write-Host "║  Login: guest / guest                     ║" -ForegroundColor White
Write-Host "║  Vá em: Queues -> vendasQueue             ║" -ForegroundColor White
Write-Host "╚════════════════════════════════════════════╝" -ForegroundColor Red
Write-Host "`nIniciando em 3 segundos..." -ForegroundColor Gray
Start-Sleep -Seconds 3

$headersCliente = @{
    "Content-Type" = "application/json"
    "Authorization" = "Bearer $tokenCliente"
}

$pedidosCriados = 0
$totalItens = 0

for ($i = 1; $i -le 5; $i++) {
    Write-Host "`n  Criando pedido $i/5..." -ForegroundColor Cyan
    
    $pedido = @{
        cliente = "Cliente Teste $i"
        itens = @(
            @{
                produtoId = $produtoId
                quantidade = 2
            }
        )
    } | ConvertTo-Json -Depth 3
    
    try {
        $responsePedido = Invoke-RestMethod -Uri "$gateway/api/gateway/vendas/api/pedidos" -Method POST -Body $pedido -Headers $headersCliente
        Write-Host "  ✅ Pedido criado! ID: $($responsePedido.id) | Status: $($responsePedido.status)" -ForegroundColor Green
        $pedidosCriados++
        $totalItens += 2
        
        if ($i -eq 1) {
            Write-Host "`n  👀 Olhe o dashboard agora! As mensagens estão sendo processadas..." -ForegroundColor Yellow
        }
        
        Start-Sleep -Milliseconds 800
    } catch {
        Write-Host "  ❌ Erro ao criar pedido: $_" -ForegroundColor Red
    }
}

# ========================================
# PASSO 5: Ver Logs do Consumer
# ========================================
Write-Host "`n`n[PASSO 5] Verificando logs do consumer..." -ForegroundColor Yellow
Start-Sleep -Seconds 1

Write-Host "`nÚltimas mensagens processadas:" -ForegroundColor Cyan
docker-compose logs --tail=20 estoque-service | Select-String -Pattern "Mensagem recebida|Estoque atualizado" | ForEach-Object {
    if ($_ -match "Mensagem recebida") {
        Write-Host "  📩 $_" -ForegroundColor White
    } elseif ($_ -match "Estoque atualizado") {
        Write-Host "  ✅ $_" -ForegroundColor Green
    }
}

# ========================================
# RESUMO
# ========================================
Write-Host "`n`n========================================" -ForegroundColor Cyan
Write-Host "  RESUMO DA DEMONSTRACAO" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  ✅ Pedidos criados: $pedidosCriados" -ForegroundColor Green
Write-Host "  ✅ Total de itens: $totalItens unidades" -ForegroundColor Green
Write-Host "  ✅ Fila processada com sucesso!" -ForegroundColor Green
Write-Host "`n  Dashboard: http://localhost:15672" -ForegroundColor Yellow
Write-Host "========================================`n" -ForegroundColor Cyan

Write-Host "Pressione qualquer tecla para fechar..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
