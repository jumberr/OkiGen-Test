using UnityEngine;

namespace Classes.FactoryMethod
{
    public abstract class GeneratedObjects
    {
        private Transform objectTransform;

        public Transform ObjectTransform
        {
            get => objectTransform;
            set => objectTransform = value;
        }

        protected GeneratedObjects(Transform objectTransform)
        {
            this.objectTransform = objectTransform;
        }
    }
}