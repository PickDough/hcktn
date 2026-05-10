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

    public async Task<string> SearchAsync(string prompt, IEnumerable<Tag> tags)
    {
        var tagList = string.Join(", ", tags.Select(t => $"{t.Id}→{t.Name}"));
        var systemPrompt = $"""
            Ти асистент платформи Видих — каталогу подій для українських ветеранів.
            Доступні теги (id→назва): {tagList}.
            Проаналізуй запит користувача та поверни структурований результат пошуку.
            tagIds має містити лише id з наведеного списку тегів.
            filters.priceType: one of free, partially_free, discounted, paid, promo (або не вказувати).
            filters.meetingType: one of online, offline, hybrid (або не вказувати).
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
            "ROUTE_REPORT" => "Ти експерт з оцінки доступності маршрутів для людей з особливими потребами. Оціни маршрут та надай короткий звіт (3-5 речень) українською. Вкажи: загальний рейтинг (добре/задовільно/складно), основні перешкоди та поради.",
            "EVENT_SUGGEST" => "Ти асистент платформи Видих. Категоризуй пропозицію події: визнач тип події, місто, орієнтовну дату. Відповідай коротко, українською мовою.",
            _ => "Ти помічник платформи Видих — каталогу подій для українських ветеранів. Допоможи знайти або дізнатись більше про події. Відповідай українською мовою."
        };

        var body = new
        {
            system_instruction = new { parts = new[] { new { text = systemPrompt } } },
            contents = new[] { new { role = "user", parts = new[] { new { text = prompt } } } }
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
