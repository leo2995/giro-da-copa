using GiroDaCopa.Application.Common.Dtos;
using GiroDaCopa.Domain.Entities;

namespace GiroDaCopa.Application.Common.Mapping;

internal static class BroadcastMapper
{
    public static BroadcastChannelDto ToDto(BroadcastChannel channel) =>
        new(
            channel.Code,
            channel.Name,
            FrontendEnumMapper.ToFrontendBroadcastType(channel.Type),
            channel.LogoColor,
            channel.UrlPlaceholder);
}
