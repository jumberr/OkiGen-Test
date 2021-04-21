using System.Collections.Generic;
using Handlers;
using UnityEngine;
using PositionToSpawn = Handlers.GameManager.PositionToSpawn;

namespace Classes
{
    public class CubeBuilder : Builder<Cube>
    {
        private readonly Transform cubesParent;
        private readonly List<GameObject> cubesGroupsGO = new List<GameObject>();
        private readonly GameObject cubesPrefab;
        private readonly float sideCube;
        private readonly PositionToSpawn positionToSpawn;

        public CubeBuilder(Transform cubesParent, GameObject cubesPrefab, float sideCube, PositionToSpawn positionToSpawn)
        {
            this.cubesParent = cubesParent;
            this.cubesPrefab = cubesPrefab;
            this.sideCube = sideCube;
            this.positionToSpawn = positionToSpawn;
        }

        public void InitializeCubes(out List<List<Cube>> cubesGroups, List<Vector3> positionsPlatforms,
            List<Quaternion> rotations,
            int amountOfPlatforms, int maxLengthAmountCubes, int maxHeightAmountCubes, int maxWidthAmountCubes,
            float lengthOfPlatform)
        {
            const float startPosX = 0f;
            const float startPosY = 0.75f;
            const float offset = 3f;

            for (var i = 0; i < amountOfPlatforms; i++)
            {
                var position = new Vector3(startPosX, startPosY);

                CreateNewEmptyList();

                var gap = 0.4f;
                var randomZ = Random.Range(1, maxLengthAmountCubes);
                var gapValueZ = Random.Range(0, 6);
                for (var z = 0; z < randomZ; z++, position.z += sideCube + gapValueZ * gap)
                {
                    var randomY = Random.Range(1, maxHeightAmountCubes);
                    for (var y = 0; y < randomY; y++, position.y += sideCube)
                    {
                        var randomX = Random.Range(1, maxWidthAmountCubes);
                        for (int x = 0, index = 1; x < randomX; x++, position.x = x * index * sideCube, index *= -1)
                        {
                            GameManager.InstantiateGeneratedObject(cubesPrefab, position, Quaternion.identity,
                                out var cubeGo);
                            var cube = new Cube(cubeGo, cubeGo.transform);
                            AddNewObject(cube);
                        }

                        position = new Vector3(startPosX, position.y, position.z);
                    }

                    position = new Vector3(startPosX, startPosY, position.z);
                }

                AddNewList(ListObj);

                GameManager.ChangeParent(cubesGroupsGO, i, this, cubesParent);
                
                GameManager.ChangePositionDependingOnObstacles(positionsPlatforms, rotations, cubesGroupsGO, i,
                    lengthOfPlatform, offset, positionToSpawn);
            }

            cubesGroups = ListGroups;
        }
    }
}