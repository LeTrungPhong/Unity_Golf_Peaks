﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum SoundPlayerType
{
    BALL_ROLL,
    BALL_FLY,
    BALL_RECOIL,
    BALL_FALLING,
}

public class BallController : MonoBehaviour
{
    //private Rigidbody rb;
    private GameManager gameManager;
    private ObstacleManager obstacleManager;
    private GameObject player;
    private CanvasScript canvasScript;
    private List<Interpolation> interpolation = new List<Interpolation>();
    private float ballSize = 0;
    private float scaleSize = 0;
    private float obstacleSize = 0;

    private int numberMove = 0;
    private int numberUp = 0;
    private int turn = -1;
    private bool checkMove = false;
    private int[] direction = new int[] { 0, 0, 0 };
    private float speed = 0.5f;

    [SerializeField] private ParticleSystem particleSystemBallMove;
    [SerializeField] private ParticleSystem particleSystemWinGame;
    private ParticleSystem spawnedParticalSystem;
    private bool playParticalSystem = false;
    private Vector3 positionBallLast = new Vector3(0, 0, 0);

    [SerializeField] private AudioSource audioLoopSound;
    [SerializeField] private AudioSource audioShotSound;
    [SerializeField] private AudioClip[] ListAudio;

    private bool stateGame = false;

    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        obstacleManager = GameObject.FindGameObjectWithTag("ObstacleManager").GetComponent<ObstacleManager>();
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        canvasScript = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasScript>();
        this.ballSize = obstacleManager.ballSize;
        this.scaleSize = player.GetComponent<Renderer>().bounds.size.x;
        this.obstacleSize = obstacleManager.obstacleSize;
        //Debug.Log("ballSize: " + this.ballSize);
        spawnedParticalSystem = Instantiate(particleSystemBallMove, transform.position, Quaternion.identity);
        spawnedParticalSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (obstacleManager.spawnObstacles != null)
        {
            movePlayer();
        }
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

    public void OnButtonTClick()
    {
        // -z
        direction = new int[] { 0, 0, -1 };
        ballMove();
    }

    public void OnButtonBClick()
    {
        // +z
        direction = new int[] { 0, 0, 1 };
        ballMove();
    }

    public void OnButtonLClick()
    {
        // +x
        direction = new int[] { 1, 0, 0 };
        ballMove();
    }

    public void OnButtonRClick()
    {
        // -x
        direction = new int[] { -1, 0, 0 };
        ballMove();
    }

