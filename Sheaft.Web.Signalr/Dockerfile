#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS=https://*:5010;http://*:5009
ENV ASPNETCORE_HTTPS_PORT=5010
EXPOSE 5010
EXPOSE 5009

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Sheaft.Web.Signalr/Sheaft.Web.Signalr.csproj", "Sheaft.Web.Signalr/"]
COPY ["Sheaft.Web.Common/Sheaft.Web.Common.csproj", "Sheaft.Web.Common/"]
COPY ["Sheaft.Application.Commands/Sheaft.Application.Commands.csproj", "Sheaft.Application.Commands/"]
COPY ["Sheaft.Application.Interop/Sheaft.Application.Interop.csproj", "Sheaft.Application.Interop/"]
COPY ["Sheaft.Application.Models/Sheaft.Application.Models.csproj", "Sheaft.Application.Models/"]
COPY ["Sheaft.Domain/Sheaft.Domain.csproj", "Sheaft.Domain/"]
COPY ["Sheaft.Core/Sheaft.Core.csproj", "Sheaft.Core/"]
COPY ["Sheaft.Exceptions/Sheaft.Exceptions.csproj", "Sheaft.Exceptions/"]
COPY ["Sheaft.Options/Sheaft.Options.csproj", "Sheaft.Options/"]
COPY ["Sheaft.Application.Handlers/Sheaft.Application.Handlers.csproj", "Sheaft.Application.Handlers/"]
COPY ["Sheaft.Infrastructure.Services/Sheaft.Infrastructure.Services.csproj", "Sheaft.Infrastructure.Services/"]
COPY ["Sheaft.Application.Events/Sheaft.Application.Events.csproj", "Sheaft.Application.Events/"]
COPY ["Sheaft.Infrastructure.Persistence/Sheaft.Infrastructure.Persistence.csproj", "Sheaft.Infrastructure.Persistence/"]
COPY ["Sheaft.Localization/Sheaft.Localization.csproj", "Sheaft.Localization/"]
RUN dotnet restore "Sheaft.Web.Signalr/Sheaft.Web.Signalr.csproj"
COPY . .
WORKDIR "/src/Sheaft.Web.Signalr"
RUN dotnet build "Sheaft.Web.Signalr.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sheaft.Web.Signalr.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sheaft.Web.Signalr.dll"]