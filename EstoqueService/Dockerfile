FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EstoqueService/EstoqueService.csproj", "EstoqueService/"]
RUN dotnet restore "EstoqueService/EstoqueService.csproj"
COPY . .
WORKDIR "/src/EstoqueService"
RUN dotnet build "EstoqueService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EstoqueService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EstoqueService.dll"]
