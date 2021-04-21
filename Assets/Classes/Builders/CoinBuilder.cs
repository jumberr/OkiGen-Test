using System.Collections.Generic;
using Classes.GenObjects;
using Handlers;
using UnityEngine;
using PositionToSpawn = Handlers.GameManager.PositionToSpawn;

namespace Classes.Builders
{
    public class CoinBuilder : Builder<Coin>
    {
        private readonly Transform coinsParent;
        private readonly List<GameObject> coinsGroupsGO = new List<GameObject>();
        private readonly GameObject coinsPrefab;
        private readonly int maxLengthAmountCoins;
        private readonly int maxWidthAmountCoins;
        private readonly float sideCoin;
        private readonly PositionToSpawn positionToSpawn;

        public CoinBuilder(Transform coinsParent, GameObject coinsPrefab, int maxLengthAmountCoins,
            int maxWidthAmountCoins, float sideCoin, PositionToSpawn positionToSpawn)
        {
            this.coinsParent = coinsParent;
            this.coinsPrefab = coinsPrefab;
            this.maxLengthAmountCoins = maxLengthAmountCoins;
            this.maxWidthAmountCoins = maxWidthAmountCoins;
            this.sideCoin = sideCoin;
            this.positionToSpawn = positionToSpawn;
        }

        public void InitializeCoins(List<Vector3> positionsPlatforms, List<Quaternion> rotations, int amountOfPlatforms,
            float lengthOfPlatform)
        {
            const float startPosX = 0f;
            const float startPosY = 0.8f;
            const float offset = 3f;

            for (var i = 0; i < amountOfPlatforms; i++)
            {
                var position = new Vector3(startPosX, startPosY);

                CreateNewEmptyList();

                var randomZ = Random.Range(0, maxLengthAmountCoins);
                for (var z = 0; z < randomZ; z++, position.z += sideCoin)
                {
                    var randomX = Random.Range(0, maxWidthAmountCoins);
                    for (int x = 0, index = 1; x < randomX; x++, position.x = x * index * sideCoin, index *= -1)
                    {
                        GameManager.InstantiateGeneratedObject(coinsPrefab, position, Quaternion.Euler(-90, 0, 0),
                            out var coinGo); // -90 is rotation of prefab
                        var coin = new Coin(coinGo, coinGo.transform);
                        AddNewObject(coin);
                    }

                    position = new Vector3(startPosX, position.y, position.z);
                }

                AddNewList(ListObj);

                GameManager.ChangeParent(coinsGroupsGO, i, this, coinsParent);

                GameManager.ChangePositionDependingOnObstacles(positionsPlatforms, rotations, coinsGroupsGO, i,
                    lengthOfPlatform, offset, positionToSpawn);
            }
        }
    }
}