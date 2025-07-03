using DG.Tweening;
using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private EffectSurfaceManager effectSurfaceManager;

    // image turn
    [SerializeField] private Texture2D textureTurnRoll;
    [SerializeField] private Texture2D textTurnFly;
    private Sprite spriteTurnRoll;
    private Sprite spriteTurnFly;
    
    private CameraMovement cameraMovement;
    private Transform hintTransform;
    private GameObject canvas;
    private GameObject player;
    private BallController playerController;
    private ObstacleManager obstacleManager;
    private LevelManager levelManager;
    private CanvasScript canvasScript;
    public bool isGameOver = false;
    public float heigtButton = 400;
    public float widthButton = 100;
    private float transButtonX = 200;
    private float transButtonY = 100;
    private float spaceButton = 20;
    private List<Button> listButton;
    public List<List<Hint>> hint;
    private List<Move> listHiddenButton;
    private Button selectButton;
    private bool hiddenSoundButtonClickFirst = true;
    public float positionEndY = -5.0f;

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
        transButtonX = canvas.GetComponent<RectTransform>().sizeDelta.x;
        transButtonY = canvas.GetComponent<RectTransform>().sizeDelta.y / 20;
        widthButton = canvas.GetComponent<RectTransform>().sizeDelta.x / 3.0f;
        heigtButton = canvas.GetComponent<RectTransform>().sizeDelta.y / 4;

        hintTransform.localScale = new Vector3(2, 2, 2);
        gameObjectHintDirect.SetActive(false);


    }

    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0; // Tắt VSync để Application.targetFrameRate hoạt động
        Application.targetFrameRate = 60; // Giới hạn FPS về mức cinematic

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
        if (player.transform.position.y < positionEndY && isGameOver == false)
        {
            Debug.Log("Game Over");
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
        playerController.checkMove = false;
        //playerController.checkBallMove = false;
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
        Debug.Log(playerController.checkMove + " " + playerController.checkBallMove);
        SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
        if (playerController.checkMove == false && playerController.checkBallMove == false)
        {
            //StartCoroutine(DelayedReset());
            Debug.Log("Reset game");
            //SceneManager.LoadScene("GamePlay");
            isGameOver = false;
            //Debug.Log(listButton.Count);
            NumberBack(listButton.Count);
            canvasScript.DisplayHint();
            effectSurfaceManager.StopEffectConveyor();
            gameObjectHintDirect.SetActive(false);
        }
    }

    public IEnumerator DelayedReset()
    {
        yield return new WaitForSeconds(1.0f);

        
    }

    public void Back()
    {
        if (isGameOver || playerController.checkMove == true)
        {
            return;
        }
        NumberBack(1);
        Debug.Log("Back");
        SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
    }

    public void NumberBack(int number)
    {
        for (int i = 0; i < number; ++i)
        {
            playerController.moveBack();
            DisplayButton();
        }
        FocusButton();
    }

    public void createButton(int[] value, int index, int length)
    {
        // Tạo Button
        GameObject buttonObject = new GameObject("MyButton");
        buttonObject.transform.SetParent(canvas.transform);

        // Thêm thành phần Button và Image
        Button button = buttonObject.AddComponent<Button>();
        Image image = buttonObject.AddComponent<Image>();
        image.color = Color.white; // Màu nền trắng cho button

        // Thêm văn bản cho Button
        //GameObject textObject = new GameObject("Text");
        //textObject.transform.SetParent(buttonObject.transform);

        //Text buttonText = textObject.AddComponent<Text>();
        //buttonText.text = value[0] == 0 ? $"Move {value[1]}, Fly {value[2]}" : $"Fly {value[2]}, Move {value[1]}";
        //buttonText.alignment = TextAnchor.UpperLeft;
        //buttonText.color = Color.black;
        //buttonText.fontSize = 35;
        //buttonText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        // roll
        GameObject objectTurnRoll = new GameObject("Text Turn Roll");
        objectTurnRoll.transform.SetParent(buttonObject.transform);

        RectTransform rectRoll = objectTurnRoll.AddComponent<RectTransform>();
        rectRoll.anchorMin = new Vector2(0, 1.0f);
        rectRoll.anchorMax = new Vector2(0, 1.0f);
        rectRoll.pivot = new Vector2(0.5f, 0.5f);
        rectRoll.sizeDelta = new Vector2(100, 100);

        Sprite spriteRoll = Sprite.Create(textureTurnRoll, new Rect(0, 0, textureTurnRoll.width, textureTurnRoll.height), new Vector2(0.5f, 0.5f));

        GameObject imageObjectRoll = new GameObject("Image Roll");
        imageObjectRoll.transform.SetParent(objectTurnRoll.transform);

        RectTransform rectRollImage = imageObjectRoll.AddComponent<RectTransform>();
        rectRollImage.anchoredPosition = new Vector2(textureTurnRoll.width / 2, -textureTurnRoll.height / 2);

        Image imageTurnRoll = imageObjectRoll.AddComponent<Image>();
        imageTurnRoll.sprite = spriteRoll;

        GameObject textObjectRoll = new GameObject("Text Roll");
        textObjectRoll.transform.SetParent(objectTurnRoll.transform);

        RectTransform rectRollText = textObjectRoll.AddComponent<RectTransform>();
        rectRollText.anchoredPosition = new Vector2(textureTurnRoll.width / 2, -textureTurnRoll.height);

        Text textRoll = textObjectRoll.AddComponent<Text>();
        textRoll.text = value[1].ToString();
        textRoll.color = Color.black;
        textRoll.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        textRoll.fontSize = 50;
        textRoll.alignment = TextAnchor.MiddleCenter;

        //TextMeshProUGUI textRoll = textObjectTurnRoll.AddComponent<TextMeshProUGUI>();
        //textRoll.text = value[1].ToString();
        //textRoll.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        //textRoll.fontSize = 30;
        //textRoll.alignment = TextAlignmentOptions.Center;
        //textRoll.color = Color.black;

        //RectTransform textRollRect = textRoll.GetComponent<RectTransform>();
        //textRollRect.anchorMin = new Vector2(0.5f, 0.5f);
        //textRollRect.anchorMax = new Vector2(0.5f, 0.5f);
        //textRollRect.pivot = new Vector2(0.5f, 0.5f);
        //textRollRect.anchoredPosition = Vector2.zero;

        // fly
        GameObject objectTurnFly = new GameObject("Text Turn Fly");
        objectTurnFly.transform.SetParent(buttonObject.transform);

        RectTransform rectFly = objectTurnFly.AddComponent<RectTransform>();
        rectFly.anchorMin = new Vector2(0, 1.0f);
        rectFly.anchorMax = new Vector2(0, 1.0f);
        rectFly.pivot = new Vector2(0.5f, 0.5f);
        rectFly.sizeDelta = new Vector2(100, 100);

        Sprite spriteFly = Sprite.Create(textTurnFly, new Rect(0, 0, textTurnFly.width, textTurnFly.height), new Vector2(0.5f, 0.5f));

        GameObject imageObjectFly = new GameObject("Image Fly");
        imageObjectFly.transform.SetParent(objectTurnFly.transform);

        RectTransform rectFlyImage = imageObjectFly.AddComponent<RectTransform>();
        rectFlyImage.localRotation = Quaternion.Euler(0, 0, -90.0f);
        rectFlyImage.anchoredPosition = new Vector2(textTurnFly.width / 2, textTurnFly.height);

        Image imageTurnFly = imageObjectFly.AddComponent<Image>();
        imageTurnFly.sprite = spriteFly;

        GameObject textObjectFly = new GameObject("Text Roll");
        textObjectFly.transform.SetParent(objectTurnFly.transform);

        RectTransform rectFlyText = textObjectFly.AddComponent<RectTransform>();
        rectFlyText.anchoredPosition = new Vector2(textureTurnRoll.width / 2 - 5, textureTurnRoll.height);

        Text textFly = textObjectFly.AddComponent<Text>();
        textFly.text = value[2].ToString();
        textFly.color = Color.black;
        textFly.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        textFly.fontSize = 50;
        textFly.alignment = TextAnchor.MiddleCenter;

        //Text textFly = textObjectTurnFly.AddComponent<Text>();
        //textFly.text = value[2].ToString();

        //GameObject textObject = new GameObject("Text");
        //textObject.transform.SetParent(buttonObject.transform);

        //if (value[0] == 0)
        //{

        //}

        //if (value[1] > 0)
        //{
        //    GameObject textObjectTurnRoll = new GameObject("Turn Roll");
        //    textObjectTurnRoll.transform.SetParent(buttonObject.transform);

        //    Sprite sprite = Sprite.Create(
        //        textureTurnRoll,
        //        new Rect(0, 0, textureTurnRoll.width, textureTurnRoll.height),
        //        new Vector2(0.5f, 0.5f)  // pivot giữa
        //    );

        //    Image imageTurnRoll = textObjectTurnRoll.AddComponent<Image>();
        //    imageTurnRoll.sprite = sprite;


        //}

        //      (0, 0): Góc dưới trái.
        //      (1, 0): Góc dưới phải.
        //      (0, 1): Góc trên trái.
        //      (1, 1): Góc trên phải.
        //      (0.5, 0.5): Chính giữa.

        // Chỉnh kích thước Button
        RectTransform btnRect = buttonObject.GetComponent<RectTransform>();
        btnRect.sizeDelta = new Vector2(widthButton, heigtButton);
        btnRect.anchorMin = new Vector2(0.5f, 0);
        btnRect.anchorMax = new Vector2(0.5f, 0);
        btnRect.pivot = new Vector2(0.5f, 0);
        btnRect.anchoredPosition = new Vector2(((float)transButtonX / (length + 4)) * ((float)index - (float)length / 2 + 0.5f), transButtonY);
        //transButtonY = transButtonY + heigtButton + spaceButton;
        
        //Debug.Log((float)length / 2);
        // Chỉnh kích thước văn bản

        //RectTransform textRect = textObject.GetComponent<RectTransform>();
        //textRect.sizeDelta = new Vector2(widthButton, heigtButton);
        //textRect.anchoredPosition = Vector2.zero;

        Outline outLine = buttonObject.AddComponent<Outline>();
        outLine.effectColor = Color.white;
        outLine.effectDistance = new Vector2(2, 2);

        // Thêm sự kiện khi nhấn Button
        button.onClick.AddListener(() => {
            //Debug.Log(value[0]);
            //Debug.Log(value[1]);
            //Debug.Log(value[2]);
            playerController.setNumber(value[0], value[1], value[2]);
            selectButton = button;
            SelectButton(button);
            if (hiddenSoundButtonClickFirst == false)
            {
                //SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
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
            float duration = 0.15f;
            if (btn == button)
            {
                btn.GetComponent<Image>().color = Color.grey;
                //btn.transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.white;
                btn.transform.DOMoveY(transButtonY + 50, duration)
                    .SetEase(Ease.Linear);
            } else
            {
                btn.GetComponent<Image>().color = Color.white;
                //btn.transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.black;
                btn.transform.DOMoveY(transButtonY, duration)
                    .SetEase(Ease.Linear);
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
        //Debug.Log("Focus button");
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
        //gameObjectHintDirect.SetActive(true);

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
                    //HintToDirect(hint[i][0].direct);
                    effectSurfaceManager.PlayEffectConveyor(playerController.getIndex(), hint[i][0].direct);
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
                        //HintToDirect(hint[i][j + 1].direct);
                        effectSurfaceManager.PlayEffectConveyor(playerController.getIndex(), hint[i][j + 1].direct);
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
        //gameObjectHintDirect.SetActive(false);
        effectSurfaceManager.StopEffectConveyor();
    }
}