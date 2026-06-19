using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HudView : MonoBehaviour
{
    private Text levelText;
    private Text statusText;
    private Button nextButton;

    public void Init(Action onClear, Action onNext)
    {
        Canvas canvas = CreateCanvas();

        levelText = CreateText(canvas.transform, "LevelText",
            new Vector2(0, 1), new Vector2(130, -30), new Vector2(260, 50));

        statusText = CreateText(canvas.transform, "StatusText",
            new Vector2(0.5f, 1), new Vector2(0, -30), new Vector2(300, 50));

        CreateButton(canvas.transform, "ClearButton", "Clear",
            new Vector2(0, 1), new Vector2(70, -80), onClear);

        nextButton = CreateButton(canvas.transform, "NextButton", "Next",
            new Vector2(1, 1), new Vector2(-70, -80), onNext);

        SetStatus("");
        ShowNext(false);
    }

    public void SetLevel(int current, int total)
    {
        levelText.text = "Level " + current + " / " + total;
    }

    public void SetStatus(string message)
    {
        statusText.text = message;
    }

    public void ShowNext(bool show)
    {
        nextButton.gameObject.SetActive(show);
    }

    private Canvas CreateCanvas()
    {
        GameObject obj = new GameObject("HUD Canvas");

        Canvas canvas = obj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        obj.AddComponent<CanvasScaler>();
        obj.AddComponent<GraphicRaycaster>();

        if (FindAnyObjectByType<EventSystem>() == null)
        {
            GameObject eventObj = new GameObject("EventSystem");
            eventObj.AddComponent<EventSystem>();
            eventObj.AddComponent<StandaloneInputModule>();
        }

        return canvas;
    }

    private Text CreateText(Transform parent, string name, Vector2 anchor, Vector2 pos, Vector2 size)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);

        Text text = obj.AddComponent<Text>();
        text.font = GetFont();
        text.fontSize = 26;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchorMin = anchor;
        rt.anchorMax = anchor;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = pos;
        rt.sizeDelta = size;

        return text;
    }

    private Button CreateButton(Transform parent, string name, string label, Vector2 anchor, Vector2 pos, Action action)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);

        Image image = obj.AddComponent<Image>();
        image.color = new Color(0.2f, 0.2f, 0.2f);

        Button button = obj.AddComponent<Button>();
        button.onClick.AddListener(() => action());

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchorMin = anchor;
        rt.anchorMax = anchor;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = pos;
        rt.sizeDelta = new Vector2(120, 45);

        Text text = CreateText(obj.transform, "Text",
            new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(120, 45));

        text.text = label;
        text.fontSize = 22;

        return button;
    }

    private Font GetFont()
    {
        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        return font != null ? font : Resources.GetBuiltinResource<Font>("Arial.ttf");
    }
}