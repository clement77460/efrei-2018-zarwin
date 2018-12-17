FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY CholletJaworskiZarwin/CholletJaworskiZarwin.csproj CholletJaworskiZarwin/
COPY Zarwin.Shared.Contracts/Zarwin.Shared.Contracts.csproj Zarwin.Shared.Contracts/
COPY Zarwin.Shared.Grader/Zarwin.Shared.Grader.csproj Zarwin.Shared.Grader/
COPY Zarwin.Shared.Tests/Zarwin.Shared.Tests.csproj Zarwin.Shared.Tests/
RUN dotnet restore CholletJaworskiZarwin/CholletJaworskiZarwin.csproj
COPY . .
WORKDIR /src/CholletJaworskiZarwin
RUN dotnet build CholletJaworskiZarwin.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish CholletJaworskiZarwin.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CholletJaworskiZarwin.dll"]
