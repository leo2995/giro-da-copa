using GiroDaCopa.Application.Features.Admin.Dtos;
using MediatR;

namespace GiroDaCopa.Application.Features.Admin.Queries.GetUsers;

public sealed record GetUsersQuery(string? Search) : IRequest<List<UserSummaryDto>>;
