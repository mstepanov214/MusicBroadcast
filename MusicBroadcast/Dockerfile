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
WORKDIR /app
COPY --from=build /out .
RUN apk add --no-cache ffmpeg=6.0-r15 yt-dlp-core=2023.07.06-r1
ENTRYPOINT ["./MusicBroadcast", "--hide-progress"]