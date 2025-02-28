using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadUILevel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textTitle;
    [SerializeField] private GameObject canvas;

    float canvasWidth = Screen.width;
    float canvasHeight = Screen.height;

    private int numberRow = 5;
    private int numberColumn = 5;
    float widthItem = 0;
    float heightItem = 0;
    private int indexLevel;
    private string nameButtonLevel = "ButtonLevel";
    private int buttonWidth = 200;
    private int buttonHeight = 50;
    // Start is called before the first frame update
    void Start()
    {
        widthItem = canvasWidth / numberColumn;
        heightItem = canvasHeight / numberRow;
        indexLevel = LevelManager.Instance.indexLevel;
        setUpLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setUpLevel()
    {
        int index = indexLevel;
        for (int i = 0; i < numberRow; ++i)
        {
            for (int j = 0; j < numberColumn; ++j)
            {
                if (i == 0)
                {
                    RectTransform rectTransform = textTitle.GetComponent<RectTransform>();
                    rectTransform.anchorMin = new Vector2(0, 1.0f);
                    rectTransform.anchorMax = new Vector2(0, 1.0f);
                    rectTransform.pivot = new Vector2(0.5f, 0.5f);
                    rectTransform.anchoredPosition = new Vector2(canvasWidth / 2, -((float)1 / 2) * heightItem);
                }
                else
                {
                    if (index >= LevelManager.Instance.listPathLevelData.Count) return;
                    createButtonLevel(i, j, index);
                    index++;
                }
            }
        }
    }

    public void createButtonLevel(int i, int j, int index)
    {
        GameObject buttonLevel = new GameObject(nameButtonLevel);
        buttonLevel.transform.SetParent(canvas.transform);

        Button button = buttonLevel.AddComponent<Button>();
        Image image = buttonLevel.AddComponent<Image>();
        image.color = Color.white;

        GameObject text = new GameObject("TextButtonLevel");
        text.transform.SetParent(buttonLevel.transform);

        Text buttonText = text.AddComponent<Text>();
        buttonText.text = $"{index + 1}";
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.color = Color.black;
        buttonText.fontSize = 20;
        buttonText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        RectTransform btnRect = buttonLevel.GetComponent<RectTransform>();
        btnRect.sizeDelta = new Vector2(buttonWidth, buttonHeight);
        btnRect.anchorMin = new Vector2(0, 1.0f);
        btnRect.anchorMax = new Vector2(0, 1.0f);
        btnRect.pivot = new Vector2(0.5f, 0.5f);
        btnRect.anchoredPosition = new Vector2(widthItem * (j + (float)1 / 2), - heightItem * (i + (float)1 / 2));

        RectTransform textRect = text.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(buttonWidth, buttonHeight);
        textRect.anchoredPosition = Vector2.zero;

        button.onClick.AddListener(() =>
        {
            LevelManager.Instance.SetLevel(index);
            SceneManager.LoadScene("GamePlay");
        });
    }
}
