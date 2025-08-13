using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRhSystem.Contracts.Common;

public record PagedRequest(int Page = 1, int PageSize = 20, string? Search = null);

public record PagedResponse<T>(IReadOnlyList<T> Items, int Page, int PageSize, int TotalCount);