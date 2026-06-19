using System.Collections.Generic;
using UnityEngine;

public static class WinRules
{
    public static bool AllConnected(LevelData level, Dictionary<int, List<Vector2Int>> paths)
    {
        foreach (DotPair pair in level.Pairs)
        {
            if (!paths.TryGetValue(pair.ColorId, out List<Vector2Int> path))
                return false;

            if (!path.Contains(pair.Start))
                return false;

            if (!path.Contains(pair.End))
                return false;
        }

        return true;
    }
}