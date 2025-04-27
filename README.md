# Shortly.NET

Минимальный URL-укоренитель на .NET 8 с поддержкой создания коротких ссылок и редиректов.

## Требования

- .NET 8 SDK ([https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download))
- Git ([https://github.com/szakharov91/Shortly.NET.git](https://github.com/szakharov91/Shortly.NET.git))

## Запуск приложения

1. Клонируйте репозиторий:
   ```bash
   git clone https://github.com/szakharov91/Shortly.NET.git
   cd Shortly.NET
   ```
2. Запустите приложение:
   ```bash
   dotnet run --configuration Release
   ```
3. По умолчанию сервис будет доступен по адресам:
   - HTTP:  `http://localhost:5000`
   - HTTPS: `https://localhost:5001`

## Описание API

### Создание короткой ссылки

- **URL:** `POST /shorten`
- **Заголовки:**
  - `Content-Type: application/json`
- **Тело запроса:**
  ```json
  { "url": "https://example.com/some/long/path" }
  ```
- **Успешный ответ:**
  - **Код:** `201 Created`
  - **Тело:**
    ```json
    {
      "id": "a1B2c3",
      "originalUrl": "https://example.com/some/long/path",
      "createdAt": "2025-04-27T12:34:56Z",
      "hits": 0
    }
    ```

### Редирект по короткой ссылке

- **URL:** `GET /{id}`
- **Поведение:**
  - Если `{id}` найден, возвращает HTTP 302 с `Location: originalUrl`.
  - Если не найден — `404 Not Found`.

## Примеры использования curl

1. **Сократить ссылку**:

   ```bash
   curl -X POST \
        -H "Content-Type: application/json" \
        -d '{"url":"https://example.com/some/long/path"}' \
        http://localhost:5000/shorten
   ```

2. **Перейти по короткой ссылке** (например, `a1B2c3`):

   ```bash
   curl -v http://localhost:5000/a1B2c3
   ```

## Сохранение данных

- Все ссылки хранятся в файле `links.json` в корне приложения.
- При перезапуске данные из этого файла загружаются автоматически.

## Лицензия

MIT © szakharov91
