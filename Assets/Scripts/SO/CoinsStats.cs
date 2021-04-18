using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "Coins", menuName = "Coin Stats", order = 0)]
    public class CoinsStats : ScriptableObject
    {
        [Space, Header("COINS")]
        [SerializeField] private GameObject coinsPrefab;
        
        public GameObject CoinsPrefab => coinsPrefab;
    }
}