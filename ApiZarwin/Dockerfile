FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ApiZarwin/ApiZarwin.csproj ApiZarwin/
COPY CholletJaworskiZarwin/CholletJaworskiZarwin.csproj CholletJaworskiZarwin/
COPY Zarwin.Shared.Contracts/Zarwin.Shared.Contracts.csproj Zarwin.Shared.Contracts/
COPY Zarwin.Shared.Grader/Zarwin.Shared.Grader.csproj Zarwin.Shared.Grader/
COPY Zarwin.Shared.Tests/Zarwin.Shared.Tests.csproj Zarwin.Shared.Tests/
COPY ChollerJaworskiZarwin.test/CholletJaworskiZarwin.test.csproj ChollerJaworskiZarwin.test/
RUN dotnet restore ApiZarwin/ApiZarwin.csproj
COPY . .
WORKDIR /src/ApiZarwin
RUN dotnet build ApiZarwin.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ApiZarwin.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ApiZarwin.dll"]
