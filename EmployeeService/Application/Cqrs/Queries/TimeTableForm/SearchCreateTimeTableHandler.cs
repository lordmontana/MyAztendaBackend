using EmployeeService.Application.Cqrs.Queries.TimeTableForm;
using EmployeeService.Application.DTOs;
using EmployeeService.Domain.Entities.Forms;
using Shared.Cqrs.Abstractions;
using Shared.Dtos;
using Shared.Filtering;
using Shared.Repositories.Abstractions;

namespace EmployeeService.Application.Cqrs.Queries.TimeTableForm;
public sealed class SearchCreateTimeTableHandler(
    IRepository<Employee> repo)
    : IQueryHandler<SearchCreateTimeTableQuery, PagedResult<EmployeeDto>>
{
    public async Task<PagedResult<EmployeeDto>> HandleAsync(
        SearchCreateTimeTableQuery q, CancellationToken ct)
    {
        // pick parser based on API-supplied mode
       var parser = ParserFactory.Get<Employee>(q.Mode);
       var preds = parser.Parse(q.Filters);
    //
       var res = await repo.QueryAsync(
          q.Page, q.PageSize, e => e.Name, true, preds);
    //
     var list = res.Data
                     .Select(e => new EmployeeDto(e.Id,e.Name, e.Gender,e.Email))
                  .ToList();

        return new PagedResult<EmployeeDto>(list, res.Total);
    }
}
