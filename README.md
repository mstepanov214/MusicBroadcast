## MusicBroadcast

Goal: endless music broadcast without pre-saved audio playlist.

### Prerequisites

- ffmpeg 5.1.2+
- yt-dlp 2023.07.06+

### Known issues

Sometimes stream interrupts during track switching. Could be reproduced on all platforms except Youtube (to continue listening you need to manually reload the player page).

### Running inside a Docker container

```sh
docker build MusicBroadcast -t music-broadcast
docker run -d --name music-broadcast -v /path/to/config.yaml:/app/config.yaml -v /path/to/bg.jpg:/app/bg.jpg music-broadcast
```