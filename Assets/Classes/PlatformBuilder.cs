using System.Collections.Generic;
using Handlers;
using UnityEngine;

namespace Classes
{
    public class PlatformBuilder : Builder<Platform>
    {
        private readonly Transform platformParent;
        private readonly List<GameObject> platformGroupsGO = new List<GameObject>();
        private readonly GameObject platformPrefab;
        private readonly GameObject cornerPrefab;
        private readonly GameObject finishPrefab;
        private readonly float lengthOfPlatform;
        private readonly float widthOfPlatform;

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
        
        public PlatformBuilder(Transform platformParent, GameObject platformPrefab, GameObject cornerPrefab, GameObject finishPrefab, float lengthOfPlatform, float widthOfPlatform)
        {
            this.platformParent = platformParent;
            this.platformPrefab = platformPrefab;
            this.cornerPrefab = cornerPrefab;
            this.finishPrefab = finishPrefab;
            this.lengthOfPlatform = lengthOfPlatform;
            this.widthOfPlatform = widthOfPlatform;
        }
        
        public void InitializePlatforms(out List<Vector3> positions, out List<Quaternion> rotations, int amountOfPlatforms, List<int> cornerIndex)
        {
            var halfLength = lengthOfPlatform / 2;
            var halfWidth = widthOfPlatform / 2;

            var allPlatforms = amountOfPlatforms + cornerIndex.Count + 1; // including finish and corners

            CreateNewEmptyList();
            
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
                    rotations[i - rotIndex] =
                        Quaternion.Euler(0, platformRotation, 0); // used for rotations of obstacles

                    GameManager.InstantiateGeneratedObject(platformPrefab, positions[i], Quaternion.Euler(0, platformRotation, 0),
                        out var platformGo);
                    var platform = new Platform(platformGo, platformGo.transform);
                    AddNewObject(platform);
                    
                    platformGroupsGO.Add(platformGo);
                }
                else
                {
                    GameManager.InstantiateGeneratedObject(finishPrefab, positions[allPlatforms - 1], Quaternion.Euler(0, platformRotation, 0),
                        out var finishGo);
                    var finish = new Platform(finishGo, finishGo.transform);
                    AddNewObject(finish);
                    
                    platformGroupsGO.Add(finishGo);
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
                            
                            GameManager.InstantiateGeneratedObject(cornerPrefab, pos, Quaternion.Euler(0, cornerRotation, 0),
                                out var cornerGo);
                            var corner = new Platform(cornerGo, cornerGo.transform);
                            AddNewObject(corner);
                            platformGroupsGO.Add(cornerGo);
                            
                            prevTurn = currentTurn;
                            i++;
                            rotIndex++;
                        }
                    }
                }
            }

            for (int i = 0; i < platformGroupsGO.Count; i++)
            {
                platformGroupsGO[i].transform.parent = platformParent;
            }

            // delete corners from list of platform's positions
            positions.RemoveAt(allPlatforms - 1);
            for (int i = 0, deleted = 0; i < cornerIndex.Count; i++)
            {
                positions.RemoveAt(cornerIndex[i] - deleted);
                deleted++;
            }
        }
    }
}