# Etapa de build com .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia todos os .csproj para o build context
COPY PortalTCMSP.Api/PortalTCMSP.Api.csproj PortalTCMSP.Api/
COPY PortalTCMSP.Domain/PortalTCMSP.Domain.csproj PortalTCMSP.Domain/
COPY PortalTCMSP.Infra.Ioc/PortalTCMSP.Infra.Ioc.csproj PortalTCMSP.Infra.Ioc/
COPY PortalTCMSP.Infra/PortalTCMSP.Infra.csproj PortalTCMSP.Infra/

# Restaura os pacotes
RUN dotnet restore "PortalTCMSP.Api/PortalTCMSP.Api.csproj"

# Copia o restante do código
COPY . .

# Compila a aplicação
WORKDIR /src/PortalTCMSP.Api
RUN dotnet build "PortalTCMSP.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa de publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PortalTCMSP.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Imagem final para execução
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PortalTCMSP.Api.dll"]
 