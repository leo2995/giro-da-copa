using GiroDaCopa.Application.Features.Admin.Dtos;
using GiroDaCopa.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GiroDaCopa.Application.Features.Admin.Queries.GetUsers;

public sealed class GetUsersQueryHandler
    : IRequestHandler<GetUsersQuery, List<UserSummaryDto>>
{
    private readonly GiroDaCopaDbContext _context;

    public GetUsersQueryHandler(GiroDaCopaDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserSummaryDto>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var usersQuery = _context.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = request.Search.Trim().ToLower();
            usersQuery = usersQuery.Where(u => u.Username.ToLower().Contains(term));
        }

        var users = await usersQuery
            .OrderBy(u => u.Username)
            .Select(u => new
            {
                u.Id,
                u.Username,
                u.Role,
                u.CreatedAt,
                HasPendingReset = _context.PasswordResetTokens.Any(
                    t => t.UserId == u.Id && !t.IsUsed && t.ExpiresAt > now)
            })
            .ToListAsync(cancellationToken);

        return users
            .Select(u => new UserSummaryDto(
                u.Id,
                u.Username,
                u.Role,
                u.CreatedAt,
                u.HasPendingReset))
            .ToList();
    }
}
