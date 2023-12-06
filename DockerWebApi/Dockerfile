#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 2222

COPY DockerWebApi/entrypoint.sh ./

#Enable SSH
RUN apt-get update \
    && apt-get install -y --no-install-recommends dialog \
    && apt-get install -y --no-install-recommends openssh-server \
    && apt-get install -y --no-install-recommends iputils-ping \
    && echo "root:Docker!" | chpasswd \
    && chmod u+x ./entrypoint.sh

COPY DockerWebApi/sshd_config /etc/ssh/

RUN \ 
  apt install -y ca-certificates && \
  update-ca-certificates

#Build 
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DockerWebApi.csproj", "."]
RUN dotnet restore "./DockerWebApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "DockerWebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DockerWebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "DockerWebApi.dll"]

#Set envrionment variables for local development, these will be overridden by the environment variables in the container when deployed to Azure
ENV ASPNETCORE_ENVIRONMENT "Development"

ENTRYPOINT ["bash", "./entrypoint.sh" ] 