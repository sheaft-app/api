#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/dotnet:3.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Sheaft.Functions/Sheaft.Functions.csproj", "Sheaft.Functions/"]
COPY ["Sheaft.Core/Sheaft.Core.csproj", "Sheaft.Core/"]
COPY ["Sheaft.Interop/Sheaft.Interop.csproj", "Sheaft.Interop/"]
COPY ["Sheaft.Application/Sheaft.Application.csproj", "Sheaft.Application/"]
COPY ["Sheaft.Services/Sheaft.Services.csproj", "Sheaft.Services/"]
COPY ["Sheaft.Infrastructure/Sheaft.Infrastructure.csproj", "Sheaft.Infrastructure/"]
COPY ["Sheaft.Domain/Sheaft.Domain.csproj", "Sheaft.Domain/"]
RUN dotnet restore "Sheaft.Functions/Sheaft.Functions.csproj"
COPY . .
WORKDIR "/src/Sheaft.Functions"
RUN dotnet build "Sheaft.Functions.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sheaft.Functions.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/app