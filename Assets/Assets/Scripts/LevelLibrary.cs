using System.Collections.Generic;
using UnityEngine;

public static class LevelLibrary
{
    public static List<LevelData> CreateLevels()
    {
        return new List<LevelData>
        {
            // Level 1
            Level(new List<DotPair>
            {
                Pair(0, 0,4, 1,0, Color.red),
                Pair(1, 1,1, 2,4, Color.green),
                Pair(2, 2,3, 2,0, Color.blue),
                Pair(3, 3,1, 4,4, Color.yellow),
                Pair(4, 4,3, 3,0, Orange())
            }),

            // Level 2
            Level(new List<DotPair>
            {
                Pair(0, 0,4, 4,1, Color.yellow),
                Pair(1, 0,1, 4,0, Color.blue),
                Pair(2, 2,2, 1,1, Color.green),
                Pair(3, 2,1, 0,0, Color.red)
            }),

            // Level 3
            Level(new List<DotPair>
            {
                Pair(0, 1,4, 0,1, Color.yellow),
                Pair(1, 2,4, 0,0, Color.blue),
                Pair(2, 3,4, 3,0, Color.green),
                Pair(3, 3,3, 2,2, Color.red),
                Pair(4, 3,1, 2,0, Orange())
            }),

            // Level 4
            Level(new List<DotPair>
            {
                Pair(0, 0,3, 3,4, Color.red),
                Pair(1, 0,0, 4,4, Color.green),
                Pair(2, 2,0, 2,2, Color.yellow),
                Pair(3, 1,0, 3,1, Color.blue)
            }),

            // Level 5
            Level(new List<DotPair>
            {
                Pair(0, 3,4, 2,0, Color.red),
                Pair(1, 4,4, 3,3, Color.green),
                Pair(2, 1,3, 4,0, Color.yellow),
                Pair(3, 2,3, 4,1, Color.blue)
            })
        };
    }

    private static LevelData Level(List<DotPair> pairs)
    {
        return new LevelData
        {
            Width = 5,
            Height = 5,
            Pairs = pairs
        };
    }

    private static DotPair Pair(int id, int sx, int sy, int ex, int ey, Color color)
    {
        return new DotPair
        {
            ColorId = id,
            Start = new Vector2Int(sx, sy),
            End = new Vector2Int(ex, ey),
            Color = color
        };
    }

    private static Color Orange()
    {
        return new Color(1f, 0.5f, 0f);
    }
}