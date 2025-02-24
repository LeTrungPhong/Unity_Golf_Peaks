using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stateGame;
    private GameObject canvas;
    private GameObject player;
    private BallController playerController;
    public bool isGameOver = false;
    private float heigtButton = 50;
    private float widthButton = 200;
    private float transButtonX = 120;
    private float transButtonY = 50;
    private float spaceButton = 10;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").gameObject;
        canvas = GameObject.Find("Canvas").gameObject;
        playerController = player.GetComponent<BallController>();

        createButton(new int[] { 0, 4, 0 });
        createButton(new int[] { 0, 2, 0 });
        createButton(new int[] { 0, 1, 0 });
        createButton(new int[] { 1, 4, 1 });
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            checkState();
        }
    }

    void checkState()
    {
        if (player.transform.position.y < -5)
        {
            GameOver();
            isGameOver = true;
        }
    }

    public void GameOver()
    {
        stateGame.color = Color.red;
        stateGame.text = "Game Over";
        stateGame.gameObject.SetActive(true);
    }

    public void GameWin()
    {
        stateGame.color = Color.green;
        stateGame.text = "Game Win";
        stateGame.gameObject.SetActive(true);
    }

    private void createButton(int[] index)
    {
        // Tạo Button
        GameObject buttonObject = new GameObject("MyButton");
        buttonObject.transform.SetParent(canvas.transform);

        // Thêm thành phần Button và Image
        Button button = buttonObject.AddComponent<Button>();
        Image image = buttonObject.AddComponent<Image>();
        image.color = Color.white; // Màu nền trắng cho button

        // Thêm văn bản cho Button
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(buttonObject.transform);

        Text buttonText = textObject.AddComponent<Text>();
        buttonText.text = index[0] == 0 ? $"Move {index[1]}, Fly {index[2]}" : $"Fly {index[2]}, Move {index[1]}";
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.color = Color.black;
        buttonText.fontSize = 20;
        buttonText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");


//      (0, 0): Góc dưới trái.
//      (1, 0): Góc dưới phải.
//      (0, 1): Góc trên trái.
//      (1, 1): Góc trên phải.
//      (0.5, 0.5): Chính giữa.

        // Chỉnh kích thước Button
        RectTransform btnRect = buttonObject.GetComponent<RectTransform>();
        btnRect.sizeDelta = new Vector2(200, 50);
        btnRect.anchorMin = new Vector2(1, 1);
        btnRect.anchorMax = new Vector2(1, 1);
        btnRect.anchoredPosition = new Vector2(-transButtonX, -transButtonY);
        transButtonY += heigtButton + spaceButton;

        // Chỉnh kích thước văn bản
        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(widthButton, heigtButton);
        textRect.anchoredPosition = Vector2.zero;

        // Thêm sự kiện khi nhấn Button
        button.onClick.AddListener(() => {
            playerController.setNumber(index[0], index[1], index[2]);
            Destroy(buttonObject);
        });
    }
}