    void ballMove()
    {
        canvasScript.HiddenHint();
        if (obstacleManager.spawnObstacles == null) return;
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

        this.addInterpolation(interpolation.Last().end, interpolation.Last().end, 0);
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

        if (obstacleManager.checkBlockRoll(new int[] { positionIndexLast[0], positionIndexLast[1] - 1, positionIndexLast[2] }))
        {
            numberMove = 0;
            return;
        }

        int checkChangeDirection = obstacleManager.checkChangeDirection(new int[] { positionIndexLast[0], positionIndexLast[1], positionIndexLast[2] });

        if (checkChangeDirection > 0)
        {
            Debug.Log("checkChangeDirection: " + checkChangeDirection);
            if (checkChangeDirection == 1 && (direction[0] < 0 || direction[2] < 0))
            {
                direction = new int[] { direction[0] == 0 ? -direction[2] : 0, 0, direction[2] == 0 ? -direction[0] : 0 };
                this.addInterpolation(positionLast, positionLast, 0.01f, SoundPlayerType.BALL_ROLL, SoundPlayerType.BALL_RECOIL);
            }
            else if (checkChangeDirection == 2 && (direction[0] < 0 || direction[2] > 0))
            {
                direction = new int[] { direction[0] == 0 ? direction[2] : 0, 0, direction[2] == 0 ? direction[0] : 0 };
                this.addInterpolation(positionLast, positionLast, 0.01f, SoundPlayerType.BALL_ROLL, SoundPlayerType.BALL_RECOIL);
            }
            else if (checkChangeDirection == 3 && (direction[0] > 0 || direction[2] > 0))
            {
                direction = new int[] { direction[0] == 0 ? -direction[2] : 0, 0, direction[2] == 0 ? -direction[0] : 0 };
                this.addInterpolation(positionLast, positionLast, 0.01f, SoundPlayerType.BALL_ROLL, SoundPlayerType.BALL_RECOIL);
            }
            else if (checkChangeDirection == 4 && (direction[0] > 0 || direction[2] < 0))
            {
                direction = new int[] { direction[0] == 0 ? direction[2] : 0, 0, direction[2] == 0 ? direction[0] : 0 };
                this.addInterpolation(positionLast, positionLast, 0.01f, SoundPlayerType.BALL_ROLL, SoundPlayerType.BALL_RECOIL);
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
                this.addInterpolation(positionLast, new Vector3(positionLast.x + direction[0] * ((float)obstacleSize / 2 - this.ballSize / 2), positionLast.y + direction[1], positionLast.z + direction[2] * ((float)obstacleSize / 2 - this.ballSize / 2)), speed / 2, SoundPlayerType.BALL_ROLL, SoundPlayerType.BALL_RECOIL);
                this.addInterpolation(interpolation[interpolation.Count - 1].end, positionLast, speed / 2);
                direction = new int[] { -direction[0], direction[1], -direction[2] };
            }
            return;
        }
        if (
            obstacleManager.checkObstacle(new int[] { positionIndexLast[0] + direction[0], positionIndexLast[1] + direction[1], positionIndexLast[2] + direction[2] }) == false
            && obstacleManager.checkBlockRoll(new int[] { positionIndexLast[0] + direction[0], positionIndexLast[1] + direction[1], positionIndexLast[2] + direction[2] }) == false
            && obstacleManager.checkDive(new int[] { positionIndexLast[0] + direction[0], positionIndexLast[1] + direction[1], positionIndexLast[2] + direction[2] }) == false
            )
        {
            // move
            this.addInterpolation(positionLast, new Vector3(positionLast.x + (float)direction[0] * ((float)1 / 2), positionLast.y, positionLast.z + (float)direction[2] * ((float)1 / 2)), speed / 2);
            checkMoveDown(this.getPositionIndex(positionLast));
            return;
        }
        else
        {
            // back
            this.addInterpolation(positionLast, new Vector3(positionLast.x + (float)direction[0] * ((float)obstacleSize / 2 - this.ballSize / 2), positionLast.y, positionLast.z + (float)direction[2] * ((float)obstacleSize / 2 - this.ballSize / 2)), speed / 2, SoundPlayerType.BALL_ROLL, SoundPlayerType.BALL_RECOIL);
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
            Vector3 position1 = new Vector3(positionLast.x + (float)direction[0] * ((float)1 / 2 + ballSize / 2), positionLast.y + obstacleManager.highFly, positionLast.z + (float)direction[2] * ((float)1 / 2 + ballSize / 2));
            this.addInterpolation(positionLast, position1, speed, SoundPlayerType.BALL_FLY, SoundPlayerType.BALL_FLY);
            numberUp--;
            while (numberUp > 0)
            {
                Vector3 positionNow = interpolation.Count == 0 ? player.transform.position : interpolation[interpolation.Count - 1].end;
                Vector3 positionFly = new Vector3(positionNow.x + (float)direction[0], positionNow.y, positionNow.z + (float)direction[2]);
                this.addInterpolation(positionNow, positionFly, speed, SoundPlayerType.BALL_FLY, SoundPlayerType.BALL_FLY);
                numberUp--;
            }
            FlyDown();
        }
    }

    void addInterpolation(Vector3 start, Vector3 end, float duration, SoundPlayerType LoopSound = SoundPlayerType.BALL_ROLL, SoundPlayerType ShotSound = SoundPlayerType.BALL_ROLL, int typeMove = 0)
    {
        interpolation.Add(new Interpolation(start, end, duration, 0, true, LoopSound, ShotSound, typeMove));
    }

