## MusicBroadcast

Goal: endless music broadcast without pre-saved audio playlist.

### Running inside a Docker container

```sh
docker build MusicBroadcast -t music-broadcast

docker run -d --name music-broadcast \
-v /path/to/config.yaml:/app/config.yaml \
-v /path/to/bg.jpg:/app/bg.jpg \
-v /path/to/cookies.txt:/app/cookies.txt \
music-broadcast
```