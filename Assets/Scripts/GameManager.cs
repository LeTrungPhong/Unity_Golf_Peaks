using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stateGame;
    private GameObject canvas;
    private GameObject player;
    private BallController playerController;
    private ObstacleManager obstacleManager;
    private LevelManager levelManager;
    public bool isGameOver = false;
    private float heigtButton = 50;
    private float widthButton = 200;
    private float transButtonX = 120;
    private float transButtonY = 50;
    private float spaceButton = 10;
    //private List<Button> listButton;
    private Button selectButton;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").gameObject;
        canvas = GameObject.Find("Canvas").gameObject;
        obstacleManager = GameObject.Find("ObstacleManager").gameObject.GetComponent<ObstacleManager>();
        playerController = player.GetComponent<BallController>();
        levelManager = GameObject.Find("LevelManager").gameObject.GetComponent<LevelManager>();
        //listButton = new List<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (obstacleManager.spawnObstacles == null)
        {
            GameNotData();
        }
        FocusButton();
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
        PlayerPrefs.SetInt(levelManager.listPathLevelData[levelManager.levelSelected], 1);
        StartCoroutine(DelayToNextMap());
    }

    public void NextGame()
    {
        levelManager.levelSelected = levelManager.levelSelected >= levelManager.listPathLevelData.Count - 1 ? levelManager.levelSelected : levelManager.levelSelected + 1;
        SceneManager.LoadScene("GamePlay");
    }

    IEnumerator DelayToNextMap()
    {
        yield return new WaitForSeconds(2f);
        NextGame();
    }

    public void GameNotData()
    {
        stateGame.color = Color.cyan;
        stateGame.text = "Not data";
        stateGame.gameObject.SetActive(true);
    }

    public void BackToSceneLevel()
    {
        Debug.Log("Back to scene Level");
        SceneManager.LoadScene("Level");
    }

    public void Reset()
    {
        Debug.Log("Reset game");
        SceneManager.LoadScene("GamePlay");
    }

    public void createButton(int[] index)
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
        btnRect.anchorMin = new Vector2(1, 0);
        btnRect.anchorMax = new Vector2(1, 0);
        btnRect.anchoredPosition = new Vector2(- transButtonX, transButtonY);
        transButtonY = transButtonY + heigtButton + spaceButton;

        // Chỉnh kích thước văn bản
        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(widthButton, heigtButton);
        textRect.anchoredPosition = Vector2.zero;

        Outline outLine = buttonObject.AddComponent<Outline>();
        outLine.effectColor = Color.white;
        outLine.effectDistance = new Vector2(2, 2);

        // Thêm sự kiện khi nhấn Button
        button.onClick.AddListener(() => {
            playerController.setNumber(index[0], index[1], index[2]);
            selectButton = button;
            SelectButton(button);
        });

        //listButton.Add(button);
    }

    public void SelectButton(Button button)
    {
        Button[] allButtons = FindObjectsOfType<Button>().Where(btn => btn.gameObject.name == "MyButton").ToArray();

        foreach(Button btn in allButtons)
        {
            if (btn == button)
            {
                btn.GetComponent<Image>().color = Color.grey;
                btn.transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.white;
            } else
            {
                btn.GetComponent<Image>().color = Color.white;
                btn.transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.black;
            }
        }
    }

    public void FocusButton()
    {
        StartCoroutine(FocusAfterDelay());
    }

    private IEnumerator FocusAfterDelay()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("Focus button");
        Button[] allButtons = FindObjectsOfType<Button>().Where(btn => btn.gameObject.name == "MyButton" && btn.gameObject != null).ToArray();

        if (allButtons.Length != 0)
        {
            foreach (Button btn in allButtons)
            {
                btn.onClick.Invoke();
                break;
            }
        }
    }

    public void DestroyButton()
    {
        if (selectButton != null)
        {
            Destroy(selectButton.gameObject);
            selectButton = null;
        }
    }
}