    void checkMoveDown(int[] positionIndex)
    {
        if (obstacleManager.checkWater(new int[] { positionIndex[0] + direction[0], positionIndex[1] - 1, positionIndex[2] + direction[2] }))
        {
            Vector3 positionLast = interpolation.Count == 0 ? player.transform.position : interpolation[interpolation.Count - 1].end;
            Vector3 position1 = new Vector3(positionLast.x + (float)direction[0] * (ballSize / 2), positionLast.y, positionLast.z + (float)direction[2] * (ballSize / 2));
            this.addInterpolation(positionLast, position1, speed / 10);
            Vector3 position2 = new Vector3(position1.x, position1.y - obstacleSize / 2, position1.z);
            this.addInterpolation(position1, position2, speed / 5);
            numberMove = 0;
            numberUp = 0;
            WaterMoveBack();
            return;
        }

        if (obstacleManager.checkObstacle(new int[] { positionIndex[0] + direction[0], positionIndex[1] - 1, positionIndex[2] + direction[2] }) || obstacleManager.checkDive(new int[] { positionIndex[0] + direction[0], positionIndex[1] - 1, positionIndex[2] + direction[2] }))
        {
            Vector3 positionLast = interpolation.Count == 0 ? player.transform.position : interpolation[interpolation.Count - 1].end;
            this.addInterpolation(positionLast, new Vector3(positionLast.x + (float)direction[0] * ((float)1 / 2), positionLast.y, positionLast.z + (float)direction[2] * ((float)1 / 2)), speed / 2);
            return;
        }

        if (obstacleManager.checkBlockRoll(new int[] { positionIndex[0] + direction[0], positionIndex[1] - 1, positionIndex[2] + direction[2] }))
        {
            Vector3 positionLast = interpolation.Count == 0 ? player.transform.position : interpolation[interpolation.Count - 1].end;
            this.addInterpolation(positionLast, new Vector3(positionLast.x + (float)direction[0] * ((float)1 / 2), positionLast.y, positionLast.z + (float)direction[2] * ((float)1 / 2)), speed / 2);
            numberMove = 0;
            return;
        }

        int plane = obstacleManager.checkPlane(new int[] { positionIndex[0] + direction[0], positionIndex[1] - 1, positionIndex[2] + direction[2] });
        if (plane > 0)
        {
            if ((plane == 1 && direction[2] > 0) || (plane == 2 && direction[0] > 0) || (plane == 3 && direction[2] < 0) || (plane == 4 && direction[0] < 0))
            {
                Vector3 positionLast = interpolation.Count == 0 ? player.transform.position : interpolation[interpolation.Count - 1].end;
                Vector3 position1 = new Vector3(positionLast.x + (float)((ballSize / 2) * (Mathf.Sqrt(2)) - ballSize / 2) * direction[0], positionLast.y, positionLast.z + ((ballSize / 2) * (Mathf.Sqrt(2)) - ballSize / 2) * (float)direction[2]);
                this.addInterpolation(positionLast, position1, speed / 10);
                Vector3 position2 = new Vector3(position1.x + (float)direction[0], position1.y - (float)obstacleSize, position1.z + (float)direction[2]);
                this.addInterpolation(position1, position2, speed);
                Vector3 position3 = new Vector3(position2.x + 0 - (float)((ballSize / 2) * (Mathf.Sqrt(2)) - ballSize / 2) * direction[0] + (float)direction[0] * (ballSize / 2), position2.y, position2.z + 0 - ((ballSize / 2) * (Mathf.Sqrt(2)) - ballSize / 2) * (float)direction[2] + (float)direction[2] * (ballSize / 2));
                this.addInterpolation(position2, position3, speed / 100);
                MoveDown();
            } else
            {
                Vector3 positionLast = interpolation.Count == 0 ? player.transform.position : interpolation[interpolation.Count - 1].end;
                Vector3 position1 = new Vector3(positionLast.x + (float)direction[0] * (ballSize / 2), positionLast.y, positionLast.z + (float)direction[2] * (ballSize / 2));
                this.addInterpolation(positionLast, position1, speed / 10);
                MoveDown();
            }
            //checkMoveDown(getPositionIndex(position3 - new Vector3(direction[0] / 2, direction[1], direction[2] / 2)));
            //Vector3 position3 = new Vector3(position2.x + (float)direction[0] * ((float)1 / 2) - (float)((ballSize / 2) * (Mathf.Sqrt(2)) - ballSize / 2) * direction[0], position2.y, position2.z + (float)direction[2] * ((float)1 / 2) - ((ballSize / 2) * (Mathf.Sqrt(2)) - ballSize / 2) * (float)direction[2]);
            //this.addInterpolation(position2, position3, speed / 2);
        }
        else
        {
            Vector3 positionLast = interpolation.Count == 0 ? player.transform.position : interpolation[interpolation.Count - 1].end;
            Vector3 position1 = new Vector3(positionLast.x + (float)direction[0] * (ballSize / 2), positionLast.y, positionLast.z + (float)direction[2] * (ballSize / 2));
            this.addInterpolation(positionLast, position1, speed / 10);
            MoveDown();
        }
    }

