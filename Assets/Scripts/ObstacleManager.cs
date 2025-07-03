using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.ProBuilder;
using static LevelManager;

public enum TypePrefab
{
    Obstacle,
    BlockRoll,
    Dive,
    Water,
    Jump,
    Portal,
    Conveyor,
    Ice,
    Ball,
    Goal,
}

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private EffectSurfaceManager effectSurfaceManager;
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameObject planePrefab;
    [SerializeField] private GameObject changeDirectionPrefab;
    [SerializeField] private GameObject goalPrefab;
    [SerializeField] private GameObject blockRollPrefab;
    [SerializeField] private GameObject divePrefab;
    [SerializeField] private GameObject waterPrefab;
    [SerializeField] private GameObject jumpPrefab;
    [SerializeField] private GameObject conveyorPrefab;
    [SerializeField] private GameObject icePrefab;
    [SerializeField] private GameObject goalGameObjectPrefab;
    private GameObject player;
    private GameObject golfBall;
    private float changeDirectionSize = 1;
    public float obstacleSize = 1;
    public float obstacleSizeX = 1;
    public float obstacleSizeY = 0.5f;
    public float obstacleSizeZ = 1;
    public float scaleSize = 0;
    public float ballSize = 0.4f;
    public int highFly = 3;
    private float scaleBlockJump = 0.5f;
    public float highBlockJump = 0.25f;
    public float timeBlockJump = 0.15f;
    private float scaleGameObjectGoal = 0.5f;

    // set up obstacle spawn points
    public ObstacleColume[][] spawnObstacles;
    public int[][] spawnPlanes;
    public int[][] spawnChangeDirectionOb;
    public int[][] spawnBlockRoll;
    public int[][] spawnDive;
    public int[][] spawnWater;
    public int[][] spawnGoal;
    public int[][] spawnBall;
    public int[][] spawnJump;
    public int[][] spawnPortal;
    public int[][] spawnConveyor;
    public int[][] spawnIce;
    public int[][] hint;
    public GameObject[][] spawnGameObjectJump;

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

        this.spawnGameObjectJump = new GameObject[this.spawnObstacles.Length][];
        this.effectSurfaceManager.effectGameObjectJump = new GameObject[this.spawnObstacles.Length][];
        for (int k = 0; k < this.spawnGameObjectJump.Length; ++k)
        {
            this.spawnGameObjectJump[k] = new GameObject[this.spawnObstacles[k].Length];
            this.effectSurfaceManager.effectGameObjectJump[k] = new GameObject[this.spawnObstacles[k].Length];
        }

        for (int i = 0; i < this.spawnObstacles.Length; ++i)
        {
            for (int j = 0; j < this.spawnObstacles[i].Length; ++j)
            {
                if (this.spawnObstacles[i][j].number > 0)
                {
                    switch (this.spawnObstacles[i][j].type)
                    {
                        case TypePrefab.Obstacle:
                            this.Spawn(i, this.spawnObstacles[i][j].number, j, TypePrefab.Obstacle);
                            break;
                        case TypePrefab.Jump:
                            this.Spawn(i, this.spawnObstacles[i][j].number, j, TypePrefab.Jump);
                            this.SpawnJump(i, this.spawnObstacles[i][j].number - 1, j);
                            effectSurfaceManager.SpawnEffectSurface(SurfaceType.Jump, new int[] { i, this.spawnObstacles[i][j].number, j });
                            break;
                        case TypePrefab.BlockRoll:
                            this.Spawn(i, this.spawnObstacles[i][j].number, j, TypePrefab.BlockRoll);
                            effectSurfaceManager.SpawnEffectSurface(SurfaceType.Wood, new int[] { i, this.spawnObstacles[i][j].number, j });
                            break;
                        case TypePrefab.Dive:
                            this.Spawn(i, this.spawnObstacles[i][j].number, j, TypePrefab.Dive);
                            effectSurfaceManager.SpawnEffectSurface(SurfaceType.QuickSand, new int[] { i, this.spawnObstacles[i][j].number, j });
                            break;
                        case TypePrefab.Water:
                            this.Spawn(i, this.spawnObstacles[i][j].number, j, TypePrefab.Water);
                            effectSurfaceManager.SpawnEffectSurface(SurfaceType.Water, new int[] { i, this.spawnObstacles[i][j].number, j });
                            break;
                        case TypePrefab.Ice:
                            effectSurfaceManager.SpawnEffectSurface(SurfaceType.Ice, new int[] { i, this.spawnObstacles[i][j].number, j });
                            this.Spawn(i, this.spawnObstacles[i][j].number, j, TypePrefab.Ice);
                            break;
                        case TypePrefab.Ball:
                            scaleSize = player.GetComponent<Renderer>().bounds.size.x;
                            player.transform.localScale = new Vector3(ballSize / scaleSize, ballSize / scaleSize, ballSize / scaleSize);
                            player.transform.position = new Vector3(i * obstacleSize, (this.spawnObstacles[i][j].number) * obstacleSizeY - obstacleSizeY / 2 + ballSize / 2, j * obstacleSize);
                            this.Spawn(i, this.spawnObstacles[i][j].number, j, TypePrefab.Obstacle);
                            break;
                        case TypePrefab.Goal:
                            effectSurfaceManager.SpawnEffectSurface(SurfaceType.Portal, new int[] { i, this.spawnObstacles[i][j].number, j }, 0);
                            GameObject goalGameObject = Instantiate(goalGameObjectPrefab, new Vector3(i * obstacleSize, (this.spawnObstacles[i][j].number - 1) * obstacleSizeY, j * obstacleSize), Quaternion.identity);
                            goalGameObject.transform.localScale = new Vector3(scaleGameObjectGoal, scaleGameObjectGoal, scaleGameObjectGoal);
                            this.Spawn(i, this.spawnObstacles[i][j].number, j, TypePrefab.Obstacle);
                            break;
                        //default:
                        //    this.Spawn(i, this.spawnObstacles[i][j].number, j, TypePrefab.Obstacle);
                        //    break;
                    }
                }
                if (this.spawnPlanes != null && this.spawnPlanes[i][j] > 0)
                {
                    GameObject planeObject = Instantiate(planePrefab, new Vector3(i * obstacleSize, (this.spawnObstacles[i][j].number) * obstacleSizeY, j * obstacleSize), Quaternion.Euler(0, 90 * (this.spawnPlanes[i][j] + 2), 0));
                    planeObject.transform.localScale = new Vector3(1, obstacleSizeY, 1);
                }
                if (this.spawnChangeDirectionOb != null && this.spawnChangeDirectionOb[i][j] > 0)
                {
                    int numberDirection = this.spawnChangeDirectionOb[i][j];
                    int changeX = (numberDirection == 4) || (numberDirection == 3) ? 1 : -1;
                    int changeZ = (numberDirection == 3) || (numberDirection == 2) ? 1 : -1;
                    GameObject changeDirectionObject = Instantiate(changeDirectionPrefab, new Vector3(i * obstacleSize + (changeDirectionSize / 2) * changeX, (this.spawnObstacles[i][j].number) * obstacleSizeY - obstacleSizeY / 2 + 0.5f * obstacleSizeY / 2, j * obstacleSize + (changeDirectionSize / 2) * changeZ), Quaternion.Euler(0, 90 * (this.spawnChangeDirectionOb[i][j] + 2), 0));
                    changeDirectionObject.transform.localScale = new Vector3(0.5f, 0.5f * obstacleSizeY, 0.5f);
                }
                //if (this.spawnGoal[i][j] > 0)
                //{
                //    //Instantiate(goalPrefab, new Vector3(i * obstacleSize, (this.spawnObstacles[i][j]) * obstacleSizeY, j * obstacleSize), Quaternion.identity);
                //    effectSurfaceManager.SpawnEffectSurface(SurfaceType.Portal, new int[] { i, this.spawnObstacles[i][j].number, j }, 0);
                //    GameObject goalGameObject = Instantiate(goalGameObjectPrefab, new Vector3(i * obstacleSize, (this.spawnObstacles[i][j].number - 1) * obstacleSizeY, j * obstacleSize), Quaternion.identity);
                //    goalGameObject.transform.localScale = new Vector3(scaleGameObjectGoal, scaleGameObjectGoal, scaleGameObjectGoal);
                //}
                //if (this.spawnBall[i][j] > 0)
                //{
                //    // Spawn Ball
                //    //golfBall.transform.localScale = new Vector3(ballSize, ballSize, ballSize);
                //    scaleSize = player.GetComponent<Renderer>().bounds.size.x;
                //    player.transform.localScale = new Vector3(ballSize / scaleSize, ballSize / scaleSize, ballSize / scaleSize);
                //    player.transform.position = new Vector3(i * obstacleSize, (this.spawnObstacles[i][j].number) * obstacleSizeY - obstacleSizeY / 2 + ballSize / 2, j * obstacleSize);
                //}
                //if (this.spawnBlockRoll != null && spawnBlockRoll.Length > i && spawnBlockRoll[i].Length > j && this.spawnBlockRoll[i][j] > 0)
                //{
                //    //Instantiate(blockRollPrefab, new Vector3(i * obstacleSize, (this.spawnObstacles[i][j]) * obstacleSize, j * obstacleSize), Quaternion.identity);
                //    this.Spawn(i, this.spawnObstacles[i][j], j, 1);
                //}
                //if (this.spawnDive != null && spawnDive.Length > i && spawnDive[i].Length > j && this.spawnDive[i][j] > 0)
                //{
                //    Instantiate(divePrefab, new Vector3(i * obstacleSize, (this.spawnObstacles[i][j]) * obstacleSize, j * obstacleSize), Quaternion.identity);
                //}
                //if (this.spawnWater != null && spawnWater.Length > i && spawnWater[i].Length > j && this.spawnWater[i][j] > 0)
                //{
                //    Instantiate(waterPrefab, new Vector3(i * obstacleSize, (this.spawnObstacles[i][j]) * obstacleSize, j * obstacleSize), Quaternion.identity);
                //}
            }
        }

        //if (this.spawnBlockRoll != null)
        //{
        //    for (int i = 0; i < this.spawnBlockRoll.Length; ++i)
        //    {
        //        for (int j = 0; j < this.spawnBlockRoll[i].Length; ++j)
        //        {
        //            if (this.spawnBlockRoll != null && spawnBlockRoll.Length > i && spawnBlockRoll[i].Length > j && this.spawnBlockRoll[i][j] > 0)
        //            {
        //                //Instantiate(blockRollPrefab, new Vector3(i * obstacleSize, (this.spawnObstacles[i][j]) * obstacleSize, j * obstacleSize), Quaternion.identity);
        //                this.Spawn(i, this.spawnBlockRoll[i][j], j, TypePrefab.BlockRoll);
        //                effectSurfaceManager.SpawnEffectSurface(SurfaceType.Wood, new int[] { i, this.spawnBlockRoll[i][j], j });
        //            }
        //        }
        //    }
        //}

        //if (this.spawnDive != null)
        //{
        //    for (int i = 0; i < this.spawnDive.Length; ++i)
        //    {
        //        for (int j = 0; j < this.spawnDive[i].Length; ++j)
        //        {
        //            if (this.spawnDive != null && spawnDive.Length > i && spawnDive[i].Length > j && this.spawnDive[i][j] > 0)
        //            {
        //                //Instantiate(blockRollPrefab, new Vector3(i * obstacleSize, (this.spawnObstacles[i][j]) * obstacleSize, j * obstacleSize), Quaternion.identity);
        //                this.Spawn(i, this.spawnDive[i][j], j, TypePrefab.Dive);
        //                effectSurfaceManager.SpawnEffectSurface(SurfaceType.QuickSand, new int[] { i, this.spawnDive[i][j], j });
        //            }
        //        }
        //    }
        //}

        //if (this.spawnWater != null)
        //{
        //    for (int i = 0; i < this.spawnWater.Length; ++i)
        //    {
        //        for (int j = 0; j < this.spawnWater[i].Length; ++j)
        //        {
        //            if (this.spawnWater != null && spawnWater.Length > i && spawnWater[i].Length > j && this.spawnWater[i][j] > 0)
        //            {
        //                //Instantiate(blockRollPrefab, new Vector3(i * obstacleSize, (this.spawnObstacles[i][j]) * obstacleSize, j * obstacleSize), Quaternion.identity);
        //                this.Spawn(i, this.spawnWater[i][j], j, TypePrefab.Water);
        //                effectSurfaceManager.SpawnEffectSurface(SurfaceType.Water, new int[] { i, this.spawnWater[i][j], j });
        //            }
        //        }
        //    }
        //}

        //if (this.spawnJump != null)
        //{
        //    this.spawnGameObjectJump = new GameObject[this.spawnJump.Length][];
        //    this.effectSurfaceManager.effectGameObjectJump = new GameObject[this.spawnJump.Length][];
        //    for (int i = 0; i < this.spawnGameObjectJump.Length; ++i)
        //    {
        //        this.spawnGameObjectJump[i] = new GameObject[this.spawnJump[i].Length];
        //        this.effectSurfaceManager.effectGameObjectJump[i] = new GameObject[this.spawnJump[i].Length];
        //    }
        //    for (int i = 0; i < this.spawnJump.Length; ++i)
        //    {
        //        for (int j = 0; j < this.spawnJump[i].Length; ++j)
        //        {
        //            if (this.spawnJump != null && spawnJump.Length > i && spawnJump[i].Length > j && this.spawnJump[i][j] > 0)
        //            {
        //                this.Spawn(i, this.spawnJump[i][j], j, TypePrefab.Jump);
        //                this.SpawnJump(i, this.spawnJump[i][j] - 1, j);
        //                effectSurfaceManager.SpawnEffectSurface(SurfaceType.Jump, new int[] { i, this.spawnJump[i][j], j });
        //            }
        //        }
        //    }
        //}

        //if (this.spawnIce != null)
        //{
        //    for (int i = 0; i < this.spawnIce.Length; ++i)
        //    {
        //        for (int j = 0; j < this.spawnIce[i].Length; ++j)
        //        {
        //            if (this.spawnIce[i][j] > 0)
        //            {
        //                //this.Spawn(i, this.spawnIce[i][j], j, TypePrefab.Ice);
        //                effectSurfaceManager.SpawnEffectSurface(SurfaceType.Ice, new int[] { i, this.spawnObstacles[i][j].number, j });
        //            }
        //        }
        //    }
        //}

        if (this.spawnPortal != null)
        {
            for (int i = 0; i < this.spawnPortal.Length; ++i)
            {
                for (int j = 0; j < this.spawnPortal[i].Length; ++j)
                {
                    if (this.spawnPortal[i][j] > 0)
                    {
                        //Debug.Log("Check Spawn Portal");
                        effectSurfaceManager.SpawnEffectSurface(SurfaceType.Portal, new int[] { i, this.spawnObstacles[i][j].number, j }, this.spawnPortal[i][j]);
                    }
                }
            }
        }

        if (this.spawnConveyor != null)
        {
            for (int i = 0; i < this.spawnConveyor.Length; ++i)
            {
                for (int j = 0; j < this.spawnConveyor[i].Length; ++j)
                {
                    if (this.spawnConveyor[i][j] > 0)
                    {
                        effectSurfaceManager.SpawnEffectSurface(SurfaceType.Conveyor_Tile, new int[] { i, this.spawnObstacles[i][j].number, j }, this.spawnConveyor[i][j]);
                    }
                }
            }
        }
    }

    public void SpawnJump(int indexX, int high, int indexZ)
    {
        GameObject obstacle = Instantiate(jumpPrefab, new Vector3(indexX * obstacleSize, high * obstacleSizeY, indexZ * obstacleSize), Quaternion.identity);
        obstacle.transform.localScale = new Vector3(obstacleSize * scaleBlockJump, obstacleSizeY, obstacleSize * scaleBlockJump);

        spawnGameObjectJump[indexX][indexZ] = obstacle;

        //Debug.Log(high * obstacleSizeY + " " + spawnGameObjectJump[indexX][indexZ].transform.position.y);

        //Transform transObstacle = spawnGameObjectJump[indexX][indexZ].transform;

        //spawnGameObjectJump[indexX][indexZ].transform.DOMoveY(transObstacle.position.y + 0.25f, 0.2f)
        //    .SetLoops(-1, LoopType.Yoyo)
        //    .SetEase(Ease.InOutSine);

        //HandleDrawLine(0, TypePrefab.Jump, obstacle, new int[] { indexX, high + 2, indexZ }, new int[] { 1, 1, 0 }, 0, 1);
        //HandleDrawLine(0, typeObject, obstacle, new int[] { indexX, i, indexZ }, new int[] { 0, 1, -1 }, 1, 2);
        //HandleDrawLine(0, typeObject, obstacle, new int[] { indexX, i, indexZ }, new int[] { -1, 1, 0 }, 2, 3);
        //HandleDrawLine(0, typeObject, obstacle, new int[] { indexX, i, indexZ }, new int[] { 0, 1, 1 }, 3, 0);

        //HandleDrawLine(0, typeObject, obstacle, new int[] { indexX, i, indexZ }, new int[] { 1, -1, 0 }, 4, 5);
        //HandleDrawLine(0, typeObject, obstacle, new int[] { indexX, i, indexZ }, new int[] { 0, -1, -1 }, 5, 6);
        //HandleDrawLine(0, typeObject, obstacle, new int[] { indexX, i, indexZ }, new int[] { -1, -1, 0 }, 6, 7);
        //HandleDrawLine(0, typeObject, obstacle, new int[] { indexX, i, indexZ }, new int[] { 0, -1, 1 }, 7, 4);

        //HandleDrawLine(1, typeObject, obstacle, new int[] { indexX, i, indexZ }, new int[] { 1, 0, 1 }, 0, 4);
        //HandleDrawLine(1, typeObject, obstacle, new int[] { indexX, i, indexZ }, new int[] { 1, 0, -1 }, 1, 5);
        //HandleDrawLine(1, typeObject, obstacle, new int[] { indexX, i, indexZ }, new int[] { -1, 0, -1 }, 2, 6);
        //HandleDrawLine(1, typeObject, obstacle, new int[] { indexX, i, indexZ }, new int[] { -1, 0, 1 }, 3, 7);
    }

    public void obstacleEffectOfJump(int indexX, int indexZ)
    {
        Transform transObstacle = spawnGameObjectJump[indexX][indexZ].transform;
        spawnGameObjectJump[indexX][indexZ].transform.DOMoveY(transObstacle.position.y + highBlockJump, timeBlockJump)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.InBounce);
    }

    public void Spawn(int indexX, int high, int indexZ, TypePrefab type)
    {
        switch(type)
        {
            case TypePrefab.Obstacle:
                SpawnObstacle(indexX, high, indexZ, obstaclePrefab, type);
                break;
            case TypePrefab.BlockRoll:
                SpawnObstacle(indexX, high, indexZ, blockRollPrefab, type);
                break;
            case TypePrefab.Dive:
                SpawnObstacle(indexX, high, indexZ, divePrefab, type);
                break;
            case TypePrefab.Water:
                SpawnObstacle(indexX, high, indexZ, waterPrefab, type);
                break;
            case TypePrefab.Jump:
                SpawnObstacle(indexX, high, indexZ, jumpPrefab, type);
                break;
            case TypePrefab.Ice:
                SpawnObstacle(indexX, high, indexZ, icePrefab, type);
                break;
            default:
                break;
        }
    }

    public void SpawnObstacle(int indexX, int high, int indexZ, GameObject gameObject, TypePrefab typeObject)
    {
        for (int i = 0; i < high; ++i)
        {
            GameObject obstacle = Instantiate(gameObject, new Vector3(indexX * obstacleSize, i * obstacleSizeY, indexZ * obstacleSize), Quaternion.identity);
            obstacle.transform.localScale = new Vector3(1, obstacleSizeY, 1);
            //GameObject obstacle = new GameObject("Obstacle");
            //obstacle.transform.position = new Vector3(indexX * obstacleSize, i * obstacleSizeY, indexZ * obstacleSize);

            //Material materialObstacle = obstacle.gameObject.GetComponent<Renderer>().material;
            //materialObstacle.SetFloat("_Mode", 3); // 3 = Transparent
            //materialObstacle.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            //materialObstacle.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            //materialObstacle.SetInt("_ZWrite", 0);
            //materialObstacle.DisableKeyword("_ALPHATEST_ON");
            //materialObstacle.EnableKeyword("_ALPHABLEND_ON");
            //materialObstacle.DisableKeyword("_ALPHAPREMULTIPLY_ON")   ;
            //materialObstacle.renderQueue = 3000;

            //Color colorObstacle = materialObstacle.color;
            //colorObstacle.a = ((float)(i + 10) / (high - 1)); // opacity = 50%
            //materialObstacle.color = colorObstacle;

            HandleDrawLine(0, typeObject, obstacle, new int[] { indexX, i, indexZ }, new int[] { 1, 1, 0 }, 0, 1);
            HandleDrawLine(0, typeObject, obstacle, new int[] { indexX, i, indexZ }, new int[] { 0, 1, -1 }, 1, 2);
            HandleDrawLine(0, typeObject, obstacle, new int[] { indexX, i, indexZ }, new int[] { -1, 1, 0 }, 2, 3);
            HandleDrawLine(0, typeObject, obstacle, new int[] { indexX, i, indexZ }, new int[] { 0, 1, 1 }, 3, 0);

            HandleDrawLine(0, typeObject, obstacle, new int[] { indexX, i, indexZ }, new int[] { 1, -1, 0 }, 4, 5);
            HandleDrawLine(0, typeObject, obstacle, new int[] { indexX, i, indexZ }, new int[] { 0, -1, -1 }, 5, 6);
            HandleDrawLine(0, typeObject, obstacle, new int[] { indexX, i, indexZ }, new int[] { -1, -1, 0 }, 6, 7);
            HandleDrawLine(0, typeObject, obstacle, new int[] { indexX, i, indexZ }, new int[] { 0, -1, 1 }, 7, 4);

            HandleDrawLine(1, typeObject, obstacle, new int[] { indexX, i, indexZ }, new int[] { 1, 0, 1 }, 0, 4);
            HandleDrawLine(1, typeObject, obstacle, new int[] { indexX, i, indexZ }, new int[] { 1, 0, -1 }, 1, 5);
            HandleDrawLine(1, typeObject, obstacle, new int[] { indexX, i, indexZ }, new int[] { -1, 0, -1 }, 2, 6);
            HandleDrawLine(1, typeObject, obstacle, new int[] { indexX, i, indexZ }, new int[] { -1, 0, 1 }, 3, 7);
        }
    }

    public void SpawnObstacleItem()
    {

    }

    public void HandleDrawLine(int typeCheck, TypePrefab typeObject, GameObject obstacle, int[] post, int[] indexCheck, int startLine, int endLine)
    {
        int[][] spawnArray = new int[this.spawnObstacles.Length][];

        for (int i = 0; i < spawnObstacles.Length; i++)
        {
            List<int> temp = new List<int>();

            foreach (ObstacleColume col in spawnObstacles[i])
            {
                if (col.type == typeObject 
                    || (typeObject == TypePrefab.Obstacle && (col.type == TypePrefab.Goal || col.type == TypePrefab.Ball)))
                {
                    temp.Add(col.number);
                }
                else
                {
                    temp.Add(0);
                }
            }

            spawnArray[i] = temp.ToArray();
        }

        switch (typeObject)
        {
            case TypePrefab.Obstacle:
                break;
            case TypePrefab.BlockRoll:
                break;
            case TypePrefab.Dive:
                break;
            case TypePrefab.Water:
                break;
            case TypePrefab.Jump:
                break;
            case TypePrefab.Ice:
                break;
            default:
                break;
        }

        if (typeCheck == 0)
        {
            if (!checkObject(new int[] { post[0], post[1] + indexCheck[1], post[2] }, spawnArray))
            {
                if (checkObject(new int[] { post[0] + indexCheck[0], post[1] + indexCheck[1], post[2] + indexCheck[2] }, spawnArray) || checkObject(new int[] { post[0] + indexCheck[0], post[1], post[2] + indexCheck[2] }, spawnArray))
                {
                    DrawLine(obstacle, startLine, endLine, lowColor);
                }
                else
                {
                    DrawLine(obstacle, startLine, endLine, highColor);
                }
            }
        }
        else if (typeCheck == 1)
        {
            if (
                !checkObject(new int[] { post[0] + indexCheck[0], post[1], post[2] }, spawnArray)
                && !checkObject(new int[] { post[0] + indexCheck[0], post[1], post[2] + indexCheck[2] }, spawnArray)
                && !checkObject(new int[] { post[0], post[1], post[2] + indexCheck[2] }, spawnArray)
                )
            {
                DrawLine(obstacle, startLine, endLine, highColor);
            }
            else
            {
                DrawLine(obstacle, startLine, endLine, lowColor);
            }
        }
    }

    public bool checkObject(int[] positionIndex, int[][] spawnObject)
    {
        //Debug.Log(positionIndex[0] + " " + positionIndex[1] + " " + positionIndex[2]);
        //Debug.Log(spawnObstacles.Length);
        if (positionIndex[0] >= spawnObject.Length || positionIndex[0] < 0)
        {
            return false;
        }
        //Debug.Log(spawnObstacles[positionIndex[0]].Length);
        if (positionIndex[2] >= spawnObject[positionIndex[0]].Length || positionIndex[2] < 0)
        {
            return false;
        }
        //Debug.Log(spawnObstacles[positionIndex[0]][positionIndex[2]]);
        if (positionIndex[1] >= spawnObject[positionIndex[0]][positionIndex[2]] || positionIndex[1] < 0)
        {
            return false;
        }
        return true;
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
        //Debug.Log(positionIndex[0]);
        //Debug.Log(positionIndex[2]);
        //Debug.Log(spawnObstacles[positionIndex[0]].Length);
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
        if (((spawnObstacles[positionIndex[0]][positionIndex[2]].type != TypePrefab.Obstacle) 
            && (spawnObstacles[positionIndex[0]][positionIndex[2]].type != TypePrefab.Goal)
            && (spawnObstacles[positionIndex[0]][positionIndex[2]].type != TypePrefab.Ball))
            || positionIndex[1] >= spawnObstacles[positionIndex[0]][positionIndex[2]].number 
            || positionIndex[1] < 0)
        {
            return false;
        }
        return true;
    }

    public int checkPlane(int[] positionIndex)
    {
        if (spawnPlanes == null)
        {
            return 0;
        }
        if (positionIndex[0] >= spawnObstacles.Length || positionIndex[0] < 0)
        {
            return 0;
        }
        if (positionIndex[2] >= spawnObstacles[positionIndex[0]].Length || positionIndex[2] < 0)
        {
            return 0;
        }
        if (spawnPlanes[positionIndex[0]][positionIndex[2]] > 0 && spawnObstacles[positionIndex[0]][positionIndex[2]].number == positionIndex[1])
        {
            return spawnPlanes[positionIndex[0]][positionIndex[2]];
        }
        return 0;
    }

    public int checkPortal(int[] positionIndex)
    {
        if (spawnPortal == null)
        {
            return 0;
        }
        if (positionIndex[0] >= spawnObstacles.Length || positionIndex[0] < 0)
        {
            return 0;
        }
        if (positionIndex[2] >= spawnObstacles[positionIndex[0]].Length || positionIndex[2] < 0)
        {
            return 0;
        }
        if (spawnPortal[positionIndex[0]][positionIndex[2]] > 0 && spawnObstacles[positionIndex[0]][positionIndex[2]].number == positionIndex[1])
        {
            return spawnPortal[positionIndex[0]][positionIndex[2]];
        }
        return 0;
    }

    public int checkConveyor(int[] positionIndex)
    {
        if (spawnConveyor == null)
        {
            return 0;
        }
        if (positionIndex[0] >= spawnObstacles.Length || positionIndex[0] < 0)
        {
            return 0;
        }
        if (positionIndex[2] >= spawnObstacles[positionIndex[0]].Length || positionIndex[2] < 0)
        {
            return 0;
        }
        if (spawnConveyor[positionIndex[0]][positionIndex[2]] > 0 && spawnObstacles[positionIndex[0]][positionIndex[2]].number == positionIndex[1])
        {
            return spawnConveyor[positionIndex[0]][positionIndex[2]];
        }
        return 0;
    }

    public Vector3 tele(int number, int indexX, int indexZ)
    {
        for (int i = 0; i < this.spawnPortal.Length; ++i)
        {
            for (int j = 0; j < this.spawnPortal[i].Length; ++j)
            {
                if (this.spawnPortal[i][j] == number && (i != indexX || j != indexZ))
                {
                    return new Vector3(i * obstacleSize, (this.spawnObstacles[i][j].number) * obstacleSizeY - obstacleSizeY / 2 + ballSize / 2, j * obstacleSize);
                }
            }
        }

        return new Vector3(-1, -1, -1);
    }

    public int checkChangeDirection(int[] positionIndex)
    {
        if (spawnChangeDirectionOb == null)
        {
            return 0;
        }
        if (positionIndex[0] >= spawnObstacles.Length || positionIndex[0] < 0)
        {
            return 0;
        }
        if (positionIndex[2] >= spawnObstacles[positionIndex[0]].Length || positionIndex[2] < 0)
        {
            return 0;
        }
        if (spawnChangeDirectionOb[positionIndex[0]][positionIndex[2]] > 0 && spawnObstacles[positionIndex[0]][positionIndex[2]].number == positionIndex[1])
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
        if (spawnObstacles[positionIndex[0]][positionIndex[2]].type == TypePrefab.Goal && spawnObstacles[positionIndex[0]][positionIndex[2]].number > 0 && spawnObstacles[positionIndex[0]][positionIndex[2]].number == positionIndex[1])
        {
            return true;
        }
        return false;
    }

    public bool checkBlockRoll(int[] positionIndex)
    {
        //if (positionIndex[0] >= spawnObstacles.Length || positionIndex[0] < 0)
        //{
        //    return false;
        //}
        //if (positionIndex[2] >= spawnObstacles[positionIndex[0]].Length || positionIndex[2] < 0)
        //{
        //    return false;
        //}
        //if (spawnBlockRoll != null && spawnBlockRoll.Length > positionIndex[0] && spawnBlockRoll[positionIndex[0]].Length > positionIndex[2] && spawnBlockRoll[positionIndex[0]][positionIndex[2]] > 0 && spawnObstacles[positionIndex[0]][positionIndex[2]] == positionIndex[1])
        //{
        //    return true;
        //}
        //return false;

        if (positionIndex[0] >= spawnObstacles.Length || positionIndex[0] < 0)
        {
            return false;
        }
        if (positionIndex[2] >= spawnObstacles[positionIndex[0]].Length || positionIndex[2] < 0)
        {
            return false;
        }
        if (spawnObstacles[positionIndex[0]][positionIndex[2]].type != TypePrefab.BlockRoll || positionIndex[1] >= spawnObstacles[positionIndex[0]][positionIndex[2]].number || positionIndex[1] < 0)
        {
            return false;
        }
        return true;
    }

    public bool checkDive(int[] positionIndex)
    {
        //if (positionIndex[0] >= spawnObstacles.Length || positionIndex[0] < 0)
        //{
        //    return false;
        //}
        //if (positionIndex[2] >= spawnObstacles[positionIndex[0]].Length || positionIndex[2] < 0)
        //{
        //    return false;
        //}
        //if (spawnDive != null && spawnDive.Length > positionIndex[0] && spawnDive[positionIndex[0]].Length > positionIndex[2] && spawnDive[positionIndex[0]][positionIndex[2]] > 0 && spawnObstacles[positionIndex[0]][positionIndex[2]] == positionIndex[1])
        //{
        //    return true;
        //}
        //return false;

        if (positionIndex[0] >= spawnObstacles.Length || positionIndex[0] < 0)
        {
            return false;
        }
        if (positionIndex[2] >= spawnObstacles[positionIndex[0]].Length || positionIndex[2] < 0)
        {
            return false;
        }
        if (spawnObstacles[positionIndex[0]][positionIndex[2]].type != TypePrefab.Dive || positionIndex[1] >= spawnObstacles[positionIndex[0]][positionIndex[2]].number || positionIndex[1] < 0)
        {
            return false;
        }
        return true;
    }

    public bool checkWater(int[] positionIndex)
    {
        //if (positionIndex[0] >= spawnObstacles.Length || positionIndex[0] < 0)
        //{
        //    return false;
        //}
        //if (positionIndex[2] >= spawnObstacles[positionIndex[0]].Length || positionIndex[2] < 0)
        //{
        //    return false;
        //}
        //if (spawnWater != null && spawnWater.Length > positionIndex[0] && spawnWater[positionIndex[0]].Length > positionIndex[2] && spawnWater[positionIndex[0]][positionIndex[2]] > 0 && spawnObstacles[positionIndex[0]][positionIndex[2]] == positionIndex[1])
        //{
        //    return true;
        //}
        //return false;

        if (positionIndex[0] >= spawnObstacles.Length || positionIndex[0] < 0)
        {
            return false;
        }
        if (positionIndex[2] >= spawnObstacles[positionIndex[0]].Length || positionIndex[2] < 0)
        {
            return false;
        }
        if (spawnObstacles[positionIndex[0]][positionIndex[2]].type != TypePrefab.Water || positionIndex[1] >= spawnObstacles[positionIndex[0]][positionIndex[2]].number || positionIndex[1] < 0)
        {
            return false;
        }
        return true;
    }

    public bool checkJump(int[] positionIndex)
    {
        if (positionIndex[0] >= spawnObstacles.Length || positionIndex[0] < 0)
        {
            return false;
        }
        if (positionIndex[2] >= spawnObstacles[positionIndex[0]].Length || positionIndex[2] < 0)
        {
            return false;
        }
        if (spawnObstacles[positionIndex[0]][positionIndex[2]].type != TypePrefab.Jump || positionIndex[1] >= spawnObstacles[positionIndex[0]][positionIndex[2]].number || positionIndex[1] < 0)
        {
            return false;
        }
        return true;
    }

    public bool checkIce(int[] positionIndex)
    {
        //if (spawnIce == null) return false;

        //if (positionIndex[0] >= spawnIce.Length || positionIndex[0] < 0)
        //{
        //    return false;
        //}
        //if (positionIndex[2] >= spawnIce[positionIndex[0]].Length || positionIndex[2] < 0)
        //{
        //    return false;
        //}
        //if (positionIndex[1] >= spawnIce[positionIndex[0]][positionIndex[2]] || positionIndex[1] < 0)
        //{
        //    return false;
        //}
        //return true;

        if (positionIndex[0] >= spawnObstacles.Length || positionIndex[0] < 0)
        {
            return false;
        }
        if (positionIndex[2] >= spawnObstacles[positionIndex[0]].Length || positionIndex[2] < 0)
        {
            return false;
        }
        if (spawnObstacles[positionIndex[0]][positionIndex[2]].type != TypePrefab.Ice || positionIndex[1] >= spawnObstacles[positionIndex[0]][positionIndex[2]].number || positionIndex[1] < 0)
        {
            return false;
        }
        return true;
    }
}