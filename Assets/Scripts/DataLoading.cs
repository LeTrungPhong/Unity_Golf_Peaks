using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static ObstacleManager;

public class DataLoading : MonoBehaviour
{
    private ObstacleManager obstacleManager;
    private GameManager gameManager;
    private void Awake()
    {
        Debug.Log("Path level: " + LevelManager.Instance.levelSelected);
        obstacleManager = GameObject.Find("ObstacleManager").gameObject.GetComponent<ObstacleManager>();
        gameManager = GameObject.Find("GameManager").gameObject.GetComponent<GameManager>();
        LoadData(LevelManager.Instance.listPathLevelData[LevelManager.Instance.levelSelected]);
    }
    // Start is called before the first frame update
    void Start()
    {
        //SaveData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveData()
    {
        SpawnData data = new SpawnData();

        int[][] dataObstacles = new int[][] {
           new int[] { 0, 0, 0, 0, 0 },
           new int[] { 0, 7, 6, 6, 0 },
           new int[] { 0, 8, 7, 6, 0 },
           new int[] { 0, 7, 8, 7, 0 },
           new int[] { 0, 0, 0, 0, 0 }
        };

        data.spawnObstacles = ConvertArray2ToListString(dataObstacles);

        int[][] dataPlanes = new int[][] {
           new int[] { 0, 0, 0, 0, 0 },
           new int[] { 0, 0, 1, 0, 0 },
           new int[] { 0, 0, 0, 4, 0 },
           new int[] { 0, 0, 0, 0, 0 },
           new int[] { 0, 0, 0, 0, 0 }
        };

        data.spawnPlanes = ConvertArray2ToListString(dataPlanes);

        int[][] dataChangeDirectionOb = new int[][] {
            new int[] { 0, 0, 0, 0, 0 },
            new int[] { 0, 0, 0, 0, 0 },
            new int[] { 0, 0, 0, 0, 0 },
            new int[] { 0, 0, 0, 0, 0 },
            new int[] { 0, 0, 0, 0, 0 }
        };

        data.spawnChangeDirectionOb = ConvertArray2ToListString(dataChangeDirectionOb);

        int[][] dataGoal = new int[][] {
            new int[] { 0, 0, 0, 0, 0 },
            new int[] { 0, 0, 0, 1, 0 },
            new int[] { 0, 0, 0, 0, 0 },
            new int[] { 0, 0, 0, 0, 0 },
            new int[] { 0, 0, 0, 0, 0 }
        };

        data.spawnGoal = ConvertArray2ToListString(dataGoal);

        int[][] dataBall = new int[][]
        {
            new int[] { 0, 0, 0, 0, 0 },
            new int[] { 0, 0, 0, 0, 0 },
            new int[] { 0, 0, 0, 0, 0 },
            new int[] { 0, 1, 0, 0, 0 },
            new int[] { 0, 0, 0, 0, 0 }
        };

        data.spawnBall = ConvertArray2ToListString(dataBall);

        int[][] dataItemMove = new int[][]
        {
            new int[] { 0, 1, 0 },
            new int[] { 1, 0, 2 },
        };

        data.itemMove = ConvertArray2ToListString(dataItemMove);

        string json = JsonUtility.ToJson(data);

        Debug.Log(json);

        File.WriteAllText(Application.persistentDataPath + "/level_9.json", json);
    }

    public void LoadData(string level)
    {
        //string path = Application.persistentDataPath + $"/{level}";
        //Debug.Log(path);

        //if (File.Exists(path))
        //{
        //    string json = File.ReadAllText(path);
        //    SpawnData data = JsonUtility.FromJson<SpawnData>(json);

        //    obstacleManager.spawnObstacles = ConvertListStringToArray2(data.spawnObstacles);
        //    obstacleManager.spawnPlanes = ConvertListStringToArray2(data.spawnPlanes);
        //    obstacleManager.spawnChangeDirectionOb = ConvertListStringToArray2(data.spawnChangeDirectionOb);
        //    obstacleManager.spawnGoal = ConvertListStringToArray2(data.spawnGoal);
        //    obstacleManager.spawnBall = ConvertListStringToArray2(data.spawnBall);

        //    int[][] buttons = ConvertListStringToArray2(data.itemMove);

        //    for (int i = 0; i < buttons.Length; ++i)
        //    {
        //        gameManager.createButton(buttons[i]);
        //    }
        //}

        SpawnObstacleLevelScriptableObject spawnObstacleLevel = AssetDatabase.LoadAssetAtPath<SpawnObstacleLevelScriptableObject>($"Assets/GameData/Level/{level}");
        
        if (spawnObstacleLevel == null)
        {
            Debug.Log($"Khong tim thay file {level}");
            return;
        }

        obstacleManager.spawnObstacles = ConvertListStringToArray2(spawnObstacleLevel.spawnObstacles);
        obstacleManager.spawnPlanes = ConvertListStringToArray2(spawnObstacleLevel.spawnPlanes);
        obstacleManager.spawnChangeDirectionOb = ConvertListStringToArray2(spawnObstacleLevel.spawnChangeDirectionOb);
        obstacleManager.spawnGoal = ConvertListStringToArray2(spawnObstacleLevel.spawnGoal);
        obstacleManager.spawnBall = ConvertListStringToArray2(spawnObstacleLevel.spawnBall);

        int[][] buttons = ConvertListStringToArray2(spawnObstacleLevel.itemMove);

        for (int i = 0; i < buttons.Length; ++i)
        {
            gameManager.createButton(buttons[i]);
        }
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
}
