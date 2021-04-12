using UnityEngine;

namespace Classes.FactoryMethod
{
    public class CoinsCreator : ObjectsCreator
    {
        public override GeneratedObjects FactoryMethod(Transform objectTransform)
        {
            return new Coin(objectTransform);
        }
    }
}