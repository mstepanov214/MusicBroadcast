FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine3.18 AS build
WORKDIR /source

COPY *.csproj .
RUN dotnet restore --runtime alpine-x64

COPY . .
RUN dotnet publish -o /out \
    --no-restore \
    --runtime alpine-x64 \
    --self-contained true \
    /p:PublishSingleFile=true 
    #/p:PublishTrimmed=true \

FROM mcr.microsoft.com/dotnet/runtime:7.0-alpine3.18

RUN adduser --disabled-password \
    --home /app \
    --gecos '' dotnetuser && chown -R dotnetuser /app

RUN apk add --no-cache ffmpeg yt-dlp-core

USER dotnetuser
WORKDIR /app
COPY --from=build /out .

ENTRYPOINT ["./MusicBroadcast", "--hide-progress"]