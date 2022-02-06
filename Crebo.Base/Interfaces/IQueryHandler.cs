namespace Crebo.Base.Interfaces
{
    public interface IQueryHandler<TQuery, TResult>
    {
        TResult Handle(TQuery query);
    }
}
