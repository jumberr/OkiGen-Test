using UnityEngine;

namespace Classes.FactoryMethod
{
    public class PlatformsCreator : ObjectsCreator
    {
        public override GeneratedObjects FactoryMethod(Transform objectTransform)
        {
            return new Platform(objectTransform);
        }
    }
}