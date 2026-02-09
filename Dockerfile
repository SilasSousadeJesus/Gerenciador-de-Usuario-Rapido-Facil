FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Gerenciado-de-Usuario-Rapido-Facil.sln .

COPY Gerenciado-de-Usuario-Rapido-Facil.Api/*.csproj Gerenciado-de-Usuario-Rapido-Facil.Api/
COPY Gerenciado-de-Usuario-Rapido-Facil.Application/*.csproj Gerenciado-de-Usuario-Rapido-Facil.Application/
COPY Gerenciado-de-Usuario-Rapido-Facil.Domain/*.csproj Gerenciado-de-Usuario-Rapido-Facil.Domain/
COPY Gerenciado-de-Usuario-Rapido-Facil.CrossCutting/*.csproj Gerenciado-de-Usuario-Rapido-Facil.CrossCutting/
COPY Gerenciado-de-Usuario-Rapido-Facil.Infra/*.csproj Gerenciado-de-Usuario-Rapido-Facil.Infra/

RUN dotnet restore

COPY . .
WORKDIR /src/Gerenciado-de-Usuario-Rapido-Facil.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Gerenciado-de-Usuario-Rapido-Facil.Api.dll"]
