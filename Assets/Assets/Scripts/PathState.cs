using System.Collections.Generic;
using UnityEngine;

public class PathState
{
    public Dictionary<int, List<Vector2Int>> Paths { get; private set; }
        = new Dictionary<int, List<Vector2Int>>();

    private readonly Dictionary<Vector2Int, int> cellOwner = new Dictionary<Vector2Int, int>();
    private Dictionary<Vector2Int, int> dotAtCell;
    public int CurrentColorId { get; private set; } = -1;

    public void Init(LevelData level, Dictionary<Vector2Int, int> dots)
    {
        dotAtCell = dots;
        cellOwner.Clear();
        Paths.Clear();

        foreach (DotPair pair in level.Pairs)
            Paths[pair.ColorId] = new List<Vector2Int>();

        CurrentColorId = -1;
    }

    public bool Begin(Vector2Int cell)
    {
        if (!dotAtCell.TryGetValue(cell, out int colorId))
            return false;

        Clear(colorId);
        CurrentColorId = colorId;
        AddCell(colorId, cell);
        return true;
    }

    public List<int> TryMove(Vector2Int newCell)
    {
        List<int> changed = new List<int>();

        if (CurrentColorId < 0)
            return changed;

        List<Vector2Int> path = Paths[CurrentColorId];
        Vector2Int last = path[path.Count - 1];

        if (newCell == last || !IsAdjacent(last, newCell))
            return changed;

        if (dotAtCell.TryGetValue(newCell, out int dotColor) && dotColor != CurrentColorId)
            return changed;

        if (TryBacktrack(path, newCell))
        {
            changed.Add(CurrentColorId);
            return changed;
        }

        if (path.Contains(newCell))
            return changed;

        if (cellOwner.TryGetValue(newCell, out int owner) && owner != CurrentColorId)
        {
            Clear(owner);
            changed.Add(owner);
        }

        AddCell(CurrentColorId, newCell);
        changed.Add(CurrentColorId);
        return changed;
    }

    public void End()
    {
        CurrentColorId = -1;
    }

    public void Clear(int colorId)
    {
        if (!Paths.ContainsKey(colorId))
            return;

        foreach (Vector2Int cell in Paths[colorId])
        {
            if (cellOwner.TryGetValue(cell, out int owner) && owner == colorId)
                cellOwner.Remove(cell);
        }

        Paths[colorId].Clear();
    }

    private void AddCell(int colorId, Vector2Int cell)
    {
        Paths[colorId].Add(cell);
        cellOwner[cell] = colorId;
    }

    private bool TryBacktrack(List<Vector2Int> path, Vector2Int newCell)
    {
        if (path.Count < 2)
            return false;

        if (newCell != path[path.Count - 2])
            return false;

        Vector2Int removed = path[path.Count - 1];
        path.RemoveAt(path.Count - 1);
        cellOwner.Remove(removed);
        return true;
    }

    private bool IsAdjacent(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) == 1;
    }
}