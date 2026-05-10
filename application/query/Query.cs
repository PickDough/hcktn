namespace hcktn.application.query;

public abstract class Query<TQ, TR>
{
    public abstract TR Handle(TQ query);
}
