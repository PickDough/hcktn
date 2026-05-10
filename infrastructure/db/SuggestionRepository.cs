using hcktn.infrastructure.db.context;

namespace hcktn.infrastructure.db;

public class SuggestionRepository(HcktnContext context) : ISuggestionRepository
{
    public void Create(string text)
    {
        context.Suggestions.Add(new SuggestionDb { Text = text, CreatedAt = DateTime.UtcNow });
        context.SaveChanges();
    }
}
