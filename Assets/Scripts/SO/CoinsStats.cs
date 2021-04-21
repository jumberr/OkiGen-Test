using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "Coins", menuName = "Coin Stats", order = 0)]
    public class CoinsStats : ScriptableObject
    {
        [Space, Header("COINS")]
        [SerializeField] private GameObject coinsPrefab;
        [SerializeField] private float coinsSide = 0.5f;
        
        public GameObject CoinsPrefab => coinsPrefab;
        public float CoinsSide => coinsSide;
    }
}