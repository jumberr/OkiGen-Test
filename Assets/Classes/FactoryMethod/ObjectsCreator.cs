using UnityEngine;

namespace Classes.FactoryMethod
{
    public abstract class ObjectsCreator
    {
        public abstract GeneratedObjects FactoryMethod(Transform objectTransform);
    }
}