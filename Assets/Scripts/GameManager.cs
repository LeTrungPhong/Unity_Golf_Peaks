using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public struct Hint
{
    public int select;
    public int direct;
}

public struct Move
{
    public int select;
    public int direct;
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stateGame;
    [SerializeField] private GameObject gameObjectHintDirect;
    [SerializeField] private GameObject gameObjectCamera;
    private CameraMovement cameraMovement;
    private Transform hintTransform;
    private GameObject canvas;
    private GameObject player;
    private BallController playerController;
    private ObstacleManager obstacleManager;
    private LevelManager levelManager;
    private CanvasScript canvasScript;
    public bool isGameOver = false;
    public float heigtButton = 50;
    public float widthButton = 200;
    private float transButtonX = 200;
    private float transButtonY = 100;
    private float spaceButton = 20;
    private List<Button> listButton;
    public List<List<Hint>> hint;
    private List<Move> listHiddenButton;
    private Button selectButton;
    private bool hiddenSoundButtonClickFirst = true;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").gameObject;
        canvas = GameObject.Find("Canvas").gameObject;
        hintTransform = gameObjectHintDirect.GetComponent<Transform>();
        obstacleManager = GameObject.FindGameObjectWithTag("ObstacleManager").gameObject.GetComponent<ObstacleManager>();
        playerController = player.GetComponent<BallController>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").gameObject.GetComponent<LevelManager>();
        canvasScript = GameObject.FindGameObjectWithTag("Canvas").gameObject.GetComponent<CanvasScript>();
        cameraMovement = gameObjectCamera.GetComponent<CameraMovement>();
        listButton = new List<Button>();
        listHiddenButton = new List<Move>();
        transButtonX = canvas.GetComponent<RectTransform>().sizeDelta.x / 2.5f / 2 + 100;
        transButtonY = canvas.GetComponent<RectTransform>().sizeDelta.y / 20;
        widthButton = canvas.GetComponent<RectTransform>().sizeDelta.x / 2.5f;
        heigtButton = canvas.GetComponent<RectTransform>().sizeDelta.y / 20;

        hintTransform.localScale = new Vector3(2, 2, 2);
        gameObjectHintDirect.SetActive(false);
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
        if (player.transform.position.y < -5 && isGameOver == false)
        {
            GameOver();
            isGameOver = true;
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        stateGame.color = Color.red;
        stateGame.text = "Game Over";
        canvasScript.HintToReset();
        //stateGame.gameObject.SetActive(true);
    }

    public void GameWin()
    {
        isGameOver = true;
        stateGame.color = Color.green;
        stateGame.text = "Game Win";
        //stateGame.gameObject.SetActive(true);
        PlayerPrefs.SetInt(levelManager.levelSelected.ToString(), 1);
        cameraMovement.CameraChangeLevel();
        StartCoroutine(DelayToNextMap());
    }

    public void NextGame()
    {
        levelManager.levelSelected = levelManager.levelSelected >= levelManager.listDataLevel.Count - 1 ? levelManager.levelSelected : levelManager.levelSelected + 1;
        SceneManager.LoadScene("GamePlay");
    }

    IEnumerator DelayToNextMap()
    {
        yield return new WaitForSeconds(1.5f);
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
        SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
    }

    public void Reset()
    {
        Debug.Log("Reset game");
        SceneManager.LoadScene("GamePlay");
        SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
    }

