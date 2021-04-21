using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "Obstacles", menuName = "Obstacle Stats", order = 0)]
    public class ObstaclesStats : ScriptableObject
    {
        [Space, Header("OBSTACLES")]
        [SerializeField] private GameObject obstaclesPrefab;
        [SerializeField] private float sideOfObstacles = 0.5f;

        public GameObject ObstaclesPrefab => obstaclesPrefab;
        public float SideOfObstacles => sideOfObstacles;
    }
}