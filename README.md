# ConstellationRestoreApi

ASP.NET Core Web API, определяющий отсутствующие символы в повреждённом коде созвездия.

## Эндпоинт

`POST /api/constellations/restore`

**Тело запроса**
```json
{
  "reference": "DRAGON",
  "damaged": "GOD"
}
```

**Ответ (200 OK)**
```json
{
  "missingSymbols": "ANR",
  "isComplete": false
}
```

Если все символы на месте:
```json
{
  "missingSymbols": "",
  "isComplete": true
}
```

## Примеры

| reference | damaged | missingSymbols | isComplete |
|-----------|---------|----------------|------------|
| DRAGON | GOD | ANR | false |
| VEGA | GAVE | (пусто) | true |
| LYRA | LRA | Y | false |
| CYGNUS | (пусто) | CGNSUY | false |

## Валидация (400 Bad Request)

Поля `reference` и `damaged` обязательны. `reference` должен содержать только уникальные латинские буквы от A до Z. `damaged` должен содержать только символы из `reference`.

## Запуск

```bash
dotnet run --project ConstellationRestoreApi/ConstellationRestoreApi.csproj
dotnet test ConstellationRestoreApi.slnx
```

API доступно по адресу из `launchSettings.json` (по умолчанию `http://localhost:5145`).

## Интерфейсы

 - Веб-форма для проверки кодов: `http://localhost:<port>/`
 - Swagger UI: `http://localhost:<port>/swagger/index.html`
 - Примеры пар кодов: `GET /api/constellations/examples`

## Реализация

 - `Dto/` — DTO запроса и ответа
 - `Services/ConstellationRestoreService` — алгоритм восстановления (поиск символов реализован вручную через циклы и сравнение символов; `Contains`/`IndexOf` не используются; сортировка — стандартными средствами C#)
 - `Validation/RestoreRequestValidator` — валидация входных данных
 - `Controllers/ConstellationsController` — HTTP-интерфейс (`POST /api/constellations/restore`)
 - `ConstellationRestoreApi.Tests` — 10 unit/интеграционных тестов (xUnit), все проходят

