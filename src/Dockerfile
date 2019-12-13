FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY FingerWhoIs.Lib/*.csproj ./FingerWhoIs.Lib/
COPY FingerWhoIs.App/*.csproj ./FingerWhoIs.App/
WORKDIR /app/FingerWhoIs.Lib
RUN dotnet restore

# copy and publish app and libraries
WORKDIR /app/
COPY FingerWhoIs.Lib/. ./FingerWhoIs.Lib/
COPY FingerWhoIs.App/. ./FingerWhoIs.App/
WORKDIR /app/FingerWhoIs.App
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/core/runtime:3.0 AS runtime
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
     && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=build /app/FingerWhoIs.App/out ./
ENTRYPOINT ["dotnet", "FingerWhoIs.App.dll"]
