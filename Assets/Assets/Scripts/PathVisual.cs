using System.Collections.Generic;
using UnityEngine;

public class PathVisual : MonoBehaviour
{
    private readonly Dictionary<int, LineRenderer> lines = new Dictionary<int, LineRenderer>();
    private readonly List<GameObject> spawned = new List<GameObject>();
    private Material lineMaterial;

    private void Awake()
    {
        Shader shader = Shader.Find("Sprites/Default");

        if (shader == null)
            shader = Shader.Find("Universal Render Pipeline/Unlit");

        lineMaterial = new Material(shader);
    }

    public void Build(LevelData level)
    {
        Clear();

        foreach (DotPair pair in level.Pairs)
        {
            GameObject obj = new GameObject("Line " + pair.ColorId);

            LineRenderer lr = obj.AddComponent<LineRenderer>();
            lr.material = new Material(lineMaterial);
            lr.startColor = pair.Color;
            lr.endColor = pair.Color;
            lr.startWidth = 0.28f;
            lr.endWidth = 0.28f;
            lr.positionCount = 0;
            lr.useWorldSpace = true;
            lr.sortingOrder = 3;
            lr.numCornerVertices = 8;
            lr.numCapVertices = 8;

            lines[pair.ColorId] = lr;
            spawned.Add(obj);
        }
    }

    public void UpdatePath(int colorId, List<Vector2Int> path, BoardView board)
    {
        if (!lines.TryGetValue(colorId, out LineRenderer lr))
            return;

        lr.positionCount = path.Count;

        for (int i = 0; i < path.Count; i++)
        {
            Vector3 pos = board.GridToWorld(path[i]);
            pos.z = -0.1f;
            lr.SetPosition(i, pos);
        }
    }

    private void Clear()
    {
        foreach (GameObject obj in spawned)
            Destroy(obj);

        spawned.Clear();
        lines.Clear();
    }
}