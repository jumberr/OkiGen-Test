using System.Collections.Generic;
using Classes.FactoryMethod;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Space, Header("PLATFORM")] [SerializeField]
    private Transform platformParent;

    [SerializeField] private GameObject platform;
    [SerializeField] private GameObject corner;
    [SerializeField] private GameObject finish;
    [SerializeField] private List<int> cornerIndex;
    [SerializeField] private float leftBorderX = -1.5f;
    [SerializeField] private float rightBorderX = 1.5f;
    [SerializeField] private int amountOfPlatforms = 7;
    [SerializeField] private float lengthOfPlatform = 20f;
    [SerializeField] private float widthOfPlatform = 3.5f;
    private PlatformsCreator platformsCreator;

    private enum Turn
    {
        None,
        Left,
        Right
    }

    private enum Coordinate
    {
        PosZ,
        NegZ,
        PosX,
        NegX
    }

    // COINS
    [Space, Header("COINS")] [SerializeField]
    private GameObject coinsPrefab;

    private ObjectsCreator coinsCreator;
    private List<Coin> coins = new List<Coin>();

    // OBSTACLES
    [Space, Header("OBSTACLES")] 
    [SerializeField] private int amountObstaclesGroups;
    [SerializeField] private GameObject obstaclesPrefab;
    [SerializeField] private int maxHeightAmountObstacles;
    [SerializeField] private int maxWidthAmountObstacles;
    [SerializeField] private float heightObstacles = 0.5f;
    [SerializeField] private float widthObstacles = 0.5f;
    private float startPosY = 0.75f;
    private ObjectsCreator obstaclesCreator;
    private List<GameObject> obstacleGroupsGO = new List<GameObject>();
    private List<List<Obstacle>> obstaclesGroups = new List<List<Obstacle>>();

    // CUBES
    [Space, Header("CUBES")] [SerializeField]
    private GameObject cubesPrefab;

    private ObjectsCreator cubesCreator;
    private List<Cube> cubes = new List<Cube>();

    private void Awake()
    {
        InitializeCreators();

        // for (var i = 0; i < coinsTransform.Count; i++)
        // {
        //     coins.Add(coinsCreator.FactoryMethod(coinsTransform[i]) as Coin);
        // }
        // for (var i = 0; i < cubesTransform.Count; i++)
        // {
        //     cubes.Add(cubesCreator.FactoryMethod(cubesTransform[i]) as Cube);
        // }

        InitializePlatforms(out var platforms, out var positions, out var rotations);

        for (int i = 0; i < amountObstaclesGroups; i++)
        {
            InitializeObstacles(out var obstacles, obstaclesCreator);
            obstaclesGroups.Add(obstacles);
            Randomize(obstaclesGroups[i]);
        }

        CreateObstaclesGroups(obstaclesGroups, obstacleGroupsGO);

        ChangeObstaclesGroupsPosRot(obstacleGroupsGO, positions, rotations);
    }

    private void InitializeCreators()
    {
        coinsCreator = new CoinsCreator();
        cubesCreator = new CubesCreator();
        obstaclesCreator = new ObstaclesCreator();
        platformsCreator = new PlatformsCreator();
    }

    private void InitializePlatforms(out List<Platform> platforms, out List<Vector3> positions, out List<Quaternion> rotations)
    {
        var halfLength = lengthOfPlatform / 2;
        var halfWidth = widthOfPlatform / 2;

        var allPlatforms = amountOfPlatforms + cornerIndex.Count + 1; // including finish and corners
        platforms = new List<Platform>();
        positions = new List<Vector3>();
        rotations = new List<Quaternion>();

        for (var i = 0; i < allPlatforms ; i++)
            positions.Add(Vector3.zero);

        for (var i = 0; i < amountOfPlatforms; i++)
            rotations.Add(Quaternion.identity);

        for (var i = 0; i < cornerIndex.Count; i++)
        {
            if (cornerIndex[i] > amountOfPlatforms) // check for errors in input
            {
                cornerIndex.RemoveAt(i);
            }
        }

        var actualCoordinate = Coordinate.PosZ;
        var prevCoord = Coordinate.PosZ;
        var prevTurn = Turn.None;
        var cornerRotation = 0;
        var platformRotation = 0;
        var rotIndex = 0;
        for (var i = 0; i < allPlatforms; i++)
        {
            platformRotation += 0;
            Vector3 currPos;
            if (i == 0)
            {
                currPos = new Vector3(0, 0, lengthOfPlatform);
                positions[i] = currPos;
            }
            else
            {
                var prevPos = positions[i - 1];
                currPos = prevPos;
                if (prevCoord == actualCoordinate)
                {
                    switch (actualCoordinate)
                    {
                        case Coordinate.PosZ:
                            currPos.z = prevPos.z + lengthOfPlatform;
                            break;
                        case Coordinate.NegZ:
                            currPos.z = prevPos.z - lengthOfPlatform;
                            break;
                        case Coordinate.PosX:
                            currPos.x = prevPos.x + lengthOfPlatform;
                            break;
                        case Coordinate.NegX:
                            currPos.x = prevPos.x - lengthOfPlatform;
                            break;
                    }
                }
                else
                {
                    switch (actualCoordinate)
                    {
                        case Coordinate.PosZ:
                            currPos.z = currPos.z + halfWidth + halfLength;
                            break;
                        case Coordinate.NegZ:
                            currPos.z = currPos.z - halfWidth - halfLength;
                            break;
                        case Coordinate.PosX:
                            currPos.x = currPos.x + halfWidth + halfLength;
                            break;
                        case Coordinate.NegX:
                            currPos.x = currPos.x - halfWidth - halfLength;
                            break;
                    }

                    prevCoord = actualCoordinate;
                    platformRotation += 90;
                }

                positions[i] = currPos;
            }

            if (i < allPlatforms - 1)
            {
                rotations[i-rotIndex] = Quaternion.Euler(0, platformRotation, 0); // used for rotations of obstacles
                
                var obj = Instantiate(platform, positions[i], Quaternion.Euler(0, platformRotation, 0), platformParent);
                platforms.Add((Platform) platformsCreator.FactoryMethod(obj.transform));
            }
            else
            {
                var end = Instantiate(finish, positions[allPlatforms - 1], Quaternion.Euler(0, platformRotation, 0),
                    platformParent);
            }

            for (var j = 0; j < cornerIndex.Count; j++)
            {
                if (i + 1 == cornerIndex[j])
                {
                    var currentTurn = (Turn) Random.Range(1, 3); // left or right

                    //                           Rotation
                    //        from        |         to          |     value
                    //      ------------------------------------------------
                    //        left        |        left         |    -90deg
                    //        right       |        right        |    +90deg
                    //     left/right     |      right/left     |    +180deg


                    //                           Coordinate
                    //        from        |         turn          |     value
                    //      --------------------------------------------------
                    //        PosZ        |         right         |     PosX           
                    //        PosZ        |         left          |     NegX           
                    //        NegZ        |         right         |     NegX           
                    //        NegZ        |         left          |     PosX           
                    //        PosX        |         right         |     NegZ           
                    //        PosX        |         left          |     PosZ           
                    //        NegX        |         right         |     PosZ           
                    //        NegX        |         left          |     NegZ         

                    if (currentTurn == Turn.Right)
                    {
                        prevCoord = actualCoordinate;
                        actualCoordinate = actualCoordinate switch
                        {
                            Coordinate.PosZ => Coordinate.PosX,
                            Coordinate.NegZ => Coordinate.NegX,
                            Coordinate.PosX => Coordinate.NegZ,
                            Coordinate.NegX => Coordinate.PosZ
                        };
                        cornerRotation += prevTurn switch
                        {
                            Turn.Left => 180,
                            Turn.Right => 90,
                            _ => 0
                        };
                    }
                    else if (currentTurn == Turn.Left)
                    {
                        prevCoord = actualCoordinate;
                        actualCoordinate = actualCoordinate switch
                        {
                            Coordinate.PosZ => Coordinate.NegX,
                            Coordinate.NegZ => Coordinate.PosX,
                            Coordinate.PosX => Coordinate.PosZ,
                            Coordinate.NegX => Coordinate.NegZ
                        };
                        cornerRotation += prevTurn switch
                        {
                            Turn.Left => -90,
                            Turn.Right => 180,
                            _ => 90
                        };
                    }

                    if (cornerRotation > 360)
                        cornerRotation -= 360;

                    else if (cornerRotation < -360)
                        cornerRotation += 360;

                    if (i > 0)
                    {
                        var pos = currPos; // pos of corner
                        switch (prevCoord)
                        {
                            case Coordinate.NegX:
                            {
                                if (actualCoordinate == Coordinate.PosZ || actualCoordinate == Coordinate.NegZ)
                                {
                                    pos.x = pos.x - halfLength - halfWidth;
                                }

                                break;
                            }
                            case Coordinate.PosX:
                            {
                                if (actualCoordinate == Coordinate.PosZ || actualCoordinate == Coordinate.NegZ)
                                {
                                    pos.x = pos.x + halfLength + halfWidth;
                                }

                                break;
                            }
                            case Coordinate.NegZ:
                            {
                                if (actualCoordinate == Coordinate.NegX || actualCoordinate == Coordinate.PosX)
                                {
                                    pos.z = pos.z - halfLength - halfWidth;
                                }

                                break;
                            }
                            case Coordinate.PosZ:
                            {
                                if (actualCoordinate == Coordinate.NegX || actualCoordinate == Coordinate.PosX)
                                {
                                    pos.z = pos.z + halfLength + halfWidth;
                                }

                                break;
                            }
                        }

                        positions[i + 1] = pos;

                        var corn = Instantiate(corner, pos, Quaternion.Euler(0, cornerRotation, 0), platformParent);
                        prevTurn = currentTurn;
                        i++;
                        rotIndex++;
                    }
                }
            }
        }

        positions.RemoveAt(allPlatforms - 1);
        for (int i = 0, deleted = 0; i < cornerIndex.Count; i++)
        {
            positions.RemoveAt(cornerIndex[i] - deleted);
            deleted++;
        }
    }

    private void InitializeObstacles(out List<Obstacle> obstacles, ObjectsCreator obstaclesCreator)
    {
        var position = new Vector3(leftBorderX, startPosY);
        obstacles = new List<Obstacle>();

        for (var y = 0; y < maxHeightAmountObstacles; y++, position.y += heightObstacles)
        {
            for (var x = 0; x < maxWidthAmountObstacles || position.x < rightBorderX; x++, position.x += widthObstacles)
            {
                var obj = Instantiate(obstaclesPrefab, position, Quaternion.identity);
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

    private void ChangeObstaclesGroupsPosRot(List<GameObject> obstacleGroupsGO, List<Vector3> positions, List<Quaternion> rotations)
    {
        for (int i = 0; i < obstacleGroupsGO.Count; i++)
        {
            obstacleGroupsGO[i].transform.position = positions[i];
            obstacleGroupsGO[i].transform.rotation = rotations[i];
        }
    }
}