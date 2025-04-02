using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static LevelManager;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameObject planePrefab;
    [SerializeField] private GameObject changeDirectionPrefab;
    [SerializeField] private GameObject goalPrefab;
    [SerializeField] private GameObject blockRollPrefab;
    [SerializeField] private GameObject divePrefab;
    [SerializeField] private GameObject waterPrefab;
    private GameObject player;
    private GameObject golfBall;
    private float changeDirectionSize = 1;
    public float obstacleSize = 1;
    public float obstacleSizeX = 1;
    public float obstacleSizeY = 1;
    public float obstacleSizeZ = 1;
    public float scaleSize = 0;
    public float ballSize = 0.4f;
    public int highFly = 3;

    // set up obstacle spawn points
    public int[][] spawnObstacles;
    public int[][] spawnPlanes;
    public int[][] spawnChangeDirectionOb;
    public int[][] spawnBlockRoll;
    public int[][] spawnDive;
    public int[][] spawnWater;
    public int[][] spawnGoal;
    public int[][] spawnBall;
    public int[][] hint;

    Color lowColor = new Color(106f / 255, 106f / 255, 106f / 255, 1f);
    Color highColor = new Color(55f / 255, 55f / 255, 55f / 255, 1f);

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").gameObject;
        //golfBall = GameObject.FindWithTag("GolfBall").gameObject;
        obstacleSize = obstaclePrefab.transform.localScale.x;
        changeDirectionSize = obstacleSize / 2;
        Spawn();
    }

    public void Spawn()
    {
        if (spawnObstacles == null) return;
        for (int i = 0; i < this.spawnObstacles.Length; ++i)
        {
            for (int j = 0; j < this.spawnObstacles[i].Length; ++j)
            {
                if (this.spawnObstacles[i][j] > 0)
                {
                    this.SpawnObstacle(i, this.spawnObstacles[i][j], j);
                }
                if (this.spawnPlanes[i][j] > 0)
                {
                    Instantiate(planePrefab, new Vector3(i * obstacleSize, (this.spawnObstacles[i][j]) * obstacleSize, j * obstacleSize), Quaternion.Euler(0, 90 * (this.spawnPlanes[i][j] + 2), 0));
                }
                if (this.spawnChangeDirectionOb[i][j] > 0)
                {
                    int numberDirection = this.spawnChangeDirectionOb[i][j];
                    int changeX = (numberDirection == 4) || (numberDirection == 3) ? 1 : -1;
                    int changeZ = (numberDirection == 3) || (numberDirection == 2) ? 1 : -1;
                    Instantiate(changeDirectionPrefab, new Vector3(i * obstacleSize + (changeDirectionSize / 2) * changeX, (this.spawnObstacles[i][j]) * obstacleSize - changeDirectionSize / 2, j * obstacleSize + (changeDirectionSize / 2) * changeZ), Quaternion.Euler(0, 90 * (this.spawnChangeDirectionOb[i][j] + 2), 0));
                }
                if (this.spawnGoal[i][j] > 0)
                {
                    Instantiate(goalPrefab, new Vector3(i * obstacleSize, (this.spawnObstacles[i][j]) * obstacleSize, j * obstacleSize), Quaternion.identity);
                }
                if (this.spawnBall[i][j] > 0)
                {
                    // Spawn Ball
                    //golfBall.transform.localScale = new Vector3(ballSize, ballSize, ballSize);
                    scaleSize = player.GetComponent<Renderer>().bounds.size.x;
                    player.transform.localScale = new Vector3(ballSize / scaleSize, ballSize / scaleSize, ballSize / scaleSize);
                    player.transform.position = new Vector3(i * obstacleSize, (this.spawnObstacles[i][j]) * obstacleSize - ballSize / 2 - ballSize, j * obstacleSize);
                }
                if (this.spawnBlockRoll != null && spawnBlockRoll.Length > i && spawnBlockRoll[i].Length > j && this.spawnBlockRoll[i][j] > 0)
                {
                    Instantiate(blockRollPrefab, new Vector3(i * obstacleSize, (this.spawnObstacles[i][j]) * obstacleSize, j * obstacleSize), Quaternion.identity);
                }
                if (this.spawnDive != null && spawnDive.Length > i && spawnDive[i].Length > j && this.spawnDive[i][j] > 0)
                {
                    Instantiate(divePrefab, new Vector3(i * obstacleSize, (this.spawnObstacles[i][j]) * obstacleSize, j * obstacleSize), Quaternion.identity);
                }
                if (this.spawnWater != null && spawnWater.Length > i && spawnWater[i].Length > j && this.spawnWater[i][j] > 0)
                {
                    Instantiate(waterPrefab, new Vector3(i * obstacleSize, (this.spawnObstacles[i][j]) * obstacleSize, j * obstacleSize), Quaternion.identity);
                }
            }
        }
    }

    public void SpawnObstacle(int indexX, int high, int indexZ)
    {
        for (int i = 0; i < high; ++i)
        {
            GameObject obstacle = Instantiate(obstaclePrefab, new Vector3(indexX * obstacleSize, i * obstacleSize, indexZ * obstacleSize), Quaternion.identity);

            //if (!checkObstacle(new int[] { indexX, i + 1, indexZ }))
            //{
            //    if (checkObstacle(new int[] { indexX + 1, i + 1, indexZ }) || checkObstacle(new int[] { indexX + 1, i, indexZ }))
            //    {
            //        DrawLine(obstacle, 0, 1, lowColor);
            //    } else
            //    {
            //        DrawLine(obstacle, 0, 1, highColor);
            //    }
            //}



            //if (!checkObstacle(new int[] { indexX, i + 1, indexZ }))
            //{
            //    if (checkObstacle(new int[] { indexX, i + 1, indexZ - 1 }) || checkObstacle(new int[] { indexX, i, indexZ - 1 }))
            //    {
            //        DrawLine(obstacle, 1, 2, lowColor);
            //    } else
            //    {
            //        DrawLine(obstacle, 1, 2, highColor);
            //    }
            //}



            //if (!checkObstacle(new int[] { indexX, i + 1, indexZ }))
            //{
            //    if (checkObstacle(new int[] { indexX - 1, i + 1, indexZ }) || checkObstacle(new int[] { indexX - 1, i, indexZ }))
            //    {
            //        DrawLine(obstacle, 2, 3, lowColor);
            //    } else
            //    {
            //        DrawLine(obstacle, 2, 3, highColor);
            //    }
            //}



            //if (!checkObstacle(new int[] { indexX, i + 1, indexZ }))
            //{
            //    if (checkObstacle(new int[] { indexX, i + 1, indexZ + 1 }) || checkObstacle(new int[] { indexX, i, indexZ + 1 }))
            //    {
            //        DrawLine(obstacle, 3, 0, lowColor);
            //    } else
            //    {
            //        DrawLine(obstacle, 3, 0, highColor);
            //    }
            //}
            HandleDrawLine(0, obstacle, new int[] { indexX, i, indexZ }, new int[] { 1, 1, 0 }, 0, 1);
            HandleDrawLine(0, obstacle, new int[] { indexX, i, indexZ }, new int[] { 0, 1, -1 }, 1, 2);
            HandleDrawLine(0, obstacle, new int[] { indexX, i, indexZ }, new int[] { -1, 1, 0 }, 2, 3);
            HandleDrawLine(0, obstacle, new int[] { indexX, i, indexZ }, new int[] { 0, 1, 1 }, 3, 0);

            HandleDrawLine(0, obstacle, new int[] { indexX, i, indexZ }, new int[] { 1, -1, 0 }, 4, 5);
            HandleDrawLine(0, obstacle, new int[] { indexX, i, indexZ }, new int[] { 0, -1, -1 }, 5, 6);
            HandleDrawLine(0, obstacle, new int[] { indexX, i, indexZ }, new int[] { -1, -1, 0 }, 6, 7);
            HandleDrawLine(0, obstacle, new int[] { indexX, i, indexZ }, new int[] { 0, -1, 1 }, 7, 4);

            //DrawLine(obstacle, 4, 5);
            //DrawLine(obstacle, 5, 6);
            //DrawLine(obstacle, 6, 7);
            //DrawLine(obstacle, 7, 4);

            HandleDrawLine(1, obstacle, new int[] { indexX, i, indexZ }, new int[] { 1, 0, 1 }, 0, 4);
            HandleDrawLine(1, obstacle, new int[] { indexX, i, indexZ }, new int[] { 1, 0, -1 }, 1, 5);
            HandleDrawLine(1, obstacle, new int[] { indexX, i, indexZ }, new int[] { -1, 0, -1 }, 2, 6);
            HandleDrawLine(1, obstacle, new int[] { indexX, i, indexZ }, new int[] { -1, 0, 1 }, 3, 7);

            //DrawLine(obstacle, 0, 4);
            //DrawLine(obstacle, 1, 5);
            //DrawLine(obstacle, 2, 6);
            //DrawLine(obstacle, 3, 7);
        }
    }

    public void HandleDrawLine(int typeCheck, GameObject obstacle, int[] post, int[] indexCheck, int startLine, int endLine)
    {
        if (typeCheck == 0)
        {
            if (!checkObstacle(new int[] { post[0], post[1] + indexCheck[1], post[2] }))
            {
                if (checkObstacle(new int[] { post[0] + indexCheck[0], post[1] + indexCheck[1], post[2] + indexCheck[2] }) || checkObstacle(new int[] { post[0] + indexCheck[0], post[1], post[2] + indexCheck[2] }))
                {
                    DrawLine(obstacle, startLine, endLine, lowColor);
                }
                else
                {
                    DrawLine(obstacle, startLine, endLine, highColor);
                }
            }
        } else if (typeCheck == 1)
        {
            if (
                !checkObstacle(new int[] { post[0] + indexCheck[0], post[1], post[2] })
                && !checkObstacle(new int[] { post[0] + indexCheck[0], post[1], post[2] + indexCheck[2] })
                && !checkObstacle(new int[] { post[0], post[1], post[2] + indexCheck[2] })
                )
            {
                DrawLine(obstacle, startLine, endLine, highColor);
            } else
            {
                DrawLine(obstacle, startLine, endLine, lowColor);
            }
        }
    }

    public void DrawLine(GameObject obstacle, int pointStart, int pointEnd, Color color)
    {
        GameObject lineObject = new GameObject("Line");
        lineObject.transform.SetParent(obstacle.transform);
        LineRenderer line = lineObject.AddComponent<LineRenderer>();
        //line.transform.SetParent(obstacle.transform);
        line.startWidth = 0.02f;
        line.endWidth = 0.02f;
        //line.positionCount = 16;
        //line.loop = true;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = color;
        line.endColor = color;

        Vector3 size = new Vector3(obstacleSizeX, obstacleSizeY, obstacleSizeZ) / 2;

        Vector3[] points = new Vector3[]
        {
                new Vector3(size.x, size.y, size.z),
                new Vector3(size.x, size.y, -size.z),
                new Vector3(-size.x, size.y, -size.z),
                new Vector3(-size.x, size.y, size.z),
                new Vector3(size.x, -size.y, size.z),
                new Vector3(size.x, -size.y, -size.z),
                new Vector3(-size.x, -size.y, -size.z),
                new Vector3(-size.x, -size.y, size.z)
        };

        int[] edges = {
                pointStart, pointEnd
            };

        Vector3[] linePositions = new Vector3[edges.Length];

        for (int j = 0; j < edges.Length; j++)
        {
            float gap = 0.001f;
            linePositions[j] = obstacle.transform.position + points[edges[j]] 
                + new Vector3(points[edges[j]].x > 0 ? gap : -gap, points[edges[j]].y > 0 ? gap : -gap, points[edges[j]].z > 0 ? gap : -gap);
        }

        line.positionCount = linePositions.Length;
        line.SetPositions(linePositions);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool checkObstacle(int[] positionIndex)
    {
        //Debug.Log(positionIndex[0] + " " + positionIndex[1] + " " + positionIndex[2]);
        //Debug.Log(spawnObstacles.Length);
        if (positionIndex[0] >= spawnObstacles.Length || positionIndex[0] < 0)
        {
            return false;
        }
        //Debug.Log(spawnObstacles[positionIndex[0]].Length);
        if (positionIndex[2] >= spawnObstacles[positionIndex[0]].Length || positionIndex[2] < 0)
        {
            return false;
        }
        //Debug.Log(spawnObstacles[positionIndex[0]][positionIndex[2]]);
        if (positionIndex[1] >= spawnObstacles[positionIndex[0]][positionIndex[2]] || positionIndex[1] < 0)
        {
            return false;
        }
        return true;
    }

    public int checkPlane(int[] positionIndex)
    {
        if (positionIndex[0] >= spawnObstacles.Length || positionIndex[0] < 0)
        {
            return 0;
        }
        if (positionIndex[2] >= spawnObstacles[positionIndex[0]].Length || positionIndex[2] < 0)
        {
            return 0;
        }
        if (spawnPlanes[positionIndex[0]][positionIndex[2]] > 0 && spawnObstacles[positionIndex[0]][positionIndex[2]] == positionIndex[1])
        {
            return spawnPlanes[positionIndex[0]][positionIndex[2]];
        }
        return 0;
    }

    public int checkChangeDirection(int[] positionIndex)
    {
        if (positionIndex[0] >= spawnObstacles.Length || positionIndex[0] < 0)
        {
            return 0;
        }
        if (positionIndex[2] >= spawnObstacles[positionIndex[0]].Length || positionIndex[2] < 0)
        {
            return 0;
        }
        if (spawnChangeDirectionOb[positionIndex[0]][positionIndex[2]] > 0 && spawnObstacles[positionIndex[0]][positionIndex[2]] == positionIndex[1])
        {
            return spawnChangeDirectionOb[positionIndex[0]][positionIndex[2]];
        }
        return 0;
    }

    public bool checkGoal(int[] positionIndex)
    {
        if (positionIndex[0] >= spawnObstacles.Length || positionIndex[0] < 0)
        {
            return false;
        }
        if (positionIndex[2] >= spawnObstacles[positionIndex[0]].Length || positionIndex[2] < 0)
        {
            return false;
        }
        if (spawnGoal[positionIndex[0]][positionIndex[2]] > 0 && spawnObstacles[positionIndex[0]][positionIndex[2]] == positionIndex[1])
        {
            return true;
        }
        return false;
    }

    public bool checkBlockRoll(int[] positionIndex)
    {
        if (positionIndex[0] >= spawnObstacles.Length || positionIndex[0] < 0)
        {
            return false;
        }
        if (positionIndex[2] >= spawnObstacles[positionIndex[0]].Length || positionIndex[2] < 0)
        {
            return false;
        }
        if (spawnBlockRoll != null && spawnBlockRoll.Length > positionIndex[0] && spawnBlockRoll[positionIndex[0]].Length > positionIndex[2] && spawnBlockRoll[positionIndex[0]][positionIndex[2]] > 0 && spawnObstacles[positionIndex[0]][positionIndex[2]] == positionIndex[1])
        {
            return true;
        }
        return false;
    }

    public bool checkDive(int[] positionIndex)
    {
        if (positionIndex[0] >= spawnObstacles.Length || positionIndex[0] < 0)
        {
            return false;
        }
        if (positionIndex[2] >= spawnObstacles[positionIndex[0]].Length || positionIndex[2] < 0)
        {
            return false;
        }
        if (spawnDive != null && spawnDive.Length > positionIndex[0] && spawnDive[positionIndex[0]].Length > positionIndex[2] && spawnDive[positionIndex[0]][positionIndex[2]] > 0 && spawnObstacles[positionIndex[0]][positionIndex[2]] == positionIndex[1])
        {
            return true;
        }
        return false;
    }

    public bool checkWater(int[] positionIndex)
    {
        if (positionIndex[0] >= spawnObstacles.Length || positionIndex[0] < 0)
        {
            return false;
        }
        if (positionIndex[2] >= spawnObstacles[positionIndex[0]].Length || positionIndex[2] < 0)
        {
            return false;
        }
        if (spawnWater != null && spawnWater.Length > positionIndex[0] && spawnWater[positionIndex[0]].Length > positionIndex[2] && spawnWater[positionIndex[0]][positionIndex[2]] > 0 && spawnObstacles[positionIndex[0]][positionIndex[2]] == positionIndex[1])
        {
            return true;
        }
        return false;
    }
}