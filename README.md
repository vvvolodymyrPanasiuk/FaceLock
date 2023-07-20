![Logo](https://www.nec.com/en/global/solutions/biometrics/img/face/face_header_sd.jpg)
# FaceLock Web API
FaceLock Web API - це веб-сервіс, який надає можливість аутентифікації користувачів за допомогою розпізнавання обличчя. 
Цей веб-сервіс побудований на платформі .NET Core та використовує базу даних Microsoft SQL Server для зберігання даних про користувачів та їх токени.

## Вимоги
- [Docker](https://www.docker.com/products/docker-desktop/)

## Встановлення та запуск

#### Для встановлення та запуску веб-сервісу необхідно виконати наступні кроки:

1. Завантажити образ контейнера з Docker Hub, використовуючи наступну команду:
```bash
docker pull volodymyrpanasiukkk/facelockwebapi:1.0.0
```
2. Створити мережу Docker, яка буде використовуватися для з'єднання контейнерів. Використовуючи команду:
```bash
docker network create facelock_network
```
3. Запустити контейнер бази даних, використовуючи наступну команду:
```bash
docker run -d --name facelock_db --network facelock_network -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=P@ssw0rd" -e "MSSQL_PID=Express" -p 1433:1433 mcr.microsoft.com/mssql/server:2019-latest
```
4. Запустити контейнер веб-додатку, використовуючи наступну команду:
```bash
docker run -d --name facelock_webapi --network facelock_network -p 8080:80 -e "ASPNETCORE_ENVIRONMENT=Development" -e "DatabaseServer=facelock_db" -e "DatabasePort=1433" -e "DatabaseUser=SA" -e "DatabasePassword=P@ssw0rd" -e "DatabaseName=FaceLockDb" volodymyrpanasiukkk/facelockwebapi:1.0.0
```
5. Відкрийте веб-браузер і перейдіть до https://localhost:8080/swagger/index.html, щоб переглянути документацію Swagger для API.

#### АБО

1. Склонуйте цей репозиторій на свій комп'ютер за допомогою команди:
```bash
git clone https://github.com/vvvolodymyrPanasiuk/FaceLock.git
```
2. Перейдіть у папку з проектом:
```bash
cd FaceLock
```
3. Запустіть веб-сервіс та базу даних у Docker-контейнерах за допомогою команди:
```bash
docker-compose up -d
```
4. Відкрийте веб-браузер і перейдіть до https://localhost:8080/swagger/index.html, щоб переглянути документацію Swagger для API.


## Badges

[![](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://opensource.org/licenses/)
[![](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)]()
[![](https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white)]()
[![](https://img.shields.io/badge/Docker-2CA5E0?style=for-the-badge&logo=docker&logoColor=white)](http://www.gnu.org/licenses/agpl-3.0)
[![](https://img.shields.io/badge/JWT-000000?style=for-the-badge&logo=JSON%20web%20tokens&logoColor=white)]()
[![](https://img.shields.io/badge/Postman-FF6C37?style=for-the-badge&logo=Postman&logoColor=white)]()
[![](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=Swagger&logoColor=white)]()
[![](https://img.shields.io/badge/GitHub-100000?style=for-the-badge&logo=github&logoColor=white)]()



