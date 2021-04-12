using UnityEngine;

namespace Classes.FactoryMethod
{
    public class CubesCreator : ObjectsCreator
    {
        public override GeneratedObjects FactoryMethod(Transform objectTransform)
        {
            return new Cube(objectTransform);
        }
    }
}