#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/dotnet:3.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Sheaft.Functions/Sheaft.Functions.csproj", "Sheaft.Functions/"]
COPY ["Sheaft.Core/Sheaft.Core.csproj", "Sheaft.Core/"]
COPY ["Sheaft.Excpetions/Sheaft.Exceptions.csproj", "Sheaft.Exceptions/"]
COPY ["Sheaft.Localization/Sheaft.Localization.csproj", "Sheaft.Localization/"]
COPY ["Sheaft.Options/Sheaft.Options.csproj", "Sheaft.Options/"]
COPY ["Sheaft.Application.Commands/Sheaft.Application.Commands.csproj", "Sheaft.Application.Commands/"]
COPY ["Sheaft.Application.Events/Sheaft.Application.Events.csproj", "Sheaft.Application.Events/"]
COPY ["Sheaft.Application.Handlers/Sheaft.Application.Handlers.csproj", "Sheaft.Application.Handlers/"]
COPY ["Sheaft.Application.Interop/Sheaft.Application.Interop.csproj", "Sheaft.Application.Interop/"]
COPY ["Sheaft.Application.Mappers/Sheaft.Application.Mappers.csproj", "Sheaft.Application.Mappers/"]
COPY ["Sheaft.Application.Models/Sheaft.Application.Models.csproj", "Sheaft.Application.Models/"]
COPY ["Sheaft.Infrastructure.Services/Sheaft.Infrastructure.Services.csproj", "Sheaft.Infrastructure.Services/"]
COPY ["Sheaft.Infrastructure.Persistence/Sheaft.Infrastructure.Persistence.csproj", "Sheaft.Infrastructure.Persistence/"]
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