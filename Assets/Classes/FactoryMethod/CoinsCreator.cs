using UnityEngine;

namespace Classes.FactoryMethod
{
    public class CoinsCreator : ObjectsCreator
    {
        public override GeneratedObjects FactoryMethod(Transform objectTransform)
        {
            Debug.Log("COINS!");
            return new Coin(objectTransform);
        }
    }
}