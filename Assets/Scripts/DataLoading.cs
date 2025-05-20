using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static ObstacleManager;

public class DataLoading : MonoBehaviour
{
    [SerializeField] private GameObject cameraMain;
    private Transform transformCamera;
    private ObstacleManager obstacleManager;
    private GameManager gameManager;

    private void Awake()
    {
        Debug.Log("Path level: " + LevelManager.Instance.levelSelected);
        obstacleManager = GameObject.FindGameObjectWithTag("ObstacleManager").gameObject.GetComponent<ObstacleManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").gameObject.GetComponent<GameManager>();
        transformCamera = cameraMain.GetComponent<Transform>();
        LoadData(LevelManager.Instance.listDataLevel[LevelManager.Instance.levelSelected]);
        //Debug.Log(LevelManager.Instance.levelSelected);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadData(SpawnObstacleLevelScriptableObject spawnObstacleLevel)
    {
        //SpawnObstacleLevelScriptableObject spawnObstacleLevel = AssetDatabase.LoadAssetAtPath<SpawnObstacleLevelScriptableObject>($"Assets/GameData/Level/{level}");
        
        if (spawnObstacleLevel == null)
        {
            Debug.Log($"Khong tim thay file");
            return;
        }

        obstacleManager.spawnObstacles = ConvertListStringToArray2(spawnObstacleLevel.spawnObstacles);
        obstacleManager.spawnPlanes = ConvertListStringToArray2(spawnObstacleLevel.spawnPlanes);
        obstacleManager.spawnChangeDirectionOb = ConvertListStringToArray2(spawnObstacleLevel.spawnChangeDirectionOb);
        obstacleManager.spawnGoal = ConvertListStringToArray2(spawnObstacleLevel.spawnGoal);
        obstacleManager.spawnBall = ConvertListStringToArray2(spawnObstacleLevel.spawnBall);
        obstacleManager.spawnJump = ConvertListStringToArray2(spawnObstacleLevel.spawnJump);

        if (spawnObstacleLevel.spawnBlockRoll != null && spawnObstacleLevel.spawnBlockRoll.Count > 0)
        {
            obstacleManager.spawnBlockRoll = ConvertListStringToArray2(spawnObstacleLevel.spawnBlockRoll);
        }

        if (spawnObstacleLevel.spawnDive != null && spawnObstacleLevel.spawnDive.Count > 0)
        {
            obstacleManager.spawnDive = ConvertListStringToArray2(spawnObstacleLevel.spawnDive);
        }

        if (spawnObstacleLevel.spawnWater != null && spawnObstacleLevel.spawnWater.Count > 0)
        {
            obstacleManager.spawnWater = ConvertListStringToArray2(spawnObstacleLevel.spawnWater);
        }

        if (spawnObstacleLevel.spawnJump != null && spawnObstacleLevel.spawnJump.Count > 0)
        {
            obstacleManager.spawnJump = ConvertListStringToArray2(spawnObstacleLevel.spawnJump);
        }

        if (spawnObstacleLevel.spawnPortal != null && spawnObstacleLevel.spawnPortal.Count > 0)
        {
            obstacleManager.spawnPortal = ConvertListStringToArray2(spawnObstacleLevel.spawnPortal);
        }

        if (spawnObstacleLevel.hint != null && spawnObstacleLevel.hint.Count > 0)
        {
            gameManager.hint = ConvertListStringToHint(spawnObstacleLevel.hint);
        }

        Vector3 post = new Vector3(spawnObstacleLevel.positionX, spawnObstacleLevel.positionY, spawnObstacleLevel.positionZ);

        if (post.x != 0 && post.y != 0 && post.z != 0)
        {
            //Debug.Log("Check camera setting");
            cameraMain.GetComponent<CameraMovement>().postCam = post;
        }

        addObstacle();

        int[][] buttons = ConvertListStringToArray2(spawnObstacleLevel.itemMove);

        //string[] parts = level.Split("_", ".");
        //if (parts.Length > 1 && int.TryParse(parts[1], out int numberLevel))
        //{
        //    Debug.Log("Level: " + numberLevel);
        //    if (numberLevel == 32)
        //    {
        //        obstacleManager.highFly = 5;
        //    }
        //} else
        //{
        //    Debug.Log(level + " khong hop le");
        //}

        obstacleManager.highFly = spawnObstacleLevel.highFly == 0 ? 3 : spawnObstacleLevel.highFly;

        for (int i = 0; i < buttons.Length; ++i)
        {
            gameManager.createButton(buttons[i]);
        }
    }

    public void addObstacle()
    {
        int addNumberObstacle = 10;

        for (int i = 0; i < obstacleManager.spawnObstacles.Length; i++)
        {
            for (int j = 0; j < obstacleManager.spawnObstacles[i].Length; j++)
            {
                if (obstacleManager.spawnObstacles[i][j] > 0)
                {
                    obstacleManager.spawnObstacles[i][j] += addNumberObstacle;
                }
            }
        }

        if (obstacleManager.spawnBlockRoll != null)
        {
            for (int i = 0; i < obstacleManager.spawnBlockRoll.Length; i++)
            {
                for (int j = 0; j < obstacleManager.spawnBlockRoll[i].Length; j++)
                {
                    if (obstacleManager.spawnBlockRoll[i][j] > 0)
                    {
                        obstacleManager.spawnBlockRoll[i][j] += addNumberObstacle;
                    }
                }
            }
        }

        if (obstacleManager.spawnDive != null)
        {
            for (int i = 0; i < obstacleManager.spawnDive.Length; i++)
            {
                for (int j = 0; j < obstacleManager.spawnDive[i].Length; j++)
                {
                    if (obstacleManager.spawnDive[i][j] > 0)
                    {
                        obstacleManager.spawnDive[i][j] += addNumberObstacle;
                    }
                }
            }
        }

        if (obstacleManager.spawnWater != null)
        {
            for (int i = 0; i < obstacleManager.spawnWater.Length; i++)
            {
                for (int j = 0; j < obstacleManager.spawnWater[i].Length; j++)
                {
                    if (obstacleManager.spawnWater[i][j] > 0)
                    {
                        obstacleManager.spawnWater[i][j] += addNumberObstacle;
                    }
                }
            }
        }

        if (obstacleManager.spawnJump != null)
        {
            for (int i = 0; i < obstacleManager.spawnJump.Length; ++i)
            {
                for (int j = 0; j < obstacleManager.spawnJump[i].Length; ++j)
                {
                    if (obstacleManager.spawnJump[i][j] > 0)
                    {
                        obstacleManager.spawnJump[i][j] += addNumberObstacle;
                    }
                }
            }
        }

        

        ChangePostCamera(addNumberObstacle);
    }

    public void ChangePostCamera(int numberObstacleAdd)
    {
        Vector3 post = cameraMain.GetComponent<CameraMovement>().postCam;
        post += new Vector3(0, numberObstacleAdd * obstacleManager.obstacleSizeY, 0);
        cameraMain.GetComponent<CameraMovement>().postCam = post;
    }

    public List<string> ConvertArray2ToListString(int[][] list)
    {
        List<string> result = new List<string>();

        foreach (int[] row in list)
        {
            result.Add(string.Join(",", row));
        }

        return result;
    }

    public int[][] ConvertListStringToArray2(List<string> list)
    {
        int[][] result = list
            .Select(row => row.Split(',').Select(int.Parse).ToArray())
            .ToArray();

        return result;
    }

    public List<List<Hint>> ConvertListStringToHint(List<string> list)
    {
        List<List<Hint>> result = new List<List<Hint>>();

        foreach (string strHintItem in list)
        {
            List<Hint> hintItem = new List<Hint>();
            string[] pairs = strHintItem.Trim().Split(',');

            foreach(string pair in pairs)
            {
                string[] parts = pair.Trim().Split('-');
                Hint hint;
                hint.select = int.Parse(parts[0]);
                hint.direct = int.Parse(parts[1]);
                hintItem.Add(hint);
            }
            result.Add(hintItem);
        }

        return result;    
    }
}
