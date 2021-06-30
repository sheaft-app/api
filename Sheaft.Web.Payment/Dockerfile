#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0.6-buster-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS=https://*:5015;http://*:5014
ENV ASPNETCORE_HTTPS_PORT=5015
EXPOSE 5015
EXPOSE 5014

FROM mcr.microsoft.com/dotnet/sdk:5.0.300-buster-slim AS build
WORKDIR /src
COPY ["Sheaft.Web.Payment/Sheaft.Web.Payment.csproj", "Sheaft.Web.Payment/"]
COPY ["Sheaft.Web.Common/Sheaft.Web.Common.csproj", "Sheaft.Web.Common/"]
COPY ["Sheaft.Application/Sheaft.Application.csproj", "Sheaft.Application/"]
COPY ["Sheaft.Domain/Sheaft.Domain.csproj", "Sheaft.Domain/"]
COPY ["Sheaft.Infrastructure/Sheaft.Infrastructure.csproj", "Sheaft.Infrastructure/"]
COPY ["Sheaft.Mediatr/Sheaft.Mediatr.csproj", "Sheaft.Mediatr/"]
COPY ["Sheaft.Business/Sheaft.Business.csproj", "Sheaft.Business/"]
COPY ["Sheaft.Core/Sheaft.Core.csproj", "Sheaft.Core/"]
COPY ["Sheaft.Options/Sheaft.Options.csproj", "Sheaft.Options/"]
RUN dotnet restore "Sheaft.Web.Payment/Sheaft.Web.Payment.csproj"
COPY . .
WORKDIR "/src/Sheaft.Web.Payment"
RUN dotnet build "Sheaft.Web.Payment.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sheaft.Web.Payment.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sheaft.Web.Payment.dll"]
