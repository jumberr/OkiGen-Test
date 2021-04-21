using System.Collections.Generic;
using Classes.Builders;
using Classes.GenObjects;
using SO;
using UnityEngine;

namespace Handlers
{
    public class GameManager : MonoBehaviour
    {
        [Space, Header("PLATFORM")] 
        [SerializeField] private PlatformsStats platformsStats;
        [SerializeField] private List<int> cornerIndex;
        [SerializeField] private Transform platformParent;
        [SerializeField] private int amountOfPlatforms = 7;
        [SerializeField] private float leftBorderX = -1.5f;
        private PlatformBuilder platformBuilder;
        private float lengthOfPlatform;
        
        
        [Space, Header("COINS")] 
        [SerializeField] private CoinsStats coinsStats;
        [SerializeField] private Transform coinsParent;
        [SerializeField] private int maxWidthAmountCoins;
        [SerializeField] private int maxLengthAmountCoins;
        private CoinBuilder coinBuilder;
        
        
        [Space, Header("OBSTACLES")] 
        [SerializeField] private ObstaclesStats obstaclesStats;
        [SerializeField] private Transform obstaclesParent;
        [SerializeField] private int amountObstaclesGroups;
        [SerializeField] private int maxWidthAmountObstacles;
        private ObstacleBuilder obstacleBuilder;
        
        
        [Space, Header("CUBES")]
        [SerializeField] private CubesStats cubesStats;
        [SerializeField] private Transform cubesParent;
        [SerializeField] private int maxHeightAmountCubes;
        [SerializeField] private int maxWidthAmountCubes;
        [SerializeField] private int maxLengthAmountCubes;
        private CubeBuilder cubeBuilder;

        
        public enum PositionToSpawn
        {
            BeforeObstacles,
            AfterObstacles
        }
        
        private void Awake()
        {
            Time.timeScale = 1f;

            InitializeBuilders();

            lengthOfPlatform = platformsStats.LengthOfPlatform;
            
            platformBuilder.InitializePlatforms(out var positions, out var rotations, amountOfPlatforms, cornerIndex);

            cubeBuilder.InitializeCubes(out var cubesGroups, positions, rotations, amountOfPlatforms,
                maxLengthAmountCubes, maxHeightAmountCubes, maxWidthAmountCoins, lengthOfPlatform);
            
            obstacleBuilder.InitializeObstacles(cubesGroups, positions, rotations, amountObstaclesGroups,
                maxWidthAmountObstacles, leftBorderX);
            
            coinBuilder.InitializeCoins(positions, rotations, amountOfPlatforms, lengthOfPlatform);
        }

        private void InitializeBuilders()
        {
            platformBuilder = new PlatformBuilder(platformParent, platformsStats.Platform, platformsStats.Corner, platformsStats.Finish,
                platformsStats.LengthOfPlatform, platformsStats.WidthOfPlatform);
            coinBuilder = new CoinBuilder(coinsParent, coinsStats.CoinsPrefab, maxLengthAmountCoins,
                maxWidthAmountCoins, coinsStats.CoinsSide, PositionToSpawn.AfterObstacles);
            obstacleBuilder = new ObstacleBuilder(obstaclesParent, obstaclesStats.ObstaclesPrefab,obstaclesStats.SideOfObstacles);
            cubeBuilder = new CubeBuilder(cubesParent, cubesStats.CubesPrefab, cubesStats.SideCubes, PositionToSpawn.BeforeObstacles);
        }

        public static void InstantiateGeneratedObject(GameObject prefab, Vector3 position, Quaternion rotation, out GameObject generatedObject)
        {
            generatedObject = Instantiate(prefab, position, rotation);
        }
        
        public static void ChangeParent<T>(List<GameObject> groupsGO, int i, Builder<T> builder, Transform parent) where T : GeneratedObjects
        {
            groupsGO.Add(new GameObject($"{typeof(T).Name}s Group #{i}"));
            for (var j = 0; j < builder.ListGroups[i].Count; j++)
            {
                builder.ListGroups[i][j].Transform.parent = groupsGO[i].transform;
            }
            groupsGO[i].transform.parent = parent;
        }

        public static void ChangePositionDependingOnObstacles(List<Vector3> positionsPlatforms, List<Quaternion> rotations,
            List<GameObject> groupsGO,  int i, float lengthOfPlatform, float offset, PositionToSpawn positionToSpawn)
        {
            var rot = rotations[i].eulerAngles.y;
            if (rot < 0)
                rot += 360;

            var pos = positionsPlatforms[i];
            var halfOfLength = lengthOfPlatform / 2 - offset; // offset is just for better positions that not match with corners pos
            
            switch (positionToSpawn)
            {
                case PositionToSpawn.BeforeObstacles:
                    groupsGO[i].transform.position = rot switch
                    {
                        0 => new Vector3(pos.x, pos.y, pos.z - halfOfLength),
                        90 => new Vector3(pos.x - halfOfLength, pos.y, pos.z),
                        180 => new Vector3(pos.x, pos.y, pos.z + halfOfLength),
                        270 => new Vector3(pos.x + halfOfLength, pos.y, pos.z)
                    };
                    break;
                case PositionToSpawn.AfterObstacles:
                    groupsGO[i].transform.position = rot switch
                    {
                        0 => new Vector3(pos.x, pos.y, pos.z + halfOfLength),
                        90 => new Vector3(pos.x + halfOfLength, pos.y, pos.z),
                        180 => new Vector3(pos.x, pos.y, pos.z - halfOfLength),
                        270 => new Vector3(pos.x - halfOfLength, pos.y, pos.z)
                    };
                    break;
            }
            groupsGO[i].transform.rotation = rotations[i];
        }
    }
}