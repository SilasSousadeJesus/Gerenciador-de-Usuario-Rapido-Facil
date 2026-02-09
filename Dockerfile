FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Gerenciador-de-Usuario-Rapido-Facil.sln .

COPY Gerenciado-de-Usuario-Papido-Facil.Api/Gerenciado-de-Usuario-Papido-Facil.Api.csproj Gerenciado-de-Usuario-Papido-Facil.Api/
COPY Gerenciado-de-Usuario-Papido-Facil.Application/Gerenciado-de-Usuario-Papido-Facil.Application.csproj Gerenciado-de-Usuario-Papido-Facil.Application/
COPY Gerenciado-de-Usuario-Papido-Facil.Domain/Gerenciado-de-Usuario-Papido-Facil.Domain.csproj Gerenciado-de-Usuario-Papido-Facil.Domain/
COPY Gerenciado-de-Usuario-Papido-Facil.CrossCutting/Gerenciado-de-Usuario-Papido-Facil.CrossCutting.csproj Gerenciado-de-Usuario-Papido-Facil.CrossCutting/
COPY Gerenciado-de-Usuario-Papido-Facil.Infra/Gerenciado-de-Usuario-Papido-Facil.Infra.csproj Gerenciado-de-Usuario-Papido-Facil.Infra/

RUN dotnet restore

COPY . .

WORKDIR /src/Gerenciado-de-Usuario-Papido-Facil.Api
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Gerenciado-de-Usuario-Papido-Facil.Api.dll"]
