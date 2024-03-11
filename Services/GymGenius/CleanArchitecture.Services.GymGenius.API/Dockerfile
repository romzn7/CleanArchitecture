#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/GymGenius/CleanArchitecture.Services.GymGenius.API/CleanArchitecture.Services.GymGenius.API.csproj", "Services/GymGenius/CleanArchitecture.Services.GymGenius.API/"]
RUN dotnet restore "Services/GymGenius/CleanArchitecture.Services.GymGenius.API/CleanArchitecture.Services.GymGenius.API.csproj"
COPY . .
WORKDIR "/src/Services/GymGenius/CleanArchitecture.Services.GymGenius.API"
RUN dotnet build "CleanArchitecture.Services.GymGenius.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CleanArchitecture.Services.GymGenius.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanArchitecture.Services.GymGenius.API.dll"]