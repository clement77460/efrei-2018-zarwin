FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ../*.sln ./
COPY ApiZarwin/*.csproj ./ApiZarwin/
COPY ChollerJaworskiZarwin.test/*.csproj ./ChollerJaworskiZarwin.test/
COPY CholletJaworskiZarwin/*.csproj ./CholletJaworskiZarwin/
COPY Zarwin.Shared.Contracts/*.csproj ./Zarwin.Shared.Contracts/
COPY Zarwin.Shared.Grader/*.csproj ./Zarwin.Shared.Grader/
COPY Zarwin.Shared.Tests/*.csproj ./Zarwin.Shared.Tests/
RUN dotnet restore

# Copy everything else and build
COPY . ./
WORKDIR /app/CholletJaworskiZarwin
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:runtime
WORKDIR /app
COPY --from=build-env /app/CholletJaworskiZarwin/out .
ENTRYPOINT ["dotnet", "CholletJaworskiZarwin.dll"]
