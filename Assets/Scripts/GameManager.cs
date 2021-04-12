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
    [Space, Header("OBSTACLES")] [SerializeField]
    private Transform obstaclesParent;

    [SerializeField] private int amountObstaclesGroups;
    [SerializeField] private GameObject obstaclesPrefab;
    [SerializeField] private int maxHeightAmountObstacles;
    [SerializeField] private int maxWidthAmountObstacles;
    [SerializeField] private float heightObstacles = 0.5f;
    [SerializeField] private float widthObstacles = 0.5f;
    private ObjectsCreator obstaclesCreator;
    private List<GameObject> obstacleGroupsGO = new List<GameObject>();
    private List<List<Obstacle>> obstaclesGroups = new List<List<Obstacle>>();

    // CUBES
    [Space, Header("CUBES")] [SerializeField]
    private Transform cubesParent;

    [SerializeField] private GameObject cubesPrefab;
    [SerializeField] private int maxHeightAmountCubes;
    [SerializeField] private int maxWidthAmountCubes;
    [SerializeField] private int maxLengthAmountCubes;
    [SerializeField] private float heightCubes = 0.4f;
    [SerializeField] private float widthCubes = 0.4f;
    [SerializeField] private float lengthCubes = 0.4f;
    private ObjectsCreator cubesCreator;
    private List<Cube> cubes = new List<Cube>();
    private List<GameObject> cubesGroupsGO = new List<GameObject>();

    private void Awake()
    {
        InitializeCreators();

        // for (var i = 0; i < coinsTransform.Count; i++)
        // {
        //     coins.Add(coinsCreator.FactoryMethod(coinsTransform[i]) as Coin);
        // }
        
        InitializePlatforms(out var platforms, out var positions, out var rotations);

        InitializeCubes(cubesCreator, out var cubes, positions, rotations);

        for (int i = 0; i < amountObstaclesGroups; i++)
        {
            InitializeObstacles(out var obstacles, obstaclesCreator);
            obstaclesGroups.Add(obstacles);
        }

        CreateObstaclesGroups(obstaclesGroups, obstacleGroupsGO);

        ChangeObstaclesGroupsPosRot(obstacleGroupsGO, positions, rotations);
    }

    private void InitializeCubes(ObjectsCreator cubesCreator, out List<List<Cube>> cubesGroups,
        List<Vector3> positionsPlatforms, List<Quaternion> rotations)
    {
        var startPosX = 0f;
        var startPosY = 0.7f;
        cubesGroups = new List<List<Cube>>();
        for (var i = 0; i < amountOfPlatforms; i++)
        {
            var position = new Vector3(startPosX, startPosY);
            cubes = new List<Cube>();

            var gap = 0.4f;
            var randomZ = Random.Range(1, maxLengthAmountCubes);
            var gapValueZ = Random.Range(0, 6);
            for (var z = 0; z < randomZ; z++, position.z += lengthCubes + gapValueZ * gap)
            {
                var randomY = Random.Range(1, maxHeightAmountCubes);
                for (var y = 0; y < randomY; y++, position.y += heightCubes)
                {
                    var randomX = Random.Range(1, maxWidthAmountCubes);
                    for (int x = 0, index = 1; x < randomX; x++, position.x = x * index * widthCubes, index *= -1)
                    {
                        var obj = Instantiate(cubesPrefab, position, Quaternion.identity);
                        cubes.Add((Cube) cubesCreator.FactoryMethod(obj.transform));
                    }

                    position = new Vector3(startPosX, position.y, position.z);
                }

                position = new Vector3(startPosX, startPosY, position.z);
            }

            cubesGroups.Add(cubes);

            cubesGroupsGO.Add(new GameObject($"Cubes Group #{i}"));
            for (var j = 0; j < cubesGroups[i].Count; j++)
            {
                cubesGroups[i][j].ObjectTransform.parent = cubesGroupsGO[i].transform;
            }

            cubesGroupsGO[i].transform.parent = cubesParent;

            // todo change cubes positions :(

            // var rot = rotations[i].eulerAngles.y;
            // if (rot < 0)
            // {
            //     rot += 360;
            // }
            //
            // var pos = cubesGroupsGO[i].transform.position;
            // cubesGroupsGO[i].transform.position = rot switch
            // {
            //     0 => new Vector3(pos.x, pos.y,pos.z - lengthOfPlatform),
            //     90 => new Vector3(pos.x - lengthOfPlatform, pos.y,pos.z),
            //     180 => new Vector3(pos.x, pos.y,pos.z + lengthOfPlatform),
            //     270 => new Vector3(pos.x + lengthOfPlatform, pos.y,pos.z)
            // };

            cubesGroupsGO[i].transform.position = positionsPlatforms[i];
            cubesGroupsGO[i].transform.rotation = rotations[i];
        }
    }

    private void InitializeCreators()
    {
        coinsCreator = new CoinsCreator();
        cubesCreator = new CubesCreator();
        obstaclesCreator = new ObstaclesCreator();
        platformsCreator = new PlatformsCreator();
    }

    private void InitializePlatforms(out List<Platform> platforms, out List<Vector3> positions,
        out List<Quaternion> rotations)
    {
        var halfLength = lengthOfPlatform / 2;
        var halfWidth = widthOfPlatform / 2;

        var allPlatforms = amountOfPlatforms + cornerIndex.Count + 1; // including finish and corners
        platforms = new List<Platform>();
        positions = new List<Vector3>();
        rotations = new List<Quaternion>();

        for (var i = 0; i < allPlatforms; i++)
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
                rotations[i - rotIndex] = Quaternion.Euler(0, platformRotation, 0); // used for rotations of obstacles

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
        var startPosY = 0.75f;
        var position = new Vector3(leftBorderX, startPosY);
        obstacles = new List<Obstacle>();

        for (var x = 0; x < maxWidthAmountObstacles; x++, position.x += widthObstacles)
        {
            var randomY = Random.Range(1, maxHeightAmountObstacles);
            for (var y = 0; y < randomY; y++, position.y += heightObstacles)
            {
                var obj = Instantiate(obstaclesPrefab, position, Quaternion.identity);
                obstacles.Add((Obstacle) obstaclesCreator.FactoryMethod(obj.transform));
            }

            position = new Vector3(position.x, startPosY);
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

            obstacleGroupsGO[i].transform.parent = obstaclesParent;
        }
    }

    private void ChangeObstaclesGroupsPosRot(List<GameObject> obstacleGroupsGO, List<Vector3> positions,
        List<Quaternion> rotations)
    {
        for (int i = 0; i < obstacleGroupsGO.Count; i++)
        {
            obstacleGroupsGO[i].transform.position = positions[i];
            obstacleGroupsGO[i].transform.rotation = rotations[i];
        }
    }
}