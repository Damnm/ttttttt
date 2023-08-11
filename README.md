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
$
```

Build project

```c#
$ dotnet build
$
```

### Run the unit tests and integration tests

***Lưu ý: để chạy thành công integration test, vui lòng xem tiếp hướng dẫn bên dưới*

```c#
$ dotnet test
$
```

### Run the service

```c#
$ dotnet run --project EPAY.ETC.Core.API
$
```

Mở trình duyệt Chrome/Edge và nhập đường link `https://localhost:7007/swagger/index.html` để kiểm tra các endpoints.

## Run in a Docker container

Yêu cầu phải có Docker được cài đặt.

```c#
$ docker --version
Docker version 20.10.20
```

Di chuyển đến thư mục gốc nơi có chứa file `README.md`

### Build Docker image using latest tag (for local use only)

```c#
$ docker build --no-cache -t epay.etc.core.api -f Dockerfile .
$
```

### Build Docker image using auto generated time based tag (for UAT/Production) for different targets. Run the following bash script using Git Bash console and follow instructions

```c#
$ ./build-main.sh
$
```

*Lưu ý: trong quá trình chạy file build-main.sh, Docker sẽ yêu cầu đăng nhập tài khoản Docker Hub trước khi Docker image được tạo ra và đẩy lên Docker Hub dưới tên tài khoản được chỉ định.*

### Run Docker container using latest tag locally

```c#
$ docker run -d -it -p 80:80 -p 443:443 -v /docker_named_volume:/path_to_log_folder_in_Docker_container --env ASPNETCORE_ENVIRONMENT=<Development|UAT|Production> --restart always epay.etc.core.api
$
```

*Lưu ý:*

* *thay đổi biến ASPNETCORE_ENVIRONMENT và target cho phù hợp với yêu cầu*
* *docker_named_volume: acvtolllogs_data. Không đổi tên acvtolllog_data vì đây là tên volume mà filebeat sẽ truy cập vào để lấy logs*
* *path_to_log_folder_in_Docker_container: luôn luôn là /app/logs*

*Sau khi Docker container đã được tạo và đăng ký chạy thành công, mở trình duyệt Chrome và nhập đường link `http://localhost/swagger/index.html` để kiểm tra các endpoints.*

### Run Docker container using time based tag from Docker Hub account

```c#
$ docker run -d -it -p 80:80 -p 443:443 -v /docker_named_volume:/path_to_log_folder_in_Docker_container --env ASPNETCORE_ENVIRONMENT=<UAT|Production> --restart always <DockerHubAccountId>/epay.etc.core.api:<tagId>
$
```

## Run integration tests

### 1. Run integration tests trực tiếp từ Visual Studio 2022

*Lưu ý: Sử dụng phương pháp này để debug trong quá trình phát triển.*

Thực hiện các bước như dưới đây:

1. Open command prompt terminal và chuyển đến thư mục gốc có chứa file `docker-compose.yaml`
2. Thực hiện lệnh sau: `docker-compose up --build --force-recreate acv-database`. Lệnh này sẽ thực hiện các tính năng sau:
   1. Khởi tạo và chạy 1 dockerized PostgreSQL container
   2. Set up database schema and data for ACV database. Các scripts có thế được tìm thấy trong thư mục `docker/sql`.
   *Lưu ý: Các script khi được thực thi trong PostgreSQL sẽ được sắp xếp theo thứ tự alpha-b*

3. Open file `ACV.Toll.Admin.API.sln` và chạy toàn bộ test suite
4. Để có thể đăng nhập vào dockerized PostgreSQL, ta có thể sử dụng pgAdmin4 với connection string như sau: `Server=localhost; Port=5432; User Id=postgres; Password=postgres; Database=MainDB;`. *Connection string này có thể được tìm thấy trong file `appsettings.IntegrationTests.json`*
5. Để clean up Docker containers sau khi chạy xong integration tests, thực hiện lệnh sau `docker-compose down`

### 2. Run integration tests bằng Docker containers

*Lưu ý: Sử dụng phương pháp này để tích hợp với CI/CD pipeline. Với phương pháp này thì các service containers sẽ cùng có chung Docker network nên trong SQL Server connection string sẽ không sử dụng localhost.*

Thực hiện các bước như dưới đây:

1. Open command prompt terminal và chuyển đến thư mục gốc có chứa file `docker-compose.yaml`
2. Thực hiện lệnh sau: `docker-compose up --build --force-recreate sdk && docker-compose up integration-tests`. Lệnh này sẽ thực hiện các tính năng sau:
   1. Khởi tạo và chạy 1 dockerized PostgreSQL container
   2. Set up database schema and data for ACV database. Các scripts có thế được tìm thấy trong thư mục `docker/sql`.
   *Lưu ý: Các script khi được thực thi trong PostgreSQL sẽ được sắp xếp theo thứ tự alpha-b*
3. Để clean up Docker containers sau khi chạy xong integration tests, thực hiện lệnh sau `docker-compose down`
