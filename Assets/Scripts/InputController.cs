using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public bool confirmMove = false;
    public RectTransform canvasRect;
    public Vector2 localPointButtonDown;
    public Vector2 localPointButtonHold;
    public GameObject imageSquareLine;
    public GameObject imageArrow;
    private GameManager gameManager;
    private RectTransform rectTransformImageSquareLine;
    private RectTransform rectTransformArrow;
    private BallController playerController;

    private int direction = 0;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Ball").GetComponent<BallController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        imageSquareLine.SetActive(false);
        imageArrow.SetActive(false);
        rectTransformImageSquareLine = imageSquareLine.GetComponent<RectTransform>();
        rectTransformArrow = imageArrow.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        playerConfirmMove();
    }

    void playerConfirmMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, null, out localPointButtonDown);
            rectTransformImageSquareLine.anchoredPosition = localPointButtonDown;
            imageSquareLine.SetActive(true);
            confirmMove = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            confirmMove = false;
            imageSquareLine.SetActive(false);
            imageArrow.SetActive(false);

            if (direction == 0)
            {
                return;
            }

            gameManager.HiddenButton();

            if (direction == 1)
            {
                playerController.OnButtonRClick();
            }
            else if (direction == 2)
            {
                playerController.OnButtonBClick();
            }
            else if (direction == 3)
            {
                playerController.OnButtonTClick();
            }
            else if (direction == 4)
            {
                playerController.OnButtonLClick();
            }
            direction = 0;

            gameManager.FocusButton();
        }
        if (Input.GetMouseButton(0) && confirmMove)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, null, out localPointButtonHold);

            Vector2 vectorDirection = localPointButtonHold - localPointButtonDown;

            float lenghtVector = vectorDirection.magnitude;

            if (lenghtVector > 50)
            {
                imageArrow.SetActive(true);
                const int moveArrow = 15;
                if (PlayerPrefs.GetString(PlayerPrefsName.playerPrefsControl, PlayerPrefsName.falsePrefs) == PlayerPrefsName.truePrefs)
                {
                    vectorDirection = new Vector2(vectorDirection.x * (-1), vectorDirection.y * (-1));
                }
                if (vectorDirection.x >= 0 && vectorDirection.y >= 0)
                {
                    // right
                    rectTransformArrow.anchoredPosition = localPointButtonDown + new Vector2(moveArrow, moveArrow);
                    rectTransformArrow.rotation = Quaternion.Euler(0, 0, 45);
                    direction = 1;
                } else if (vectorDirection.x >= 0 && vectorDirection.y <= 0)
                {
                    // bottom
                    rectTransformArrow.anchoredPosition = localPointButtonDown + new Vector2(moveArrow, -moveArrow);
                    rectTransformArrow.rotation = Quaternion.Euler(0, 0, -45);
                    direction = 2;
                } else if (vectorDirection.x <= 0 && vectorDirection.y >= 0)
                {
                    // top
                    rectTransformArrow.anchoredPosition = localPointButtonDown + new Vector2(-moveArrow, moveArrow);
                    rectTransformArrow.rotation = Quaternion.Euler(0, 0, 135);
                    direction = 3;
                } else if (vectorDirection.x <= 0 && vectorDirection.y <= 0)
                {
                    // left
                    rectTransformArrow.anchoredPosition = localPointButtonDown + new Vector2(-moveArrow, -moveArrow);
                    rectTransformArrow.rotation = Quaternion.Euler(0, 0, -135);
                    direction = 4;
                }
            } else
            {
                imageArrow.SetActive(false);
                direction = 0;
            }
        }
    }
}
