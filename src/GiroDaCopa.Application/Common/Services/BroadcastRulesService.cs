using GiroDaCopa.Application.Common.Dtos;

namespace GiroDaCopa.Application.Common.Services;

public static class BroadcastRulesService
{
    private static readonly BroadcastChannelDto CazeTv = new(
        "caze_tv",
        "CazéTV",
        "streaming",
        "#FF6F00",
        "https://www.youtube.com/@CazeTV");

    private static readonly BroadcastChannelDto GrupoGlobo = new(
        "grupo_globo",
        "Grupo Globo",
        "free",
        "#0054F0",
        "https://globoplay.globo.com");

    private static readonly BroadcastChannelDto Sbt = new(
        "sbt",
        "SBT",
        "free",
        "#5B21B6",
        "https://www.sbt.com.br");

    private static readonly BroadcastChannelDto NSports = new(
        "nsports",
        "N Sports",
        "streaming",
        "#10B981",
        "https://www.nsports.com.br");

    public static IReadOnlyList<BroadcastChannelDto> ApplyRules(
        MatchDto match)
    {
        var isBrazilMatch =
            match.TeamA.Id == "BRA" || match.TeamB.Id == "BRA";

        var targets = new List<BroadcastChannelDto> { CazeTv };

        if (isBrazilMatch)
        {
            targets.Add(GrupoGlobo);
            targets.Add(Sbt);
            targets.Add(NSports);
        }

        var current = match.Broadcasts.ToList();

        if (isBrazilMatch)
        {
            current = current
                .Where(c =>
                    c.Name != "TV Globo" &&
                    c.Id != "grupo_globo" &&
                    c.Id != "sbt" &&
                    c.Id != "nsports" &&
                    c.Id != "caze_tv")
                .ToList();
        }
        else
        {
            current = current
                .Where(c => c.Id != "caze_tv")
                .ToList();
        }

        foreach (var target in targets)
        {
            if (current.Any(c =>
                    c.Id == target.Id ||
                    string.Equals(
                        c.Name,
                        target.Name,
                        StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            current.Add(target);
        }

        return current;
    }
}
