using System.Text.Json;
using hcktn.infrastructure.ai;
using hcktn.infrastructure.db;

namespace hcktn.application.command.aiQuery;

public class AiQueryHandler(AiService aiService, ITagRepository tagRepo)
{
    public async Task<object> Handle(AiQueryCommand command)
    {
        switch (command.Type.ToUpper())
        {
            case "SEARCH":
                var tags = tagRepo.List();
                var jsonStr = await aiService.SearchAsync(command.Prompt, tags);
                var result = JsonSerializer.Deserialize<JsonElement>(jsonStr);
                return new { type = command.Type, result };

            case "ROUTE_REPORT":
            case "EVENT_SUGGEST":
            case "CHAT":
                var text = await aiService.TextAsync(command.Prompt, command.Type);
                return new { type = command.Type, result = new { report = text } };

            default:
                throw new ArgumentException($"Unknown query type: {command.Type}");
        }
    }
}
