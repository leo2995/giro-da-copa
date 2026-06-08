namespace GiroDaCopa.Application.Common.Dtos;

public sealed record BroadcastChannelDto(
    string Id,
    string Name,
    string Type,
    string LogoColor,
    string UrlPlaceholder);
