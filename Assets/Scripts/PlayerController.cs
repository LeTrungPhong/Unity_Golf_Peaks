using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    //private Rigidbody rb;
    private GameManager gameManager;
    private ObstacleManager obstacleManager;
    private GameObject player;
    private List<Interpolation> interpolation = new List<Interpolation>();
    [SerializeField] private Button buttonT;
    [SerializeField] private Button buttonB;
    [SerializeField] private Button buttonL;
    [SerializeField] private Button buttonR;
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    [SerializeField] private Button button3;
    [SerializeField] private Button button4;
    private float ballSize = 0;
    private float obstacleSize = 0;

    private int numberMove = 0;
    private int numberUp = 0;
    private int turn = -1;

    private int[] direction = new int[] { 0, 0, 0 };

    private float speed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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

    public void setNumber(int turn, int numberMove, int numberUp)
    {
        this.numberMove = numberMove;
        this.numberUp = numberUp;
        this.turn = turn;
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
        direction = new int[] { 0, 0, -1 };
        ballMove();
    }

    void OnButtonBClick()
    {
        // +z
        direction = new int[] { 0, 0, 1 };
        ballMove();
    }

    void OnButtonLClick()
    {
        // +x
        direction = new int[] { 1, 0, 0 };
        ballMove();
    }

    void OnButtonRClick()
    {
        // -x
        direction = new int[] { -1, 0, 0 };
        ballMove();
    }

    void ballMove()
    {
        if (turn == 0)
        {
            addMoveValid();
            addFlyValid();
        }
        else if (turn == 1)
        {
            addFlyValid();
            addMoveValid();
        }
    }

    void addMoveValid()
    {
        while (numberMove > 0)
        {
            handleMoveValid();
            numberMove--;
        }
    }

    void addFlyValid()
    {
        handleFlyValid();
    }

    void handleMoveValid()
    {
        Vector3 positionLast = interpolation.Count == 0 ? player.transform.position : interpolation[interpolation.Count - 1].end;
        int[] positionIndexLast = getPositionIndex(positionLast);

        int checkChangeDirection = obstacleManager.checkChangeDirection(new int[] { positionIndexLast[0], positionIndexLast[1], positionIndexLast[2] });
        
        if (checkChangeDirection > 0)
        {
            Debug.Log("checkChangeDirection: " + checkChangeDirection);
            if (checkChangeDirection == 1 && (direction[0] < 0 || direction[2] < 0))
            {
                direction = new int[] { direction[0] == 0 ? - direction[2] : 0, 0, direction[2] == 0 ? - direction[0] : 0 };
            }
            else if (checkChangeDirection == 2 && (direction[0] < 0 || direction[2] > 0))
            {
                direction = new int[] { direction[0] == 0 ? direction[2] : 0, 0, direction[2] == 0 ? direction[0] : 0 };
            }
            else if (checkChangeDirection == 3 && (direction[0] > 0 || direction[2] > 0))
            {
                direction = new int[] { direction[0] == 0 ? - direction[2] : 0, 0, direction[2] == 0 ? - direction[0] : 0 };
            }
            else if (checkChangeDirection == 4 && (direction[0] > 0 || direction[2] < 0))
            {
                direction = new int[] { direction[0] == 0 ? direction[2] : 0, 0, direction[2] == 0 ? direction[0] : 0 };
            }
        }

        int checkPlane = obstacleManager.checkPlane(new int[] { positionIndexLast[0] + direction[0], positionIndexLast[1] + direction[1], positionIndexLast[2] + direction[2] });
        //Debug.Log("checkPlane: " + checkPlane);
        if (checkPlane > 0)
        {
            Vector3 position1 = new Vector3(positionLast.x + (float)direction[0] / 2 + (float)(ballSize / 2 - (ballSize / 2) * (Mathf.Sqrt(2))) * direction[0], positionLast.y, positionLast.z + (float)direction[2] / 2 + (ballSize / 2 - (ballSize / 2) * (Mathf.Sqrt(2))) * (float)direction[2]);
            if ((checkPlane == 1 && direction[2] == -1) || (checkPlane == 2 && direction[0] == -1) || (checkPlane == 3 && direction[2] == 1) || (checkPlane == 4 && direction[0] == 1))
            {
                this.addInterpolation(positionLast, position1, speed / 2);
                this.addInterpolation(position1, new Vector3(position1.x + (float)direction[0] / 2, position1.y + (float)obstacleSize / 2, position1.z + (float)direction[2] / 2), speed / 2);
                Vector3 position2 = interpolation.Count == 0 ? player.transform.position : interpolation[interpolation.Count - 1].end;
                int[] positionIndex2 = getPositionIndex(position2);
                if (numberMove >= 2)
                {
                    // move
                    Vector3 position3 = new Vector3(position2.x + (float)direction[0] / 2, position2.y + (float)obstacleSize / 2, position2.z + (float)direction[2] / 2);
                    this.addInterpolation(position2, position3, speed / 2);
                    Vector3 position4 = new Vector3(position3.x + (float)((ballSize / 2) * (Mathf.Sqrt(2)) - ballSize / 2) * direction[0], position3.y, position3.z + ((ballSize / 2) * (Mathf.Sqrt(2)) - ballSize / 2) * (float)direction[2]);
                    this.addInterpolation(position3, position4, speed / 10);
                    //this.addInterpolation(position4, new Vector3(position4.x + (float)direction[0] * ballSize / 2, position4.y, position4.z + (float)direction[2] * ballSize / 2), speed / 10);
                    checkMoveDown(this.getPositionIndex(new Vector3(position2.x, position2.y + obstacleSize, position2.z)));
                    numberMove--;
                }
                else
                {
                    // back
                    this.addInterpolation(position2, position1, speed / 2);
                    this.addInterpolation(position1, positionLast, speed / 2);
                    direction = new int[] { -direction[0], direction[1], -direction[2] };
                }
            }
            else
            {
                // back
                this.addInterpolation(positionLast, new Vector3(positionLast.x + direction[0] * ((float)obstacleSize / 2 - this.ballSize / 2), positionLast.y + direction[1], positionLast.z + direction[2] * ((float)obstacleSize / 2 - this.ballSize / 2)), speed / 2);
                this.addInterpolation(interpolation[interpolation.Count - 1].end, positionLast, speed / 2);
                direction = new int[] { -direction[0], direction[1], -direction[2] };
            }
            return;
        }
        if (obstacleManager.checkObstacle(new int[] { positionIndexLast[0] + direction[0], positionIndexLast[1] + direction[1], positionIndexLast[2] + direction[2] }) == false)
        {
            // move
            this.addInterpolation(positionLast, new Vector3(positionLast.x + (float)direction[0] * ((float)1 / 2), positionLast.y, positionLast.z + (float)direction[2] * ((float)1 / 2)), speed);
            checkMoveDown(this.getPositionIndex(positionLast));
            return;
        }
        else
        {
            // back
            this.addInterpolation(positionLast, new Vector3(positionLast.x + (float)direction[0] * ((float)obstacleSize / 2 - this.ballSize / 2), positionLast.y, positionLast.z + (float)direction[2] * ((float)obstacleSize / 2 - this.ballSize / 2)), speed / 2);
            this.addInterpolation(interpolation[interpolation.Count - 1].end, positionLast, speed / 2);
            direction = new int[] { -direction[0], direction[1], -direction[2] };
            return;
        }
    }

    void handleFlyValid()
    {
        if (numberUp > 0)
        {
            Vector3 positionLast = interpolation.Count == 0 ? player.transform.position : interpolation[interpolation.Count - 1].end;
            Vector3 position1 = new Vector3(positionLast.x + (float)direction[0], positionLast.y + 3, positionLast.z + (float)direction[2]);
            this.addInterpolation(positionLast, position1, speed);
            numberUp--;
            while (numberUp > 0)
            {
                Vector3 positionNow = interpolation.Count == 0 ? player.transform.position : interpolation[interpolation.Count - 1].end;
                Vector3 positionFly = new Vector3(positionNow.x + (float)direction[0], positionNow.y, positionNow.z + (float)direction[2]);
                this.addInterpolation(positionNow, positionFly, speed);
                numberUp--;
            }
            checkDown();
        }
    }

    void addInterpolation(Vector3 start, Vector3 end, float duration)
    {
        interpolation.Add(new Interpolation(start, end, duration, 0, true));
    }

    void checkMoveDown(int[] positionIndex)
    {
        Vector3 positionLast = interpolation.Count == 0 ? player.transform.position : interpolation[interpolation.Count - 1].end;
        if (obstacleManager.checkObstacle(new int[] { positionIndex[0] + direction[0], positionIndex[1] - 1, positionIndex[2] + direction[2] }))
        {
            this.addInterpolation(positionLast, new Vector3(positionLast.x + (float)direction[0] * ((float)1 / 2), positionLast.y, positionLast.z + (float)direction[2] * ((float)1 / 2)), speed / 2);
        } else
        {
            int checkPlane = obstacleManager.checkPlane(new int[] { positionIndex[0] + direction[0], positionIndex[1] - 1, positionIndex[2] + direction[2] });
            if (checkPlane > 0)
            {
                // only check all
                Vector3 position1 = new Vector3(positionLast.x + (float)((ballSize / 2) * (Mathf.Sqrt(2)) - ballSize / 2) * direction[0], positionLast.y, positionLast.z + ((ballSize / 2) * (Mathf.Sqrt(2)) - ballSize / 2) * (float)direction[2]);
                this.addInterpolation(positionLast, position1, speed / 10);
                Vector3 position2 = new Vector3(position1.x + (float)direction[0], position1.y - (float)obstacleSize, position1.z + (float)direction[2]);
                this.addInterpolation(position1, position2, speed);
                Vector3 position3 = new Vector3(position2.x + (float)direction[0] * ((float)1 / 2) - (float)((ballSize / 2) * (Mathf.Sqrt(2)) - ballSize / 2) * direction[0], position2.y, position2.z + (float)direction[2] * ((float)1 / 2) - ((ballSize / 2) * (Mathf.Sqrt(2)) - ballSize / 2) * (float)direction[2]);
                this.addInterpolation(position2, position3 , speed / 2);
            } else
            {
                Vector3 position1 = new Vector3(positionLast.x + (float)direction[0] * (ballSize / 2), positionLast.y, positionLast.z + (float)direction[2] * (ballSize / 2));
                this.addInterpolation(positionLast, position1, speed / 10);
                Vector3 position2 = new Vector3(position1.x, position1.y - (float)1, position1.z);
                this.addInterpolation(position1, position2, speed / 2);
                this.addInterpolation(position2, new Vector3(position2.x + (float)direction[0] * ((float)1 / 2 - ballSize / 2), position2.y, position2.z + (float)direction[2] * ((float)1 / 2 - ballSize / 2)), speed / 2);
            }
        }

            for (int i = 0; i <= 10; i++)
            {
                Vector3 vector3 = interpolation.Count == 0 ? player.transform.position : interpolation[interpolation.Count - 1].end;
                int[] positionIndex3 = getPositionIndex(vector3);

                if (obstacleManager.checkObstacle(new int[] { positionIndex3[0], positionIndex3[1] - 1, positionIndex3[2] }) == false)
                {
                    this.addInterpolation(vector3, new Vector3(vector3.x, vector3.y - obstacleSize, vector3.z), speed / 4);
                }
                else
                {
                    return;
                }
            }
    }

    void checkDown()
    {
        for (int i = 0; i <= 10; i++)
        {
            Vector3 vector3 = interpolation.Count == 0 ? player.transform.position : interpolation[interpolation.Count - 1].end;
            int[] positionIndex3 = getPositionIndex(vector3);
            int plane = obstacleManager.checkPlane(new int[] { positionIndex3[0], positionIndex3[1] - 1, positionIndex3[2] });
            if (plane > 0)
            {
                Vector3 position1 = new Vector3(vector3.x, vector3.y - (ballSize / 2) - (obstacleSize / 2) + (ballSize / 2) * Mathf.Sqrt(2), vector3.z);
                this.addInterpolation(vector3, position1, speed / 5);
                if (plane == 1)
                {
                    direction = new int[] { 0, 0, 1 };
                }
                else if (plane == 2)
                {
                    direction = new int[] { 1, 0, 0 };
                }
                else if (plane == 3)
                {
                    direction = new int[] { 0, 0, -1 };
                }
                else if (plane == 4)
                {
                    direction = new int[] { -1, 0, 0 };
                }
                Vector3 position2 = new Vector3(position1.x + (float)direction[0] * (obstacleSize / 2) + (float)direction[0] * (ballSize / 2) * (Mathf.Sqrt(2) - (float)1), position1.y - (obstacleSize / 2) + (ballSize / 2) - (ballSize / 2) * Mathf.Sqrt(2), position1.z + (float)direction[2] * (obstacleSize / 2) + (float)direction[2] * (ballSize / 2) * (Mathf.Sqrt(2) - (float)1));
                this.addInterpolation(position1, position2, speed / 4);
                Vector3 position3 = new Vector3(position2.x + (float)direction[0] * (obstacleSize / 2) - (float)direction[0] * (ballSize / 2) * (Mathf.Sqrt(2) - (float)1), position2.y, position2.z + (float)direction[2] * (obstacleSize / 2) - (float)direction[2] * (ballSize / 2) * (Mathf.Sqrt(2) - (float)1));
                this.addInterpolation(position2, position3, speed / 4);
                return;
            }
            if (obstacleManager.checkObstacle(new int[] { positionIndex3[0], positionIndex3[1] - 1, positionIndex3[2] }) == false)
            {
                this.addInterpolation(vector3, new Vector3(vector3.x, vector3.y - obstacleSize, vector3.z), speed / 4);
            }
            else
            {
                return;
            }
        }
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
        if (interpolation.Count > 0 && gameManager.isGameOver == false)
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

        if (numberMove == 0 && numberUp == 0)
        {
            checkGameOver();
        }
    }

    void checkGameOver()
    {
        if (obstacleManager.checkGoal(getPositionIndex(player.transform.position)))
        {
            gameManager.GameWin();
            return;
        }

        Button[] buttons = FindObjectsOfType<Button>();

        // Duyệt qua danh sách và đếm số Button có tên "MyButton"
        foreach (Button btn in buttons)
        {
            if (btn.gameObject.name == "MyButton")
            {
                return;
            }
        }

        gameManager.GameOver();
    }
}

