using System.Collections.Generic;
using System.Linq;
using Classes.FactoryMethod;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Space, Header("PLATFORM")] 
    [SerializeField] private float leftBorderX = -1.5f;
    [SerializeField] private float rightBorderX = 1.5f;

    // COINS
    [Space, Header("COINS")] [SerializeField]
    private GameObject coinsPrefab;
    private ObjectsCreator coinsCreator;
    private List<Coin> coins = new List<Coin>();

    // OBSTACLES
    [Space, Header("OBSTACLES")] 
    [SerializeField] private int amountObstaclesGroups;
    [SerializeField] private Transform obstaclesParent;
    [SerializeField] private GameObject obstaclesPrefab;
    [SerializeField] private int maxHeightAmountObstacles;
    [SerializeField] private int maxWidthAmountObstacles;
    [SerializeField] private float heightObstacles = 0.5f;
    [SerializeField] private float widthObstacles = 0.5f;
    private float startPosY = 0.75f;
    private ObjectsCreator obstaclesCreator;
    //private List<Obstacle> obstacles = new List<Obstacle>();
    private List<GameObject> obstacleGroupsGO = new List<GameObject>();
    private List<List<Obstacle>> obstaclesGroups = new List<List<Obstacle>>();

    // CUBES
    [Space, Header("CUBES")] 
    [SerializeField] private GameObject cubesPrefab;

    private ObjectsCreator cubesCreator;
    private List<Cube> cubes = new List<Cube>();

    private void Awake()
    {
        coinsCreator = new CoinsCreator();
        cubesCreator = new CubesCreator();
        obstaclesCreator = new ObstaclesCreator();

        // for (var i = 0; i < coinsTransform.Count; i++)
        // {
        //     coins.Add(coinsCreator.FactoryMethod(coinsTransform[i]) as Coin);
        // }
        // for (var i = 0; i < cubesTransform.Count; i++)
        // {
        //     cubes.Add(cubesCreator.FactoryMethod(cubesTransform[i]) as Cube);
        // }
        
        
        for (int i = 0; i < amountObstaclesGroups; i++)
        {
            InitializeObstacles(out var obstacles);
            obstaclesGroups.Add(obstacles);
            Randomize(obstaclesGroups[i]);
        }

        CreateObstaclesGroups(obstaclesGroups, obstacleGroupsGO);

        ChangeObstaclesGroupsPosition(obstacleGroupsGO);


        //SpawnPosition(obstacles);
    }

    private void InitializeObstacles(out List<Obstacle> obstacles)
    {
        var position = new Vector3(leftBorderX, startPosY);
        obstacles = new List<Obstacle>();

        for (var y = 0; y < maxHeightAmountObstacles; y++, position.y += heightObstacles)
        {
            for (var x = 0; x < maxWidthAmountObstacles || position.x < rightBorderX; x++, position.x += widthObstacles)
            {
                var obj = Instantiate(obstaclesPrefab, position, Quaternion.identity, obstaclesParent);
                obstacles.Add((Obstacle) obstaclesCreator.FactoryMethod(obj.transform));
            }
            position = new Vector3(leftBorderX, position.y);
        }
    }

    private void Randomize(List<Obstacle> obstacles)
    {
        var maxAmount = maxHeightAmountObstacles * maxWidthAmountObstacles;
        var amountOfObstacles = Random.Range(0, maxAmount);
        for (int i = 0; i < maxWidthAmountObstacles; i++)
        {
            var blocksInColumn = Random.Range(0, maxHeightAmountObstacles);
            if (amountOfObstacles >= blocksInColumn)
            {
                amountOfObstacles -= blocksInColumn;
            }
            else
            {
                blocksInColumn = amountOfObstacles;
            }

            for (var j = 0; j < maxWidthAmountObstacles; j++)
            {
                obstacles[j * maxHeightAmountObstacles + i].ObjectTransform.gameObject
                    .SetActive(j < blocksInColumn);
            }
        }
    }
    
    private void CreateObstaclesGroups(List<List<Obstacle>> obstaclesGroups, List<GameObject> obstacleGroupsGO)
    {
        for (var i = 0; i < amountObstaclesGroups; i++)
        {
            obstacleGroupsGO.Add(new GameObject($"Obstacle Group #{i}"));
            for (var j = 0; j < obstaclesGroups[i].Count; j++)
            {
                obstaclesGroups[i][j].ObjectTransform.parent = obstacleGroupsGO[i].transform;
            }
        }
    }

    private void ChangeObstaclesGroupsPosition(List<GameObject> obstacleGroupsGO)
    {
        var position = Vector3.zero;
        for (int i = 0, z = 20; i < obstacleGroupsGO.Count; i++, z+=20)
        {
            position.z = z;
            obstacleGroupsGO[i].transform.position = position;
        }
    }
}