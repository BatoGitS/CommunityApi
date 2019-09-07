FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["CommunityAPI/CommunityAPI.csproj", "CommunityAPI/"]
COPY ["Contracts/Contracts.csproj", "Contracts/"]
RUN dotnet restore "CommunityAPI/CommunityAPI.csproj"
COPY . .
WORKDIR "/src/CommunityAPI"
RUN dotnet build "CommunityAPI.csproj" -c Release -o /app
RUN dotnet ef migrations add Init
RUN dotnet ef database update

FROM build AS publish
RUN dotnet publish "CommunityAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY --from=build /src/CommunityAPI/2.db .
ENTRYPOINT ["dotnet", "CommunityAPI.dll"]