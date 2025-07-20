namespace Shared.Filtering;

public static class ParserFactory
{
    public static IFilterParser<TEntity> Get<TEntity>(string mode) =>
        mode.Equals("advanced", StringComparison.OrdinalIgnoreCase)
        ? new AdvancedFilterParser<TEntity>()
        : new SimpleFilterParser<TEntity>();     // default
}
