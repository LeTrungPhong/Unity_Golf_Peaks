﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadUILevel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textTitle;
    [SerializeField] private GameObject canvas;

    private LevelManager levelManager;
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
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        Debug.Log("Screen: " + Screen.width + " " + Screen.height);
        widthItem = Screen.width / numberColumn;
        heightItem = Screen.height / numberRow;
        Debug.Log("Item: " + widthItem + " " + heightItem);
        setUpLevel();
        //StartCoroutine(WaitForScreenSize());
    }

    //IEnumerator WaitForScreenSize()
    //{
    //    yield return new WaitForEndOfFrame(); 
        
    //}

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
                    rectTransform.anchoredPosition = new Vector2(Screen.width / 2, -((float)1 / 2) * heightItem);
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
        string pathLevel = index > 0 ? levelManager.listPathLevelData[index - 1] : levelManager.listPathLevelData[index];
        int checkPassLevel = index == 0 ? 1 : PlayerPrefs.GetInt(pathLevel, 0);
        GameObject buttonLevel = new GameObject(nameButtonLevel);
        buttonLevel.transform.SetParent(canvas.transform);

        Button button = buttonLevel.AddComponent<Button>();
        Image image = buttonLevel.AddComponent<Image>();
        image.color = checkPassLevel == 1 ? Color.white : Color.gray;

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
        float postX = widthItem * (j + (float)1 / 2);
        float postY = -heightItem * (i + (float)1 / 2);
        if (index == 0)
        {
            Debug.Log(widthItem + " " + heightItem);
        }
        btnRect.anchoredPosition = new Vector2(postX, postY);

        RectTransform textRect = text.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(buttonWidth, buttonHeight);
        textRect.anchoredPosition = Vector2.zero;

        if (checkPassLevel == 1)
        {
            button.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
                LevelManager.Instance.SetLevel(index);
                SceneManager.LoadScene("GamePlay");
            });
        }
    }
}
