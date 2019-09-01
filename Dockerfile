# FROM mcr.microsoft.com/dotnet/core/sdk:2.2
 
# WORKDIR /home/app
# COPY LibraryManagementSystem/bin/Debug/netcoreapp2.2/publish .
# COPY LibraryManagementSystem/appsettings.Production.json .
# COPY LibraryManagementSystem/appsettings.json .
# COPY LibraryManagementSystem/UserSeedData.json .
# COPY LibraryManagementSystem/AssetSeedData.json .
# COPY LibraryManagementSystem/AuthorSeedData.json .
 
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 as build-image
 
WORKDIR /home/app
 
# COPY ./*.sln ./
# COPY ./*/*.csproj ./
# RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done

COPY LibraryManagementSystem/LibraryManagementSystem.csproj LibraryManagementSystem/
COPY LMSService/LMSService.csproj LMSService/
COPY LMSRepository/LMSRepository.csproj LMSRepository/
COPY LibraryManagementSystem.Tests/LibraryManagementSystem.Tests.csproj Tests/

RUN dotnet restore LibraryManagementSystem/LibraryManagementSystem.csproj
 
COPY . .
 
RUN dotnet test ./Tests/LibraryManagementSystem.Tests.csproj
 
RUN dotnet publish ./LibraryManagementSystem/LibraryManagementSystem.csproj -o /publish/
 
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
 
WORKDIR /publish
 
COPY --from=build-image /publish .

# ENV ASPNETCORE_URLS=http://*:80

# EXPOSE 5000
# EXPOSE 5000
 
ENTRYPOINT ["dotnet", "LibraryManagementSystem.dll"]