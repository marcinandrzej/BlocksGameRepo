using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void action(GameObject button);
public delegate void menuAction();

public class GuiManagerScript
{
    public GameObject CreatePanel(GameObject parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector3 localScale,
        Vector3 localPosition, Vector2 sizeDelta, Vector2 anchoredPosition, Sprite image, Color32 color)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent.transform);
        panel.AddComponent<RectTransform>();
        panel.AddComponent<Image>();

        panel.GetComponent<Image>().sprite = image;
        panel.GetComponent<Image>().type = Image.Type.Sliced;
        panel.GetComponent<Image>().color = color;

        panel.GetComponent<RectTransform>().anchorMin = anchorMin;
        panel.GetComponent<RectTransform>().anchorMax = anchorMax;
        panel.GetComponent<RectTransform>().localScale = localScale;
        panel.GetComponent<RectTransform>().localPosition = localPosition;
        panel.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        panel.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        return panel;
    }

    public GameObject CreateButton(GameObject parent, string name, Vector2 anchorMin, Vector2 anchorMax,
       Vector3 localScale, Vector3 localPosition, Vector2 sizeDelta, Vector2 anchoredPosition, Sprite image, Color32 color)
    {
        GameObject button = new GameObject(name);
        button.transform.SetParent(parent.transform);
        button.AddComponent<RectTransform>();
        button.AddComponent<Image>();
        button.AddComponent<Button>();

        button.GetComponent<Image>().sprite = image;
        button.GetComponent<Image>().type = Image.Type.Sliced;
        button.GetComponent<Image>().color = color;

        button.GetComponent<RectTransform>().anchorMin = anchorMin;
        button.GetComponent<RectTransform>().anchorMax = anchorMax;
        button.GetComponent<RectTransform>().localScale = localScale;
        button.GetComponent<RectTransform>().localPosition = localPosition;
        button.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        button.GetComponent<RectTransform>().sizeDelta = sizeDelta;

        return button;
    }

    public GameObject CreateText(GameObject parent, string name, Vector2 anchorMin, Vector2 anchorMax,
       Vector3 localScale, Vector3 localPosition, Vector2 sizeDelta, Vector2 anchoredPosition, string text, Color32 color)
    {
        GameObject textBlock = new GameObject(name);
        textBlock.transform.SetParent(parent.transform);
        textBlock.AddComponent<RectTransform>();
        textBlock.AddComponent<Text>();

        textBlock.GetComponent<Text>().resizeTextForBestFit = true;
        textBlock.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        textBlock.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        textBlock.GetComponent<Text>().fontStyle = FontStyle.Bold;
        textBlock.GetComponent<Text>().color = color;
        textBlock.GetComponent<Text>().text = text;

        textBlock.GetComponent<RectTransform>().anchorMin = anchorMin;
        textBlock.GetComponent<RectTransform>().anchorMax = anchorMax;
        textBlock.GetComponent<RectTransform>().localScale = localScale;
        textBlock.GetComponent<RectTransform>().localPosition = localPosition;
        textBlock.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        textBlock.GetComponent<RectTransform>().sizeDelta = sizeDelta;

        return textBlock;
    }

    public GameObject[,] FillWithButtons(GameObject panel, int buttonCount, int rowsCount, Sprite s, Color32[] colors)
    {
        GameObject[,] buttons = new GameObject[buttonCount, rowsCount];
        float buttonW = Mathf.Abs(panel.GetComponent<RectTransform>().sizeDelta.x) / (float)buttonCount;
        float offsetx = buttonW / 2.0f;
        float buttonH = Mathf.Abs(panel.GetComponent<RectTransform>().sizeDelta.y) / (float)rowsCount;
        float offsety = buttonH / 2.0f;
        for (int j = 0; j < rowsCount; j++)
        {
            for (int i = 0; i < buttonCount; i++)
            {
                int colIndx = Random.Range(0, colors.Length);
                GameObject but = CreateButton(panel, ("Button" + (j * rowsCount + i).ToString()),
                    new Vector2(0, 1), new Vector2(0, 1),
                   new Vector3(1, 1, 1), new Vector3(0, 0, 0), new Vector2(buttonW, buttonH),
                   new Vector3((offsetx + i * buttonW), (-offsety - j * buttonH), 0), s,
                   colors[colIndx]);

                but.AddComponent<ButtonScript>();
                but.GetComponent<ButtonScript>().SetIndexes(i, j);
                but.GetComponent<ButtonScript>().ColorIndex = colIndx;
                buttons[i, j] = but;
            }
        }
        return buttons;
    }

    public void SetAction(GameObject[,] buttons, action act)
    {
        foreach (GameObject but in buttons)
        {
            but.GetComponent<Button>().onClick.AddListener(delegate { act.Invoke(but); });
        }
    }

    public void SetMenuAction(GameObject button, menuAction act)
    {
        button.GetComponent<Button>().onClick.AddListener(delegate { act.Invoke(); });
    }
}
