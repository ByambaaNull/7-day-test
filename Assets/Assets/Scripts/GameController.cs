using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private BoardView board;
    private PathVisual visual;
    private HudView hud;

    private readonly PathState pathState = new PathState();

    private List<LevelData> levels;
    private LevelData currentLevel;
    private int currentLevelIndex = 0;

    private void Awake()
    {
        board = GetOrAdd<BoardView>();
        visual = GetOrAdd<PathVisual>();
        hud = GetOrAdd<HudView>();
    }

    private void Start()
    {
        levels = LevelLibrary.CreateLevels();
        hud.Init(ClearLevel, NextLevel);
        LoadLevel(0);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            HandleMouseDown();

        if (Input.GetMouseButton(0))
            HandleMouseDrag();

        if (Input.GetMouseButtonUp(0))
        {
            pathState.End();
            CheckWin();
        }
    }

    public void ClearLevel()
    {
        LoadLevel(currentLevelIndex);
    }

    public void NextLevel()
    {
        int next = currentLevelIndex + 1;

        if (next < levels.Count)
            LoadLevel(next);
    }

    private void LoadLevel(int index)
    {
        currentLevelIndex = index;
        currentLevel = levels[currentLevelIndex];

        board.Build(currentLevel);
        visual.Build(currentLevel);
        pathState.Init(currentLevel, board.DotAtCell);

        hud.SetLevel(currentLevelIndex + 1, levels.Count);
        hud.ShowNext(false);
        hud.SetStatus("");
    }

    private void HandleMouseDown()
    {
        CellData cell = board.GetCellUnderMouse();

        if (cell == null)
            return;

        if (!pathState.Begin(cell.GridPos))
            return;

        hud.ShowNext(false);
        hud.SetStatus("");
        UpdateColor(pathState.CurrentColorId);
    }

    private void HandleMouseDrag()
    {
        CellData cell = board.GetCellUnderMouse();

        if (cell == null)
            return;

        List<int> changedColors = pathState.TryMove(cell.GridPos);

        foreach (int colorId in changedColors)
            UpdateColor(colorId);

        if (changedColors.Count > 0)
            CheckWin();
    }

    private void CheckWin()
    {
        bool won = WinRules.AllConnected(currentLevel, pathState.Paths);
        bool hasNextLevel = currentLevelIndex + 1 < levels.Count;

        hud.ShowNext(won && hasNextLevel);

        if (won && !hasNextLevel)
            hud.SetStatus("Completed!");
        else if (!won)
            hud.SetStatus("");
    }

    private void UpdateColor(int colorId)
    {
        visual.UpdatePath(colorId, pathState.Paths[colorId], board);
    }

    private T GetOrAdd<T>() where T : Component
    {
        T component = GetComponent<T>();

        if (component == null)
            component = gameObject.AddComponent<T>();

        return component;
    }
}