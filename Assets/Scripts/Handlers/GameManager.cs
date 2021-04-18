using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Handlers
{
    public class GameManager : MonoBehaviour
    {
        [Space, Header("PLATFORM")] 
        [SerializeField] private Transform platformParent;
        [SerializeField] private GameObject platform;
        [SerializeField] private GameObject corner;
        [SerializeField] private GameObject finish;
        [SerializeField] private List<int> cornerIndex;
        [SerializeField] private float leftBorderX = -1.5f;
        [SerializeField] private int amountOfPlatforms = 7;
        [SerializeField] private float lengthOfPlatform = 20f;
        [SerializeField] private float widthOfPlatform = 3.5f;

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
        [Space, Header("COINS")] 
        [SerializeField] private Transform coinsParent;
        [SerializeField] private GameObject coinsPrefab;
        [SerializeField] private int maxWidthAmountCoins;
        [SerializeField] private int maxLengthAmountCoins;
        private List<GameObject> coinsGroupsGO = new List<GameObject>();
        private List<Transform> coins = new List<Transform>();

        // OBSTACLES
        [Space, Header("OBSTACLES")] 
        [SerializeField] private Transform obstaclesParent;
        [SerializeField] private int amountObstaclesGroups;
        [SerializeField] private GameObject obstaclesPrefab;
        [SerializeField] private int maxWidthAmountObstacles;
        [SerializeField] private float heightObstacles = 0.5f;
        [SerializeField] private float widthObstacles = 0.5f;
        private List<GameObject> obstacleGroupsGO = new List<GameObject>();

        // CUBES
        [Space, Header("CUBES")] 
        [SerializeField] private Transform cubesParent;
        [SerializeField] private GameObject cubesPrefab;
        [SerializeField] private int maxHeightAmountCubes;
        [SerializeField] private int maxWidthAmountCubes;
        [SerializeField] private int maxLengthAmountCubes;
        [SerializeField] private float heightCubes = 0.5f;
        [SerializeField] private float widthCubes = 0.5f;
        [SerializeField] private float lengthCubes = 0.5f;
        private List<Transform> cubes = new List<Transform>();
        private List<GameObject> cubesGroupsGO = new List<GameObject>();

        private void Awake()
        {
            Time.timeScale = 1f;

            InitializePlatforms(out var positions, out var rotations);

            InitializeCubes(out var cubesGroups, positions, rotations);

            InitializeObstacles(cubesGroups, obstacleGroupsGO, positions, rotations);

            InitializeCoins(positions, rotations);
        }

        private void InitializeCoins(List<Vector3> positionsPlatforms, List<Quaternion> rotations)
        {
            var startPosX = 0f;
            var startPosY = 0.75f;
            var coinsGroups = new List<List<Transform>>();
            for (var i = 0; i < amountOfPlatforms; i++)
            {
                var position = new Vector3(startPosX, startPosY);
                coins = new List<Transform>();


                var randomZ = Random.Range(1, maxLengthAmountCoins);
                for (var y = 0; y < randomZ; y++, position.z += heightCubes)
                {
                    var randomX = Random.Range(1, maxWidthAmountCoins);
                    for (int x = 0, index = 1; x < randomX; x++, position.x = x * index * widthCubes, index *= -1)
                    {
                        var obj = Instantiate(coinsPrefab, position,
                            Quaternion.Euler(-90, 0, 0)); // -90 is rotation of prefab
                        coins.Add(obj.transform);
                    }

                    position = new Vector3(startPosX, position.y, position.z);
                }

                coinsGroups.Add(coins);

                coinsGroupsGO.Add(new GameObject($"Coins Group #{i}"));
                for (var j = 0; j < coinsGroups[i].Count; j++)
                {
                    coinsGroups[i][j].parent = coinsGroupsGO[i].transform;
                }

                coinsGroupsGO[i].transform.parent = coinsParent;

                // change position of spawned cubes
                var rot = rotations[i].eulerAngles.y;
                if (rot < 0)
                {
                    rot += 360;
                }

                var pos = positionsPlatforms[i];
                var halfOfLength =
                    lengthOfPlatform / 2 - 3f; // -3 just for better positions that not match with corners pos
                coinsGroupsGO[i].transform.position = rot switch
                {
                    0 => new Vector3(pos.x, pos.y, pos.z + halfOfLength),
                    90 => new Vector3(pos.x + halfOfLength, pos.y, pos.z),
                    180 => new Vector3(pos.x, pos.y, pos.z - halfOfLength),
                    270 => new Vector3(pos.x - halfOfLength, pos.y, pos.z)
                };

                coinsGroupsGO[i].transform.rotation = rotations[i];
            }
        }

        private void InitializeCubes(out List<List<Transform>> cubesGroups,
            List<Vector3> positionsPlatforms, List<Quaternion> rotations)
        {
            var startPosX = 0f;
            var startPosY = 0.75f;
            cubesGroups = new List<List<Transform>>();
            for (var i = 0; i < amountOfPlatforms; i++)
            {
                var position = new Vector3(startPosX, startPosY);
                cubes = new List<Transform>();

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
                            cubes.Add(obj.transform);
                        }

                        position = new Vector3(startPosX, position.y, position.z);
                    }

                    position = new Vector3(startPosX, startPosY, position.z);
                }

                cubesGroups.Add(cubes);

                cubesGroupsGO.Add(new GameObject($"Cubes Group #{i}"));
                for (var j = 0; j < cubesGroups[i].Count; j++)
                {
                    cubesGroups[i][j].parent = cubesGroupsGO[i].transform;
                }

                cubesGroupsGO[i].transform.parent = cubesParent;

                // change position of spawned cubes
                var rot = rotations[i].eulerAngles.y;
                if (rot < 0)
                {
                    rot += 360;
                }

                var pos = positionsPlatforms[i];
                var halfOfLength =
                    lengthOfPlatform / 2 - 3f; // -3 just for better positions that not match with corners pos
                cubesGroupsGO[i].transform.position = rot switch
                {
                    0 => new Vector3(pos.x, pos.y, pos.z - halfOfLength),
                    90 => new Vector3(pos.x - halfOfLength, pos.y, pos.z),
                    180 => new Vector3(pos.x, pos.y, pos.z + halfOfLength),
                    270 => new Vector3(pos.x + halfOfLength, pos.y, pos.z)
                };

                cubesGroupsGO[i].transform.rotation = rotations[i];
            }
        }

        private void InitializePlatforms(out List<Vector3> positions,
            out List<Quaternion> rotations)
        {
            var halfLength = lengthOfPlatform / 2;
            var halfWidth = widthOfPlatform / 2;

            var allPlatforms = amountOfPlatforms + cornerIndex.Count + 1; // including finish and corners
            var platforms = new List<Transform>();
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
                    platforms.Add(obj.transform);
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

        private void InitializeObstacles(List<List<Transform>> cubesGroups,
            List<GameObject> obstacleGroupsGO, List<Vector3> positions, List<Quaternion> rotations)
        {
            var startPosY = 0.75f;
            var obstaclesGroups = new List<List<Transform>>();

            for (var i = 0; i < amountObstaclesGroups; i++)
            {
                var position = new Vector3(leftBorderX, startPosY);
                var obstacles = new List<Transform>();
                for (var x = 0; x < maxWidthAmountObstacles; x++, position.x += widthObstacles)
                {
                    var maxLengthY = cubesGroups[i].Count;
                    var randomY = Random.Range(1, maxLengthY);
                    for (var y = 0; y < randomY; y++, position.y += heightObstacles)
                    {
                        var obj = Instantiate(obstaclesPrefab, position, Quaternion.identity);
                        obstacles.Add(obj.transform);
                    }

                    position = new Vector3(position.x, startPosY);
                }

                obstaclesGroups.Add(obstacles);

                obstacleGroupsGO.Add(new GameObject($"Obstacle Group #{i}"));
                for (var j = 0; j < obstaclesGroups[i].Count; j++)
                {
                    obstaclesGroups[i][j].parent = obstacleGroupsGO[i].transform;
                }

                obstacleGroupsGO[i].transform.parent = obstaclesParent;

                obstacleGroupsGO[i].transform.position = positions[i];
                obstacleGroupsGO[i].transform.rotation = rotations[i];
            }
        }
    }
}