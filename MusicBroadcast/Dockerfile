FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /source

COPY *.csproj .
RUN dotnet restore --runtime linux-musl-x64

COPY . .
RUN dotnet publish -o /out \
    --no-restore \
    --runtime linux-musl-x64 \
    --self-contained true \
    /p:PublishSingleFile=true
    #/p:PublishTrimmed=true \

FROM mcr.microsoft.com/dotnet/runtime:9.0-alpine

RUN adduser --disabled-password \
    --home /app \
    --gecos '' dotnetuser && chown -R dotnetuser /app

RUN apk add --no-cache ffmpeg yt-dlp-core

USER dotnetuser
WORKDIR /app
COPY --from=build /out .

ENTRYPOINT ["./MusicBroadcast", "--hide-progress"]