    public void Back()
    {
        if (isGameOver)
        {
            return;
        }

        Debug.Log("Back");
        playerController.moveBack();
        DisplayButton();
        FocusButton();
        SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
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
        buttonText.fontSize = 35;
        buttonText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

//      (0, 0): Góc dưới trái.
//      (1, 0): Góc dưới phải.
//      (0, 1): Góc trên trái.
//      (1, 1): Góc trên phải.
//      (0.5, 0.5): Chính giữa.

        // Chỉnh kích thước Button
        RectTransform btnRect = buttonObject.GetComponent<RectTransform>();
        btnRect.sizeDelta = new Vector2(widthButton, heigtButton);
        btnRect.anchorMin = new Vector2(1, 0);
        btnRect.anchorMax = new Vector2(1, 0);
        btnRect.anchoredPosition = new Vector2(- transButtonX, transButtonY);
        transButtonY = transButtonY + heigtButton + spaceButton;

        // Chỉnh kích thước văn bản
        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(widthButton * 2, heigtButton * 2);
        textRect.anchoredPosition = Vector2.zero;

        Outline outLine = buttonObject.AddComponent<Outline>();
        outLine.effectColor = Color.white;
        outLine.effectDistance = new Vector2(2, 2);

        // Thêm sự kiện khi nhấn Button
        button.onClick.AddListener(() => {
            playerController.setNumber(index[0], index[1], index[2]);
            selectButton = button;
            SelectButton(button);
            if (hiddenSoundButtonClickFirst == false)
            {
                SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
            } else
            {
                hiddenSoundButtonClickFirst = false;
            }
        });

        listButton.Add(button);
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

    public void HiddenButton(int direction)
    {
        if (selectButton != null)
        {
            for (int i = 0; i < listButton.Count; ++i)
            {
                if (listButton[i] == selectButton)
                {
                    Move move;
                    move.select = i;
                    move.direct = direction;
                    listHiddenButton.Add(move);
                    selectButton.gameObject.SetActive(false);
                    return;
                }
            }
        }
    }

    public void DisplayButton()
    {
        if (listHiddenButton.Count > 0 && listHiddenButton[0].select >= 0 && listHiddenButton[0].select < listButton.Count)
        {
            listButton[listHiddenButton[listHiddenButton.Count - 1].select].gameObject.SetActive(true);
            listHiddenButton.RemoveAt(listHiddenButton.Count - 1);
        }
    }

    public void ButtonHintOnClick()
    {
        gameObjectHintDirect.SetActive(true);
        

        if (hint == null)
        {
            return;
        }

        for (int i = 0; i < listHiddenButton.Count; ++i)
        {
            Debug.Log(listHiddenButton[i]);
        }
            
        for (int i = 0; i < hint.Count; ++i)
        {
            if (listHiddenButton.Count < hint[i].Count)
            {
                if (listHiddenButton.Count == 0 && hint[i].Count > 0)
                {
                    // hint
                    Debug.Log("Select: " + hint[i][0].select + ", direct: " + hint[i][0].direct);
                    canvasScript.HintToMove(listButton[hint[i][0].select].transform.position);
                    HintToDirect(hint[i][0].direct);
                    return;
                }
                for (int j = 0; j < listHiddenButton.Count; ++j)
                {
                    if (listHiddenButton[j].select != hint[i][j].select || listHiddenButton[j].direct != hint[i][j].direct)
                    {
                        Debug.Log(listHiddenButton[j] + " != " + hint[i][j]);
                        break;
                    }

                    if (j == listHiddenButton.Count - 1)
                    {
                        // hint
                        Debug.Log("Select: " + hint[i][j + 1].select + ", direct: " + hint[i][j + 1].direct);
                        canvasScript.HintToMove(listButton[hint[i][j + 1].select].transform.position);
                        HintToDirect(hint[i][j + 1].direct);
                        return;
                    }
                }
            }
        }

        Debug.Log("Reset");
        canvasScript.HintToReset();
        HiddenDirect();
    }

    public void HintToDirect(int direct)
    {
        Debug.Log("Hint arrow");
        Vector3 transHint = new Vector3(0, obstacleManager.obstacleSizeY * 2, 0);
        Vector3 rotateHint = new Vector3(0, 90 * (direct - 1 + 2), 0);
        hintTransform.rotation = Quaternion.Euler(rotateHint);
        hintTransform.position = player.GetComponent<Transform>().position + transHint;
    }

    public void HiddenDirect()
    {
        gameObjectHintDirect.SetActive(false);
    }
}