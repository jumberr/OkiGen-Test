using UnityEngine;

namespace Classes.FactoryMethod
{
    public class ObstaclesCreator : ObjectsCreator
    {
        public override GeneratedObjects FactoryMethod(Transform objectTransform)
        {
            Debug.Log("OBSTACLES!");
            return new Obstacle(objectTransform);
        }
    }
}