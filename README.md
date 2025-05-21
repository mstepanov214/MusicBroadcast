## MusicBroadcast

Goal: endless music broadcast without pre-saved audio playlist.

### Running inside a Docker container

```bash
docker build MusicBroadcast -t music-broadcast
```

```bash
docker run -d --name music-broadcast \
-v /path/to/config.yaml:/app/config.yaml \
-v /path/to/bg.jpg:/app/bg.jpg \
-v /path/to/cookies.txt:/app/cookies.txt \
music-broadcast
```

### Mounted files description

| File          | Description                                                 |
| --------------| ------------------------------------------------------------|
| `config.yaml` | Main configuration file for the MusicBroadcast application. Copy and modify the [sample template](https://github.com/mstepanov214/MusicBroadcast/blob/main/MusicBroadcast/config.sample.yaml) to suit your needs. |
| `bg.jpg`      | Background image used for the broadcast's visual interface. |
| `cookies.txt` | YouTube cookies file used for authentication (to bypass _"Sign in to confirm you’re not a bot"_ error). <br> See  [How do I pass cookies to yt-dlp?](https://github.com/yt-dlp/yt-dlp/wiki/FAQ#how-do-i-pass-cookies-to-yt-dlp) and [Exporting YouTube cookies](https://github.com/yt-dlp/yt-dlp/wiki/Extractors#exporting-youtube-cookies). |
