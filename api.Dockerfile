FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 59518
EXPOSE 44364

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY *.sln ./
COPY ApiZarwin/*.csproj ./ApiZarwin/
COPY ChollerJaworskiZarwin.test/*.csproj ./ChollerJaworskiZarwin.test/
COPY CholletJaworskiZarwin/*.csproj ./CholletJaworskiZarwin/
COPY Zarwin.Shared.Contracts/*.csproj ./Zarwin.Shared.Contracts/
COPY Zarwin.Shared.Grader/*.csproj ./Zarwin.Shared.Grader/
COPY Zarwin.Shared.Tests/*.csproj ./Zarwin.Shared.Tests/
RUN dotnet restore
COPY . .
WORKDIR /src/ApiZarwin
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish ApiZarwin.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ApiZarwin.dll"]