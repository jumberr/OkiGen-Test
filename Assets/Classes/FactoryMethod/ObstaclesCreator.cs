using UnityEngine;

namespace Classes.FactoryMethod
{
    public class ObstaclesCreator : ObjectsCreator
    {
        public override GeneratedObjects FactoryMethod(Transform objectTransform)
        {
            return new Obstacle(objectTransform);
        }
    }
}