using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "Platforms", menuName = "Platforms Stats", order = 0)]
    public class PlatformsStats : ScriptableObject
    {
        [Space, Header("PLATFORM")]
        [SerializeField] private GameObject platform;
        [SerializeField] private GameObject corner;
        [SerializeField] private GameObject finish;
        [SerializeField] private float leftBorderX = -1.5f;
        [SerializeField] private float lengthOfPlatform = 20f;
        [SerializeField] private float widthOfPlatform = 3.5f;
        
        public GameObject Platform => platform;
        public GameObject Corner => corner;
        public GameObject Finish => finish;
        public float LeftBorderX => leftBorderX;
        public float LengthOfPlatform => lengthOfPlatform;
        public float WidthOfPlatform => widthOfPlatform;
    }
}