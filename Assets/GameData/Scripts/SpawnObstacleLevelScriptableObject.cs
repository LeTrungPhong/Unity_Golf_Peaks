using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Position
{
    public float x;
    public float y;
    public float z;
}

[CreateAssetMenu(fileName = "SpawnObstacleLevel", menuName = "ScriptableObjects/SpawnObstacleLevelScriptableObject", order = 1)]
public class SpawnObstacleLevelScriptableObject : ScriptableObject
{
    public List<string> spawnObstacles;
    public List<string> spawnPlanes;
    public List<string> spawnChangeDirectionOb;
    public List<string> spawnBlockRoll;
    public List<string> spawnDive;
    public List<string> spawnWater;
    public List<string> spawnGoal;
    public List<string> spawnBall;
    public List<string> itemMove;
    public List<string> hint;
    public int highFly;

    public float positionX;
    public float positionY;
    public float positionZ;
}
