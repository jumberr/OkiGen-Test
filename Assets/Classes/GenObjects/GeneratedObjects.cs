using UnityEngine;

namespace Classes.GenObjects
{
    public class GeneratedObjects
    {
        private GameObject gameObject;
        private Transform transform;

        public GameObject GameObject => gameObject;
        public Transform Transform => transform;
        
        public GeneratedObjects(GameObject gameObject, Transform transform)
        {
            this.gameObject = gameObject;
            this.transform = transform;
        }
    }
}
