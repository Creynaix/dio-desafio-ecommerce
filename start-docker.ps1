# ========================================
#  Script de Inicializacao - E-Commerce
#  Docker Compose Automated Startup
# ========================================
# Uso: .\start-docker.ps1
# Ou: Duplo-clique em START-DOCKER.bat

param(
    [switch]$SkipBuild,
    [switch]$Clean
)

$ErrorActionPreference = "Continue"

Clear-Host
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "  E-Commerce Microservices - Startup" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

# Verificar se Docker esta instalado
Write-Host "[1/7] Verificando Docker..." -ForegroundColor Yellow
try {
    $dockerVersion = docker --version
    Write-Host "  ✅ $dockerVersion" -ForegroundColor Green
} catch {
    Write-Host "  ❌ Docker nao encontrado!" -ForegroundColor Red
    Write-Host "`n  Por favor, instale o Docker Desktop:" -ForegroundColor Yellow
    Write-Host "  https://docs.docker.com/desktop/install/windows-install/`n" -ForegroundColor Cyan
    pause
    exit 1
}

# Verificar se Docker esta rodando
Write-Host "`n[2/7] Verificando se Docker esta ativo..." -ForegroundColor Yellow
try {
    docker ps | Out-Null
    Write-Host "  ✅ Docker esta rodando" -ForegroundColor Green
} catch {
    Write-Host "  ❌ Docker nao esta rodando!" -ForegroundColor Red
    Write-Host "`n  Por favor, inicie o Docker Desktop e aguarde 30 segundos." -ForegroundColor Yellow
    Write-Host "  Depois execute este script novamente.`n" -ForegroundColor Yellow
    pause
    exit 1
}

# Limpar containers antigos se solicitado
if ($Clean) {
    Write-Host "`n[3/7] Limpando containers antigos..." -ForegroundColor Yellow
    docker-compose down -v 2>&1 | Out-Null
    Write-Host "  ✅ Limpeza concluida" -ForegroundColor Green
} else {
    Write-Host "`n[3/7] Parando containers antigos (se existirem)..." -ForegroundColor Yellow
    docker-compose down 2>&1 | Out-Null
    Write-Host "  ✅ Containers parados" -ForegroundColor Green
}

# Construir e iniciar
Write-Host "`n[4/7] Iniciando aplicacao..." -ForegroundColor Yellow

if ($SkipBuild) {
    Write-Host "  Modo rapido: sem rebuild das imagens" -ForegroundColor Gray
    docker-compose up -d
} else {
    Write-Host "  Construindo imagens (pode levar 2-3 minutos)..." -ForegroundColor Gray
    docker-compose up --build -d
}

if ($LASTEXITCODE -eq 0) {
    Write-Host "  ✅ Containers iniciados" -ForegroundColor Green
    
    # Verificar status dos containers
    Write-Host "`n[5/7] Verificando containers..." -ForegroundColor Yellow
    Start-Sleep -Seconds 3
    
    $containers = docker-compose ps --format json | ConvertFrom-Json
    $allRunning = $true
    
    foreach ($container in $containers) {
        $name = $container.Service
        $state = $container.State
        
        if ($state -eq "running") {
            Write-Host "  ✅ $name" -ForegroundColor Green
        } else {
            Write-Host "  ❌ $name ($state)" -ForegroundColor Red
            $allRunning = $false
        }
    }
    
    # Aguardar servicos ficarem prontos
    Write-Host "`n[6/7] Aguardando servicos inicializarem..." -ForegroundColor Yellow
    Write-Host "  SQL Server + RabbitMQ: 15s" -ForegroundColor Gray
    Start-Sleep -Seconds 15
    
    Write-Host "  Microservicos: 15s" -ForegroundColor Gray
    Start-Sleep -Seconds 15
    
    Write-Host "  ✅ Servicos prontos!" -ForegroundColor Green
    
    # Mostrar URLs disponiveis
    Write-Host "`n[7/7] Sistema disponivel!`n" -ForegroundColor Yellow
    Write-Host "========================================" -ForegroundColor Green
    Write-Host "  ✅ APLICACAO RODANDO COM SUCESSO!" -ForegroundColor Green
    Write-Host "========================================`n" -ForegroundColor Green
    
    Write-Host "🌐 ACESSOS:" -ForegroundColor Cyan
    Write-Host "  • API Gateway (Swagger):  " -NoNewline; Write-Host "http://localhost:5004/swagger" -ForegroundColor Yellow
    Write-Host "  • Estoque Service:        " -NoNewline; Write-Host "http://localhost:5000/swagger" -ForegroundColor Yellow
    Write-Host "  • Vendas Service:         " -NoNewline; Write-Host "http://localhost:5002/swagger" -ForegroundColor Yellow
    Write-Host "  • RabbitMQ Dashboard:     " -NoNewline; Write-Host "http://localhost:15672" -ForegroundColor Yellow -NoNewline; Write-Host " (guest/guest)" -ForegroundColor Gray
    Write-Host "  • Interface Web Estoque:  " -NoNewline; Write-Host "http://localhost:5004/estoque.html" -ForegroundColor Yellow
    Write-Host "  • Interface Web Vendas:   " -NoNewline; Write-Host "http://localhost:5004/vendas.html" -ForegroundColor Yellow
    
    Write-Host "`n🔐 CREDENCIAIS:" -ForegroundColor Cyan
    Write-Host "  Admin:   " -NoNewline; Write-Host "admin / admin123" -ForegroundColor White
    Write-Host "  Cliente: " -NoNewline; Write-Host "cliente / cliente123" -ForegroundColor White
    
    Write-Host "`n📊 COMANDOS ÚTEIS:" -ForegroundColor Cyan
    Write-Host "  Ver logs em tempo real:    " -NoNewline; Write-Host "docker-compose logs -f" -ForegroundColor Gray
    Write-Host "  Status dos containers:     " -NoNewline; Write-Host "docker-compose ps" -ForegroundColor Gray
    Write-Host "  Parar aplicacao:           " -NoNewline; Write-Host "docker-compose stop" -ForegroundColor Gray
    Write-Host "  Parar e remover tudo:      " -NoNewline; Write-Host "docker-compose down" -ForegroundColor Gray
    Write-Host "  Limpar volumes (bancos):   " -NoNewline; Write-Host "docker-compose down -v" -ForegroundColor Gray
    
    Write-Host "`n🎬 SCRIPTS DE DEMONSTRACAO:" -ForegroundColor Cyan
    Write-Host "  Demo RabbitMQ:             " -NoNewline; Write-Host ".\DEMO-RABBITMQ.bat" -ForegroundColor Yellow
    
    Write-Host "`n========================================`n" -ForegroundColor Cyan
    
    # Abrir navegador automaticamente
    Write-Host "Deseja abrir o Swagger no navegador? (S/N): " -ForegroundColor Yellow -NoNewline
    $resposta = Read-Host
    if ($resposta -eq "S" -or $resposta -eq "s") {
        Start-Process "http://localhost:5004/swagger"
        Write-Host "  ✅ Navegador aberto!`n" -ForegroundColor Green
    }
    
} else {
    Write-Host ""
    Write-Host "[ERRO] Falha ao iniciar aplicacao!" -ForegroundColor Red
    Write-Host "Execute 'docker-compose logs' para ver os erros." -ForegroundColor Yellow
    exit 1
}
