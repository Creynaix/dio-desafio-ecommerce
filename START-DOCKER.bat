@echo off
title E-Commerce Microservices - Docker Startup
color 0B
cls
echo.
echo ========================================
echo   E-COMMERCE MICROSERVICES
echo   Inicializacao Automatica com Docker
echo ========================================
echo.
echo Executando script PowerShell...
echo.
powershell -ExecutionPolicy Bypass -File "%~dp0start-docker.ps1"
