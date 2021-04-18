using UnityEngine;

namespace Classes
{
    public class Obstacles : GeneratedObjects
    {
        private int x;
        private int y;

        public int X => x;
        public int Y => y;
    
        public Obstacles(GameObject gameObject, Transform transform, int x, int y) : base(gameObject, transform)
        {
            this.x = x;
            this.y = y;
        }

    }
}
