namespace Shared.Dtos;

public sealed record PagedRequest(
    int Page = 0,
    int PageSize = 25,
    string Mode = "simple",
    List<FilterDto>? Filters = null);
