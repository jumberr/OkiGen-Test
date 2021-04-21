using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "Cubes", menuName = "Cube Stats", order = 0)]
    public class CubesStats : ScriptableObject
    {
        [Space, Header("CUBES")]
        [SerializeField] private GameObject cubesPrefab;
        [SerializeField] private float sideCubes = 0.5f;

        public GameObject CubesPrefab => cubesPrefab;
        public float SideCubes => sideCubes;
    }
}