using System.Collections.Generic;
using Classes.GenObjects;
using Handlers;
using UnityEngine;

namespace Classes.Builders
{
    public class ObstacleBuilder : Builder<Obstacle>
    {
        private readonly Transform obstaclesParent;
        private readonly List<GameObject> obstacleGroupsGO = new List<GameObject>();
        private readonly GameObject obstaclesPrefab;
        private readonly float sideOfObstacle;

        public ObstacleBuilder(Transform obstaclesParent, GameObject obstaclesPrefab, float sideOfObstacle)
        {
            this.obstaclesParent = obstaclesParent;
            this.obstaclesPrefab = obstaclesPrefab;
            this.sideOfObstacle = sideOfObstacle;
        }

        public void InitializeObstacles(List<List<Cube>> cubesGroups, List<Vector3> positions,
            List<Quaternion> rotations, int amountObstaclesGroups, int maxWidthAmountObstacles, float leftBorderX)
        {
            var startPosX = leftBorderX;
            const float startPosY = 0.75f;

            for (var i = 0; i < amountObstaclesGroups; i++)
            {
                var position = new Vector3(startPosX, startPosY);
                
                CreateNewEmptyList();
                
                for (var x = 0; x < maxWidthAmountObstacles; x++, position.x += sideOfObstacle)
                {
                    var maxLengthY = cubesGroups[i].Count;
                    var randomY = Random.Range(1, maxLengthY);
                    for (var y = 0; y < randomY; y++, position.y += sideOfObstacle)
                    {
                        GameManager.InstantiateGeneratedObject(obstaclesPrefab, position, Quaternion.identity,
                            out var obstacleGo);
                        var obstacle = new Obstacle(obstacleGo, obstacleGo.transform, x, y);
                        AddNewObject(obstacle);
                    }

                    position = new Vector3(position.x, startPosY);
                }
                
                AddNewList(ListObj);
                
                GameManager.ChangeParent(obstacleGroupsGO, i, this, obstaclesParent);

                obstacleGroupsGO[i].transform.position = positions[i];
                obstacleGroupsGO[i].transform.rotation = rotations[i];
            }
        }
    }
}