#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN \ 
  apt update && \
  apt install -y ca-certificates && \
  update-ca-certificates

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DockerWebApi2/DockerWebApi2.csproj", "DockerWebApi2/"]
RUN dotnet restore "DockerWebApi2/DockerWebApi2.csproj"
COPY . .
WORKDIR "/src/DockerWebApi2"
RUN dotnet build "DockerWebApi2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DockerWebApi2.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DockerWebApi2.dll"]