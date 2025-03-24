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
        obstacleManager = GameObject.FindGameObjectWithTag("ObstacleManager").gameObject.GetComponent<ObstacleManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").gameObject.GetComponent<GameManager>();
        LoadData(LevelManager.Instance.listPathLevelData[LevelManager.Instance.levelSelected]);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadData(string level)
    {
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

        if (LevelManager.Instance.levelSelected + 1 == 32)
        {
            obstacleManager.highFly = 5;
        }

        for (int i = 0; i < buttons.Length; ++i)
        {
            gameManager.createButton(buttons[i]);
        }
    }

    void ChangeHighFly()
    {

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
