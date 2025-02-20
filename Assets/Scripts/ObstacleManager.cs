using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameObject planePrefab;
    [SerializeField] private GameObject changeDirectionPrefab;
    [SerializeField] private GameObject ball;
    private float obstacleSize = 1;
    private float changeDirectionSize = 1;
    private float ballSize = 0.25f;

    // set up obstacle spawn points
    public int[][] spawnObstacles;
    public int[][] spawnPlanes;
    public int[][] spawnChangeDirectionOb;

    // Start is called before the first frame update
    void Start()
    {
        obstacleSize = obstaclePrefab.transform.localScale.x;
        changeDirectionSize = obstacleSize / 2;

        this.spawnObstacles = new int[][] {
           new int[] { 0, 7, 6, 6, 7 },
           new int[] { 7, 0, 6, 0, 6 },
           new int[] { 6, 5, 5, 5, 6 },
           new int[] { 6, 0, 6, 0, 7 },
           new int[] { 7, 6, 6, 7, 0 }
        };

        this.spawnPlanes = new int[][] {
           new int[] { 0, 0, 0, 3, 0 },
           new int[] { 0, 0, 0, 0, 2 },
           new int[] { 0, 1, 0, 3, 0 },
           new int[] { 4, 0, 0, 0, 0 },
           new int[] { 0, 1, 0, 0, 0 }
        };

        this.spawnChangeDirectionOb = new int[][] {
            new int[] { 0, 0, 1, 0, 0 },
            new int[] { 0, 0, 0, 0, 0 },
            new int[] { 1, 0, 0, 0, 0 },
            new int[] { 0, 0, 0, 0, 0 },
            new int[] { 4, 0, 0, 0, 0 }
        };

        for (int i = 0; i < this.spawnObstacles.Length; ++i)
        {
            for (int j = 0; j < this.spawnObstacles[i].Length; ++j)
            {
                if (this.spawnObstacles[i][j] > 0)
                {
                    this.SpawnObstacle(this.spawnObstacles[i][j], i, j);
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
                if (i == 2 && j == 2)
                {
                    // Spawn Ball
                    ball.transform.localScale = new Vector3(ballSize, ballSize, ballSize);
                    ball.transform.position = new Vector3(i * obstacleSize, (this.spawnObstacles[i][j]) * obstacleSize - ballSize / 2 - ballSize, j * obstacleSize);
                }
            }
        }
    }

    public void SpawnObstacle(int high, int indexX, int indexY)
    {
        for (int i = 0; i < high; ++i)
        {
            Instantiate(obstaclePrefab, new Vector3(indexX * obstacleSize, i * obstacleSize, indexY * obstacleSize), Quaternion.identity);
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
