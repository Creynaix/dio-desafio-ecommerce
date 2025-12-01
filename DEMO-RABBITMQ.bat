@echo off
title DEMO RABBITMQ - E-Commerce Microservices
color 0A
echo.
echo ========================================
echo   INICIANDO DEMONSTRACAO RABBITMQ
echo ========================================
echo.
echo Abrindo RabbitMQ Dashboard...
start http://localhost:15672
timeout /t 2 /nobreak > nul
echo.
echo Executando script de demonstracao...
echo.
powershell -ExecutionPolicy Bypass -File "%~dp0demo-rabbitmq.ps1"
