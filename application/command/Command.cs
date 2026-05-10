namespace hcktn.application.command;

public abstract class Command<TC, TR>
{
    public abstract TR Handle(TC command);
}
