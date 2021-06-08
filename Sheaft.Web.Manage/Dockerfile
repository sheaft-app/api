#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0.6-buster-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS=https://*:5008;http://*:5007
ENV ASPNETCORE_HTTPS_PORT=5008
EXPOSE 5008
EXPOSE 5007

FROM mcr.microsoft.com/dotnet/sdk:5.0.300-buster-slim AS build
WORKDIR /src
COPY ["Sheaft.Web.Manage/Sheaft.Web.Manage.csproj", "Sheaft.Web.Manage/"]
COPY ["Sheaft.Web.Common/Sheaft.Web.Common.csproj", "Sheaft.Web.Common/"]
COPY ["Sheaft.Application/Sheaft.Application.csproj", "Sheaft.Application/"]
COPY ["Sheaft.Domain/Sheaft.Domain.csproj", "Sheaft.Domain/"]
COPY ["Sheaft.Mailing/Sheaft.Mailing.csproj", "Sheaft.Mailing/"]
COPY ["Sheaft.Infrastructure/Sheaft.Infrastructure.csproj", "Sheaft.Infrastructure/"]
COPY ["Sheaft.Localization/Sheaft.Localization.csproj", "Sheaft.Localization/"]
COPY ["Sheaft.Mediatr/Sheaft.Mediatr.csproj", "Sheaft.Mediatr/"]
COPY ["Sheaft.Business/Sheaft.Business.csproj", "Sheaft.Business/"]
COPY ["Sheaft.Core/Sheaft.Core.csproj", "Sheaft.Core/"]
COPY ["Sheaft.Options/Sheaft.Options.csproj", "Sheaft.Options/"]
RUN dotnet restore "Sheaft.Web.Manage/Sheaft.Web.Manage.csproj"
COPY . .
WORKDIR "/src/Sheaft.Web.Manage"
RUN dotnet build "Sheaft.Web.Manage.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sheaft.Web.Manage.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sheaft.Web.Manage.dll"]
