using Classes;
using UnityEngine;

namespace Handlers
{
    public class PlayerHandler : MonoBehaviour
    {
        private Player player;

        private void Awake()
        {
            player = new Player();
            GenObjBehavior.OnAddCoins += player.AddCoins;
        }

        public int GetCoins()
        {
            return player.Coins;
        }
    }
}
