using UnityEngine;

namespace Classes.FactoryMethod
{
    public class CubesCreator : ObjectsCreator
    {
        public override GeneratedObjects FactoryMethod(Transform objectTransform)
        {
            Debug.Log("CUBES!");
            return new Cube(objectTransform);
        }
    }
}