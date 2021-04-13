using UnityEngine;

public class CoinsBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent<Player>(out var player)) return;
        player.AddCoins(1);
        gameObject.SetActive(false);
    }
}
