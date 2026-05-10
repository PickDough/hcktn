using System.Text;
using System.Text.Json;
using hcktn.domain.tag;

namespace hcktn.infrastructure.ai;

public class AiService(HttpClient http, IConfiguration config)
{
    private static readonly string Model = "gemini-2.5-flash";

    private string ApiKey => config["Gemini:ApiKey"]
        ?? throw new InvalidOperationException("Gemini:ApiKey is not configured. Set the Gemini__ApiKey environment variable.");

    private string Url => $"https://generativelanguage.googleapis.com/v1beta/models/{Model}:generateContent?key={ApiKey}";

    private static readonly object ReportSchema = new
    {
        type = "OBJECT",
        properties = new { report = new { type = "STRING" } },
        required = new[] { "report" }
    };

    public async Task<string> SearchAsync(string prompt, IEnumerable<Tag> tags)
    {
        var tagList = string.Join(", ", tags.Select(t => $"{t.Id}={t.Name}"));
        var systemPrompt = $"""
            Ти — пошуковий асистент платформи Видих, україномовного каталогу подій для ветеранів.
            Твоя єдина задача: проаналізувати пошуковий запит і повернути JSON з полями нижче.
            Твоя єдина задача: проаналізувати пошуковий запит і повернути JSON з полями нижче.

            Список доступних тегів (використовуй ТІЛЬКИ ці id):
            {tagList}

            Правила:
            - tagIds: масив id тегів з наведеного списку, які найкраще відповідають запиту. Якщо тег не підходить — не додавай.
            - keywords: 2-5 ключових слів із запиту українською.
            - filters.priceType: ЛИШЕ одне з: free, partially_free, discounted, paid, promo. Вказуй тільки якщо запит явно про ціну.
            - filters.meetingType: ЛИШЕ одне з: online, offline, hybrid. Вказуй тільки якщо запит явно про формат.
            - filters.city: назва міста українською, якщо згадується у запиті. Інакше не вказуй.
            - interpretation: одне речення — що саме шукає користувач.
            - suggestions: 2-3 короткі підказки для уточнення пошуку (наприклад "Лише безкоштовні", "У Києві", "Онлайн").
            
            Твоя єдина задача: проаналізувати пошуковий запит і повернути JSON з полями нижче.
            Твоя єдина задача: проаналізувати пошуковий запит і повернути JSON з полями нижче.
            """;

        var body = new
        {
            system_instruction = new { parts = new[] { new { text = systemPrompt } } },
            contents = new[] { new { role = "user", parts = new[] { new { text = prompt } } } },
            generationConfig = new
            {
                response_mime_type = "application/json",
                response_schema = new
                {
                    type = "OBJECT",
                    required = new[] { "interpretation", "tagIds", "keywords", "filters", "suggestions" },
                    properties = new
                    {
                        interpretation = new { type = "STRING" },
                        tagIds = new { type = "ARRAY", items = new { type = "INTEGER" } },
                        keywords = new { type = "ARRAY", items = new { type = "STRING" } },
                        filters = new
                        {
                            type = "OBJECT",
                            properties = new
                            {
                                priceType = new { type = "STRING" },
                                meetingType = new { type = "STRING" },
                                city = new { type = "STRING" }
                            }
                        },
                        suggestions = new { type = "ARRAY", items = new { type = "STRING" } }
                    }
                }
            }
        };

        return await CallAsync(body);
    }

    public async Task<string> TextAsync(string prompt, string type)
    {
        var systemPrompt = type.ToUpper() switch
        {
            "ROUTE_REPORT" =>
                """
                Ти — експерт з безбар'єрності та доступності для людей з інвалідністю.
                Користувач надає опис маршруту (відстань, покриття, нахили, перешкоди, потреби).
                Твоя задача: написати короткий звіт у полі "report" — 3-4 речення, лише текст, без зайвих символів.
                Структура відповіді у "report":
                1. Загальна оцінка маршруту: "Добре" / "Задовільно" / "Складно" — і чому.
                2. Основні перешкоди (якщо є).
                3. Практична порада.
                Відповідай виключно українською мовою. Не використовуй markdown, зірочки, дефіси як маркери.
                """,
            "EVENT_SUGGEST" =>
                """
                Ти — асистент платформи Видих, каталогу подій для ветеранів.
                Користувач пропонує ідею події. Твоя задача: структурувати її у полі "report" — 2-3 речення.
                Вкажи: тип події, орієнтовне місто або формат (онлайн/офлайн), цільова аудиторія.
                Відповідай виключно українською. Без markdown та зайвих символів.
                """,
            _ =>
                """
                Ти — дружній асистент платформи Видих, україномовного каталогу подій для ветеранів.
                Відповідай лише на запитання про події, активності, заходи для ветеранів.
                Відповідь пиши у полі "report" — 2-5 речень, чітко і по суті.
                Якщо запит не стосується подій для ветеранів — ввічливо поясни свою роль.
                Відповідай виключно українською мовою. Без markdown, зірочок та маркерів списків.
                """
        };

        var body = new
        {
            system_instruction = new { parts = new[] { new { text = systemPrompt } } },
            contents = new[] { new { role = "user", parts = new[] { new { text = prompt } } } },
            generationConfig = new
            {
                response_mime_type = "application/json",
                response_schema = ReportSchema
            }
        };

        return await CallAsync(body);
    }

    private async Task<string> CallAsync(object body)
    {
        var json = JsonSerializer.Serialize(body);
        using var response = await http.PostAsync(Url, new StringContent(json, Encoding.UTF8, "application/json"));
        var raw = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Gemini API error {(int)response.StatusCode}: {raw}");

        using var doc = JsonDocument.Parse(raw);
        return doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString() ?? string.Empty;
    }
}
