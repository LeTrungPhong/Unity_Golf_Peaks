using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    //private Rigidbody rb;
    private ObstacleManager obstacleManager;
    private GameObject player;
    private List<Interpolation> interpolation = new List<Interpolation>();
    [SerializeField] private Button buttonT;
    [SerializeField] private Button buttonB;
    [SerializeField] private Button buttonL;
    [SerializeField] private Button buttonR;
    private float ballSize = 0;
    private float obstacleSize = 0;

    // direction
    private int x = 0;
    private int y = 0;
    private int z = 0;

    private int numberMove = 10;
    private int numberUp = 0;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        obstacleManager = GameObject.Find("ObstacleManager").GetComponent<ObstacleManager>();
        player = GameObject.FindWithTag("Player").gameObject;
        buttonT.onClick.AddListener(OnButtonTClick);
        buttonB.onClick.AddListener(OnButtonBClick);
        buttonL.onClick.AddListener(OnButtonLClick);
        buttonR.onClick.AddListener(OnButtonRClick);
        this.ballSize = obstacleManager.ballSize;
        this.obstacleSize = obstacleManager.obstacleSize;
        //Debug.Log("ballSize: " + this.ballSize);
    }

    // Update is called once per frame
    void Update()
    {
        movePlayer();
    }

//      Vector3.up(0, 1, 0)	Lên trên(trục Y)
//      Vector3.down(0, -1, 0)	Xuống dưới(trục Y)
//      Vector3.left(-1, 0, 0)	Trái(trục X)
//      Vector3.right(1, 0, 0)	Phải(trục X)
//      Vector3.forward(0, 0, 1)	Tiến lên(trục Z)
//      Vector3.back(0, 0, -1)	Lùi lại(trục Z)

    void OnButtonTClick()
    {
        // -z
        addMoveValid(new int[] { 0, 0, -1 });
    }

    void OnButtonBClick()
    {
        // +z
        addMoveValid(new int[] { 0, 0, 1 });
    }

    void OnButtonLClick()
    {
        // +x
        addMoveValid(new int[] { 1, 0, 0 });
    }

    void OnButtonRClick()
    {
        // -x
        addMoveValid(new int[] { -1, 0, 0 });
    }

    void addMoveValid(int[] direction)
    {
        Vector3 positionLast = interpolation.Count == 0 ? player.transform.position : interpolation[interpolation.Count - 1].end;
        int[] positionIndexLast = getPositionIndex(positionLast);
        while (numberMove > 0)
        {
            int checkPlane = this.checkPlane(new int[] { positionIndexLast[0] + direction[0], positionIndexLast[1] + direction[1], positionIndexLast[2] + direction[2] });
            if (checkPlane > 0)
            {
                //Debug.Log(checkPlane);
                Vector3 position1 = new Vector3(positionLast.x + (float)direction[0] / 2 + (float)(ballSize / 2 - (ballSize / 2) * (Mathf.Sqrt(2))) * direction[0], positionLast.y, positionLast.z + (float)direction[2] / 2 + (ballSize / 2 - (ballSize / 2) * (Mathf.Sqrt(2))) * (float)direction[2]);
                if ((checkPlane == 1 && direction[2] == -1) || (checkPlane == 2 && direction[0] == -1) || (checkPlane == 3 && direction[2] == 1) || (checkPlane == 4 && direction[0] == 1))
                {
                    this.addInterpolation(positionLast, position1, 0.5f);
                    this.addInterpolation(position1, new Vector3(position1.x + (float)direction[0] / 2, position1.y + (float)obstacleSize / 2, position1.z + (float)direction[2] / 2), 0.5f);
                    Vector3 position2 = interpolation.Count == 0 ? player.transform.position : interpolation[interpolation.Count - 1].end;
                    int[] positionIndex2 = getPositionIndex(position2);
                    if (numberMove >= 2)
                    {
                        Vector3 position3 = new Vector3(position2.x + (float)direction[0] / 2, position2.y + (float)obstacleSize / 2, position2.z + (float)direction[2] / 2);
                        this.addInterpolation(position2, position3, 0.5f);
                        this.addInterpolation(position3, new Vector3(position3.x + (float)direction[0] / 2 + (float)((ballSize / 2) * (Mathf.Sqrt(2)) - ballSize / 2) * direction[0], position3.y, position3.z + (float)direction[2] / 2 + ((ballSize / 2) * (Mathf.Sqrt(2)) - ballSize / 2) * (float)direction[2]), 0.5f);
                        numberMove--;
                    } else
                    {
                        this.addInterpolation(position2, position1, 0.5f);
                        this.addInterpolation(position1, positionLast, 0.5f);
                    }
                }
                else
                {
                    this.addInterpolation(positionLast, new Vector3(positionLast.x + direction[0] * ((float)obstacleSize / 2 - this.ballSize / 2), positionLast.y + direction[1], positionLast.z + direction[2] * ((float)obstacleSize / 2 - this.ballSize / 2)), 0.5f);
                    this.addInterpolation(interpolation[interpolation.Count - 1].end, positionLast, 0.5f);
                }
                numberMove--;
                return;
            }
            if (this.checkObstacle(new int[] { positionIndexLast[0] + direction[0], positionIndexLast[1] + direction[1], positionIndexLast[2] + direction[2] }) == false)
            {
                this.addInterpolation(positionLast, new Vector3(positionLast.x + direction[0], positionLast.y + direction[1], positionLast.z + direction[2]), 1);
                numberMove--;
                return;
            } else
            {
                this.addInterpolation(positionLast, new Vector3(positionLast.x + direction[0] * ((float)obstacleSize / 2 - this.ballSize / 2), positionLast.y + direction[1], positionLast.z + direction[2] * ((float)obstacleSize / 2 - this.ballSize / 2)), 0.5f);
                this.addInterpolation(interpolation[interpolation.Count - 1].end, positionLast, 0.5f);
                numberMove--;
                return;
            }
        }
    }

    void addInterpolation(Vector3 start, Vector3 end, float duration)
    {
        interpolation.Add(new Interpolation(start, end, duration, 0, true));
    }

    bool checkObstacle(int[] positionIndex)
    {
        if (obstacleManager.checkObstacle(positionIndex))
        {
            return true;
        }
        return false;
    }

    int checkPlane(int[] positionIndex)
    {
        return obstacleManager.checkPlane(positionIndex);
    }

    int[] getPositionIndex(Vector3 position)
    {
        int x = (int)Math.Round(position.x);
        int y = (int)Math.Round(position.y);
        int z = (int)Math.Round(position.z);
        return new int[] { x, y, z };
    }

    void movePlayer()
    {
        if (interpolation.Count > 0)
        {
            for (int i = 0; i < interpolation.Count; ++i)
            {
                Interpolation inter = interpolation[i];
                if (inter.check == true)
                {
                    inter.time += Time.deltaTime;
                    player.transform.position = Vector3.Lerp(inter.start, inter.end, inter.time / inter.duration);
                    if (inter.time >= inter.duration)
                    {
                        inter.check = false;
                    }
                    return;
                }
            }
        }
    }
}

