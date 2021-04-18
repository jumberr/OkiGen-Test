using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "Cubes", menuName = "Cube Stats", order = 0)]
    public class CubesStats : ScriptableObject
    {
        [Space, Header("CUBES")]
        [SerializeField] private GameObject cubesPrefab;
        [SerializeField] private float heightCubes = 0.5f;
        [SerializeField] private float widthCubes = 0.5f;
        [SerializeField] private float lengthCubes = 0.5f;
        
        public GameObject CubesPrefab => cubesPrefab;
        public float HeightCubes => heightCubes;
        public float WidthCubes => widthCubes;
        public float LengthCubes => lengthCubes;
    }
}