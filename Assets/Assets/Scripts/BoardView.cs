using System.Collections.Generic;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    public Dictionary<Vector2Int, int> DotAtCell { get; private set; }
        = new Dictionary<Vector2Int, int>();

    private readonly List<GameObject> spawned = new List<GameObject>();
    private Sprite squareSprite;
    private Sprite circleSprite;
    private float cellSize = 1f;

    private void Awake()
    {
        squareSprite = RuntimeSprites.Square(32);
        circleSprite = RuntimeSprites.Circle(64);
    }

    public void Build(LevelData level)
    {
        Clear();
        DotAtCell.Clear();

        for (int x = 0; x < level.Width; x++)
        {
            for (int y = 0; y < level.Height; y++)
                SpawnCell(new Vector2Int(x, y));
        }

        foreach (DotPair pair in level.Pairs)
        {
            SpawnDot(pair.Start, pair.Color, pair.ColorId);
            SpawnDot(pair.End, pair.Color, pair.ColorId);
        }

        CenterCamera(level);
    }

    private void SpawnCell(Vector2Int gridPos)
    {
        GameObject cell = new GameObject("Cell " + gridPos.x + "," + gridPos.y);
        cell.transform.position = GridToWorld(gridPos);
        cell.transform.localScale = Vector3.one * 0.9f;

        SpriteRenderer sr = cell.AddComponent<SpriteRenderer>();
        sr.sprite = squareSprite;
        sr.color = new Color(0.8f, 0.8f, 0.8f);
        sr.sortingOrder = 0;

        cell.AddComponent<BoxCollider2D>();

        CellData data = cell.AddComponent<CellData>();
        data.GridPos = gridPos;

        spawned.Add(cell);
    }

    private void SpawnDot(Vector2Int gridPos, Color color, int colorId)
    {
        GameObject dot = new GameObject("Dot " + gridPos.x + "," + gridPos.y);
        dot.transform.position = GridToWorld(gridPos);
        dot.transform.localScale = Vector3.one * 0.55f;

        SpriteRenderer sr = dot.AddComponent<SpriteRenderer>();
        sr.sprite = circleSprite;
        sr.color = color;
        sr.sortingOrder = 5;

        DotAtCell[gridPos] = colorId;
        spawned.Add(dot);
    }

    public CellData GetCellUnderMouse()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouse2D = new Vector2(mouseWorld.x, mouseWorld.y);

        RaycastHit2D hit = Physics2D.Raycast(mouse2D, Vector2.zero);
        return hit.collider == null ? null : hit.collider.GetComponent<CellData>();
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * cellSize, gridPos.y * cellSize, 0);
    }

    private void CenterCamera(LevelData level)
    {
        Camera.main.orthographic = true;
        Camera.main.transform.position = new Vector3((level.Width - 1) / 2f, (level.Height - 1) / 2f, -10);
        Camera.main.orthographicSize = level.Height / 2f + 1.2f;
        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.backgroundColor = new Color(0.15f, 0.15f, 0.15f);
    }

    private void Clear()
    {
        foreach (GameObject obj in spawned)
            Destroy(obj);

        spawned.Clear();
    }
}