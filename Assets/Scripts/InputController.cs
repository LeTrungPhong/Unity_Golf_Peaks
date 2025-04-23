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
    private float moveArrow = 15;

    private int direction = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<BallController>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        imageSquareLine.SetActive(false);
        imageArrow.SetActive(false);
        rectTransformImageSquareLine = imageSquareLine.GetComponent<RectTransform>();
        rectTransformArrow = imageArrow.GetComponent<RectTransform>();

        //if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        //{
        //    Debug.Log("Đang chạy trên điện thoại.");

        //} else
        //{
        //    Debug.Log("Khong phai chay tren dien thoai");
        //}

        RectTransform rectImageSquareLine = imageSquareLine.GetComponent<RectTransform>();
        rectImageSquareLine.sizeDelta = new Vector2(250, 250);

        RectTransform rectImageArrow = imageArrow.GetComponent<RectTransform>();
        rectImageArrow.sizeDelta = new Vector2(100, 100);

        moveArrow = 30;
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

            Debug.Log("Direction: " + direction);

            gameManager.HiddenButton(direction);

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
                playerController.OnButtonLClick();
            }
            else if (direction == 4)
            {
                playerController.OnButtonTClick();
            }
            direction = 0;

            gameManager.FocusButton();
            gameManager.HiddenDirect();
        }
        if (Input.GetMouseButton(0) && confirmMove)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, null, out localPointButtonHold);

            Vector2 vectorDirection = localPointButtonHold - localPointButtonDown;

            float lenghtVector = vectorDirection.magnitude;

            if (lenghtVector > 50)
            {
                imageArrow.SetActive(true);
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
                    direction = 4;
                } else if (vectorDirection.x <= 0 && vectorDirection.y <= 0)
                {
                    // left
                    rectTransformArrow.anchoredPosition = localPointButtonDown + new Vector2(-moveArrow, -moveArrow);
                    rectTransformArrow.rotation = Quaternion.Euler(0, 0, -135);
                    direction = 3;
                }
            } else
            {
                imageArrow.SetActive(false);
                direction = 0;
            }
        }
    }
}
