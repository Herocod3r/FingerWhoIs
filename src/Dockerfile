FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
     && rm -rf /var/lib/apt/lists/*
WORKDIR /app

# copy csproj and restore as distinct layers
COPY FingerWhoIs.Lib/*.csproj ./FingerWhoIs.Lib/
COPY FingerWhoIs.Test/*.csproj ./FingerWhoIs.Test/
WORKDIR /app/FingerWhoIs.Lib
RUN dotnet restore

# copy and publish app and libraries
WORKDIR /app/
COPY FingerWhoIs.Lib/. ./FingerWhoIs.Lib/
COPY FingerWhoIs.Test/. ./FingerWhoIs.Test/
WORKDIR /app/FingerWhoIs.Test
RUN dotnet test