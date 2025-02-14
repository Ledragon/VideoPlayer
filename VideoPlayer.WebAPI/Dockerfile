FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY bin/Release/net8.0 .
RUN apt-get clean
RUN apt-get -y update
RUN apt-get -y upgrade
RUN apt-get install -y ffmpeg
ENTRYPOINT [ "dotnet", "VideoPlayer.WebAPI.dll"]  