# 🖥 SoundBathing Server

Серверная часть приложения **SoundBathingApp**, разработанная на ASP.NET Core.

## ⚙️ Технологии

- 💻 C#
- 🌐 ASP.NET Core
- 🗃 PostgreSQL (через Docker)
- 🧩 Entity Framework Core (ORM)
- ☁️ Amazon S3 — для хранения медиафайлов
- 🔐 Microsoft Azure Key Vault — для хранения секретов
- 🔐 JWT токены для аутентификации

## 🚀 Запуск (локально)

1. Клонируйте репозиторий:

   ```bash
   git clone https://github.com/SoundHealingApp/SoundBathing_Server.git
   cd SoundBathing_Server
   ```

2. Запустите базу данных PostgreSQL:

   ```bash
   docker-compose up -d
   ```

3. Перейдите в директорию `SoundHealing.Api`:

   ```bash
   cd SoundHealing.Api
   dotnet restore
   dotnet run
   ```

4. Перейдите в Swagger UI:
   [https://localhost:7035/index.html](https://localhost:7035/index.html)

## 🌐 Продакшн

API доступен по адресу:
👉 [https://sound-wellness-application.azurewebsites.net/index.html](https://sound-wellness-application.azurewebsites.net/index.html)
