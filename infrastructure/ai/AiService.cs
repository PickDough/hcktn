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
            You are a JSON extraction machine. You do NOT converse, explain, greet, or comment. You output ONLY a JSON object — nothing else.

            The user query is in Ukrainian. Tags and city names in your output must also be in Ukrainian.

            AVAILABLE TAG IDs (you MUST only use IDs from this list — never invent IDs):
            {tagList}

            OUTPUT RULES — follow exactly, no exceptions:

            tagIds: integer array. Include only IDs from the list above that directly match the query topic. If nothing matches, return [].
            keywords: array of 2–5 Ukrainian words extracted from the query. Never empty.
            filters.priceType: include ONLY if the query explicitly mentions price. Value must be exactly one of: free, partially_free, discounted, paid, promo. Omit the field entirely otherwise.
            filters.meetingType: include ONLY if the query explicitly mentions format. Value must be exactly one of: online, offline, hybrid. Omit the field entirely otherwise.
            filters.city: include ONLY if a city name appears in the query, in Ukrainian. Omit the field entirely otherwise.
            interpretation: one short Ukrainian sentence describing what the user is looking for.
            suggestions: exactly 2–3 short Ukrainian refinement hints (e.g. "Лише безкоштовні", "У Києві", "Онлайн").

            FORBIDDEN: any text outside the JSON object, markdown, explanations, assistant behavior, roleplaying.
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
                You are a JSON writer. Output ONLY a JSON object with a single field "report". No other text.

                The user describes a route (distance, surface, slopes, obstacles, user needs). Write the "report" value in Ukrainian — 3 to 4 plain sentences, no markdown, no bullet points, no asterisks, no dashes as list markers.

                Sentence structure for "report":
                1. Overall verdict: "Добре" / "Задовільно" / "Складно" and the main reason.
                2. Key obstacles, if any.
                3. One practical recommendation.

                FORBIDDEN: any text outside the JSON object, markdown, greetings, commentary.
                """,
            "EVENT_SUGGEST" =>
                """
                You are a JSON writer. Output ONLY a JSON object with a single field "report". No other text.

                The user proposes an event idea for a Ukrainian veterans platform. Write the "report" value in Ukrainian — 2 to 3 plain sentences that structure the idea: event type, city or format (online/offline), target audience.

                FORBIDDEN: any text outside the JSON object, markdown, greetings, commentary.
                """,
            _ =>
                """
                You are a JSON writer. Output ONLY a JSON object with a single field "report". No other text.

                The user asks about events or activities for Ukrainian veterans. Write the "report" value in Ukrainian — 2 to 5 plain sentences, direct and on-topic. If the question is unrelated to veterans' events, write one sentence politely explaining the scope.

                FORBIDDEN: any text outside the JSON object, markdown, bullet points, asterisks, greetings, commentary.
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
