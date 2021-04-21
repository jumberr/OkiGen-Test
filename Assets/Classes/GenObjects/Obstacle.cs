using UnityEngine;

namespace Classes.GenObjects
{
    public class Obstacle : GeneratedObjects
    {
        private int x;
        private int y;

        public int X => x;
        public int Y => y;
    
        public Obstacle(GameObject gameObject, Transform transform, int x, int y) : base(gameObject, transform)
        {
            this.x = x;
            this.y = y;
        }

    }
}
