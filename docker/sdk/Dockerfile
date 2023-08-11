FROM mcr.microsoft.com/dotnet/sdk:6.0

ENV REPORTGENERATOR_VERSION 4.8.12
RUN dotnet tool install dotnet-reportgenerator-globaltool \
    --tool-path /tools \
    --version $REPORTGENERATOR_VERSION

RUN mkdir -p /code
COPY *.sh /code/
RUN chmod +x /code/*.sh
WORKDIR /code
