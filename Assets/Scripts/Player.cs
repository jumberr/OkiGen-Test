using UnityEngine;

public class Player : MonoBehaviour
{
    private int coins;

    public int Coins => coins;

    private void Awake()
    {
        coins = PlayerPrefs.GetInt("Money");
    }

    public void AddCoins(int value)
    {
        coins += value;
        PlayerPrefs.SetInt("Money", coins);
    }
}
