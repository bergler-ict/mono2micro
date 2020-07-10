#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ProductService.Stub.MessageContracts/*.csproj", "ProductService.Stub.MessageContracts/"]
COPY ["SalesService.Data/*.csproj", "SalesService.Data/"]
COPY ["SalesService.Web/*.csproj", "SalesService.Web/"]

RUN dotnet restore "SalesService.Web/SalesService.Web.csproj"
COPY . .

WORKDIR "/src/ProductService.Stub.MessageContracts"
RUN dotnet build "ProductService.Stub.MessageContracts.csproj" -c Release -o /app/build

WORKDIR "/src/SalesService.Data"
RUN dotnet build "SalesService.Data.csproj" -c Release -o /app/build

WORKDIR "/src/SalesService.Web"
RUN dotnet build "SalesService.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SalesService.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SalesService.Web.dll"]

