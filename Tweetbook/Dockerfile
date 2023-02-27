#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

ARG BUILDCONFIG=Banaan
ARG VERSION=1.0.0
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_USE_POLLING_FILE_WATCHER=true  
#ENV ASPNETCORE_URLS=http://+:80  
#EXPOSE 80

ENV DOTNET_URLS=http://+:5000

WORKDIR /src
COPY ["Tweetbook.csproj", "."]
RUN dotnet restore "./Tweetbook.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Tweetbook.csproj" -c $BUILDCONFIG -o /app/build

FROM build AS publish
RUN dotnet publish "Tweetbook.csproj" -c $BUILDCONFIG -o /app/publish #/p:Version=$VERSION

FROM publish AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tweetbook.dll"]