    void WaterMoveBack()
    {
        Vector3 positionLast = interpolation.Count == 0 ? player.transform.position : interpolation[interpolation.Count - 1].end;
        for (int i = interpolation.Count - 1; i >= 0; --i)
        {
            Vector3 position = interpolation[i].start;
            int[] index = getPositionIndex(position);
            if (
                (
                    obstacleManager.checkObstacle(new int[] { index[0], index[1] - 1, index[2] }) == true
                    || obstacleManager.checkBlockRoll(new int[] { index[0], index[1] - 1, index[2] }) == true
                )
                && obstacleManager.checkPlane(index) == 0
                && obstacleManager.checkWater(new int[] { index[0], index[1], index[2] }) == false
                )
            {
                Vector3 position1 = new Vector3(index[0], index[1] - (float)1 / 2 + ballSize / 2, index[2]);
                this.addInterpolation(positionLast, position1, speed / 5);
                return;
            }
        }
    }

    void MoveDown()
    {
        for (int i = 0; i <= 20; i++)
        {
            Vector3 vector1 = interpolation.Count == 0 ? player.transform.position : interpolation[interpolation.Count - 1].end;
            int[] positionIndex = getPositionIndex(vector1);

            if (obstacleManager.checkWater(new int[] { positionIndex[0], positionIndex[1] - 1, positionIndex[2] }))
            {
                Vector3 positionLast = interpolation.Count == 0 ? player.transform.position : interpolation[interpolation.Count - 1].end;
                Vector3 position1 = new Vector3(positionLast.x, positionLast.y - (float)obstacleSize / 2, positionLast.z);
                this.addInterpolation(positionLast, position1, speed / 5);
                numberMove = 0;
                numberUp = 0;
                WaterMoveBack();
                return;
            }

            if (obstacleManager.checkBlockRoll(new int[] { positionIndex[0], positionIndex[1] - 1, positionIndex[2] }))
            {
                //Vector3 position1 = new Vector3(vector1.x, vector1.y - (float)obstacleSize, vector1.z);
                //this.addInterpolation(vector1, position1, speed / 2);
                Vector3 position2 = new Vector3(vector1.x + (float)direction[0] * (obstacleSize / 2 - ballSize / 2), vector1.y, vector1.z + (float)direction[2] * (obstacleSize / 2 - ballSize / 2));
                this.addInterpolation(vector1, position2, speed / 5, SoundPlayerType.BALL_ROLL, SoundPlayerType.BALL_ROLL, 2);
                numberMove = 0;
                return;
            }

            int plane = obstacleManager.checkPlane(new int[] { positionIndex[0], positionIndex[1] - 1, positionIndex[2] });
            if (plane > 0)
            {
                if ((plane == 1 && direction[2] > 0) || (plane == 2 && direction[0] > 0) || (plane == 3 && direction[2] < 0) || (plane == 4 && direction[0] < 0))
                {
                    Vector3 positionLast = interpolation.Count == 0 ? player.transform.position : interpolation[interpolation.Count - 1].end;
                    Vector3 position1 = new Vector3(positionLast.x + (float)direction[0], positionLast.y - (float)obstacleSize, positionLast.z + (float)direction[2]);
                    this.addInterpolation(positionLast, position1, speed / 2, SoundPlayerType.BALL_ROLL, SoundPlayerType.BALL_ROLL, 2);
                } else
                {
                    Vector3 positionLast = interpolation.Count == 0 ? player.transform.position : interpolation[interpolation.Count - 1].end;
                    Vector3 vector3 = new Vector3(positionLast.x + (float)direction[0] * ((float)obstacleSize / 2 - ballSize / 2), positionLast.y, positionLast.z + (float)direction[2] * ((float)obstacleSize / 2 - ballSize / 2));
                    this.addInterpolation(positionLast, vector3, speed / 5);
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
                    this.addInterpolation(position1, position2, speed / 4, SoundPlayerType.BALL_ROLL, SoundPlayerType.BALL_ROLL, 2);
                    Vector3 position3 = new Vector3(position2.x + (float)direction[0] * (ballSize / 2) - (float)direction[0] * (ballSize / 2) * (Mathf.Sqrt(2) - (float)1), position2.y, position2.z + (float)direction[2] * (ballSize / 2) - (float)direction[2] * (ballSize / 2) * (Mathf.Sqrt(2) - (float)1));
                    this.addInterpolation(position2, position3, speed / 4);
                }
            } else if (obstacleManager.checkObstacle(new int[] { positionIndex[0], positionIndex[1] - 1, positionIndex[2] }) == false && obstacleManager.checkDive(new int[] { positionIndex[0], positionIndex[1] - 1, positionIndex[2] }) == false)
            {
                this.addInterpolation(vector1, new Vector3(vector1.x, vector1.y - obstacleSize, vector1.z), speed / 4);
            }
            else
            {
                Vector3 positionLast2 = interpolation.Count == 0 ? player.transform.position : interpolation[interpolation.Count - 1].end;
                Vector3 position2 = new Vector3(positionLast2.x + (float)direction[0] * ((float)1 / 2 - ballSize / 2), positionLast2.y, positionLast2.z + (float)direction[2] * ((float)1 / 2 - ballSize / 2));
                this.addInterpolation(positionLast2, position2, speed / 2, SoundPlayerType.BALL_ROLL, SoundPlayerType.BALL_ROLL, 0);
                return;
            }
        }
    }

