using UnityEngine;

namespace Classes
{
    public class Player
    {
        private int coins;
        public int Coins => coins;

        public Player()
        {
            coins = PlayerPrefs.GetInt("Money");
        
        }
    
        public void AddCoins()
        {
            coins++;
            PlayerPrefs.SetInt("Money", coins);
        }
    }
}
