# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copia a Solution
COPY ["Gerenciado-de-Usuario-Rapido-Facil.sln", "./"]

# 2. Copia todos os .csproj mantendo a estrutura de pastas (Necessário para o Restore)
COPY ["Gerenciado-de-Usuario-Rapido-Facil.Api/Gerenciado-de-Usuario-Rapido-Facil.Api.csproj", "Gerenciado-de-Usuario-Rapido-Facil.Api/"]
COPY ["Gerenciado-de-Usuario-Rapido-Facil.Application/Gerenciado-de-Usuario-Rapido-Facil.Application.csproj", "Gerenciado-de-Usuario-Rapido-Facil.Application/"]
COPY ["Gerenciado-de-Usuario-Rapido-Facil.Domain/Gerenciado-de-Usuario-Rapido-Facil.Domain.csproj", "Gerenciado-de-Usuario-Rapido-Facil.Domain/"]
COPY ["Gerenciado-de-Usuario-Rapido-Facil.Infra/Gerenciado-de-Usuario-Rapido-Facil.Infra.csproj", "Gerenciado-de-Usuario-Rapido-Facil.Infra/"]
COPY ["Gerenciado-de-Usuario-Rapido-Facil.CrossCutting/Gerenciado-de-Usuario-Rapido-Facil.CrossCutting.csproj", "Gerenciado-de-Usuario-Rapido-Facil.CrossCutting/"]

# 3. Restaura as dependências da Solution inteira
RUN dotnet restore "Gerenciado-de-Usuario-Rapido-Facil.sln"

# 4. Agora copia o restante do código fonte
COPY . .

# 5. Build e Publish da API
WORKDIR "/src/Gerenciado-de-Usuario-Rapido-Facil.Api"
RUN dotnet publish "Gerenciado-de-Usuario-Rapido-Facil.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio de Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
# O .NET 8 usa a porta 8080 por padrão
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Gerenciado-de-Usuario-Rapido-Facil.Api.dll"]