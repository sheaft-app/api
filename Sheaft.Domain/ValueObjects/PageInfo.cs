namespace Sheaft.Domain;

public record PageInfo
{
    private PageInfo(int? page = 1, int? take = 10)
    {
        Page = page.HasValue ? (page < 1 ? 1 : page.Value) : 1;
        Take = take.HasValue ? (take < 1 ? 1 : take.Value) : 10;
    }

    public int Page { get; }
    public int Take { get; }
    public int Skip => (Page - 1) * Take;

    public static PageInfo From(int? page, int? take)
    {
        return new PageInfo(page, take);
    }
}