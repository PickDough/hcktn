using hcktn.infrastructure.db;

namespace hcktn.application.command.createSuggestion;

public class CreateSuggestionHandler(ISuggestionRepository repo) : Command<CreateSuggestionCommand, bool>
{
    public override bool Handle(CreateSuggestionCommand command)
    {
        repo.Create(command.Text);
        return true;
    }
}