    void FlyDown()
    {
        Vector3 positionLast = interpolation.Count == 0 ? player.transform.position : interpolation[interpolation.Count - 1].end;
        int[] positionIndex = getPositionIndex(positionLast);
        Vector3 positionLastTemp = positionLast;
        float numberDown = 0;
        for (int i = 0; i <= 20; i++)
        {
            if (obstacleManager.checkWater(new int[] { positionIndex[0], positionIndex[1] - 1, positionIndex[2] }))
            {
                Vector3 position1 = new Vector3(positionLastTemp.x, positionLastTemp.y - obstacleSize * numberDown - (float)1 / 2, positionLastTemp.z);
                this.addInterpolation(positionLastTemp, position1, speed / 5 + (speed / 4) * numberDown);
                WaterMoveBack();
                numberMove = 0;
                numberUp = 0;
                return;
            }

            //if (obstacleManager.checkBlockRoll(new int[] { positionIndex[0], positionIndex[1] - 1, positionIndex[2] }))
            //{
            //    Vector3 position1 = new Vector3(positionLastTemp.x, positionLastTemp.y - obstacleSize * numberDown, positionLastTemp.z);
            //    this.addInterpolation(positionLastTemp, position1, (speed / 4) * numberDown, SoundPlayerType.BALL_FLY, numberDown == 0 ? SoundPlayerType.BALL_FLY : SoundPlayerType.BALL_FALLING);
            //    Vector3 position2 = new Vector3(position1.x + (float)direction[0] * ((float)1 / 2 - ballSize / 2), position1.y, position1.z + (float)direction[2] * ((float)1 / 2 - ballSize / 2));
            //    this.addInterpolation(position1, position2, speed, SoundPlayerType.BALL_ROLL, SoundPlayerType.BALL_ROLL, 1);
            //    positionLast = position2;
            //    positionIndex = getPositionIndex(positionLast);
            //    numberMove = 0;
            //    return;
            //}

            // (!)
            
            int plane = obstacleManager.checkPlane(new int[] { positionIndex[0], positionIndex[1] - 1, positionIndex[2] });
            if (plane > 0)
            {
                if (numberDown != 0)
                {
                    Vector3 position0 = new Vector3(positionLastTemp.x + (float)direction[0] * ((float)1 / 2 - ballSize / 2), positionLastTemp.y - obstacleSize * numberDown, positionLastTemp.z + (float)direction[2] * ((float)1 / 2 - ballSize / 2));
                    this.addInterpolation(positionLastTemp, position0, (speed / 4) * numberDown, SoundPlayerType.BALL_FLY, SoundPlayerType.BALL_FLY);
                    positionLastTemp = position0;
                }
                Vector3 position1 = new Vector3(positionLastTemp.x, positionLastTemp.y - (ballSize / 2) - (obstacleSize / 2) + (ballSize / 2) * Mathf.Sqrt(2), positionLastTemp.z);
                this.addInterpolation(positionLastTemp, position1, speed / 5, SoundPlayerType.BALL_FLY, numberDown == 0 ? SoundPlayerType.BALL_FLY : SoundPlayerType.BALL_FALLING);
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
                this.addInterpolation(position1, position2, speed / 2, SoundPlayerType.BALL_ROLL, SoundPlayerType.BALL_ROLL, 1);
                Vector3 position3 = new Vector3(position2.x + (float)direction[0] * (obstacleSize / 2) - (float)direction[0] * (ballSize / 2) * (Mathf.Sqrt(2) - (float)1), position2.y, position2.z + (float)direction[2] * (obstacleSize / 2) - (float)direction[2] * (ballSize / 2) * (Mathf.Sqrt(2) - (float)1));
                this.addInterpolation(position2, position3, speed / 4);
                positionLast = position3;
                positionIndex = getPositionIndex(positionLast);
                numberDown = 0;
                positionLastTemp = positionLast;
            } else if (
                obstacleManager.checkObstacle(new int[] { positionIndex[0], positionIndex[1] - 1, positionIndex[2] }) == false 
                && obstacleManager.checkDive(new int[] { positionIndex[0], positionIndex[1] - 1, positionIndex[2] }) == false
                && obstacleManager.checkBlockRoll(new int[] { positionIndex[0], positionIndex[1] - 1, positionIndex[2] }) == false
                )
            {
                if (
                    obstacleManager.checkObstacle(new int[] { positionIndex[0], positionIndex[1] - 2, positionIndex[2] }) == false 
                    && obstacleManager.checkDive(new int[] { positionIndex[0], positionIndex[1] - 2, positionIndex[2] }) == false
                    && obstacleManager.checkBlockRoll(new int[] { positionIndex[0], positionIndex[1] - 2, positionIndex[2] }) == false
                    )
                {
                    //this.addInterpolation(positionLast, new Vector3(positionLast.x, positionLast.y - obstacleSize, positionLast.z), speed / 4, SoundPlayerType.BALL_FLY, SoundPlayerType.BALL_FLY);
                    numberDown = numberDown + 1;
                    positionLast = new Vector3(positionLast.x, positionLast.y - obstacleSize, positionLast.z);
                    positionIndex = getPositionIndex(positionLast);
                }
                else
                {
                    numberDown = numberDown + 1;
                    Vector3 position1 = new Vector3(positionLastTemp.x, positionLastTemp.y - obstacleSize * numberDown, positionLastTemp.z);
                    this.addInterpolation(positionLastTemp, position1, (speed / 4) * numberDown, SoundPlayerType.BALL_FLY, numberDown == 1 ? SoundPlayerType.BALL_FLY : SoundPlayerType.BALL_FALLING);
                    Vector3 position2 = new Vector3(position1.x + (float)direction[0] * ((float)1 / 2 - ballSize / 2), position1.y, position1.z + (float)direction[2] * ((float)1 / 2 - ballSize / 2));
                    this.addInterpolation(position1, position2, speed, SoundPlayerType.BALL_ROLL, SoundPlayerType.BALL_ROLL, 1);
                    positionLast = position2;
                    positionIndex = getPositionIndex(positionLast);
                    positionLastTemp = positionLast;
                    numberDown = 0;

                    if (obstacleManager.checkBlockRoll(new int[] { positionIndex[0], positionIndex[1] - 1, positionIndex[2] }) == true)
                    {
                        numberMove = 0;
                        return;
                    }
                }
            }
            else
            {
                return;
            }
        }

        if (numberDown > 0)
        {
            Debug.Log(numberDown);
            Vector3 position1 = new Vector3(positionLastTemp.x, positionLastTemp.y - obstacleSize * numberDown, positionLastTemp.z);
            this.addInterpolation(positionLastTemp, position1, numberDown * (speed / 4), SoundPlayerType.BALL_FLY, SoundPlayerType.BALL_FLY);
            numberDown = 0;
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
                    if (playParticalSystem == false && particleSystemBallMove != null)
                    {
                        Debug.Log("Play Partical System");
                        spawnedParticalSystem.Play();
                        playParticalSystem = true;
                        positionBallLast = transform.position;
                    } else
                    {
                        spawnedParticalSystem.transform.position = transform.position;
                        Vector3 directionParticalSystem = transform.position - positionBallLast;
                        spawnedParticalSystem.transform.rotation = Quaternion.LookRotation(-directionParticalSystem);
                        positionBallLast = transform.position;
                    }
                    if (inter.time == 0)
                    {   
                        PlayLoopSound(this.ListAudio[(int)inter.LoopSound]);
                        //player.transform.DOKill();
                        
                        switch(inter.typeMove)
                        {
                            case 0:
                                player.transform.DOMove(inter.end, inter.duration).SetEase(Ease.Linear);
                                break;
                            case 1:
                                int numberJump = 2;
                                player.transform.DOJump(inter.end, 0.25f, numberJump, inter.duration).SetEase(Ease.Linear);
                                player.transform.DOScale(new Vector3(ballSize / scaleSize, (ballSize * 0.5f) / scaleSize, ballSize / scaleSize), inter.duration / (numberJump * 2))
                                    .SetLoops(numberJump * 2, LoopType.Yoyo)
                                    .SetEase(Ease.InOutSine);
                                break;
                            case 2:
                                numberJump = 1;
                                player.transform.DOJump(inter.end, 0.1f, numberJump, inter.duration).SetEase(Ease.Linear);
                                player.transform.DOScale(new Vector3(ballSize / scaleSize, (ballSize * 0.5f) / scaleSize, ballSize / scaleSize), inter.duration / (numberJump * 2))
                                    .SetLoops(numberJump * 2, LoopType.Yoyo)
                                    .SetEase(Ease.InOutSine);
                                break;
                            default:
                                break;
                        }
                    }
                    inter.time += Time.deltaTime;
                    //player.transform.position = Vector3.Lerp(inter.start, inter.end, inter.time / inter.duration);

                    checkMove = true;
                    if (inter.time >= inter.duration)
                    {
                        StopLoopSound();
                        if (inter.LoopSound != inter.ShotSound)
                        {
                            PlayShotSound(this.ListAudio[(int)inter.ShotSound]);
                        }
                        inter.check = false;
                        

                        if (inter.duration == 0)
                        {
                            Vector3 positionLast = interpolation.Count == 0 ? player.transform.position : interpolation[interpolation.Count - 1].end;
                            int[] positionIndexLast = getPositionIndex(positionLast);

                            if (obstacleManager.checkDive(new int[] { positionIndexLast[0], positionIndexLast[1] - 1, positionIndexLast[2] }))
                            {
                                gameManager.GameOver();
                                return;
                            }
                        }
                    }
                    return;
                }
            }
        }
        if (playParticalSystem == true)
        {
            spawnedParticalSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            playParticalSystem = false;
        }
        checkMove = false;
        checkGameOver();
    }

    public void moveBack()
    {
        bool checkPause = true;
        while (interpolation.Count > 0)
        {
            if (checkPause == false && interpolation.Last().start == interpolation.Last().end && interpolation.Last().duration == 0)
            {
                return;
            }
            if (checkPause == true && interpolation.Last().start == interpolation.Last().end && interpolation.Last().duration == 0)
            {
                checkPause = false;
            }
            StopLoopSound();
            player.transform.position = interpolation.Last().start;
            interpolation.RemoveAt(interpolation.Count - 1);
        }
    }

    void checkGameOver()
    {
        if (obstacleManager.checkGoal(getPositionIndex(player.transform.position)))
        {
            if (stateGame == false)
            {
                Instantiate(particleSystemWinGame, player.transform.position, Quaternion.LookRotation(Vector3.up));
            }
            stateGame = true;
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

        stateGame = true;
        gameManager.GameOver();
    }

    void PlayLoopSound(AudioClip audioClip)
    {
        if (audioClip != null)
        {
            audioLoopSound.clip = audioClip;
            audioLoopSound.Play();
        }
    }

    void StopLoopSound()
    {
        audioLoopSound.Stop();
    }

    void PlayShotSound(AudioClip audioClip)
    {
        if (audioClip != null)
        {
            audioShotSound.PlayOneShot(audioClip);
        }
    }
}
