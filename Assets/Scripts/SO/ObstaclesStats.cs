using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "Obstacles", menuName = "Obstacle Stats", order = 0)]
    public class ObstaclesStats : ScriptableObject
    {
        [Space, Header("OBSTACLES")]
        [SerializeField] private GameObject obstaclesPrefab;
        [SerializeField] private float heightObstacles = 0.5f;
        [SerializeField] private float widthObstacles = 0.5f;
        
        public GameObject ObstaclesPrefab => obstaclesPrefab;
        public float HeightObstacles => heightObstacles;
        public float WidthObstacles => widthObstacles;
    }
}