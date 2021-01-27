FROM mcr.microsoft.com/dotnet/sdk:5.0  AS build

WORKDIR /app
COPY *.sln .
COPY AWS.Cognito.Net/*.csproj ./AWS.Cognito.Net/
WORKDIR /app/AWS.Cognito.Net
RUN dotnet restore

COPY AWS.Cognito.Net/. /app/AWS.Cognito.Net/
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=build /app/AWS.Cognito.Net/out ./
ENTRYPOINT ["dotnet", "AWS.Cognito.Net.dll"]