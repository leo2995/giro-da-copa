using GiroDaCopa.Application.Common.Dtos;
using GiroDaCopa.Domain.Entities;

namespace GiroDaCopa.Application.Common.Mapping;

internal static class TeamMapper
{
    public static TeamDto ToDto(Team team) =>
        new(
            team.Code,
            team.Name,
            team.Country.FlagEmoji,
            team.PrimaryColor);
}
