using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float speed = 5.0f;
    private float speedRotate = 2.0f;
    public RectTransform canvasRect;
    public Vector2 localPointButtonDown;
    public Vector2 localPointButtonHold;
    public Vector3 angleCameraButtonDown;
    private bool confirmRotate = false;
    float moveY = 0;
    private float sensitivity = 3;
    private float gravity = 3;
    public Vector3 postCam;
    private Vector3 postChange = new Vector3(20, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = postCam - postChange;
        gameObject.transform.DOMove(postCam, 1.5f).SetEase(Ease.OutQuad);
    }

    // Update is called once per frame
    void Update()
    {
        cameraMove();
        cameraRotate();
    }

    void cameraMove()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.E))
        {
            moveY = moveY + sensitivity * Time.deltaTime;
            if (moveY > 1) moveY = 1;
        } else if (Input.GetKey(KeyCode.Q))
        {
            moveY = moveY - sensitivity * Time.deltaTime;
            if (moveY < -1) moveY = -1;
        } else
        {
            if (moveY > 0)
            {
                moveY = moveY - gravity * Time.deltaTime;
                if (moveY * (moveY - gravity * Time.deltaTime) <= 0)
                {
                    moveY = 0;
                }
            } else if (moveY < 0)
            {
                moveY = moveY + gravity * Time.deltaTime;
                if (moveY * (moveY + gravity * Time.deltaTime) <= 0)
                {
                    moveY = 0;
                }
            }
        }

        Vector3 transformMoveXZ = new Vector3(moveX, 0, moveZ) * speed * Time.deltaTime;
        Vector3 transformMoveY = new Vector3(0, moveY, 0) * speed * Time.deltaTime;
        transform.Translate(transformMoveXZ, Space.Self);
        transform.Translate(transformMoveY, Space.World);
    }

    void cameraRotate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, null, out localPointButtonDown);
            confirmRotate = true;
            angleCameraButtonDown = transform.eulerAngles;
        }
        if (Input.GetMouseButtonUp(1))
        {
            confirmRotate = false;

        }
        if (Input.GetMouseButton(1) && confirmRotate)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Input.mousePosition, null, out localPointButtonHold);

            Vector2 vectorDirection = localPointButtonHold - localPointButtonDown;

            float angleX = -vectorDirection.y * speedRotate * 0.05f;
            float angleY = vectorDirection.x * speedRotate * 0.05f;

            transform.eulerAngles = angleCameraButtonDown + new Vector3(angleX, angleY, 0);
        }
    }

    public void CameraChangeLevel()
    {
        gameObject.transform.DOMove(postCam + postChange, 2).SetEase(Ease.InQuad);
    }
}