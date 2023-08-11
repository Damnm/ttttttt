# EPAY.ETC.Core.API

## Run locally

Yêu cầu phải có .NET 6.0 được cài đặt.

```c#
$ dotnet --version
7.0.101
```

Di chuyển vào thư mục src có chứa file *EPAY.ETC.Core.API.sln*

```c#
$ cd src
```

Build project

```c#
$ dotnet build
```

### Run the unit tests and integration tests

*Lưu ý: để chạy thành công integration test, vui lòng xem tiếp hướng dẫn bên dưới*
```c#
$ dotnet test
```

### Run the service

```c#
$ dotnet run --project EPAY.ETC.Core.API
```

Mở trình duyệt Chrome/Edge và nhập đường link `https://localhost:7007/swagger/index.html` để kiểm tra các endpoints.

## Run in a Docker container

Yêu cầu phải có Docker được cài đặt.

```c#
$ docker --version
Docker version 20.10.20, build 9fdeb9c
```

Di chuyển đến thư mục gốc nơi có chứa file `README.md`

### Build Docker image using latest tag (for local use only)

```c#
$ docker build --no-cache -t epay.etc.core.api -f Dockerfile .
```

### Build Docker image using latest tag (for local use only)

### Build Docker image using auto generated time based tag (for UAT/Production) for different targets. Run the following bash script using Git Bash console and follow instructions

### Run Docker container using latest tag locally

### Run Docker container using time based tag from Docker Hub account

## Run integration tests
