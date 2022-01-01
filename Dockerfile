FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://+:5000;https://+:5001
# ENV ASPNETCORE_URLS=https://+:443;http://+:80

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM node:14 as client-app
WORKDIR /usr/src/app
COPY ["./src/spa/package.json", "./src/spa/package-lock.json", "/usr/src/app/"]
RUN npm install --silent
COPY ./src/spa/ /usr/src/app
RUN npm run lint
RUN npm run build-prod

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["./src/api/LibraryManagementSystem/LibraryManagementSystem.csproj", "LibraryManagementSystem/"]
COPY ["./src/api/LMSContracts/LMSContracts.csproj", "LMSContracts/"]
COPY ["./src/api/LMSEntities/LMSEntities.csproj", "LMSEntities/"]
COPY ["./src/api/LMSRepository/LMSRepository.csproj", "LMSRepository/"]
COPY ["./src/api/LMSService/LMSService.csproj", "LMSService/"]
RUN dotnet restore "LibraryManagementSystem/LibraryManagementSystem.csproj"
COPY ./src/api/ .
WORKDIR "/src/LibraryManagementSystem"
COPY --from=client-app /usr/src/api/LibraryManagementSystem/wwwroot /src/LibraryManagementSystem/wwwroot
RUN dotnet build "LibraryManagementSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LibraryManagementSystem.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src/LibraryManagementSystem/Data ./Data
ENTRYPOINT ["dotnet", "LibraryManagementSystem.dll"]
